using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.Persistent.BaseImpl;
using System;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.DC;

namespace CIIP.Module.BusinessObjects.Analysis
{
    [DefaultClassOptions]
    [NavigationItem("Reports")]
    public class 销售分析 : BaseObject
    {
        public 销售分析(Session session)
            : base(session)
        {

        }

        private string _过滤条件;
        [Size(-1)]
        [ElementTypeProperty("DataType")]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor), CriteriaOptions("DataType")]
        public string 过滤条件
        {
            get { return _过滤条件; }
            set { SetPropertyValue("过滤条件", ref _过滤条件, value); }
        }

        private Type DataType
        {
            get
            {
                return typeof(销售分析数据);
            }
        }

        [Association, DevExpress.Xpo.Aggregated]
        [XafDisplayName("数据")]
        public XPCollection<销售分析数据> Items
        {
            get { return GetCollection<销售分析数据>("Items"); }
        }
    }

    //要统计的值类型
    //1,销量
    //2.销售额
    //3.计划销量
    //4.计划销售额
    //5.预算销量
    //6.预算销售额
    //7.利润
    //8.实际收款
    //9.欠款

    //业务上，每个刑政区域，制定自己的预算与计划
    //分摊到最小单位上去，最小单位为 业务员、天（或月）的销售情况
    //统计时：
    //1,最小统计单位：
    //1.1 销售员、产品、客户、日期，为最小唯度，预算、计划，可以取销售员+产品的，日计划、预算
    //欠款：产品成交价-已收金额
    //利润：进货价-成交价

    //计划数据的分日统计。
    //

    //需要完成的内容：
    //1.定义数据模型《完成》
    //2.设置显示格式
    //3.生成测试数据

}
