using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using System.Linq;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using CIIP.Module.Controllers;

namespace CIIP.Controller {
    /// <summary>
    /// 定性ShowNavigationItemController，是否有权限可执行
    /// 注意，所有继承原生的Controller只能单根继承，多继承会出现未可预知的问题
    /// </summary>
    public partial class SecurityShowNavigationItemController : ShowNavigationItemController
    {
        public SecurityShowNavigationItemController()
        {
            TargetWindowType = WindowType.Main; 
            base.CustomInitializeItems += SecurityShowNavigationItemController_CustomInitializeItems;

            InitializeComponent();
            RegisterActions(components);
            //this.ShowNavigationItemAction.ExecuteCompleted += ShowNavigationItemAction_ExecuteCompleted;
        }

      
        
        protected override IModelViews GetModelViews()
        {
            if (Application == null)
            {
                Application = Application;
                //GepEnvironment.Instance.XafApplication;
                //return GepEnvironment.Instance.XafApplication.Model.Views;
            }
            var rst = base.GetModelViews();
            return rst;
            //return GepEnvironment.Instance.XafApplication.Model.Views;
        }
        
        void SecurityShowNavigationItemController_CustomInitializeItems(object sender, HandledEventArgs e)
        {
            if (Application.GetType().Name == "ERPDataServiceMobileApplication") return;
            e.Handled = true;
            //var all = (CaptionHelper.ApplicationModel as IModelApplicationNavigationItems).NavigationItems.Items;
            var all = (this.Application.Model as IModelApplicationNavigationItems).NavigationItems.Items;
            var s = Selected;
            if (string.IsNullOrEmpty(s))
            {
                s = all.First().Id;
            }

            foreach (IModelNavigationItem item in all[s].Items)
            {
                this.ProcessItemNew(item, this.ShowNavigationItemAction.Items);
            }
            this.OnItemsInitialized();
        }

        protected void ProcessItemNew(IModelNavigationItem item, ChoiceActionItemCollection choiceActionItems)
        {
            ChoiceActionItem item2;
            if (item.View != null)
            {
                var viewShortcut = new ViewShortcut(item.View.Id, item.ObjectKey);
                var lvcni = item as IListViewCriteriaNavigationItem;
                if (lvcni != null)
                {
                    viewShortcut.Add("Criteria", lvcni.Criteria + "");
                }
                item2 = new ChoiceActionItem(item, viewShortcut);
                item2.Active["HasRights"] = this.HasRights(item2, item.Application.Views);
            }
            else
            {
                item2 = new ChoiceActionItem(item)
                {
                    ActiveItemsBehavior = ActiveItemsBehavior.RequireActiveItems
                };
            }
            item2.Active["Visible"] = item.Visible;
            choiceActionItems.Add(item2);
            foreach (IModelNavigationItem item3 in item.Items)
            {
                this.ProcessItemNew(item3, item2.Items);
            }
            this.OnNavigationItemCreated(item, item2);
        }

        //protected override bool HasRights(ChoiceActionItem item, DevExpress.ExpressApp.Model.IModelViews views)
        //{
        //    if (SecuritySystem.CurrentUser == null)
        //        return false;
        //    var has = base.HasRights(item, views); 

        //    var data = (ViewShortcut)item.Data;

        //    if (has)
        //    {
        //        var rst = SecuritySystemHelper.IsGrantedView(data.ViewId);
        //        return rst;
        //    }

        //    return false;
        //}

        //public bool HasRight(ViewShortcut item, IModelViews views) {
        //    if (SecuritySystem.CurrentUser == null)
        //        return false;
        //    var has = OldHasRightsCore(item, views);

        //    var data = item;

        //    if (has)
        //    {
        //        var rst = SecuritySystemHelper.IsGrantedView(data.ViewId);
        //        return rst;
        //    }

        //    return false;
        //}

        //protected bool OldHasRightsCore(ViewShortcut item, IModelViews views)
        //{
        //    ViewShortcut data = item;
        //    IModelView view = views[data.ViewId];
        //    if (view == null)
        //    {
        //        throw new ArgumentException(string.Format("Cannot find the '{0}' view specified by the shortcut: {1}", data.ViewId, data.ToString()));
        //    }
        //    Type type = (view is IModelObjectView) ? ((IModelObjectView)view).ModelClass.TypeInfo.Type : null;
        //    if (type != null)
        //    {
        //        if (!string.IsNullOrEmpty(data.ObjectKey) && !data.ObjectKey.StartsWith("@"))
        //        {
        //            try
        //            {
        //                using (IObjectSpace space = this.CreateObjectSpace(type))
        //                {
        //                    object objectByKey = space.GetObjectByKey(type, space.GetObjectKey(type, data.ObjectKey));
        //                    return (DataManipulationRight.CanRead(type, null, objectByKey, null, space) && DataManipulationRight.CanNavigate(type, objectByKey, space));
        //                }
        //            }
        //            catch
        //            {
        //                goto Label_00D2;
        //            }
        //        }
        //        return DataManipulationRight.CanNavigate(type, null, null);
        //    }
        //Label_00D2:
        //    return true;



