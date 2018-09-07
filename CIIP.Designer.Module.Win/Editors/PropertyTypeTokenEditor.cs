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
using CIIP.Designer;
using CIIP.Module.Win.Editors;

namespace CIIP.Module.Win
{
    /// <summary>
    /// 仅用于实现基类+接口控件
    /// </summary>
    [PropertyEditor(typeof(object), CIIP.Editors.PropertyTypeTokenEditor)]
    public class PropertyTypeTokenEditor : DXPropertyEditor,IComplexViewItem
    {
        
        public PropertyTypeTokenEditor(Type objectType, IModelMemberViewItem model)
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
                    //var values = (this.PropertyValue as IEnumerable).OfType<GenericParameterDefine>().Select(x => x.Name);// as System.ComponentModel.IBindingList;
                    if (PropertyValue != null)
                        control.EditValue = (PropertyValue as BusinessObjectBase).Oid.ToString(); //string.Join(control.Properties.EditValueSeparatorChar.ToString(), values);
                    else
                        control.EditValue = null;
                }
            }
        }

        protected override void WriteValueCore()
        {
            var t = control.SelectedItems.FirstOrDefault() as ImplementToken;
            PropertyValue = t?.BusinessObject;
        }

        PropertyBase _tokenService;
        PropertyBase tokenService
        {
            get
            {
                if (_tokenService == null)
                {
                    _tokenService = CurrentObject as PropertyBase;
                    if(_tokenService == null)
                    {
                        throw new Exception("CurrentObject must be is a PropertyBase!");
                    }
                }
                return _tokenService;
            }
        }

        TokenEdit control;
        protected override object CreateControlCore()
        {
            control = new TokenEdit();
            control.DoubleClick += Control_DoubleClick;
            return control;
        }

        private void Control_DoubleClick(object sender, EventArgs e)
        {

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
            i.EditMode = TokenEditMode.TokenList;
            i.ShowDropDown = true;
            i.DropDownShowMode = TokenEditDropDownShowMode.Default;
            i.EditValueSeparatorChar = ',';
            i.PopupPanelOptions.ShowMode = TokenEditPopupPanelShowMode.Default;
            i.PopupPanelOptions.ShowPopupPanel = true;
            i.PopupPanelOptions.Location = TokenEditPopupPanelLocation.Default;
            i.CheckMode = TokenEditCheckMode.Single;
            i.ValidateToken += I_ValidateToken;
            var flyoutPanel = new FlyoutPanel();
            flyoutPanel.Width = 500;
            flyoutPanel.Height = 100;
            i.PopupPanel = flyoutPanel;
            i.BeforeShowPopupPanel += I_BeforeShowPopupPanel;
            i.SelectedItemsChanged += (s, e) => { WriteValue(); };
            i.EditValueType = TokenEditValueType.String;
            if (CurrentObject != null)
            {
                var list = tokenService.Session.Query<BusinessObjectBase>().ToArray();

                i.Tokens.AddRange(
                    list.Select(x => new ImplementToken
                    {
                        Value = x.Oid.ToString(),
                        BusinessObject = x,
                        Description = x.Caption
                    }));
            }


            i.TokenClick += I_TokenClick;
            i.MaxExpandLines = 10;
            i.MinRowCount = 1;
            
        }

        private void I_ValidateToken(object sender, TokenEditValidateTokenEventArgs e)
        {
            
        }

        private void I_TokenClick(object sender, TokenEditTokenClickEventArgs e)
        {
            var info = control.CalcHitInfo(control.PointToClient(System.Windows.Forms.Control.MousePosition));
            //control.FlyoutPopupPanelController.ShowPopupPanel(info.TokenInfo);
        }
        
        private void I_BeforeShowPopupPanel(object sender, TokenEditBeforeShowPopupPanelEventArgs e)
        {
            //var value = tokenService.GenericParameterDefines.FirstOrDefault(x=>x.Name == e.Description);
            //if (value != null)
            //{
            //    var view = application.CreateDetailView(os, value, false);
            //    view.CreateControls();
            //    var fp = control.Properties.PopupPanel as FlyoutPanel;
            //    fp.Controls.Clear();
            //    fp.Controls.Add((Control)view.Control);
            //}
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