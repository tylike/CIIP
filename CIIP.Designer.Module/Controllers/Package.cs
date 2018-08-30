using DevExpress.ExpressApp;

namespace CIIP.Module.Controllers
{
    public abstract class Package
    {
        protected IObjectSpace os;
        public Package(IObjectSpace os)
        {
            this.os = os;
        }
        public abstract void Create(bool deleteExist);
        public abstract void AutoRun();
    }
}
