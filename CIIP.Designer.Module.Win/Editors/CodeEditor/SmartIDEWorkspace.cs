using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using CIIP.Module.BusinessObjects.SYS;
using CIIP.Module.BusinessObjects.SYS.BOBuilder;
using CIIP.Module.BusinessObjects.SYS.Logic;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using CIIP;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Recommendations;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Formatting;
using System.Windows.Media.Imaging;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System.Windows.Media;
using DevExpress.Xpo;
using cp = CIIP.ProjectManager.Project;

namespace CIIP.Module.Win.Editors
{
    /// <summary>
    /// 用于编辑器的后台服务,包含了Solution等信息
    /// </summary>
    public class SmartIDEWorkspace
    {
        public static SmartIDEWorkspace GetIDE(IObjectSpace os,cp project)
        {
            return new SmartIDEWorkspace(os,project);
            //if (Instance == null)
            //{
            //    Instance = new SmartIDEWorkspace(os);
            //}
            //return Instance;
        }

        public cp Project { get; set; }

        public static SmartIDEWorkspace Instance { get; private set; }
        
        public AdhocWorkspace Workspace { get;  }

        Microsoft.CodeAnalysis.Project ModuleProject {
            get { return Workspace.CurrentSolution.Projects.First(); }
        }

        private IObjectSpace objectSpace;

        public SmartIDEWorkspace(IObjectSpace objectSpace,cp project)
        {
            this.Workspace = new AdhocWorkspace();// MSBuildWorkspace.Create();
            this.objectSpace = objectSpace;
            this.Project = project;
            CreateSolution();

            InitializeKeywordItems();
        }

        string keywords = "var dynamic abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using virtual while bool byte char class const decimal double enum float int long sbyte short static string struct uint ulong ushort void";

        private readonly List<CompletionData> keywordItmes = new List<CompletionData>();
        
        //public string Code { get; }

        List<ImageSource> images = new List<ImageSource>();

        private void InitializeKeywordItems()
        {
            var path = AdmiralEnvironment.ApplicationPath + "\\AutoCompleteIcons\\";

            for (int i = 0; i <= 150; i++)
            {
                images.Add(new BitmapImage(new Uri(path + i + ".bmp")));
            }
            
            var ks = keywords.Split(' ');

            foreach (var s in ks)
            {
                keywordItmes.Add(new CompletionData(s, images[0], "", TokenType.iKeyWords));
            }
        }

        public EmitResult Compile(string path)
        {
            foreach (var doc in this.ModuleProject.Documents)
            {
                Debug.WriteLine(doc.Name);
            }
            return ModuleProject.GetCompilationAsync().Result.Emit(path);
        }

        public IEnumerable<Diagnostic> GetDocumentDiagnostic(Guid documentGuid)
        {
            return Documents[documentGuid].GetSemanticModelAsync().Result.GetDiagnostics();
        }

        protected void CreateSolution()
        {
            CreateModuleProject();
        }
        List<MetadataReference> CollectReferencedAssemblies()
        {
            List<MetadataReference> refs = new List<MetadataReference>();
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var item in typeof(CIIPDesignerModule).Assembly.GetReferencedAssemblies())
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
            //refs.Add(MetadataReference.CreateFromFile(typeof(CIIP.Module.ERPModule).Assembly.Location));
            refs.Add(MetadataReference.CreateFromFile(typeof(XPCollection).Assembly.Location));
            return refs;
        }

