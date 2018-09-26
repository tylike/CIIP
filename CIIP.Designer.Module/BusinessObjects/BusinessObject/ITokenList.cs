using System.Collections.Generic;

namespace CIIP.Designer
{
    public interface ITokenService
    {
        IEnumerable<TokenItem> GetTokenList(string propertyName);
    }

    public interface IToken
    {
        string Name { get;  }
        string Oid { get;  }
    }

    public class TokenItem {
        public object Value { get; set; }
        public string Description { get; set; }
    }
}