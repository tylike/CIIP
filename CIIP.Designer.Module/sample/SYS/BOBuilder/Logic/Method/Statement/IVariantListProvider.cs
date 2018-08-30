using System.Collections.Generic;

namespace IMatrix.ERP.Module.BusinessObjects.SYS.Logic
{
    public interface IVariantListProvider
    {
        IEnumerable<string> GetNames(string propertyName,string inputed);
    }
}