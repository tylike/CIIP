using System;

namespace CIIP.Designer
{
    public interface IDocumentProvider
    {
        Guid GetDocumentGuid();

        string GetFileName();

        string GetCode();                
    }
}