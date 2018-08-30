using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win.SystemModule;

namespace CIIP.Module.Win.Controllers
{
    public class SuppressConfirmationStartPageController : ViewController<DetailView>
    {
        public SuppressConfirmationStartPageController()
        {
            TargetViewId = "起始页";
        }

        private WinModificationsController worker;
        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            worker = Frame.GetController<WinModificationsController>();
            worker.ModificationsHandlingMode = ModificationsHandlingMode.AutoRollback;
        }
        protected override void OnDeactivated()
        {
            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            worker = null;
            base.OnDeactivated();
        }
        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (worker != null)
            {
                worker.ModificationsHandlingMode = ModificationsHandlingMode.Confirmation;
            }
        }
    }
}