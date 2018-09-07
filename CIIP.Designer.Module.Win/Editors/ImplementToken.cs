using DevExpress.XtraEditors;
using CIIP.Module.BusinessObjects.SYS;
using CIIP.Designer;

namespace CIIP.Module.Win.Editors
{
    public class ImplementToken : TokenEditToken
    {
        public BusinessObjectBase BusinessObject { get; set; }
    }
}