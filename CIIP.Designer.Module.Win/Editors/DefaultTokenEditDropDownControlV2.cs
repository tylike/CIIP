using DevExpress.XtraEditors;
using System.Collections;
using DevExpress.XtraEditors.Popup;
using System.Collections.Generic;

namespace CIIP.Module.Win.Editors
{
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

}