using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CIIP;

namespace CIIP.Module.BusinessObjects.SYS.BOBuilder
{
    public class AssemblyBuilder
    {
        public string GenerateBusinessObjectClasses(IList<BusinessObject> bos)
        {
            var rst = new StringBuilder();
            var userbos = bos.Where(x => x.IsRuntimeDefine);
            foreach (var item in userbos)
            {
                rst.Append(GenerateClass(item));
            }
            return rst.ToString();
        }

        public string GenerateClass(BusinessObject bo)
        {
            var rst = new StringBuilder();

            rst.AppendLine($"namespace {bo.Category.FullName}{{");
            rst.AppendLine("//BO:" + bo.Oid);
            if (!bo.IsPersistent)
            {
                rst.AppendLine("[NonPersistent]");
            }

            if (bo.IsCloneable.HasValue && bo.IsCloneable.Value)
            {
                rst.AppendLine("[ModelDefault(\"Cloneable\",\"True\")]");
            }
            if (bo.IsCreatableItem.HasValue && bo.IsCreatableItem.Value)
            {
                rst.AppendLine("[ModelDefault(\"Createable\",\"True\")]");
            }
            if (bo.IsVisibileInReports.HasValue && bo.IsVisibileInReports.Value)
            {
                rst.AppendLine("[VisibleInReport]");
            }

            rst.AppendLine($"public { (bo.CanInherits ? "" : "sealed") + (bo.IsAbstract ? "abstract" : "") } class { bo.名称 } : ");

            if (bo.Base.IsGenericTypeDefine)
            {
                var n = bo.Base.FullName;
                rst.Append(n.Substring(0, n.Length - 2));
                rst.AppendFormat("<{0}>", string.Join(",", bo.GenericParameters.Select(x => x.ParameterValue.FullName).ToArray()));
            }
            else
            {
                rst.Append(bo.Base.FullName);
            }
            //begin class
            rst.AppendLine("{");
            
            rst.AppendLine($"   public {bo.名称}(Session s):base(s){{  }}");

            foreach (var item in bo.Properties)
            {
                rst.AppendLine($"{ item.PropertyType.FullName } _{ item.名称 };");

                if (item.Size != 100 && item.Size != 0)
                {
                    rst.AppendLine($"[Size({item.Size})]");
                }

                rst.AppendLine($"public { item.PropertyType.FullName } { item.名称 }");
                rst.AppendLine("{");
                rst.AppendLine("get");
                rst.AppendLine("{");
                rst.AppendLine($"    return _{item.名称};");
                rst.AppendLine("}");
                rst.AppendLine("set");
                rst.AppendLine("{");
                rst.AppendLine($"SetPropertyValue(\"{item.名称}\",ref _{item.名称},value);");
                rst.AppendLine("}");
                rst.AppendLine("}");
            }

            foreach (var item in bo.CollectionProperties)
            {
                if (item.Aggregated)
                {
                    rst.AppendLine("[DevExpress.Xpo.Aggrgated]");
                }
                rst.AppendLine($"public XpCollection<{item.PropertyType.FullName}> {item.名称}{{ get{{ return GetCollection<{item.PropertyType.FullName}>(\"{item.名称}\"); }} }}");
            }

            foreach (var method in bo.Methods)
            {
                rst.Append(method.MethodDefineCode);
            }
            
            //end class
            rst.AppendLine("}");
            rst.AppendLine("}");


            return rst.ToString();
        }

        public string CreateAssemblyAttributes()
        {
            #region GetVersion

            Version ver = BusinessBuilder.GetVersion(AdmiralEnvironment.UserDefineBusinessFile);

            if (ver != null)
            {
                ver = new Version(ver.Major + 1, ver.Minor, ver.Build, ver.Revision);
            }
            else
            {
                ver = new Version(1, 0, 0, 0);
            }

            #endregion
            
            return $"[assembly: {typeof(AssemblyVersionAttribute).FullName}(\"{ver.ToString()}\")]\n";
        }

        public string CreateXafModule()
        {
            return $"public class RuntimeModule:{ typeof(RuntimeModuleBase).FullName } {{ public RuntimeModule():base(){{}} }}\n";
        }

        public ComplierError GenerateModuleAssembly(IList<BusinessObject> bos)
        {
            string code;
            SyntaxTree tree = GetSyntaxTree(bos,out code);

            //var cw = MSBuildWorkspace.Create();

            //OptionSet options = cw.Options;
            //options = options.WithChangedOption(CSharpFormattingOptions.OpenBracesInNewLineForMethods, false);
            //options = options.WithChangedOption(CSharpFormattingOptions.OpenBracesInNewLineForTypes, false);
            //SyntaxNode formattedNode = Formatter.Format(
            //    tree.GetRoot(),
            //    cw,
            //    options
            //    );
            //code = formattedNode.ToString();



            var refs = CollectReferencedAssemblies();
            
            var compilation = CSharpCompilation.Create("IMatrixRuntimeBusinessObject.dll", new[] { tree },
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                references: refs);

            var outputPath = AdmiralEnvironment.UserDefineBusinessTempFile.FullName;
            var rst = compilation.Emit(outputPath);
            if (rst.Success)
                return null;
            else
            {
                var obj = new ComplierError(string.Join("\n", rst.Diagnostics.Select(x => x.ToString())));
                obj.Code = tree.GetText().ToString();
                return obj;
            }
            //Assembly compiledAssembly;
            //using (var stream = new MemoryStream())
            //{
            //    compiledAssembly = Assembly.Load(stream.GetBuffer());
            //}

            //var calculatorClass = compiledAssembly.GetType("Calculator");
            //var evaluateMethod = calculatorClass.GetMethod("Evaluate");
            //var result = evaluateMethod.Invoke(null, null).ToString();
            //Console.WriteLine(result);
        }

        public SyntaxTree GetSyntaxTree(IList<BusinessObject> bos,out string code)
        {
            var codes = new StringBuilder();

            codes.Append(@"using System;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.Linq;
");

            codes.Append(CreateAssemblyAttributes());
            codes.Append(CreateXafModule());

            codes.Append(GenerateBusinessObjectClasses(bos));
            code = codes.ToString();

            return SyntaxFactory.ParseSyntaxTree(code);
        }

        static List<MetadataReference> CollectReferencedAssemblies()
        {
            List<MetadataReference> refs = new List<MetadataReference>();
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var item in typeof(AssemblyBuilder).Assembly.GetReferencedAssemblies())
            {
                var asm = asms.SingleOrDefault(x => x.FullName == item.FullName);
                if (asm == null)
                {
                    asm = Assembly.Load(item);
                }
                if (asm == null)
                {
                    throw new Exception("Not found referenced assembly:" + item.FullName);
                }
                refs.Add(MetadataReference.CreateFromFile(asm.Location));
            }
            refs.Add(MetadataReference.CreateFromFile(typeof(AssemblyBuilder).Assembly.Location));
            return refs;
        }
    }
}
