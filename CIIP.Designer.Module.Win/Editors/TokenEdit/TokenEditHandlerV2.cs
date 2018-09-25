using DevExpress.XtraEditors;

namespace CIIP.Module.Win.Editors
{
    public class TokenEditHandlerV2 : TokenEditHandler
    {
        public TokenEditHandlerV2(TokenEdit tokenEdit) : base(tokenEdit)
        {
        }

        public override void OnMouseHover()
        {
            if (!this.OwnerEdit.ReadOnly)
            {
                var info = this.OwnerEdit.CalcHitInfo(this.GetMousePos());
                if (info.InLink)
                {
                    //this.OwnerEdit.Properties.RaiseTokenMouseHover(TokenEditTokenBasedEventArgsBase.Create<TokenEditTokenMouseHoverEventArgs>(info.TokenInfo));
                }
            }
        }
    }

}