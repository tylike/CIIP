using System;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using CIIP.Module.BusinessObjects.SYS;
using System.Linq;
using DevExpress.Xpo;
using System.Collections;
using DevExpress.Utils;
using DevExpress.ExpressApp;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors.Popup;

namespace CIIP.Module.Win.Editors
{
    /// <summary>
    /// 仅用于实现基类+接口控件
    /// </summary>
    [PropertyEditor(typeof(object), false)]
    public class ImplementPropertyEditor : DXPropertyEditor,IComplexViewItem
    {
        public ImplementPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model)
        {
            //ControlBindingProperty = "EditValue";
        }

        protected override void ReadValueCore()
        {
            if (Control != null)
            {
                if (CurrentObject != null)
                {
                    var values = (this.PropertyValue as IEnumerable).OfType<ImplementRelation>().Select(x => x.ImplementBusinessObject.Oid );// as System.ComponentModel.IBindingList;

                    control.EditValue = string.Join(",", values);
                }
            }
        }

        protected override void WriteValueCore()
        {
            //base.WriteValueCore();
            //if (control != null)
            //{
            //    if (CurrentObject != null)
            //    {
            //        var attachmentList = this.PropertyValue as System.ComponentModel.IBindingList;
            //        var result = String.Empty;
            //        foreach (TokenEditToken item in control.Properties.Tokens)
            //        {
            //            var fileData = attachmentList.AddNew() as DevExpress.Persistent.Base.IFileData;
            //            var archivo = (string)item.Value;
            //            fileData.LoadFromStream(System.IO.Path.GetFileName(archivo), System.IO.File.OpenRead(archivo));
            //        }
            //    }
            //}
        }
        BusinessObjectBase _tokenService;
        BusinessObjectBase tokenService
        {
            get
            {
                if (_tokenService == null)
                {
                    _tokenService = CurrentObject as BusinessObjectBase;
                    if(_tokenService == null)
                    {
                        throw new Exception("CurrentObject Must Be implement ITokenService!");
                    }
                }
                return _tokenService;
            }
        }

        TokenEditExt control;
        protected override object CreateControlCore()
        {
            control = new TokenEditExt();
            return control;
        }
        private void Control_ValidateToken(object sender, TokenEditValidateTokenEventArgs e)
        {
            e.IsValid = true;//DocFormatRegex.IsMatch(e.Description);
        }
        protected override void SetupRepositoryItem(RepositoryItem item)
        {
            base.SetupRepositoryItem(item);
            this.AllowEdit.RemoveItem("MemberIsNotReadOnly");
            RepositoryItemTokenEdit i = item as RepositoryItemTokenEdit;
            i.EditMode = TokenEditMode.Manual;
            i.ShowDropDown = true;
            i.DropDownShowMode = TokenEditDropDownShowMode.Default;
            //i.EditValueSeparatorChar = Environment.NewLine;// '\n';
            i.Separators.Add(Environment.NewLine);
            i.PopupPanelOptions.ShowMode = TokenEditPopupPanelShowMode.Default;
            i.PopupPanelOptions.ShowPopupPanel = true;
            i.PopupPanelOptions.Location = TokenEditPopupPanelLocation.Default;
            var flyoutPanel = new FlyoutPanel();
            flyoutPanel.Width = 500;
            flyoutPanel.Height = 300;
            i.PopupPanel = flyoutPanel;
            i.BeforeShowPopupPanel += I_BeforeShowPopupPanel;

            i.EditValueType = TokenEditValueType.String;
            var list = tokenService.Session.Query<BusinessObjectBase>().ToArray();

            i.Tokens.AddRange(list.Select(x => new ImplementToken { Value = x.Oid, BusinessObject = x, Description = x.Caption }));
            i.ValidateToken += Control_ValidateToken;
            i.TokenAdded += I_TokenAdded;
            i.TokenRemoved += I_TokenRemoved;
            i.DoubleClick += I_DoubleClick;
            i.TokenClick += I_TokenClick;
            i.ShowTokenGlyph = true;
            i.ShowDropDown = true;
            i.MaxExpandLines = 10;
            i.MinRowCount = 1;
            //i.CustomDrawTokenGlyph += Properties_CustomDrawTokenGlyph;
        }

        private void I_TokenClick(object sender, TokenEditTokenClickEventArgs e)
        {
            var info = control.CalcHitInfo(control.PointToClient(System.Windows.Forms.Control.MousePosition));
            control.FlyoutPopupPanelController.ShowPopupPanel(info.TokenInfo);
        }

        private void I_DoubleClick(object sender, EventArgs e)
        {
            XtraMessageBox.Show("double click!");
        }

        EditorButton btn = new EditorButton(ButtonPredefines.SpinLeft);
        void Properties_CustomDrawTokenGlyph(object sender, TokenEditCustomDrawTokenGlyphEventArgs e)
        {
            
            EditorButtonPainter painter = new SkinEditorButtonPainter(UserLookAndFeel.Default.ActiveLookAndFeel);
            EditorButtonObjectInfoArgs args = new EditorButtonObjectInfoArgs(btn, Control.Properties.Appearance);
            args.Bounds = e.Bounds;
            args.Graphics = e.Graphics;
            painter.DrawObject(args);
            e.Handled = true;
        }
        private void I_BeforeShowPopupPanel(object sender, TokenEditBeforeShowPopupPanelEventArgs e)
        {
            var value = tokenService.Implements.FirstOrDefault(x=>x.ImplementBusinessObject.Oid == (Guid)e.Value);
            if (value != null)
            {
                var view = application.CreateDetailView(os, value, false);
                view.CreateControls();
                var fp = control.Properties.PopupPanel as FlyoutPanel;
                fp.Controls.Clear();
                fp.Controls.Add((Control)view.Control);
            }
        }

        private void I_TokenRemoved(object sender, TokenEditTokenRemovedEventArgs e)
        {
            var token = tokenService.Implements.FirstOrDefault(x => x.ImplementBusinessObject.Oid == (Guid)e.Token.Value);
            tokenService.Implements.Remove(token);
            //(tokenService as IXPReceiveOnChangedFromXPPropertyDescriptor).FireChangedByXPPropertyDescriptor("Implements");
            OnValueStored();
        }

        private void I_TokenAdded(object sender, TokenEditTokenAddedEventArgs e)
        {
            var token = new ImplementRelation(tokenService.Session);
            token.ImplementBusinessObject = (e.Token as ImplementToken).BusinessObject;
            tokenService.Implements.Add(token);
            //(tokenService as IXPReceiveOnChangedFromXPPropertyDescriptor).FireChangedByXPPropertyDescriptor("Implements");
            OnValueStored();
        }

        protected override RepositoryItem CreateRepositoryItem()
        {
            return new RepositoryItemTokenEdit();
        }
        IObjectSpace os;
        XafApplication application;
        public void Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.os = objectSpace;
            this.application = application;
        }
    }


    public class TokenEditExt : TokenEdit
    {
        protected override TokenEditHandler CreateHandler()
        {
            return new TokenEditHandlerExt(this);
        }
        public PopupPanelController FlyoutPopupPanelController
        {
            get
            {
                return base.PopupPanelController;
            }
        }
    }

    public class TokenEditHandlerExt : TokenEditHandler
    {
        public TokenEditHandlerExt(TokenEdit tokenEdit) : base(tokenEdit)
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