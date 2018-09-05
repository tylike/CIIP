using System;
using System.IO;
using System.Reflection;

namespace CIIP.Module.BusinessObjects.SYS
{
    public class BusinessBuilder
    {
        public static Version GetVersion(FileInfo file)
        {
            if (file != null && file.Exists)
            {
                try
                {
                    return AssemblyName.GetAssemblyName(file.FullName).Version;
                }
                catch
                {
                }
            }
            return null;
        }


    }
}