using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Popup;

namespace CIIP.Module.Win.Editors
{
    public class TokenEditPopupFormV2 : TokenEditPopupForm
    {
        public TokenEditPopupFormV2(TokenEdit edit)
            : base(edit)
        {
        }

        protected override ITokenEditDropDownControl CreateDropDownControl()
        {
            var control = new DefaultTokenEditDropDownControlV2();
            return control;
        }
    }

}