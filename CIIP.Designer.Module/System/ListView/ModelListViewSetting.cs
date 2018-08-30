using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;

namespace CIIP
{
    [DomainComponent]
    [XafDisplayName("列表设置")]
    public class ModelListViewSetting
    {

        public bool 禁止详细视图 { get; set; }
        public bool 显示页角 { get; set; }

        public bool 编辑 { get; set; }

        public NewItemRowPosition 新建位置 { get; set; }

    }
}