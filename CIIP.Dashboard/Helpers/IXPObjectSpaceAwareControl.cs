using DevExpress.ExpressApp;

namespace CIIP.Win.General.DashBoard.Helpers
{
    public interface IXPObjectSpaceAwareControl {
        void UpdateDataSource(IObjectSpace objectSpace);
    }
}