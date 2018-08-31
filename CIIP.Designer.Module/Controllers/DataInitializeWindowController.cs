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
using CIIP.Module.BusinessObjects;
using System.Diagnostics;
using System.Drawing;
using CIIP.Module.BusinessObjects.SYS;
using CIIP.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CIIP.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class DataInitializeWindowController : WindowController
    {
        public DataInitializeWindowController()
        {
            InitializeComponent();
            TargetWindowType = WindowType.Main;
            // Target required Windows (via the TargetXXX properties) and create their Actions.
        }
        
        private void 数据初始化_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var os = Application.CreateObjectSpace();
            CreateSystemTypes(os,Application.Model,true);
            os.CommitChanges();
        }

        public static void CreateSystemTypes(IObjectSpace ObjectSpace,IModelApplication app,bool deleteExists)
        {
            var objs = ObjectSpace.GetObjectsQuery<BusinessObject>().Where(x => !x.IsRuntimeDefine).ToList(); //(new BinaryOperator("IsRuntimeDefine", false));
            if (!deleteExists && objs.Count > 0)
                return;

            ObjectSpace.Delete(objs);

            #region 系统类型

            //第一步,创建出所有基类类型

            var exists = ObjectSpace.GetObjects<BusinessObjectBase>(null, true);
            var types = new List<Type>() { typeof(BaseObject),typeof(XPBaseObject),typeof(XPLiteObject),typeof(XPCustomObject),typeof(XPObject) };
            var commonBase = CreateNameSpace("常用基类", ObjectSpace);
            AddBusinessObject(typeof(BaseObject), "BaseObject", commonBase, "主键:GUID,逻辑删除:是,并发锁:是", false, ObjectSpace);
            AddBusinessObject(typeof(XPBaseObject), "XPBaseObject", commonBase, "主键:无,逻辑删除:是,并发锁:无", false, ObjectSpace);
            AddBusinessObject(typeof(XPLiteObject), "XPLiteObject", commonBase, "主键:无,逻辑删除:否,并发锁:否", false, ObjectSpace);
            AddBusinessObject(typeof(XPCustomObject), "XPCustomObject", commonBase, "主键:无,逻辑删除:是,并发锁:是", false, ObjectSpace);
            AddBusinessObject(typeof(XPObject), "XPObject", commonBase, "主键:int,逻辑删除:是,并发锁:是", false, ObjectSpace);
//Class Name  Deferred Deletion   Optimistic Locking  Built -in OID key
//XPBaseObject - +-
//XPLiteObject - - -
//XPCustomObject + +-
//XPObject + + +


            //第二步,创建这些类的属性,因为属性中可能使用了类型,所以先要创建类型.
            var bos = ObjectSpace.GetObjects<BusinessObject>(null, true);
            foreach (var bob in bos.Where(x => !x.IsRuntimeDefine))
            {
                var type = ReflectionHelper.FindType(bob.FullName);

                if (type.BaseType != null && type.BaseType != typeof (object))
                {
                    var fullName = type.BaseType.Namespace + "." + type.BaseType.Name;

                    var baseType = bos.SingleOrDefault(x => x.FullName == fullName);
                    //bob.Base = baseType;

                    //if (bob.Base == null)
                    //{
                    //    Debug.WriteLine(type.FullName + "没有找到基类:" + fullName);
                    //}
                }

                if (type.IsGenericType)
                {
                    //bob.DisableCreateGenericParameterValues = true;

                    foreach (var item in type.GetGenericArguments())
                    {
                        var gp = ObjectSpace.CreateObject<GenericParameterInstance>();
                        //gp.Owner = bob;
                        gp.Name = item.Name;
                        if (item.IsGenericParameter)
                        {
                            gp.ParameterIndex = item.GenericParameterPosition;
                        }
                        else
                        {
                        }
                        if (!string.IsNullOrEmpty(item.FullName))
                            gp.ParameterValue = exists.SingleOrDefault(x => x.FullName == item.FullName);

                        var att =
                            item.GetCustomAttributes(typeof (ItemTypeAttribute), false)
                                .OfType<ItemTypeAttribute>()
                                .FirstOrDefault();
                        if (att != null)
                        {
                            var gi = bos.SingleOrDefault(x => x.FullName == att.ItemType.FullName);
                            gp.DefaultGenericType = gi;
                        }
                    }
                }


                var typeInfo = CaptionHelper.ApplicationModel.BOModel.GetClass(type);
                if (typeInfo != null && false)
                {
                    foreach (var tim in typeInfo.OwnMembers)
                    {
                        if (tim.MemberInfo.IsAssociation)
                        {
                            var collectionMember = ObjectSpace.CreateObject<CollectionProperty>();
                            collectionMember.BusinessObject = bob;

                            collectionMember.Aggregated = tim.MemberInfo.IsAggregated;
                            collectionMember.Name = tim.Name;
                            collectionMember.PropertyType = bos.SingleOrDefault(x => x.FullName == tim.Type.FullName);
                            if (collectionMember.PropertyType == null)
                            {
                                Debug.WriteLine("没有找到属性类型:" + tim.Type.FullName);
                                //throw new Exception("没有找到属性类型" + tim.Type.FullName);
                            }
                        }
                        else
                        {
                            var member = ObjectSpace.CreateObject<Property>();
                            member.BusinessObject = bob;
                            member.Name = tim.Name;
                            member.PropertyType = exists.SingleOrDefault(x => x.FullName == tim.Type.FullName);
                            member.BusinessObject = bob;
                            if (member.PropertyType == null)
                            {
                                Debug.WriteLine("没有找到属性类型:" + tim.Type.FullName);
                                //throw new Exception("没有找到属性类型" + tim.Type.FullName);
                            }
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("没有找到类型:" + type.FullName);
                }
            }

            //第三步,设置属性的关联属性,因为可能用到第二步中创建的属性,及,创建关系.
            foreach (var bob in bos.Where(x => !x.IsRuntimeDefine))
            {
                var cps = bob.Properties.OfType<CollectionProperty>();
                if (bob.Properties.Count > 0)
                {
                    var type = ReflectionHelper.FindType(bob.FullName);
                    var typeInfo = CaptionHelper.ApplicationModel.BOModel.GetClass(type);
                    foreach (var cp in cps)
                    {
                        var mi = typeInfo.FindMember(cp.Name);
                        cp.RelationProperty = cp.PropertyType.FindProperty(mi.MemberInfo.AssociatedMemberInfo.Name);
                    }
                }
            }

            #endregion

            ObjectSpace.CommitChanges();
        }

        public static Namespace CreateNameSpace(string ns,IObjectSpace ObjectSpace)
        {
            var find = ObjectSpace.FindObject<Namespace>(new BinaryOperator("FullName", ns));

            if (find == null)
            {
                find = ObjectSpace.CreateObject<Namespace>();
                find.Name = ns;
                //var part = ns.Split('.');
                //if (part.Length > 0)
                //{
                //    find = ObjectSpace.CreateObject<Namespace>();
                //    find.Name = part[part.Length - 1];
                //    if (part.Length > 1)
                //    {
                //        find.Parent = CreateNameSpace(string.Join(".", part.Take(part.Length - 1)), ObjectSpace);
                //    }
                //}
            }
            return find;
        }
        public static BusinessObject AddBusinessObject(Type type, string caption, Namespace nameSpace, string description, bool isRuntimeDefine,IObjectSpace ObjectSpace)
        {
            var t = ObjectSpace.FindObject<BusinessObject>(new BinaryOperator("FullName", type.FullName),true);
            if (t == null)
            {
                t = ObjectSpace.CreateObject<BusinessObject>();
                //t.DisableCreateGenericParameterValues = true;
                t.Category = nameSpace;
                t.Name = type.Name;
                t.Caption = caption;
                t.Description = description;
                t.FullName = type.FullName;
                t.DomainObjectModifier = Modifier.None;
                if (type.IsAbstract)
                    t.DomainObjectModifier = Modifier.Abstract;
                if (type.IsSealed)
                    t.DomainObjectModifier = Modifier.Sealed;

                //t.CanCustomLogic = typeof(ICustomLogic).IsAssignableFrom(type);
                
                t.IsRuntimeDefine = isRuntimeDefine;
            }
            return t;
        }


    }
}
