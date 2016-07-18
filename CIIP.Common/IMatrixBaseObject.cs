using DevExpress.Xpo;

namespace CIIP
{
    [NonPersistent]
    public abstract class IMatrixBaseObject : XPLiteObject
    {
        public IMatrixBaseObject(Session s) : base(s)
        {

        }
    }
}