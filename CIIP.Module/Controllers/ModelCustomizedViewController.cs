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
using CIIP.CodeFirstView;
using CIIP.Module.BusinessObjects.SYS;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using CIIP;

namespace CIIP.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ModelCustomizedViewController : WindowController
    {
        public ModelCustomizedViewController()
        {
            InitializeComponent();
            TargetWindowType = WindowType.Main;
            this.更新模型.Active["hidden"] = false;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        
        private void 更新模型_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var os = Application.CreateObjectSpace();
            UpdelModel(Application, os);
        }

        public static void UpdelModel(XafApplication Application, IObjectSpace os)
        {
            //非持久化的列表视图,都设置为client,不能用server mode,否则就报错了.

            #region 非serverMode

            foreach (var item in Application.Model.Views.OfType<IModelListView>())
            {
                if (!item.ModelClass.TypeInfo.IsPersistent)
                    item.DataAccessMode = CollectionSourceDataAccessMode.Client;
            }

            #endregion

            //更新代码中定义的视图
            new ViewsGeneratorUpdater().UpdateNode(Application.Model.Views as ModelNode);

            //在导航设置上面设置了自动生成生成项目的,在这里生成
            //统一设置为按状态\按日期生成导航结点.

            #region 自动生成导航

            var items = Application.Model as IModelApplicationNavigationItems;

            //遍历子系统
            foreach (var item in items.NavigationItems.Items)
            {
                //遍历子系统菜单
                foreach (IModelNavigationItem l2 in item.Items)
                {
                    var autoGenerate = l2 as IListViewCriteriaNavigationItem;
                    if (autoGenerate != null && autoGenerate.AutoGenerate && l2.View!=null)
                    {
                        l2.ChildItemsDisplayStyle =
                            DevExpress.ExpressApp.Templates.ActionContainers.ItemsDisplayStyle.List;
                        l2.Items.ClearNodes();
                        AddNavigationItem(l2, "本月单据", l2.View.Id, "IsThisMonth(下单日期)");

                        AddNavigationItem(l2, "新建单据", l2.View.Id, "状态.Caption='新建'");
                        AddNavigationItem(l2, "已审单据", l2.View.Id, "状态.Caption='已审核'");

                        AddNavigationItem(l2, "本日单据", l2.View.Id, "下单日期>=LocalDateTimeToday()");
                        AddNavigationItem(l2, "昨日单据", l2.View.Id,
                            "下单日期>=LocalDateTimeToday() and 下单日期<=LocalDateTimeToday()");
                        AddNavigationItem(l2, "本周单据", l2.View.Id, "IsThisWeek(下单日期)");
                        AddNavigationItem(l2, "上月单据", l2.View.Id, "DateDiffMonth(下单日期, Now() ) == 1");
                        AddNavigationItem(l2, "本年单据", l2.View.Id, "IsThisYear(下单日期)");
                        AddNavigationItem(l2, "上年单据", l2.View.Id, "DateDiffYear(下单日期, Now() ) == 1");

                        AddNavigationItem(l2, "已完成单据", l2.View.Id, "状态.Caption='已完成'");
                        AddNavigationItem(l2, "已关闭单据", l2.View.Id, "状态.Caption='已关闭'");
                        AddNavigationItem(l2, "已作废单据", l2.View.Id, "状态.Caption='已作废'");

                        AddNavigationItem(l2, "所有单据", l2.View.Id, "");
                    }
                }
            }

            #endregion

            UpdateNavigation(Application, os);
        }

        public static void AddNavigationItem(IModelNavigationItem parent, string caption, string id, string criteria)
        {
            var order = parent.Items.AddNode<IModelNavigationItem>(id + caption);
            order.View = parent.View;
            order.Caption = caption;
            var criteriaNav = order as IListViewCriteriaNavigationItem;
            criteriaNav.Criteria = criteria;
            order.Index = parent.Items.Count;
        }

        //更新导航栏
        private static void UpdateNavigation(XafApplication Application,IObjectSpace os)
        {
            //运行时生成的业务对象，设置了导航项目的，则为需要处理的
            if (CaptionHelper.ApplicationModel == null)
                CaptionHelper.Setup(Application.Model);

            var bos = os.GetObjects<BusinessObject>(CriteriaOperator.Parse("IsRuntimeDefine && NavigationItem is not null"));

            foreach (var businessObject in bos)
            {
                if (businessObject.NavigationItem != null)
                {
                    if (businessObject.NavigationItem.ModelItem != null)
                    {
                        var exist = businessObject.NavigationItem.ModelItem.Items.Any(x => x.Id == businessObject.FullName);
                        if (!exist)
                        {
                            var type = ReflectionHelper.FindType(businessObject.FullName);
                            if (type != null)
                            {
                                var view = businessObject.NavigationItem.ModelItem.Items.AddNode<IModelNavigationItem>(businessObject.FullName);
                                view.View = Application.Model.BOModel.GetClass(type).DefaultListView;
                            }
                        }
                        
                    }
                }
            }

        }
    }
}
