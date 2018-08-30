using CIIP.Module.BusinessObjects.SYS.Logic;
using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.SYS
{
    [XafDisplayName("¿ØÖÆÆ÷")]
    public abstract class RuntimeController : CodeFile
    {
        public RuntimeController(Session s) : base(s)
        {

        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            var temp = @"

";
            this.Code = new CsharpCode(temp, this);


        }

        public override string GetFileName()
        {
            return this.GetType().Name + "_" + this.Oid.ToString();
        }
    }
}