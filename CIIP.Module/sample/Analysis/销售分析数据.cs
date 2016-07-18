using CIIP.Module.BusinessObjects.Product;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.Analysis
{
    [DeferredDeletion(false), OptimisticLocking(false)]
    public class 销售分析数据 : BaseObject
    {
        public 销售分析数据(Session s) : base(s)
        {

        }

        private 销售分析 _销售分析;

        [Association]
        public 销售分析 销售分析
        {
            get { return _销售分析; }
            set { SetPropertyValue("销售分析", ref _销售分析, value); }
        }


        private 客户 _客户;

        public 客户 客户
        {
            get { return _客户; }
            set { SetPropertyValue("客户", ref _客户, value); }
        }



        private 产品 _产品;

        [XafDisplayName("产品")]
        public 产品 产品
        {
            get { return _产品; }
            set { SetPropertyValue("产品", ref _产品, value); }
        }

        private 行政区域 _销售区域;

        public 行政区域 销售区域
        {
            get { return _销售区域; }
            set { SetPropertyValue("销售区域", ref _销售区域, value); }
        }

        private 员工 _业务员工;
        public 员工 业务员工
        {
            get { return _业务员工; }
            set { SetPropertyValue("业务员工", ref _业务员工, value); }
        }
        
        private 日期维度 _日期维度;
        public 日期维度 日期维度
        {
            get { return _日期维度; }
            set { SetPropertyValue("日期维度", ref _日期维度, value); }
        }

        private decimal _销量;

        public decimal 销量
        {
            get { return _销量; }
            set { SetPropertyValue("销量", ref _销量, value); }
        }
        
        private decimal _总价;

        public decimal 总价
        {
            get { return _总价; }
            set { SetPropertyValue("总价", ref _总价, value); }
        }

        private decimal _单价;

        public decimal 单价
        {
            get { return _单价; }
            set { SetPropertyValue("单价", ref _单价, value); }
        }

        private decimal _利润;

        public decimal 利润
        {
            get { return _利润; }
            set { SetPropertyValue("利润", ref _利润, value); }
        }

        #region plan

        private decimal _计划销量;
        public decimal 计划销量
        {
            get { return _计划销量; }
            set { SetPropertyValue("计划销量", ref _计划销量, value); }
        }

        private decimal _计划售额;
        public decimal 计划售额
        {
            get { return _计划售额; }
            set { SetPropertyValue("计划售额", ref _计划售额, value); }
        }

        #endregion

        //上年同月情况,今年上月情况
        #region last year

        private decimal _上年销量;

        public decimal 上年销量
        {
            get { return _上年销量; }
            set { SetPropertyValue("上年销量", ref _上年销量, value); }
        }

        private decimal _上年售额;

        public decimal 上年售额
        {
            get { return _上年售额; }
            set { SetPropertyValue("上年售额", ref _上年售额, value); }
        }

        private decimal _上年单价;

        public decimal 上年单价
        {
            get { return _上年单价; }
            set { SetPropertyValue("上年单价", ref _上年单价, value); }
        }

        private decimal _上年利润;

        public decimal 上年利润
        {
            get { return _上年利润; }
            set { SetPropertyValue("上年利润", ref _上年利润, value); }
        }

        #endregion

        #region 环比
        private decimal _上月销量;
        public decimal 上月销量
        {
            get { return _上月销量; }
            set { SetPropertyValue("上月销量", ref _上月销量, value); }
        }

        private decimal _上月单价;
        public decimal 上月单价
        {
            get { return _上月单价; }
            set { SetPropertyValue("上月单价", ref _上月单价, value); }
        }

        private decimal _上月售额;
        public decimal 上月售额
        {
            get { return _上月售额; }
            set { SetPropertyValue("上月售额", ref _上月售额, value); }
        }

        private decimal _上月利润;
        public decimal 上月利润
        {
            get { return _上月利润; }
            set { SetPropertyValue("上月利润", ref _上月利润, value); }
        }

        #endregion
    }
}