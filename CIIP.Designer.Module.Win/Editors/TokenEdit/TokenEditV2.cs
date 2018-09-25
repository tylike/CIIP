using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Popup;
using System.ComponentModel;

namespace CIIP.Module.Win.Editors
{
    public class TokenEditV2 : TokenEdit
    {
        protected override TokenEditHandler CreateHandler()
        {
            return new TokenEditHandlerV2(this);
        }
        public PopupPanelController FlyoutPopupPanelController
        {
            get
            {
                return base.PopupPanelController;
            }
        }

        internal void OnCustomFilterText(TokenEditFilterEventArgs args)
        {
            this.Properties.OnCustomFilterText(args);
        }

        static TokenEditV2()
        {
            RepositoryItemTokenEditV2.RegisterTokenEditV2();
        }

        public TokenEditV2()
        {
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new RepositoryItemTokenEditV2 Properties
        {
            get
            {
                return base.Properties as RepositoryItemTokenEditV2;
            }
        }

        public override string EditorTypeName
        {
            get
            {
                return RepositoryItemTokenEditV2.CustomEditName;
            }
        }

        protected override TokenEditPopupForm CreatePopupForm()
        {
            return new TokenEditPopupFormV2(this);
        }

        protected override BaseTokenEditPopupController CreatePopupController()
        {
            if (Properties.EditMode == TokenEditMode.TokenList || Properties.EditMode == TokenEditMode.Default) return new TokenEditTokenListPopupControllerV2(this);
            return new TokenEditManualModePopupController(this);
        }
    }

}