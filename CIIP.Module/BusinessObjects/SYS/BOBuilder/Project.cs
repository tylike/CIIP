using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects.SYS
{
    public class Project : NameObject
    {
        public Project(Session s) : base(s)
        {
        }

        public string ProjectPath
        {
            get { return GetPropertyValue<string>(nameof(ProjectPath)); }
            set { SetPropertyValue(nameof(ProjectPath), value); }
        }

    }
}