using System.Collections.Generic;

namespace CIIP
{
    public interface IVariantListProvider
    {
        IEnumerable<string> GetNames(string propertyName,string inputed);
    }
}