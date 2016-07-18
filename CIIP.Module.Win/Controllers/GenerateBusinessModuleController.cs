using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System.IO;
using DevExpress.ExpressApp.Xpo.Updating;
using DevExpress.Xpo;
using System.ComponentModel;
using CIIP;
using 常用基类;
using SR = System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using CIIP.Module.Win.Editors;

namespace CIIP.Module.BusinessObjects.SYS.BOBuilder
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class GenerateBusinessModuleController : WindowController
    {
        public GenerateBusinessModuleController()
        {
            InitializeComponent();
            this.TargetWindowType = WindowType.Main;
            // Target required Windows (via the TargetXXX properties) and create their Actions.
        }

        private void 生成业务模型_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Compile(e,false);
        }

        private void Compile(SimpleActionExecuteEventArgs e,bool showCode)
        {
            var os = Application.CreateObjectSpace();
            var workspace = SmartIDEWorkspace.GetIDE(os);
            var rst = workspace.Compile();
            if (rst != null)
            {

                if (!rst.Success || showCode)
                {
                    var solution = new Solution();
                    solution.Code = new SYS.Logic.CsharpCode("", null);
                    solution.Code.ShowSolutionFiles = true;
                    solution.Code.Workspace = workspace;
                    solution.Code.Diagnostics = rst.Diagnostics.ToList();
                    var view = Application.CreateDetailView(os, solution);
                    e.ShowViewParameters.CreatedView = view;
                    e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                }
                else
                {
                    var runtimeModule = os.FindObject<BusinessModule>(new BinaryOperator("ModuleName", "RuntimeModule.dll"));
                    if (runtimeModule == null)
                    {
                        runtimeModule = os.CreateObject<BusinessModule>();
                        runtimeModule.Description = "用户定义的模块";
                    }

                    using (var stream = File.OpenRead(AdmiralEnvironment.UserDefineBusinessTempFile.FullName))
                    {
                        runtimeModule.File = new DevExpress.Persistent.BaseImpl.FileData(runtimeModule.Session);
                        runtimeModule.File.LoadFromStream(AdmiralEnvironment.UserDefineBusinessTempFile.Name, stream);
                        runtimeModule.Save();
                        os.CommitChanges();
                    }


                    var moduleInfo = os.FindObject<ModuleInfo>(new BinaryOperator("Name", "RuntimeModule"));
                    if (moduleInfo != null)
                    {
                        os.Delete(moduleInfo);
                        os.CommitChanges();
                    }
                    var restart = Application as IRestartApplication;
                    if (restart != null)
                    {
                        restart.RestartApplication();
                    }
                }
            }
        }

        //Solution GenateAssembly()
        //{
        //    var ObjectSpace = this.Application.CreateObjectSpace();
        //    var allTypes = ObjectSpace.GetObjects<BusinessObject>();
        //    var builder = new AssemblyBuilder();
        //    var rst = builder.GenerateModuleAssembly(allTypes);
        //    #region 删除模块信息,让系统执行更新表结构动作
        //    if (rst == null)
        //    {
        //        var moduleInfo = ObjectSpace.FindObject<ModuleInfo>(new BinaryOperator("Name", "ERPModule"));
        //        if (moduleInfo != null)
        //        {
        //            ObjectSpace.Delete(moduleInfo);
        //            ObjectSpace.CommitChanges();
        //        }
        //        var restart = Application as IRestartApplication;
        //        if (restart != null)
        //        {
        //            restart.RestartApplication();
        //        }
        //    }
        //    return rst;

        //    #endregion
        //}


        //private void GenerateModuleAssembly()
        //{
        //    #region GetVersion

        //    Version ver = BusinessBuilder.GetVersion(AdmiralEnvironment.UserDefineBusinessFile);

        //    if (ver != null)
        //    {
        //        ver = new Version(ver.Major + 1, ver.Minor, ver.Build, ver.Revision);
        //    }
        //    else
        //    {
        //        ver = new Version(1, 0, 0, 0);
        //    }

        //    #endregion

        //    var assemblyName = "AdmiralDynamicDC";
        //    var newFileName = AdmiralEnvironment.UserDefineBusinessTempFile.FullName;

        //    #region 定义程序集

        //    var asmName = new AssemblyNameDefinition(assemblyName, ver);
        //    //[assembly: AssemblyFileVersionAttribute("1.0.0.12")]
        //    var assembly =
        //        AssemblyDefinition.CreateAssembly(
        //            asmName,
        //            "MainModule",
        //            ModuleKind.Dll
        //            );
        //    //AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Save, AdmiralEnvironment.UserDefineBusinessDirectoryInfo.FullName);

        //    #region 定义模块

        //    var module = assembly.MainModule;

        //    #endregion

        //    #endregion

        //    #region 设置文件版本

        //    var asmFileVerCtor = typeof(SR.AssemblyFileVersionAttribute).GetConstructor(new[] { typeof(string) });

        //    var asmFileVerCtorRef = assembly.MainModule.ImportReference(asmFileVerCtor);
        //    var ca = new Mono.Cecil.CustomAttribute(asmFileVerCtorRef);
        //    ca.ConstructorArguments.Add(new CustomAttributeArgument(
        //        assembly.MainModule.ImportReference(typeof(string)), ver.ToString()));
        //    assembly.CustomAttributes.Add(ca);

        //    #endregion

        //    #region XafModule

        //    var xafModule = new TypeDefinition("", "RuntimeModule",
        //        TypeAttributes.Public | TypeAttributes.Class,
        //        assembly.MainModule.ImportReference(typeof(RuntimeModuleBase)));

        //    //module.DefineType("RuntimeModule", TypeAttributes.Public | TypeAttributes.Class, typeof(RuntimeModuleBase));
        //    var ctor = new MethodDefinition(".ctor",
        //        MethodAttributes.Public
        //        | MethodAttributes.HideBySig
        //        | MethodAttributes.SpecialName
        //        | MethodAttributes.RTSpecialName,
        //        module.ImportReference(typeof(void))
        //        );
        //    //.method public hidebysig specialname rtspecialname instance void .ctor() cil managed

        //    //xafModule.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard | CallingConventions.HasThis, Type.EmptyTypes);
        //    var baseCtor = typeof(RuntimeModuleBase).GetConstructor(Type.EmptyTypes);

        //    var il = ctor.Body.Instructions;

        //    //.maxstack 8
        //    //L_0000: ldarg.0 
        //    //L_0001: call instance void Admiral.ERP.Module.RuntimeModuleBase::.ctor()
        //    //L_0006: ret 
        //    il.Add(Instruction.Create(OpCodes.Ldarg_0));
        //    il.Add(Instruction.Create(OpCodes.Call, module.ImportReference(baseCtor)));
        //    il.Add(Instruction.Create(OpCodes.Ret));
        //    xafModule.Methods.Add(ctor);
        //    module.Types.Add(xafModule);

        //    #endregion

        //    #region 创建业务对象
        //    var ObjectSpace = this.Application.CreateObjectSpace();
        //    var allTypes = ObjectSpace.GetObjects<BusinessObjectBase>();

        //    var userDefinedBos = allTypes.OfType<BusinessObject>().Where(x => x.IsRuntimeDefine).ToList(); //ObjectSpace.GetObjects<BusinessObject>(new BinaryOperator("IsRuntimeDefine", true)).OrderBy(x => x.CreateIndex).ToArray();
        //    var dllTypes = allTypes.Except(userDefinedBos); //ObjectSpace.GetObjects<BusinessObject>(new BinaryOperator("IsRuntimeDefine", false));
        //    module.InitializeTypeReferences(dllTypes);



        //    //第一步只生成类型定义，是因为可能有交叉使用类型，如，基类是未定义的类型，则无法使用
        //    #region 定义类型生成的代码，包含属性修饰：-->[NonPersistent]pubic class {    }
        //    foreach (var bo in userDefinedBos)
        //    {
        //        var typeAtt = TypeAttributes.Class | TypeAttributes.Public;
        //        if (bo.IsAbstract)
        //        {
        //            typeAtt |= TypeAttributes.Abstract;
        //        }
        //        else if (!bo.CanInherits)
        //        {
        //            typeAtt |= TypeAttributes.Sealed;
        //        }

        //        var type = new TypeDefinition(bo.Category.FullName, bo.名称, typeAtt);
        //        module.Types.Add(type);

        //        if (!bo.IsPersistent)
        //        {
        //            type.AddNonPersistentDc();
        //        }

        //        if (bo.IsCloneable.HasValue)
        //        {
        //            type.ModelDefault("IsCloneable", bo.IsCloneable.Value.ToString().ToLower() );
        //        }

        //        if (bo.IsCreatableItem.HasValue)
        //        {
        //            type.ModelDefault("IsCreatableItem", bo.IsCreatableItem.Value.ToString().ToLower());
        //        }

        //        if (bo.IsVisibileInReports.HasValue)
        //        {
        //            type.VisibileInReport(bo.IsVisibileInReports.Value);
        //        }
        //        type.AddToTypeReferences(bo);
        //    }
        //    #endregion

        //    #region 生成剩下的内容
        //    foreach (var bo in userDefinedBos)
        //    {
        //        var type = bo.GetTypeDefintion();
        //        if (type == null)
        //            throw new Exception("错误!");

        //        #region 处理基类

        //        var boBaseType = bo.Base.GetTypeReference() ?? module.ImportReference(typeof(SimpleObject));
        //        MethodReference boBaseCtor;

        //        if (boBaseType.HasGenericParameters)
        //        {
        //            type.BaseType = boBaseType.MakeGenericType(bo.GenericParameters.Select(gp => gp.ParameterValue.GetTypeReference()).ToArray());

        //            //var tb = boBaseType.Resolve().MakeGenericType(bo.GenericParameters.Select(gp => gp.ParameterValue.FindType(typeReferences)).ToArray());

        //            boBaseCtor =
        //                //(tb as TypeDefinition).Methods.Single(x => x.Name == ".ctor" && x.Parameters.First().ParameterType.FullName == typeof(Session).FullName);
        //                module.ImportReference(
        //                    type.BaseType.Resolve()
        //                        .Methods.Single(
        //                            x => x.Name == ".ctor" && x.Parameters.First().ParameterType.FullName == typeof(Session).FullName)
        //                    );
        //            boBaseCtor = boBaseCtor.MakeGeneric(bo.GenericParameters.Select(gp => gp.ParameterValue.GetTypeReference()).ToArray());
        //        }
        //        else
        //        {
        //            type.BaseType = boBaseType;
        //            boBaseCtor = module.ImportReference(
        //                boBaseType.Resolve().Methods.Single(x => x.Name == ".ctor" && x.Parameters.Count == 1 && x.Parameters.Single().ParameterType.FullName == typeof(Session).FullName)
        //                );
        //        }

        //        if (boBaseCtor != null)
        //        {
        //            var boctor = new MethodDefinition(".ctor",
        //                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.RTSpecialName,
        //                //CallingConventions.Standard | CallingConventions.HasThis,
        //                module.ImportReference(typeof(void))
        //                );
        //            // type.DefineConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.RTSpecialName, CallingConventions.Standard | CallingConventions.HasThis, new Type[] { typeof(Session) });
        //            var p = new ParameterDefinition("session", ParameterAttributes.None, module.ImportReference(typeof(Session)));
        //            boctor.Parameters.Add(p);
        //            var boil = boctor.Body.GetILProcessor();
        //            //.GetILGenerator();

        //            boil.Emit(OpCodes.Ldarg_0);
        //            boil.Emit(OpCodes.Ldarg_1);
        //            boil.Emit(OpCodes.Call, boBaseCtor);
        //            boil.Emit(OpCodes.Nop);
        //            boil.Emit(OpCodes.Nop);
        //            boil.Emit(OpCodes.Ret);

        //            //L_0000: ldarg.0
        //            //L_0001: ldarg.1
        //            //L_0002: call instance void IMatrix.ERP.Module.BusinessObjects.订单`1 <class IMatrix.ERP.Module.BusinessObjects.PMS.采购订单明细>::.ctor(class [DevExpress.Xpo.v15.2]
        //            //DevExpress.Xpo.Session)
        //            //L_0007: nop
        //            //L_0008: nop
        //            //L_0009: ret

        //            //.method public hidebysig specialname rtspecialname 
        //            //instance void  .ctor(class ['DevExpress.Xpo.v15.2']DevExpress.Xpo.Session session) cil managed
        //            //{
        //            //// 代码大小       10 (0xa)
        //            //.maxstack  8
        //            //IL_0000:  ldarg.0
        //            //IL_0001:  ldarg.1
        //            //IL_0002:  call       instance void [IMatrix.ERP.Module]IMatrix.ERP.Module.BusinessObjects.'订单`1'::.ctor<class '业务'.'采购计划明细'>(class ['DevExpress.Xpo.v15.2']DevExpress.Xpo.Session)
        //            //IL_0007:  nop
        //            //IL_0008:  nop
        //            //IL_0009:  ret
        //            //} // end of method '采购计划'::.ctor

        //            type.Methods.Add(boctor);
        //        }

        //        #endregion

        //        #region 填加属性

        //        foreach (var p in bo.Properties)
        //        {
        //            BuildPropertyCore(p, type);
        //        }

        //        #endregion

        //        #region 填加集合属性
        //        foreach (var item in bo.CollectionProperties)
        //        {
        //            BuildCollectionProperty(item, type);
        //        }
        //        #endregion
        //    }
        //    #endregion

        //    #endregion

        //    #region 保存生成的程序集
        //    assembly.Write(newFileName);
        //    #endregion

        //    #region 删除模块信息,让系统执行更新表结构动作

        //    var moduleInfo = ObjectSpace.FindObject<ModuleInfo>(new BinaryOperator("Name", "RuntimeModule"));
        //    if (moduleInfo != null)
        //    {
        //        ObjectSpace.Delete(moduleInfo);
        //        ObjectSpace.CommitChanges();
        //    }
        //    var restart = Application as IRestartApplication;
        //    if (restart != null)
        //    {
        //        restart.RestartApplication();
        //    }
        //    #endregion

        //}

        //public PropertyDefinition BuildCollectionProperty(CollectionProperty property,TypeDefinition type)
        //{
        //    //如果是一对多关系,即关联属性是单值属性时,直接在上面加上关联关系
        //    //如果是多对多,则不需要处理,因为所属类型会执行这个动作


        //    var mod = type.Module;
        //    var elementType = property.PropertyType.GetTypeReference();
        //    var ptype = mod.ImportReference(typeof(XPCollection<>)).MakeGenericType(elementType);
        //    var propertyInfo = new PropertyDefinition(property.名称, PropertyAttributes.None, ptype) { HasThis = true };
        //    type.Properties.Add(propertyInfo);

        //    propertyInfo.Assocication(property);
        //    if (property.Aggregated)
        //    {
        //        propertyInfo.Aggregate();
        //    }

        //    #region getter
        //    //.method public hidebysig specialname instance string get_创建者() cil managedMono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.HideBySig | Mono.Cecil.MethodAttributes.SpecialName
        //    var attr = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.FamANDAssem | MethodAttributes.Family;

        //    var getMethod = new MethodDefinition("get_" + property.名称, attr, ptype);

        //    var getMethodIl = getMethod.Body.GetILProcessor();
        //    getMethod.Body.Variables.Add(new VariableDefinition(ptype));

        //    getMethod.Body.MaxStackSize = 8;

        //    getMethod.Body.InitLocals = true;
        //    //.maxstack 2
        //    //.locals init ([0] class [DevExpress.Xpo.v15.2]DevExpress.Xpo.XPCollection`1<class IMatrix.ERP.Module.BusinessObjects.员工> xps)
        //    //L_0000: nop 
        //    //L_0001: ldarg.0 
        //    //L_0002: ldstr "\u8054\u7cfb\u4eba"
        //    //L_0007: call instance class [DevExpress.Xpo.v15.2]DevExpress.Xpo.XPCollection`1<!!0> [DevExpress.Xpo.v15.2]DevExpress.Xpo.XPBaseObject::GetCollection<class IMatrix.ERP.Module.BusinessObjects.员工>(string)
        //    //L_000c: stloc.0 
        //    //L_000d: br.s L_000f
        //    //L_000f: ldloc.0 
        //    //L_0010: ret 
        //    getMethodIl.Emit(OpCodes.Nop);
        //    getMethodIl.Emit(OpCodes.Ldarg_0);
        //    getMethodIl.Emit(OpCodes.Ldstr, property.名称);
        //    var clrGetCollection = typeof(SimpleObject).GetMethods(SR.BindingFlags.NonPublic | SR.BindingFlags.Instance).Single(x => x.Name == "GetCollection" && x.ReturnType.IsGenericType);
        //    var getCollection = mod.ImportReference(clrGetCollection);    //(tb as TypeDefinition).Methods.Single(x => x.Name == ".ctor" && x.Parameters.First().ParameterType.FullName == typeof(Session).FullName);
        //    getCollection = getCollection.MakeGenericMethod(elementType);

        //    getMethodIl.Emit(OpCodes.Call, getCollection);
        //    getMethodIl.Emit(OpCodes.Stloc_0);

        //    var ldloc0 = getMethodIl.Create(OpCodes.Ldloc_0);
        //    getMethodIl.Emit(OpCodes.Br_S, ldloc0);
        //    getMethodIl.Append(ldloc0);
        //    getMethodIl.Emit(OpCodes.Ret);            
        //    type.Methods.Add(getMethod);
        //    propertyInfo.GetMethod = getMethod;

        //    #endregion

        //    return propertyInfo;
        //}

        //public static PropertyDefinition BuildPropertyCore(Property property, TypeDefinition type)
        //{
        //    var mod = type.Module;
        //    //从缓存中查找clr type
        //    var ptype = property.PropertyType.GetTypeReference();

        //    var fieldInfo = new FieldDefinition("_" + property.名称, FieldAttributes.Private, ptype);
        //    type.Fields.Add(fieldInfo);
        //    // type.DefineField("_" + property.名称, ptype, FieldAttributes.Private);

        //    var propertyInfo = new PropertyDefinition(property.名称, PropertyAttributes.None, ptype) { HasThis = true };
        //    type.Properties.Add(propertyInfo);
        //    #region 基本属性设置

        //    if (property.Size != 100 && property.Size != 0)
        //    {
        //        propertyInfo.Size(property.Size);
        //    }

        //    //if (!string.IsNullOrEmpty(property.Expression))
        //    //{
        //    //    propertyInfo.PersistentAlias(property.Expression);
        //    //}

        //    #endregion;

        //    #region getter
        //    //.method public hidebysig specialname instance string get_创建者() cil managedMono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.HideBySig | Mono.Cecil.MethodAttributes.SpecialName
        //    var attr = MethodAttributes.Public | MethodAttributes.HideBySig| MethodAttributes.SpecialName| MethodAttributes.FamANDAssem | MethodAttributes.Family; 
        //    var setattr = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.FamANDAssem | MethodAttributes.Family;

        //    var getMethod = new MethodDefinition("get_" + property.名称, attr, ptype);

        //    var getMethodIl = getMethod.Body.GetILProcessor();

        //    getMethod.Body.MaxStackSize = 8;
        //    getMethod.Body.InitLocals = true;

        //    getMethodIl.Emit(OpCodes.Ldarg_0);
        //    getMethodIl.Emit(OpCodes.Ldfld, fieldInfo);
        //    getMethodIl.Emit(OpCodes.Ret);
        //    type.Methods.Add(getMethod);
        //    propertyInfo.GetMethod = getMethod;

        //    #endregion

        //    #region 非计算类型

        //    if (string.IsNullOrEmpty(property.Expression))
        //    {
        //        var set = new MethodDefinition("set_" + property.名称, setattr, mod.ImportReference(typeof (void)));
        //        var para = new ParameterDefinition("value", ParameterAttributes.None, ptype);
        //        set.Parameters.Add(para);
        //        var setPropertyValue = mod.ImportReference(
        //            typeof (PersistentBase)
        //                .GetMethods(
        //                    SR.BindingFlags.InvokeMethod | SR.BindingFlags.NonPublic |
        //                    SR.BindingFlags.Instance)
        //                .Single(x => x.Name.StartsWith("SetPropertyValue") && x.IsGenericMethod &&
        //                             x.GetParameters().Length == 3)
        //            );
        //        var setPropertyValueMethod = new GenericInstanceMethod(setPropertyValue);
        //        setPropertyValueMethod.GenericArguments.Add(ptype);
        //        //setPropertyValue =  setPropertyValue.MakeGenericMethod(ptype);

        //        var setIl = set.Body.Instructions;
        //        setIl.Add(Instruction.Create(OpCodes.Ldarg_0));
        //        setIl.Add(Instruction.Create(OpCodes.Ldstr, property.名称));
        //        setIl.Add(Instruction.Create(OpCodes.Ldarg_0));
        //        setIl.Add(Instruction.Create(OpCodes.Ldflda, fieldInfo));
        //        setIl.Add(Instruction.Create(OpCodes.Ldarg_1));
        //        setIl.Add(Instruction.Create(OpCodes.Call, setPropertyValueMethod));
        //        setIl.Add(Instruction.Create(OpCodes.Pop));
        //        setIl.Add(Instruction.Create(OpCodes.Ret));
        //        propertyInfo.SetMethod = set;
        //        type.Methods.Add(set);
        //    }

        //    #endregion

        //    return propertyInfo;
        //}

        //private void AddDefaultProperty(TypeDefinition type, Property property)
        //{
        //    var ctor = typeof(DefaultPropertyAttribute).GetConstructors().Single(x => x.IsPublic && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(string));
        //    var att = new CustomAttributeBuilder(ctor, new object[] { property.名称 });

        //    type.SetCustomAttribute(att);
        //}

        //private Type ImportTypeReference(BusinessObjectBase bo, Dictionary<BusinessObjectBase, TypeBuilder> asm)
        //{
        //    if (bo is SimpleType)
        //    {
        //        var biType = ReflectionHelper.FindType(bo.FullName);
        //        return biType;
        //    }
        //    else
        //    {
        //        return asm[bo];
        //    }
        //}

        //public void AddNavigationItem(TypeBuilder type, string group)
        //{
        //    var ctor = typeof(NavigationItemAttribute).GetConstructors().Single(x => x.IsPublic && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(string));
        //    var att = new CustomAttributeBuilder(ctor, new object[] { group });

        //    type.SetCustomAttribute(att);
        //}

        private static ModuleBase GetModule()
        {
            var assembly = SR.Assembly.LoadFile(Path.GetFullPath(@".\Module1.dll"));
            ModuleBase module = (ModuleBase)assembly.GetType("Module1.Module1Module").GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
            return module;
        }

        private void simpleAction1_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ModuleBase module = GetModule();
            if (!HasModule(module))
            {
                Application.Modules.Add(module);
                Reload();
            }
        }

        private void simpleAction2_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ModuleBase module = GetModule();
            if (HasModule(module))
            {
                for (int i = Application.Modules.Count - 1; i >= 0; i--)
                {
                    if (AreSameModules(module, Application.Modules[i]))
                    {
                        Application.Modules.RemoveAt(i);
                        break;
                    }
                }
                Reload();
            }
        }

        private void Reload()
        {
            Application.Setup();
            Application.LogOff();
        }

        private bool HasModule(ModuleBase module)
        {
            bool hasModule = false;
            if (module != null)
            {
                foreach (ModuleBase item in Application.Modules.Where(item => AreSameModules(module, item)))
                {
                    hasModule = true;
                }
            }
            return hasModule;
        }

        private static bool AreSameModules(ModuleBase one, ModuleBase two)
        {
            return two.Name == one.Name && two.AssemblyName == one.AssemblyName;
        }

        private void ViewSourceCode_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Compile(e, true);
        }
    }
}