        private void CreateModuleProject()
        {
            var refs = CollectReferencedAssemblies();

            var moduleProjectName = "RuntimeModule";

            var moduleProjectID = ProjectId.CreateNewId();

            var versionStamp = VersionStamp.Create();

            var projInfo = ProjectInfo.Create(moduleProjectID, versionStamp, moduleProjectName, moduleProjectName,
                LanguageNames.CSharp,
                compilationOptions: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            projInfo = projInfo.WithMetadataReferences(refs);

            Workspace.AddProject(projInfo);

            //取得所有用户定义的业务类型,并生成文档.
            var businessObjects = this.objectSpace.GetObjects<BusinessObject>(new BinaryOperator("IsRuntimeDefine", true));

            CreateAssembyInfoDocument();
            CreateRuntimeModule();
            Documents.Clear();
            foreach (var bo in businessObjects)
            {
                CreateDocument(bo);
            }

            var partialLogics = this.objectSpace.GetObjects<BusinessObjectPartialLogic>(null,true);
            foreach (var logic in partialLogics)
            {
                CreateDocument(logic);
            }
            
            var layouts = this.objectSpace.GetObjects<BusinessObjectLayout>();
            foreach (var item in layouts)
            {
                CreateDocument(item);
            }
            var controllers = this.objectSpace.GetObjects<RuntimeController>(null,true);
            foreach (var item in controllers)
            {
                CreateDocument(item);
            }
        }
        
        string AggregatedAttribute = typeof(AggregatedAttribute).FullName;

        public static string GetCommonUsing()
        {
            return BusinessObjectCodeGenerateExtendesion.CommonUsing();
        }
        
        private Dictionary<Guid, Document> Documents = new Dictionary<Guid, Document>();
        private Dictionary<Guid, SemanticModel> SemanticModels = new Dictionary<Guid, SemanticModel>();

        public void CreateAssembyInfoDocument()
        {

            #region GetVersion

            var ver = BusinessBuilder.GetVersion(AdmiralEnvironment.UserDefineBusinessFile);

            if (ver != null)
            {
                ver = new Version(ver.Major + 1, ver.Minor, ver.Build, ver.Revision);
            }
            else
            {
                ver = new Version(1, 0, 0, 0);
            }

            #endregion

            var str = $"[assembly: {typeof (AssemblyVersionAttribute).FullName}(\"{ver.ToString()}\")]\n";
            Workspace.AddDocument(ModuleProject.Id, "AssemblyInfo.cs", SourceText.From(str,Encoding.UTF8));
        }

        public void CreateRuntimeModule()
        {
            var str =
                $"public class RuntimeModule:{typeof(ModuleBase).FullName} {{ public RuntimeModule():base(){{}} }}\n";
            Workspace.AddDocument(ModuleProject.Id, "RunttimeModule.cs", SourceText.From(str,Encoding.UTF8));
            
        }

        #region document provider services

        #region create document
        private void CreateDocument(IDocumentProvider documentProvider)
        {

            var doc = Workspace.AddDocument(ModuleProject.Id, documentProvider.GetFileName() + ".cs", SourceText.From(documentProvider.GetCode(),Encoding.UTF8));
            
            var updated = Workspace.TryApplyChanges(doc.Project.Solution);
            Debug.WriteLine("Updated:" + updated);
            Documents.Add(documentProvider.GetDocumentGuid(), doc);

            SemanticModels.Add(documentProvider.GetDocumentGuid(), doc.GetSemanticModelAsync().Result);
        }
        #endregion

        #region get text
        public string GetText(IDocumentProvider doc)
        {
            return Documents[doc.GetDocumentGuid()].GetTextAsync().Result.ToString();
        }
        #endregion

        #region update text
        private void UpdateText(string text, IDocumentProvider doc)
        {
            SourceText sourceText = SourceText.From(text,Encoding.UTF8);
            var document = Documents[doc.GetDocumentGuid()];
            document = document.WithText(sourceText);
            var rs = Workspace.TryApplyChanges(document.Project.Solution);

            Debug.WriteLine("enter updated:" + rs);
            //重要：当更改了项目后，文档实例被变化了，必须重新保存
            document = this.ModuleProject.Documents.First(x => x.Id == document.Id);
            Documents[doc.GetDocumentGuid()] = document;
            SemanticModels[doc.GetDocumentGuid()] = document.GetSemanticModelAsync().Result;
        }
        #endregion

        #region get intellisense items
        /// <summary>
        /// 取得光标位置的可用智能感知项目
        /// </summary>
        /// <param name="bodytext">代码</param>
        /// <param name="logic">对应项目</param>
        /// <param name="index">光标所在位置</param>
        /// <returns>可能智能感知项目</returns>
        public IEnumerable<ISymbol> GetRecommendedSymbolsAtPositionAsync(string bodytext, IDocumentProvider logic, int index)
        {
            if (logic is IPartCodeProvider)
            {
                var doc = Documents[logic.GetDocumentGuid()];
                bodytext = (logic as IPartCodeProvider).ReplaceNewCode(doc.GetTextAsync().Result.ToString(), bodytext);


                //var doc = Documents[method.BusinessObject.Oid];
                //var newText = method.ReplaceNewCode(doc.GetTextAsync().Result.ToString(), bodytext);

                //UpdateText(newText, method.BusinessObject);

                
                //var semanticModel = SemanticModels[method.BusinessObject.Oid];
                //return Recommender.GetRecommendedSymbolsAtPositionAsync(semanticModel, pos, Workspace).Result;
            }

            UpdateText(bodytext, logic);
            if (logic is IPartCodeProvider)
            {
                var define = (logic as IPartCodeProvider).DefaultLocation;
                index = bodytext.IndexOf(define) + define.Length + index + 1;
            }
           

            var semanticModel = SemanticModels[logic.GetDocumentGuid()];
            return Recommender.GetRecommendedSymbolsAtPositionAsync(semanticModel, index, Workspace).Result;
        }

        public void GetIntellisenseItems(IDocumentProvider document, int cartIndex,
    bool needKeyword, string code, string inputed, IList<ICompletionData> result)
        {
            IEnumerable<ISymbol> symbols = GetRecommendedSymbolsAtPositionAsync(code, document, cartIndex);
            ProcessSymbols(needKeyword, symbols, inputed, result);
        }

        private void ProcessSymbols(bool needKeyword, IEnumerable<ISymbol> symbols, string inputed, IList<ICompletionData> list)
        {

            TokenType idx = TokenType.iValueType;

            if (!string.IsNullOrEmpty(inputed))
            {
                symbols = symbols.Where(x => x.Name.Contains(inputed)).ToArray();
            }

            if (symbols == null)
            {

            }

            foreach (var symbol in symbols)
            {
                if (symbol is INamespaceSymbol)
                {
                    idx = TokenType.iNamespace;
                }
                else if (symbol is ITypeSymbol)
                {
                    idx = TokenType.iClass;
                }
                else if (symbol is IMethodSymbol)
                {
                    var m = symbol as IMethodSymbol;
                    if (m.IsExtensionMethod)
                    {
                        idx = TokenType.iMethodShortCut;
                    }
                    else if (symbol.DeclaredAccessibility == Accessibility.Public)
                    {
                        idx = TokenType.iMethod;
                    }
                    else if (symbol.DeclaredAccessibility == Accessibility.Protected)
                    {
                        idx = TokenType.iMethodProtected;
                    }
                    else if (symbol.DeclaredAccessibility == Accessibility.Private)
                    {
                        idx = TokenType.iMethodPrivate;
                    }
                    else if (symbol.DeclaredAccessibility == Accessibility.Internal)
                    {
                        idx = TokenType.iMethodFriend;
                    }
                    else
                    {
                        idx = TokenType.iMethodShortCut;
                    }
                }
                else if (symbol is IPropertySymbol)
                {
                    if (symbol.DeclaredAccessibility == Accessibility.Public)
                    {
                        idx = TokenType.iProperties;
                    }
                    else if (symbol.DeclaredAccessibility == Accessibility.Protected)
                    {
                        idx = TokenType.iPropertiesProtected;
                    }
                    else if (symbol.DeclaredAccessibility == Accessibility.Private)
                    {
                        idx = TokenType.iPropertiesPrivate;
                    }
                    else if (symbol.DeclaredAccessibility == Accessibility.Internal)
                    {
                        idx = TokenType.iPropertiesFriend;
                    }
                    else
                    {
                        idx = TokenType.iPropertiesShortCut;
                    }
                }
                else if (symbol is IFieldSymbol)
                {
                    if (symbol.DeclaredAccessibility == Accessibility.Public)
                    {
                        idx = TokenType.iField;
                    }
                    else if (symbol.DeclaredAccessibility == Accessibility.Protected)
                    {
                        idx = TokenType.iFieldProtected;
                    }
                    else if (symbol.DeclaredAccessibility == Accessibility.Private)
                    {
                        idx = TokenType.iFieldPrivate;
                    }
                    else if (symbol.DeclaredAccessibility == Accessibility.Internal)
                    {
                        idx = TokenType.iFieldFriend;
                    }
                    else
                    {
                        idx = TokenType.iFieldShortCut;
                    }
                }
                else if (symbol is IEventSymbol)
                {
                    if (symbol.DeclaredAccessibility == Accessibility.Public)
                    {
                        idx = TokenType.iEvent;
                    }
                    else if (symbol.DeclaredAccessibility == Accessibility.Protected)
                    {
                        idx = TokenType.iEventProtected;
                    }
                    else if (symbol.DeclaredAccessibility == Accessibility.Private)
                    {
                        idx = TokenType.iEventPrivate;
                    }
                    else if (symbol.DeclaredAccessibility == Accessibility.Internal)
                    {
                        idx = TokenType.iEventFriend;
                    }
                    else
                    {
                        idx = TokenType.iEventFriend;
                    }
                }
                else if (symbol is ILocalSymbol)
                {
                    idx = TokenType.iValueType;
                }
                else if (symbol is IParameterSymbol)
                {
                    idx = TokenType.iField;
                }
                else if (symbol is IPreprocessingSymbol)
                {
                    idx = TokenType.iProperties;
                }
                //if (idx != (int) AutoListIcons.iNamespace)
                if (symbol is IMethodSymbol)
                {
                    var m = symbol as IMethodSymbol;
                    list.Add(
                        new CompletionData(
                            symbol.Name + "(" + string.Join(",", m.Parameters.Select(x => x.Type.Name)) + ")",
                            images[(int)idx], m.ToString(), idx));
                }
                else
                {
                    list.Add(new CompletionData(symbol.Name, images[(int)idx], "", idx));
                }
            }

            if (needKeyword)
            {
                foreach (var item in keywordItmes)
                {
                    list.Add(item);
                }
            }
        }
        #endregion

        #region get diagnostics
        public IEnumerable<Diagnostic> GetDiagnostics(string text, IDocumentProvider method)
        {
            if (method is IPartCodeProvider)
            {
                var doc = Documents[method.GetDocumentGuid()];
                text = (method as IPartCodeProvider).ReplaceNewCode(doc.GetTextAsync().Result.ToString(), text);
            }
            UpdateText(text, method);
            var rst = SemanticModels[method.GetDocumentGuid()].GetDiagnostics();
            return rst;
        }
        #endregion

        #endregion
        public string GetAllCode()
        {
            var sb = new StringBuilder();
            
            foreach (var document in ModuleProject.Documents)
            {
                var root = document.GetSyntaxRootAsync().Result;
                var formattedResult = Formatter.Format(root, this.Workspace);
                var code = formattedResult.GetText().ToString();

                sb.AppendLine(code);
            }
            return sb.ToString();
        }


    }
}