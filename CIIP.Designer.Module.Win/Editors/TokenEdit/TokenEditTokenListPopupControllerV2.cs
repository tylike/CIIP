using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Popup;

namespace CIIP.Module.Win.Editors
{
    public class TokenEditTokenListPopupControllerV2 : TokenEditTokenListPopupController
    {
        public TokenEditTokenListPopupControllerV2(TokenEdit edit)
            : base(edit)
        {
        }
        public override void UpdateFilter(string filter)
        {
            if (((OwnerEdit as TokenEditV2).Properties as RepositoryItemTokenEditV2).UseCustomFilter)
                filter = string.Empty;
            base.UpdateFilter(filter);
        }
    }

}