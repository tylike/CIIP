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
using DevExpress.XtraEditors.Registrator;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraEditors.ViewInfo;
using CIIP.Designer;

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

        TokenEditV2 control;
        protected override object CreateControlCore()
        {
            control = new TokenEditV2();
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
            var i = item as RepositoryItemTokenEditV2;
            i.EditMode = TokenEditMode.TokenList;
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
            i.UseCustomFilter = true;
            i.CustomFilterHandler += I_CustomFilterHandler;
            i.EditValueType = TokenEditValueType.String;
            var list = tokenService.Session.Query<BusinessObjectBase>().Where(x => x.DomainObjectModifier != BusinessObjectModifier.Sealed).ToArray();

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

        private void I_CustomFilterHandler(object sender, TokenEditFilterEventArgs e)
        {

            var selected = control.SelectedItems.OfType<ImplementToken>();
            var hasClass = selected.Any(x => x.BusinessObject is BusinessObject);
            if (hasClass)
            {
                e.IsValidToken = (e.Token as ImplementToken).BusinessObject is Interface;
            }
            else
            {
                e.IsValidToken = true;
            }
        }

        private void I_TokenClick(object sender, TokenEditTokenClickEventArgs e)
        {
            try
            {
                var info = control.CalcHitInfo(control.PointToClient(System.Windows.Forms.Control.MousePosition));
                var ti = info?.TokenInfo;
                if (ti != null)
                    control.FlyoutPopupPanelController.ShowPopupPanel(info.TokenInfo);
            }
            catch (Exception ex)
            {
                throw;
            }
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

    [UserRepositoryItem("RegisterTokenEditV2")]
    public class RepositoryItemTokenEditV2 : RepositoryItemTokenEdit
    {
        static RepositoryItemTokenEditV2()
        {
            RegisterTokenEditV2();
        }

        public bool UseCustomFilter { get; set; }

        public EventHandler<TokenEditFilterEventArgs> CustomFilterTokens { get; set; }

        public const string CustomEditName = "TokenEditV2";

        public RepositoryItemTokenEditV2()
        {
        }

        public override string EditorTypeName
        {
            get
            {
                return CustomEditName;
            }
        }

        public event EventHandler<TokenEditFilterEventArgs> CustomFilterHandler;

        public static void RegisterTokenEditV2()
        {
            Image img = null;
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(CustomEditName, typeof(TokenEditV2), typeof(RepositoryItemTokenEditV2), typeof(TokenEditViewInfoV2), new TokenEditPainter(), true, img));
        }

        public override void Assign(RepositoryItem item)
        {
            BeginUpdate();
            try
            {
                base.Assign(item);
                RepositoryItemTokenEditV2 source = item as RepositoryItemTokenEditV2;
                if (source == null) return;
                this.UseCustomFilter = source.UseCustomFilter;
                this.CustomFilterTokens = source.CustomFilterTokens;
                //
            }
            finally
            {
                EndUpdate();
            }
        }

        internal void OnCustomFilterText(TokenEditFilterEventArgs args)
        {
            CustomFilterHandler?.Invoke(this, args);
        }
    }

    public class TokenEditViewInfoV2 : TokenEditViewInfo
    {
        public TokenEditViewInfoV2(RepositoryItem item) : base(item)
        {
        }
    }

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

    public class DefaultTokenEditDropDownControlV2 : DefaultTokenEditDropDownControl
    {
        public DefaultTokenEditDropDownControlV2()
            : base()
        {
        }

        private IList GetCustomFilterSourceCore()
        {
            TokenEditTokenCollection tokCol = Properties.Tokens;
            TokenEditSelectedItemCollection selCol = Properties.SelectedItems;
            if (selCol.Count == 0) return tokCol;
            HashSet<int> indices = new HashSet<int>();
            for (int i = 0; i < selCol.Count; i++)
            {
                TokenEditToken tok = selCol[i];
                indices.Add(Properties.Tokens.IndexOf(tok));
            }
            List<TokenEditToken> list = new List<TokenEditToken>(tokCol.Count);
            for (int i = 0; i < tokCol.Count; i++)
            {
                if (indices.Contains(i)) continue;
                list.Add(tokCol[i]);
            }
            return list;
        }

        private IList GetCustomFilterSource()
        {
            IList list = GetCustomFilterSourceCore();
            List<TokenEditToken> newList = new List<TokenEditToken>();
            var edit = this.OwnerEdit as TokenEditV2;
            for (int i = 0; i < list.Count; i++)
            {
                TokenEditFilterEventArgs args = new TokenEditFilterEventArgs(list[i] as TokenEditToken);
                edit.OnCustomFilterText(args);
                if (args.IsValidToken)
                {
                    newList.Add(list[i] as TokenEditToken);
                }
            }
            return newList;
        }

        public override void SetDataSource(object obj)
        {
            if (((OwnerEdit as TokenEditV2).Properties as RepositoryItemTokenEditV2).UseCustomFilter)
                base.SetDataSource(GetCustomFilterSource());
            else
                base.SetDataSource(obj);
        }
    }

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

    public class TokenEditFilterEventArgs : EventArgs
    {
        // Fields...
        private bool _IsValidToken;
        private TokenEditToken _Token;
        public TokenEditToken Token
        {
            get { return _Token; }
            set
            {
                _Token = value;
            }
        }

        public bool IsValidToken
        {
            get { return _IsValidToken; }
            set
            {
                _IsValidToken = value;
            }
        }

        public TokenEditFilterEventArgs(TokenEditToken token)
        {
            this.Token = token;
            IsValidToken = true;
        }
    }

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