using System;
using System.Collections.Generic;
using System.Linq;
using CIIP.Win.General.DashBoard.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;

namespace CIIP.Win.General.DashBoard.Controllers {
    public abstract partial class DashboardNavigationController : WindowController, IModelExtender {
        Dictionary<ChoiceActionItem, DashboardDefinition> _dashboardActions;
        ShowNavigationItemController _showNavigationItemController;

        protected DashboardNavigationController() {
            TargetWindowType = WindowType.Main;
        }

        protected Dictionary<ChoiceActionItem, DashboardDefinition> DashboardActions {
            get { return _dashboardActions ?? (_dashboardActions = new Dictionary<ChoiceActionItem, DashboardDefinition>()); }
        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            extenders.Add<IModelOptionsDashboard, IModelOptionsDashboardNavigation>();
        }

        protected override void OnDeactivated() {
            UnsubscribeFromEvents();
            base.OnDeactivated();
        }

        void SubscribeToEvents() {
            _showNavigationItemController = Frame.GetController<ShowNavigationItemController>();
            if (_showNavigationItemController != null)
                _showNavigationItemController.ItemsInitialized += ShowNavigationItemControllerItemsInitialized;
        }

        void UnsubscribeFromEvents() {
            if (_showNavigationItemController != null) {
                _showNavigationItemController.ItemsInitialized -= ShowNavigationItemControllerItemsInitialized;
                _showNavigationItemController = null;
            }
        }

        protected override void OnFrameAssigned() {
            UnsubscribeFromEvents();
            base.OnFrameAssigned();
            SubscribeToEvents();
        }

        void ShowNavigationItemControllerItemsInitialized(object sender, EventArgs e) {
            IModelView view = Application.FindModelView(Application.FindListViewId(typeof(DashboardDefinition)));
            var options = ((IModelOptionsDashboards)Application.Model.Options);
            var navAction = ((ShowNavigationItemController)sender).ShowNavigationItemAction;
            var currentGroup = navAction.Items.FirstOrDefault()?.Model.Parent.Parent.GetValue<string>("Id");

            var dashboardOptions = ((IModelOptionsDashboardNavigation)options.Dashboards);
            if (currentGroup == "Reports" && dashboardOptions.DashboardsInGroup) {
                ReloadDashboardActions();
                var actions = new List<ChoiceActionItem>();
                if (DashboardActions.Count > 0) {
                    var dashboardGroup = GetGroupFromActions(navAction, dashboardOptions.DashboardGroupCaption);
                    if (dashboardGroup == null)
                    {
                        dashboardGroup = new ChoiceActionItem(dashboardOptions.DashboardGroupCaption, null)
                        {
                            ImageName = "BO_DashboardDefinition"
                        };
                        var reports = navAction.Items;//.FirstOrDefault(x => x.Id == "Dashboard");

                        //var dashboard = reports?.Items.FirstOrDefault(x => x.Id == "Dashboard");
                        reports.Add(dashboardGroup);

                        //((ShowNavigationItemController)sender).ShowNavigationItemAction.Items.Add(dashboardGroup);
                    }
                    while (dashboardGroup.Items.Count != 0) {
                        ChoiceActionItem item = dashboardGroup.Items[0];
                        dashboardGroup.Items.Remove(item);
                        actions.Add(item);
                    }
                    foreach (ChoiceActionItem action in DashboardActions.Keys) {
                        action.Active["HasRights"] = HasRights(action, view);
                        actions.Add(action);
                    }
                    foreach (ChoiceActionItem action in actions.OrderBy(action => action.Model.Index))
                        dashboardGroup.Items.Add(action);

                }
            }
        }

        protected virtual bool HasRights(ChoiceActionItem item, IModelView view) {
            var data = (ViewShortcut)item.Data;
            if (view == null) {
                throw new ArgumentException(string.Format("Cannot find the '{0}' view specified by the shortcut: {1}",
                                                          data.ViewId, data));
            }
            Type type = (view is IModelObjectView) ? ((IModelObjectView)view).ModelClass.TypeInfo.Type : null;
            if (type != null) {
                if (!string.IsNullOrEmpty(data.ObjectKey) && !data.ObjectKey.StartsWith("@")) {
                    try {
                        using (IObjectSpace space = CreateObjectSpace()) {
                            object objectByKey = space.GetObjectByKey(type, space.GetObjectKey(type, data.ObjectKey));
                            return (DataManipulationRight.CanRead(type, null, objectByKey, null, space) &&
                                    DataManipulationRight.CanNavigate(type, objectByKey, space));
                        }
                    } catch {
                        return true;
                    }
                }
                return DataManipulationRight.CanNavigate(type, null, null);
            }
            return true;
        }

        protected virtual IObjectSpace CreateObjectSpace() {
            return Application.CreateObjectSpace();
        }

        public virtual void UpdateNavigationImages() {
        }

        void ReloadDashboardActions() {
            DashboardActions.Clear();
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            IOrderedEnumerable<DashboardDefinition> templates =
                objectSpace.GetObjects<DashboardDefinition>().Where(t => t.Active).OrderBy(i => i.Index);
            foreach (DashboardDefinition template in templates) {
                var action = new ChoiceActionItem(
                    template.Oid.ToString(),
                    template.Name,
                    new ViewShortcut(typeof(DashboardDefinition), template.Oid.ToString(), "DashboardViewer_DetailView")) {
                        ImageName = "BO_DashboardDefinition"
                    };
                action.Model.Index = template.Index;
                DashboardActions.Add(action, template);
            }
        }

        public void RecreateNavigationItems() {
            _showNavigationItemController.RecreateNavigationItems();
        }

        ChoiceActionItem GetGroupFromActions(SingleChoiceAction action, String name) {
            return action.Items.FirstOrDefault(item => item.Caption.Equals(name));
        }
    }
}
