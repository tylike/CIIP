using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    public class SystemSetting : SimpleObject
    {
        public SystemSetting(Session s) : base(s)
        {

        }

        static SystemSetting _setting;
        public static SystemSetting GetSetting(Session s)
        {
            if (_setting == null)
            {
                _setting = s.FindObject<SystemSetting>(null);
            }
            return _setting;
        }

        private bool _启用价格目录;
        [ToolTip("开启后，在采购、销售、报价时，默认查找目录中的产品价格!")]
        public bool 启用价格目录
        {
            get { return _启用价格目录; }
            set { SetPropertyValue("启用价格目录", ref _启用价格目录, value); }
        }


        private bool _仅从价格目录;
        [ToolTip("当启用价格目录与本选项同时开启，在制单时，没有找到价格目录中的产品，则不可以继续做单，即没有产品可选!")]
        public bool 仅从价格目录
        {
            get { return _仅从价格目录; }
            set { SetPropertyValue("仅从价格目录", ref _仅从价格目录, value); }
        }



    }
}