        //}


        static string selected;
        public static string Selected
        {
            get; set;
            //get
            //{
            //    if (GepEnvironment.Instance.XafApplication is WinApplication)
            //    {
            //        return selected;
            //    }
            //    else
            //    {
            //        return "" + HttpContext.Current.Session["SubSystem"];
            //    }
            //}
            //set
            //{
            //    if (GepEnvironment.Instance.XafApplication is WinApplication)
            //    {
            //        selected = value;
            //    }
            //    else
            //    {
            //        HttpContext.Current.Session["SubSystem"] = value;
            //    }
            //}
        }

        protected override void ShowNavigationItem(SingleChoiceActionExecuteEventArgs args)
        {
            base.ShowNavigationItem(args);
        }

        /// <summary>
        /// 重新设置导航信息，防止用户切换组织时，没有StartupNavigationItem     
        /// 原this.RecreateNavigationItems方法中有bug，即当StartupNavigationItem没有时，该方法会出错
        /// </summary>
        public void ReCreateNavigationItems()
        {
            //this.RecreateNavigationItems();
            try
            {
                this.InitializeItems();
            }
            catch (Exception e)
            {
                throw e;
            }
            ChoiceActionItem selectItem = ShowNavigationItemAction.SelectedItem;
            ViewShortcut data = null;
            if (selectItem == null)
            {
                if (ShowNavigationItemAction.Items.Count > 0)
                {
                    selectItem = ShowNavigationItemAction.Items[0];
                    //ShowNavigationItemAction.SelectedItem = selectItem;
                    data = selectItem.Data as ViewShortcut;
                    if (data == null)
                    {
                        if (selectItem.Items.Count > 0)
                        {
                            data = selectItem.Items[0].Data as ViewShortcut;
                        }

                    }
                }
                else
                {

                }
            }
            if (data != null)
            {
                this.UpdateSelectedItem(data);
            }
        }

        public static event EventHandler<IObjectSpace> AutoRunFunctions;

        protected override void OnActivated()
        {
            if (Module.ERPModule.IsNewVersion && !Module.ERPModule.IsSafeMode)
            {
                var os = Application.CreateObjectSpace();
                ModelCustomizedViewController.UpdelModel(Application, os);
                DataInitializeWindowController.CreateSystemTypes(os, Application.Model, false);
                DataInitializeWindowController.CreateWMS(os, false);
                
                os.CommitChanges();
            }

            base.OnActivated();
            Frame.TemplateChanged += Frame_TemplateChanged;
            //CustomShowNavigationItem += NavigationController_CustomShowNavigationItem;
        }

        private void Frame_TemplateChanged(object sender, EventArgs e)
        {
            TemplateChanged();
        }

        protected virtual void TemplateChanged()
        {
            
        }

        protected override void OnDeactivated() {
            //CustomShowNavigationItem -= NavigationController_CustomShowNavigationItem;
            Frame.TemplateChanged -= Frame_TemplateChanged;
            base.OnDeactivated();
        }

        protected override void OnItemsInitialized()
        {
            base.OnItemsInitialized();
            
            ChoiceActionItem item2;
            //if (item.View != null)
            //{
            //    item2 = new ChoiceActionItem(item, new ViewShortcut(item.View.Id, item.ObjectKey));
            //    item2.Active["HasRights"] = this.HasRights(item2, item.Application.Views);
            //}
            //else
            //{
            //    item2 = new ChoiceActionItem(item)
            //    {
            //        ActiveItemsBehavior = ActiveItemsBehavior.RequireActiveItems
            //    };
            //}
            //item2.Active["Visible"] = item.Visible;
            //choiceActionItems.Add(item2);
            //foreach (IModelNavigationItem item3 in item.Items)
            //{
            //    this.ProcessItem(item3, item2.Items);
            //}
            //this.OnNavigationItemCreated(item, item2);
        }
        
        //private void NavigationController_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e) {
        //    if (e.ActionArguments.SelectedChoiceActionItem == null) return;
        //    var viewShortcut = e.ActionArguments.SelectedChoiceActionItem.Data as ViewShortcut;
        //    if (viewShortcut != null && e.Handled == false) {
        //        ReplaceParameter(viewShortcut);
        //        e.ActionArguments.ShowViewParameters.CreatedView = Application.ProcessShortcut(viewShortcut);
        //        e.ActionArguments.ShowViewParameters.TargetWindow = TargetWindow.Current;
        //    }
        //}
        ///// <summary>
        ///// 替换自定义的ReadOnlyParameter
        ///// </summary>
        ///// <param name="viewShortcut"></param>
        //private void ReplaceParameter(ViewShortcut viewShortcut) {
        //    if (viewShortcut.ObjectKey.StartsWith("@")) {
        //        IParameter parameter = ParametersFactory.CreateParameter(viewShortcut.ObjectKey.Substring(1));
        //        viewShortcut.ObjectKey = parameter.CurrentValue.ToString();
        //    }
        //}
    }
}
