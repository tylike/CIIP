using System;
using DevExpress.XtraEditors;

namespace CIIP.Module.Win.Editors
{
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

}