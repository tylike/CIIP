using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CIIP.Designer
{
    public class RuleRange:BaseObject
    {
        public RuleRange(Session s):base(s)
        {

        }

        private decimal _Begin;
        [XafDisplayName("¿ªÊ¼")]
        public decimal Begin
        {
            get { return _Begin; }
            set { SetPropertyValue("Begin", ref _Begin, value); }
        }

        private decimal _End;
        [XafDisplayName("½áÊø")]        
        public decimal End
        {
            get { return _End; }
            set { SetPropertyValue("End", ref _End, value); }
        }
    }
}