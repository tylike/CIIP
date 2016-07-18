using System;

namespace CIIP.Module.BusinessObjects.SYS
{
    public interface IDocumentProvider
    {
        Guid GetDocumentGuid();

        string GetFileName();

        string GetCode();                
    }
}