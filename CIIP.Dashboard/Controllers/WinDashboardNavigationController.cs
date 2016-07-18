using System;
using DevExpress.ExpressApp;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp.Win.Templates.ActionContainers;
using DevExpress.ExpressApp.Actions;

namespace CIIP.Win.General.DashBoard.Controllers {
    public partial class WinDashboardNavigationController : DashboardNavigationController {
        NavBarNavigationControl _navBarNavigationControl;

        public WinDashboardNavigationController() {
            TargetWindowType = WindowType.Main;
        }

        protected override void OnActivated() {
            base.OnActivated();
            Window.TemplateChanged += window_TemplateChanged;
        }

        protected override void OnDeactivated() {
            Window.TemplateChanged -= window_TemplateChanged;
            if (_navBarNavigationControl != null) {
                _navBarNavigationControl.Items.CollectionChanged -= Items_CollectionChanged;
                _navBarNavigationControl = null;
            }
            base.OnDeactivated();
        }

        private void window_TemplateChanged(object sender, EventArgs e) {
            if (Window.Template != null)
                foreach (NavigationActionContainer actionContainer in Window.Template.GetContainers().OfType<NavigationActionContainer>()) {
                    _navBarNavigationControl = actionContainer.NavigationControl as NavBarNavigationControl;
                    if (_navBarNavigationControl != null) {
                        _navBarNavigationControl.Items.CollectionChanged += Items_CollectionChanged;
                        UpdateNavigationImages();
                    }
                }
        }

        void Items_CollectionChanged(object sender, CollectionChangeEventArgs e) {
            UpdateNavigationImages();
        }

        public override void UpdateNavigationImages() {
            var dashboardActions = _navBarNavigationControl.ActionItemToItemLinkMap.Keys.Intersect(DashboardActions.Keys);
            foreach (var action in dashboardActions)
                UpdateActionIcon(action);
        }

        private void UpdateActionIcon(ChoiceActionItem action) {
            var icon = DashboardActions[action].Icon;
            if (icon != null)
                _navBarNavigationControl.ActionItemToItemLinkMap[action].Item.LargeImage = icon;
        }
    }
}
