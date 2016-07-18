using System;
using DevExpress.Xpo;

namespace CIIP.FormCode
{
    public class SequenceGenerator
    {
        // Fields
        private static object flag = new object();

        // Methods
        public static long GenerateNextSequence(string typeName)
        {
            using (var work = new UnitOfWork(DefaultDataLayer))
            {
                object obj2 = work.ExecuteScalar($"exec GetAutoNumber N'{typeName}'");
                if (obj2 is long)
                {
                    return (long)obj2;
                }
                return 1L;
            }
        }

        // Properties
        public static IDataLayer DefaultDataLayer
        {
            get; set;
        }
    }
}