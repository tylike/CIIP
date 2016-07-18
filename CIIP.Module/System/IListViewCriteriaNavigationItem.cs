using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.SystemModule;

namespace CIIP
{
    public interface IListViewCriteriaNavigationItem 
    {
        [Description("指定打开列表使用的过滤条件")]
        string Criteria { get; set; }

        bool AutoGenerate { get; set; }
    }
}
