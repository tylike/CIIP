using System;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Registrator;
using System.Drawing;

namespace CIIP.Module.Win.Editors
{
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

}