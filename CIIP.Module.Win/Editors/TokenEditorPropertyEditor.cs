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

namespace CIIP.Module.Win.Editors
{
    public class ImplementToken : TokenEditToken
    {
        public BusinessObjectBase BusinessObject { get; set; }
    }
    [PropertyEditor(typeof(object), false)]
    public class TokenEditorPropertyEditor : DXPropertyEditor,IComplexViewItem
    {
        public TokenEditorPropertyEditor(Type objectType, IModelMemberViewItem model)
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
                    var values = (this.PropertyValue as IEnumerable).OfType<ImplementRelation>().Select(x => x.ImplementBusinessObject.Oid);// as System.ComponentModel.IBindingList;
                    control.EditValue = string.Join(control.Properties.EditValueSeparatorChar.ToString(), values);
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

        TokenEdit control;
        protected override object CreateControlCore()
        {
            control = new TokenEdit();
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
            i.EditValueSeparatorChar = ',';
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
}