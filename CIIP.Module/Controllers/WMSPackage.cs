using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIIP.Module.BusinessObjects.Security;
using CIIP.Module.BusinessObjects.SYS;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using 常用基类;
using DevExpress.ExpressApp.Utils;
using System.IO;
using CIIP.Module.BusinessObjects.Flow;
using CIIP.StateMachine;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Xpo;

namespace CIIP.Module.Controllers
{
    public abstract class Package
    {
        protected IObjectSpace os;
        public Package(IObjectSpace os)
        {
            this.os = os;
        }
        public abstract void Create(bool deleteExist);
        public abstract void AutoRun();
    }

    public class WMSPackage:Package
    {
        private BusinessObject NameObject;
        private NameSpace NS基础信息;

        public WMSPackage(IObjectSpace os) : base(os)
        {
        }
        
        public override void Create(bool deleteExist)
        {
            var exist = os.GetObjects<BusinessObject>(CriteriaOperator.Parse("IsRuntimeDefine", true));
            if (exist.Count > 0 && !deleteExist)
                return;
            //如果已经创建完成，就不要再创建了！

            

            var paritalLogics = os.GetObjects<BusinessObjectPartialLogic>(null, true);
            os.Delete(exist);
            os.Delete(paritalLogics);
            var layouts = os.GetObjects<BusinessObjectLayout>(null, true);
            os.Delete(layouts);

            var runtimeControllers = os.GetObjects<RuntimeController>();
            os.Delete(runtimeControllers);

            var SimpleObject = os.FindBusinessObject<SimpleObject>();
            var baseObject = os.FindBusinessObject<BaseObject>();
            NameObject = os.FindBusinessObject<NameObject>();   //.FindObject<BusinessObject>(new BinaryOperator("FullName", typeof(NameObject).FullName));
            NS基础信息 = CreateNameSpace("基础信息");
            var NS常用基类 = CreateNameSpace("常用基类");

            var productns = CreateNameSpace("基础信息.产品");

            var 产品分类 = CreateNameObject("产品分类", productns);
            var 计量单位 = CreateNameObject("计量单位", productns);
            var 品牌 = CreateNameObject("品牌", productns);
            var 型号 = CreateNameObject("型号", productns);

            #region 产品

            var product = CreateNameObject("产品", productns);
            product.AddProperty("品牌", 品牌);
            product.AddProperty("型号", 型号);
            product.AddProperty<string>("说明", -1);
            product.AddProperty<decimal>("零售价");
            product.AddProperty<decimal>("进货价格");
            product.AddProperty("计量单位", 计量单位);
            product.AddProperty("产品分类", 产品分类);

            #endregion

            #region 产品价格

            var xpliteObject = os.FindBusinessObject<XPLiteObject>();
            var productPrice = CreateObject("产品价格", xpliteObject, productns);
            productPrice.AddProperty("产品", product);
            productPrice.AddProperty<decimal>("价格");
            productPrice.AddProperty("计量单位", 计量单位);
            productPrice.AddProperty<string>("来源");

            #endregion

            #region options
            var 常用税率 = CreateNameObject("常用税率", NS基础信息);
            常用税率.AddProperty<decimal>("税率");

            var 结算方式 = CreateNameObject("结算方式", NS基础信息);
            var 运输方式 = CreateNameObject("运输方式", NS基础信息);
            #endregion

            #region 仓库 库位
            var NS仓库管理 = CreateNameSpace("基础信息.仓库管理");

            var 仓库 = CreateNameObject("仓库", NS仓库管理);
            仓库.AddProperty<string>("详细地址");

            var 库位 = CreateNameObject("库位", NS仓库管理);
            var 库位仓库 = 库位.AddProperty("仓库", 仓库);

            仓库.AddAssociation("库位", 库位, true, 库位仓库);

            #endregion

            #region 地理

            var 地理 = CreateNameSpace("基础信息.地理");

            var 地理单位 = CreateNameObject("地理单位", 地理);
            地理单位.IsPersistent = false;
            地理单位.Modifier = Modifier.Abstract;

            地理单位.AddProperty<decimal>("经度");
            地理单位.AddProperty<decimal>("纬度");


            var 省份 = CreateObject("省份", 地理单位, 地理);

            var 城市 = CreateObject("城市", 地理单位, 地理);

            var 城市省份 = 城市.AddProperty("省份", 省份);

            省份.AddAssociation("城市", 城市, true, 城市省份);

            var 销售区域 = CreateObject("销售区域", 地理单位, 地理);
            var 销售区域城市 = 销售区域.AddProperty("城市", 城市);

            城市.AddAssociation("销售区域", 销售区域, true, 销售区域城市);

            var 地址 = CreateNameObject("地址", 地理);
            地址.AddSavingEvent(
                @"
if(往来单位.默认地址 == null)
            {
                this.往来单位.默认地址 = this;
            }
            base.OnSaving();
"
                );

            #endregion

            #region options
            var 渠道 = CreateNameObject("渠道", NS基础信息);

            var NS往来单位 = CreateNameSpace("基础信息.往来单位");
            #endregion

            #region 客户分类

            var customerCategory = CreateNameObject("客户分类", NS往来单位);
            var parent = customerCategory.AddProperty("上级", customerCategory);
            customerCategory.AddAssociation("子级", customerCategory, true, parent);

            customerCategory.AddPartialLogic(@"
namespace 基础信息.往来单位
{
    public partial class 客户分类 : ITreeNode
    {
            IBindingList ITreeNode.Children
            {
                get { return 子级; }
            }

            string ITreeNode.Name
            {
                get { return 名称; }
            }

            ITreeNode ITreeNode.Parent
            {
                get { return 上级; }
            }
    }
}
");

            #endregion

            #region 员工及相关

            var company = CreateNameObject("往来单位", NS往来单位);
            var NS组织架构 = CreateNameSpace("基础信息.组织架构");
            var dept = CreateNameObject("部门", NS组织架构);
            var title = CreateNameObject("职位", NS组织架构);
            var emp = CreateNameObject("员工", NS组织架构);
            var empDept = emp.AddProperty("部门", dept);
            var 地址往来单位 = 地址.AddProperty("往来单位", company);

            #region dept 属性

            var deptCompany = dept.AddProperty("往来单位", company);
            dept.AddProperty<string>("备注");
            dept.AddProperty("负责人", emp);
            dept.AddAssociation("员工", emp, true, empDept);

            #endregion

            #region 员工属性

            emp.AddProperty<系统用户>("系统用户");
            var 员工往来单位 = emp.AddProperty("往来单位", company);
            emp.AddProperty("职位", title);
            emp.AddProperty<string>("手机");
            emp.AddProperty<string>("家庭地址");
            emp.AddProperty<string>("邮编");
            emp.AddProperty<string>("电子邮件");
            emp.AddProperty<string>("QQ");
            emp.AddProperty<string>("微信");
            emp.AddProperty<string>("其他联系方式");

            #endregion

            #region 员工事件

            emp.AddSavingEvent(@"
		if(!this.IsDeleted && 往来单位!=null)
		{
			
			if (往来单位.默认联系人 == null)
            {
                往来单位.默认联系人 = this;
            }
            base.OnSaving();
        }
");

            #endregion

            #region 往来单位属性

            company.AddProperty<bool>("客户");
            company.AddProperty<bool>("供应商");
            company.AddProperty("销售区域", 销售区域);
            company.AddProperty("默认联系人", emp);
            company.AddProperty("默认地址", 地址);
            company.AddProperty("渠道", 渠道);
            company.AddProperty("Category", customerCategory);
            company.AddAssociation("联系人", emp, true, 员工往来单位);
            company.AddAssociation("部门", dept, true, deptCompany);
            company.AddAssociation("地址", 地址, true, 地址往来单位);

            #endregion

            #region 往来单位分部逻辑

            company.AddPartialLogic(@"
namespace 基础信息.往来单位
{
    public partial class 往来单位 : ICategorizedItem
    {
        ITreeNode ICategorizedItem.Category
        {
            get { return Category; }

            set { this.Category = (客户分类)value; }
        }
    }
}
");

            #endregion


            #endregion

            #region 订单

            var form = os.FindBusinessObject(typeof(单据<>));
            var order = CreateObject("订单", form, NS常用基类);
            order.GenericParameters[0].Constraint = "DevExpress.Xpo.XPBaseObject";
            order.Modifier = Modifier.Abstract;
            order.IsPersistent = false;
            order.Caption = "订单<明细>";

            order.AddProperty<DateTime>("下单日期");

            var wd = order.AddProperty<string>("下单日期维度");
            wd.Browsable = false;

            order.AddProperty("业务员", emp);
            var orderDept = order.AddProperty("部门", dept);
            orderDept.DataSourceProperty = "业务员.往来单位.部门";



            order.AddProperty<DateTime>("预计送达时间");
            order.AddProperty<DateTime>("期望到货时间");

            order.AddProperty("运输方式", 运输方式);
            order.AddProperty("结算方式", 结算方式);

            order.AddProperty("税率", 常用税率);
            order.AddProperty("客户", company);

            order.AddProperty("收货地址", 地址).DataSourceProperty = "客户.地址";


            order.AddProperty("发货地址", 地址).DataSourceProperty = "供应商.地址";

            order.AddProperty("客户联系人", emp).DataSourceProperty = "客户.联系人";
            order.AddProperty("供应商", company);
            order.AddProperty("供应商联系人", emp).DataSourceProperty = "供应商.联系人";

            order.AddProperty<decimal>("订单总金额").AllowEdit = false;
            order.AddProperty<decimal>("折扣总金额").AllowEdit = false;
            order.AddProperty<decimal>("折后总金额").AllowEdit = false;
            order.AddProperty<decimal>("税金").AllowEdit = false;
            order.AddProperty<decimal>("含税总金额").AllowEdit = false;
            order.IsGenericTypeDefine = true;

            order.AddAfterConstruction(@"
            base.AfterConstruction();
            //业务员 = Session.GetObjectByKey<系统用户>(SecuritySystem.CurrentUserId).员工;
            下单日期 = DateTime.Now;
");
            order.AddSavingEvent(@"
            this.下单日期维度 = 下单日期.ToString(""yyyyMMdd"");
            base.OnSaving();
            ");
            order.AddPropertyChangedEvent(@"
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == ""供应商"")
                {
                供应商联系人 = 供应商.默认联系人;
                发货地址 = 供应商.默认地址;
            }

            if (propertyName == ""客户"")
            {
                客户联系人 = 客户.默认联系人;
                收货地址 = 客户.默认地址;
            }

            if (propertyName == ""业务员"")
            {
                this.部门 = this.业务员?.部门;
            }

            if (propertyName == ""税率"")
            {
                var v = newValue == null ? 0 : 税率.税率;
                foreach (XPBaseObject item in 明细项目)
                {
                    item.SetMemberValue(""税率"", v);
                }
            }
        }
");

            var formItem = os.FindBusinessObject(typeof(明细<>));
            var orderItem = CreateObject("订单明细", formItem, NS常用基类);
            orderItem.名称 = "订单明细";
            orderItem.Caption = "订单明细<订单>";
            orderItem.Modifier = Modifier.Abstract;
            orderItem.IsPersistent = false;
            //formItem.AddProperty("产品价格",产品价格)
            orderItem.AddProperty("产品", product);
            orderItem.AddProperty("单位", 计量单位);
            orderItem.AddProperty<decimal>("单价").ImmediatePostData = true;
            var zkl = orderItem.AddProperty<decimal>("折扣率");
            zkl.ImmediatePostData = true;
            zkl.Range = new RuleRange(zkl.Session);
            zkl.Range.Begin = 0;
            zkl.Range.End = 1;
            zkl.DisplayFormat = "p";
            zkl.EditMask = "p";

            orderItem.AddProperty<decimal>("折扣单价").AllowEdit = false;
            orderItem.AddProperty<decimal>("折后总价").AllowEdit = false;
            orderItem.AddProperty<decimal>("折扣金额").AllowEdit = false;
            orderItem.AddProperty<decimal>("数量").ImmediatePostData = true;
            orderItem.AddProperty<decimal>("总价").AllowEdit = false;
            orderItem.AddProperty<decimal>("税率").AllowEdit = false;
            orderItem.AddProperty<decimal>("含税单价").AllowEdit = false;
            orderItem.AddProperty<decimal>("含税总价").AllowEdit = false;
            orderItem.IsGenericTypeDefine = true;
            order.GenericParameters[0].DefaultGenericType = orderItem;

            orderItem.AddPartialLogic(@"
namespace 常用基类
{
    public abstract partial class 订单明细<TMaster>:ICalc
    {
        public void Calc()
        {
            折扣单价 = 单价 * (1-折扣率);
            折后总价 = 折扣单价 * 数量;
            折扣金额 = (单价 - 折扣单价) * 数量;
            含税单价 = 单价 * (1 + 税率);
            含税总价 = 含税单价 * 数量;
            总价 = 单价 * 数量;
            var calc = this.单据 as ICalc;
            if (calc != null)
                calc.Calc();

        }
    }
    public interface ICalc
    {
        void Calc();
    }
}
");
            orderItem.AddPropertyChangedEvent(@"
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == ""产品价格"")
                {
                //this.产品 = this.产品价格.产品;
                //this.单价 = this.产品价格.价格;
                //this.单位 = this.产品价格.单位;
                }
            }
            if (propertyName == ""税率"" || propertyName == ""单价"" || propertyName == ""数量"" || propertyName == ""折扣率"" || propertyName == ""折扣单价"")
            {
                Calc();
            }
        
");
            #region layout
            order.AddPartialLogic(@"
namespace 常用基类
{
    public abstract partial class 订单<TItem> : ICalc
    {
        public void Calc()
        {
            this.订单总金额 = Convert.ToDecimal(this.Evaluate(""明细项目.Sum(总价)""));
            this.折后总金额 = Convert.ToDecimal(this.Evaluate(""明细项目.Sum(折后总价)""));
            this.折扣总金额 = Convert.ToDecimal(this.Evaluate(""明细项目.Sum(折扣金额)""));
            this.含税总金额 = Convert.ToDecimal(this.Evaluate(""明细项目.Sum(含税总价)""));
            this.税金 = this.含税总金额 - this.订单总金额;
        }
    }
}
");
            order.AddLayout(@"
namespace 常用基类
{
    using DevExpress.ExpressApp.Utils;
    public abstract class 订单布局<TMaster, TItem> : 单据布局<TMaster, TItem>
        where TMaster : 订单<TItem>
        where TItem : 订单明细<TMaster>
    {
        protected virtual void LayoutListViewCore()
        {
            LayoutColumns(x => x.编号, x => x.供应商, x => x.预计送达时间, x => x.运输方式, x => x.订单总金额, x => x.供应商联系人, x => x.结算方式,
                x => x.税金, x => x.税率, x => x.状态, x => x.收货地址, x => x.折扣总金额, x => x.折后总金额, x => x.含税总金额, x => x.创建时间,
                x => x.创建者, x => x.修改时间, x => x.修改者);
        }

        public override void LayoutListView()
        {
            //默认列表
            LayoutListViewCore();
            base.LayoutListView();
        }
        
        public override void LayoutDetailView()
        {
            DetailViewLayout.ClearNodes();
            LayoutDetailViewCore();

            base.LayoutDetailView();
        }

        protected virtual void LayoutDetailViewCore()
        {
            HGroup(10, t => t.编号, t => t.供应商, t => t.供应商联系人, t => t.发货地址);

            HGroup(20, t => t.状态, t => t.客户 , t => t.客户联系人, t => t.收货地址);

            HGroup(30, t => t.订单总金额, t => t.含税总金额, t => t.折扣总金额, t => t.折后总金额);

            HGroup(40, t => t.税率, t => t.税金, t => t.业务员, t => t.部门);

            HGroup(50, t => t.运输方式, t => t.结算方式, t => t.预计送达时间, t => t.期望到货时间);

            var g = HGroup(90, t => t.备注).First() as DevExpress.ExpressApp.Model.IModelLayoutItem;
            g.MaxSize = new System.Drawing.Size(0,60);            
            g.SizeConstraintsType = DevExpress.ExpressApp.Layout.XafSizeConstraintsType.Custom;

            var tg = TabbedGroup(100, ItemsPropertyName, ""单据流程"", ""状态记录"");

            SetItemsPropertyEditor(tg[0]);

            HGroup(1000, x => x.创建者, x => x.创建时间, x => x.修改者, x => x.修改时间);

            var statusListView = GetChildListView(p => p.单据流程);

            LayoutListViewColumns<CIIP.Module.BusinessObjects.单据流程状态记录>(statusListView, x => x.创建者, x => x.创建时间, x => x.修改者, x => x.修改时间);

        }

        public override void LayoutItemsListView()
        {
            //明细列表
            LayoutItemsListViewCore();
            base.LayoutItemsListView();
        }

        protected virtual void LayoutItemsListViewCore()
        {
            //ItemsListView.Columns[""产品价格""].View = CaptionHelper.ApplicationModel.Views[""产品价格_ListView""];
            LayoutItemsColumns(x => x.产品, x => x.单价, x => x.数量, x => x.折扣率, x => x.单位, x => x.总价, x => x.税率, x => x.含税单价, x => x.含税总价, x => x.折扣单价, x => x.折后总价);
        }

        public override void LayoutItemsDetailView()
        {
            ItemsViewLayout.ClearNodes();
            //ItemsViewLayout.ToArray();

            ItemsHGroup(10, x => x.产品, x => x.单价, x => x.数量, x => x.总价);
            ItemsHGroup(20, x => x.单位, x => x.含税单价, x => x.含税总价, x => x.税率);
            ItemsHGroup(30, x => x.折扣单价, x => x.折扣率, x => x.折后总价, x => x.折扣金额);
            ItemsHGroup(40, x => x.创建者, x => x.创建时间, x => x.修改者, x => x.修改时间);
            base.LayoutItemsDetailView();
        }
    }
}
");
            #endregion

            #endregion

            #region wms
            var NSCRM = CreateNameSpace("客户关系");
            var NSSMS = CreateNameSpace("销售管理");
            var NSPMS = CreateNameSpace("采购管理");
            var NSFMS = CreateNameSpace("财务管理");
            var NSSAS = CreateNameSpace("售后服务");
            var NSBIS = CreateNameSpace("数据分析");
            var NSQMS = CreateNameSpace("质量检测");
            var NSWMS = CreateNameSpace("仓库管理");
            var NSPDS = CreateNameSpace("生产管理");


            var pmsRequest = CreateForm("采购申请", order, NSPMS, "采购管理");
            var pmsQuery = CreateForm("采购询价", order, NSPMS, "采购管理");
            var pmsOrder = CreateForm("采购订单", order, NSPMS, "采购管理");
            var pmsContract = CreateForm("采购合同", order, NSPMS, "采购管理");
            var pmsDH = CreateForm("采购到货", order, NSPMS, "采购管理");
            var pmsTH = CreateForm("采购退货", order, NSPMS, "采购管理");

            var smsQuery = CreateForm("销售报价", order, NSSMS, "销售管理");
            var smsOrder = CreateForm("销售订单", order, NSSMS, "销售管理");
            var smsContract = CreateForm("销售合同", order, NSSMS, "销售管理");
            var smsFH = CreateForm("销售发货", order, NSSMS, "销售管理");
            var smsTH = CreateForm("销售退货", order, NSSMS, "销售管理");
            #endregion

            #region 库存流水
            var 库存流水 = CreateObject("库存流水", SimpleObject, NSWMS);
            var p1 = 库存流水.AddProperty("产品", product);
            p1.RuleRequiredField = true;
            p1 = 库存流水.AddProperty("库位", 库位);
            p1.RuleRequiredField = true;
            p1.DataSourceProperty = "库位来源";

            库存流水.AddProperty<decimal>("数量");
            库存流水.AddProperty("计量单位", 计量单位);
            库存流水.AddProperty<decimal>("单价");
            库存流水.AddProperty<decimal>("总价");
            //库存流水.AddProperty("库存操作类型", 库存操作类型);
            库存流水.AddProperty<DateTime>("操作时间");
            库存流水.AddProperty<bool>("有效");

            库存流水.AddPartialLogic(@"
namespace 仓库管理
{
    public partial class 库存流水
    {
        private 库存操作类型 _操作类型;
        /// <summary>
        /// 操作类型
        /// </summary>
        [ModelDefault(""AllowEdit"",""False"")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public 库存操作类型 操作类型
        {
            get { return _操作类型; }
            set { SetPropertyValue(""操作类型"", ref _操作类型, value); }
        }

        protected virtual XPCollection<基础信息.仓库管理.库位> 库位来源
        {
            get{
                return null;
            }
        }
    }

    public enum 库存操作类型
    {
        不操作 = 0,
        入库 = 1,
        出库 = -1
    }
}    
");
            库存流水.AddPropertyChangedEvent(@"
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == ""单价"" || propertyName == ""数量"")
                {
                    this.总价 = this.单价 * this.数量;
                }
            }
");
            库存流水.AddSavingEvent(@"
            base.OnSaving();
            this.操作时间 = DateTime.Now;
");
            #endregion

            #region wmsFormItemBase
            var wmsFormItemBase = CreateObject("库存单据明细", 库存流水, NS常用基类);
            var generic1 = os.CreateObject<GenericParameter>();
            generic1.Constraint = "常用基类.单据";
            generic1.Name = "TMaster";
            wmsFormItemBase.GenericParameters.Add(generic1);
            wmsFormItemBase.IsGenericTypeDefine = true;

            wmsFormItemBase.AddPartialLogic(@"
namespace 常用基类
{
    public partial class 库存单据明细<TMaster>
    {
        public TMaster 单据
        {
            get { return this.GetPropertyValue<TMaster>(""Master""); }
            set
            {
                SetPropertyValue(""Master"", value);
                if (!IsLoading)
                {
                    OnSetMaster(value);
                }
            }
        }

        protected override XPCollection<基础信息.仓库管理.库位> 库位来源
        {
            get
            {
                return (单据.GetMemberValue(""目标仓库"") as 基础信息.仓库管理.仓库)?.库位;
            }
        }

        protected virtual void OnSetMaster(TMaster value)
        {

        }

    }
}
");
            wmsFormItemBase.AddPropertyChangedEvent(@"
            base.OnChanged(propertyName, oldValue, newValue);
            if (propertyName == ""Master"")
            {
                var order = newValue as 常用基类.IWarehouseOrder;
                if (order != null)
                {
                    this.操作类型 = order.操作类型;
                }
            }
            ");
            #endregion

            #region wmsFormBase
            var wmsFormBase = CreateObject("仓库单据基类", form, NS常用基类);
            wmsFormBase.GenericParameters[0].Constraint = "DevExpress.Xpo.XPBaseObject";
            wmsFormBase.GenericParameters[0].DefaultGenericType = wmsFormItemBase;
            wmsFormBase.Modifier = Modifier.Abstract;
            wmsFormBase.IsPersistent = false;
            wmsFormBase.IsGenericTypeDefine = true;
            wmsFormBase.Caption = "仓库单据基类<库存单据明细>";

            wmsFormBase.AddProperty("目标仓库", 仓库);
            wmsFormBase.AddProperty<DateTime>("操作日期");
            wmsFormBase.AddProperty<string>("操作人");

            wmsFormBase.AddPropertyChangedEvent(@"
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == ""状态"")
                {
                    var rst = this.状态.Value!=null && this.状态.Value.Oid == ""Checked"";
                    foreach (var item in this.明细项目)
                    {
                        item.SetMemberValue(""有效"",rst);
                    }
                }
            }
");

            wmsFormBase.AddPartialLogic(@"
namespace 常用基类
{
    public partial class 仓库单据基类<TItem>:IWarehouseOrder
    {
        /// <summary>
        /// 操作类型
        /// </summary>        
        [ModelDefault(""AllowEdit"",""False"")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public virtual 仓库管理.库存操作类型 操作类型
        {
            get { 
                return 仓库管理.库存操作类型.不操作;
            }
        }
    }   
    public interface IWarehouseOrder
    {
        仓库管理.库存操作类型 操作类型 { get; }
    } 
}

");

            #endregion

            #region wms form layout

            wmsFormBase.AddLayout(@"
namespace 基础信息
{
    public abstract class 仓库单据布局<TMaster, TItem> : 单据布局<TMaster, TItem>
        where TMaster: 仓库单据基类<TItem>
        where TItem: 库存单据明细<TMaster>
    {
        protected virtual void LayoutListViewCore()
        {
            LayoutColumns(x => x.编号, x => x.目标仓库, x => x.操作日期, x => x.状态, x => x.操作人, x => x.创建时间,
                x => x.创建者,
                x => x.修改时间, x => x.修改者);
        }

        public override void LayoutListView()
        {
            //默认列表
            LayoutListViewCore();
            base.LayoutListView();
        }

        public override void LayoutDetailView()
        {
            LayoutDetailViewCore();
            base.LayoutDetailView();
        }

        protected virtual void LayoutDetailViewCore()
        {
            DetailViewLayout.ClearNodes();
            //默认详细视图
            LayoutDetailViewBaseInfo();
            
            var g = HGroup(90, t => t.备注).First() as DevExpress.ExpressApp.Model.IModelLayoutItem;
            g.MaxSize = new System.Drawing.Size(0,60);            
            g.SizeConstraintsType = DevExpress.ExpressApp.Layout.XafSizeConstraintsType.Custom;

            var tg = TabbedGroup(100, this.ItemsPropertyName, ""单据流程"");

            SetItemsPropertyEditor(tg[0]);

            HGroup(1000, x => x.创建者, x => x.创建时间, x => x.修改者, x => x.修改时间);

            var statusListView = GetChildListView(p => p.单据流程);

            LayoutListViewColumns<CIIP.Module.BusinessObjects.单据流程状态记录>(statusListView, x => x.创建者, x => x.创建时间, x => x.修改者, x => x.修改时间);
        }

        protected virtual void LayoutDetailViewBaseInfo()
        {
            HGroup(10, t => t.编号, t => t.目标仓库,  t => t.操作人,t=>t.状态);
        }

        public override void LayoutItemsListView()
        {
            //明细列表
            LayoutItemsColumns(x => x.产品, x => x.单价, x => x.数量, x => x.计量单位, x => x.总价, x => x.库位);
            base.LayoutItemsListView();
        }

        public override void LayoutItemsDetailView()
        {
            //Debug.WriteLine(((ModelNode)ItemsViewLayout).Path);
            //ItemsViewLayout.ClearNodes();
            //ItemsViewLayout[0].Remove();
            //ItemsHGroup(10, x => x.产品, x => x.单价, x => x.数量, x => x.总价);
            //ItemsHGroup(20, x => x.计量单位, x => x.总价, x => x.库位);

            base.LayoutItemsDetailView();
        }
    }
}
");
            #endregion

            #region 仓库管理
            var pmsIn = CreateWMSForm("采购入库", wmsFormBase, NSWMS, "采购管理", false);
            var pmsReturn = CreateWMSForm("采购退货出库", wmsFormBase, NSWMS, "采购管理", true, "采购退货");
            var smsOut = CreateWMSForm("销售出库", wmsFormBase, NSWMS, "销售管理", true);
            var smsReturn = CreateWMSForm("销售退货入库", wmsFormBase, NSWMS, "销售管理", false, "销售退货");

            var wmsBS = CreateWMSForm("报损单", wmsFormBase, NSWMS, "仓库管理", true, "库存盘点");
            var wmsBY = CreateWMSForm("报溢单", wmsFormBase, NSWMS, "仓库管理", false, "库存盘点");
            var wmsPD = CreateWMSForm("库存盘点", wmsFormBase, NSWMS, "仓库管理", null, "库存盘点");
            var wmsDBCK = CreateWMSForm("调拨出库", wmsFormBase, NSWMS, "仓库管理", true, "产品调拨");
            var wmsDBRK = CreateWMSForm("调拨入库", wmsFormBase, NSWMS, "仓库管理", false, "产品调拨");
            
            #region 调拨单
            var wmsdbd = CreateWMSForm("调拨单", wmsFormBase, NSWMS, "仓库管理", null, "产品调拨", t =>
            {
                t.ItemBusinessObject.名称 = "调拨明细";
                t.ItemBusinessObject.Caption = "调拨明细";
            });
            wmsdbd.AddProperty("调出仓库", 仓库);
            wmsdbd.AddProperty("调出库位", 库位);
            wmsdbd.AddProperty("目标库位", 库位);
            wmsdbd.AddPropertyChangedEvent(@"
base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == ""调出仓库"")
                {
                foreach (var i in this.明细项目)
                {
                    i.调出仓库 = this.调出仓库;
                }
                this.调出库位 = null;
            }
            if (propertyName == ""调出库位"")
            {
                foreach (var i in this.明细项目)
                {
                    i.调出库位 = this.调出库位;
                }
            }

            if (propertyName == ""目标库位"")
            {
                foreach (var i in this.明细项目)
                {
                    i.库位 = this.目标库位;
                }
            }

            if (propertyName == ""目标仓库"")
            {
                foreach (var i in this.明细项目)
                {
                    i.目标仓库 = this.目标仓库;
                }
                this.目标库位 = null;
            }
        }
");
            var dbi = wmsdbd.ItemBusinessObject;
            dbi.AddProperty("调出仓库", 仓库);
            dbi.AddProperty("调出库位", 库位);
            dbi.AddProperty("目标仓库", 仓库);
            dbi.AddPropertyChangedEvent(@"
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == ""调出仓库"")
                {
                this.调出库位 = null;
            }
            if (propertyName == ""目标仓库"")
            {
                this.库位 = null;
            }
        }
");

            dbi.AddPartialLogic(@"
namespace 仓库管理
{
    public partial class 调拨明细
    {
        protected override void OnSetMaster(调拨单 value)
        {
            this.调出仓库 = value.调出仓库;
            this.调出库位 = value.调出库位;

            this.目标仓库 = value.目标仓库;

            this.库位 = value.目标库位;
            base.OnSetMaster(value);
        }
    }
}
");

            #endregion

            #endregion

            #region 库位产品库存
            var productKWInvertory = CreateObject("库位产品库存", xpliteObject, NSWMS, "仓库管理", "库存情况");
            productKWInvertory.AddProperty<decimal>("数量");
            productKWInvertory.AddProperty<decimal>("总价");
            productKWInvertory.AddProperty("产品", product);
            productKWInvertory.AddProperty("库位", 库位);

            productKWInvertory.AddPartialLogic(@"
namespace 仓库管理
{
    public partial class 库位产品库存
    {
		[Key, Persistent, Browsable(false)]
        public InvertoryKey Key { get; set; }
    }
    
    public struct InvertoryKey
    {
        [Persistent(""产品""), Browsable(false)]
        public 基础信息.产品.产品 产品 { get; set; }

        [Persistent(""库位""), Browsable(false)]
        public 基础信息.仓库管理.库位 库位 { get; set; }
    }
}");
            #endregion

            #region 产品库存
            var productInvertory = CreateObject("产品库存", xpliteObject, NSWMS,"仓库管理", "库存情况");
            productInvertory.AddProperty<decimal>("数量");
            productInvertory.AddProperty<decimal>("总价");
            //productInvertory.AddProperty("产品", product);
            productInvertory.AddPartialLogic(@"
namespace 仓库管理
{
    public partial class 产品库存
    {
		[Key, Persistent]
        public 基础信息.产品.产品 产品 { get; set; }
    }
    
    //public struct ProductInvertoryKey
    //{
    //    [Persistent(""产品""), Browsable(false)]
    //    public 基础信息.产品.产品 产品 { get; set; }
    //}

}");
            #endregion

            #region 仓库产品库存
            var productWHInvertory = CreateObject("仓库产品库存", xpliteObject, NSWMS,"仓库管理", "库存情况");
            productWHInvertory.AddProperty<decimal>("数量");
            productWHInvertory.AddProperty<decimal>("总价");
            productWHInvertory.AddProperty("产品", product);
            productWHInvertory.AddProperty("仓库", 仓库);

            productWHInvertory.AddPartialLogic(@"
namespace 仓库管理
{
    public partial class 仓库产品库存
    {
		[Key, Persistent, Browsable(false)]
        public WarehouseInvertoryKey Key { get; set; }
    }
    
    public struct WarehouseInvertoryKey
    {
        [Persistent(""产品""), Browsable(false)]
        public 基础信息.产品.产品 产品 { get; set; }

        [Persistent(""仓库""), Browsable(false)]
        public 基础信息.仓库管理.仓库 仓库 { get; set; }
    }
}");
            #endregion

            #region 销售计划，预算
            var jhys = CreateObject("计划销量",baseObject,NSSMS);
            jhys.AddProperty("产品", product);
            //jhys.AddProperty("")
            #endregion

            CreateController<RuntimeWindowController>(AdmiralEnvironment.ApplicationPath + "\\Controllers\\NZController.cs");
        }

        public void CreateController<T>(string filePath)
            where T : RuntimeController
        {
            var ctrl = os.CreateObject<T>();
            ctrl.Code.Code = File.ReadAllText(filePath);
        }

        public override void AutoRun()
        {
            var exist = ReflectionHelper.FindType("采购管理.采购询价");
            if (exist != null)
            {
                CreateView("库位产品库存",
@"SELECT   产品, 库位, SUM(数量 * 操作类型) AS 数量, SUM(总价 * 操作类型) AS 总价
FROM      dbo.库存流水
Where 有效=1
GROUP BY 产品, 库位");

                CreateView("产品库存",
    @"SELECT   产品, SUM(数量 * 操作类型) AS 数量, SUM(总价 * 操作类型) AS 总价
FROM      dbo.库存流水
Where 有效=1
GROUP BY 产品");

                CreateView("仓库产品库存",
    @"SELECT   产品,库位.仓库, SUM(数量 * 操作类型) AS 数量, SUM(总价 * 操作类型) AS 总价
FROM      dbo.库存流水 left join 库位
on dbo.库存流水.库位 = 库位.Oid
Where 有效=1
GROUP BY 产品,库位.仓库");
            }

            checkedState = os.GetObjectByKey<CIIPXpoStateValue>("Checked");
            if(checkedState == null)
            {
                checkedState = os.CreateObject<CIIPXpoStateValue>();
                checkedState.Oid = "Checked";
                checkedState.Caption = "已审核";
                checkedState.Save();
            }

            CreateFormConvert();

        }

        CIIPXpoStateValue checkedState;

        private void CreateView(string bo, string viewSql)
        {

            var session = (os as XPObjectSpace).Session;
            var deleteTable = @"
IF EXISTS(SELECT 1 FROM sys.tables WHERE name = N'" + bo + @"')
DROP TABLE [dbo].[" + bo + "]";
            var checkViewExist = @"SELECT count(*) FROM sys.views WHERE name = N'" + bo + "'";


            session.ExecuteNonQuery(deleteTable);
            var exist = Convert.ToInt32(session.ExecuteScalar(checkViewExist));
            if (exist <= 0)
            {
                var createView = @"
CREATE VIEW [dbo].[" + bo + @"]
AS
" + viewSql;
                session.ExecuteNonQuery(createView);
            }
        }

        public void CreateStateMachine(string targetTypeName)
        {
            var targetType = ReflectionHelper.FindType(targetTypeName);
            if (targetType == null)
            {
                return;
            }
            var cls = CaptionHelper.ApplicationModel.BOModel.GetClass(targetType);
            var name = cls.Caption + "状态转换";

            var obj = os.FindObject<CIIPXpoStateMachine>(new BinaryOperator("Name", name));
            if (obj == null)
            {
                obj = os.CreateObject<CIIPXpoStateMachine>();
                obj.TargetObjectType = targetType;
                obj.StatePropertyName = new StringObject(cls.AllMembers.FirstOrDefault(x => x.MemberInfo.MemberType == typeof(CIIPXpoState))?.Name);
                obj.Name = name;
                obj.Active = true;
                obj.ExpandActionsInDetailView = true;
                obj.Save();
                var f = obj as IFlow;
                var NEW = f.CreateNode(307, 137, 64, 64, null, "新建");
                obj.StartState = NEW as CIIPXpoState;
                var CHECKED = f.CreateNode(658, 215, 64, 64, null, "已审核");

                (CHECKED as CIIPXpoState).Value = checkedState;

                var FINISHED = f.CreateNode(585, 426, 64, 64, null, "已完成");
                var CLOSED = f.CreateNode(392, 391, 64, 64, null, "已关闭");

                CreateSMAction(f, NEW, CHECKED, 0, 0);
                CreateSMAction(f, CHECKED, FINISHED, 0, 0);
                CreateSMAction(f, FINISHED, CLOSED, 0, 0);
            }
            
            //SELECT
            //'var ','T_' + replace(Oid, '-', '') ,'=', 'f.CreateNode(',
            //[X],','
            //,[Y],','
            //,[Width],','
            //,[Height],','
            //,'null',','
            //,'"'+[Caption]+'"',');'      
            //FROM[IMatrix.ERP.R2].[dbo].[CIIPXpoState]
           
        }

        void CreateSMAction(IFlow flow, IFlowNode from, IFlowNode to, int beginIndex, int endIndex)
        {
            var fa = flow.CreateAction(from, to) as StateMachine.CIIPXpoTransition;
            fa.BeginItemPointIndex = beginIndex;
            fa.EndItemPointIndex = endIndex;
        }

        void CreateAction(IFlow flow, IFlowNode from,IFlowNode to,int beginIndex,int endIndex)
        {
            var fa = flow.CreateAction(from,to) as FlowAction;
            fa.Created();
            fa.BeginItemPointIndex = beginIndex;
            fa.EndItemPointIndex = endIndex;
        }

        public IFlowNode CreateNode(IFlow flow, int x,int y,int width,int height,string form,string caption)
        {
            var n = flow.CreateNode(x, y, width, height, form, caption);
            CreateStateMachine(form);
            return n;
        }

        public void CreateFormConvert()
        {
            var exist = ReflectionHelper.FindType("采购管理.采购询价");
            if (exist != null)
            {
                var flow = os.FindObject<Flow>(new BinaryOperator("名称", "单据转换流程"));
                if (flow == null)
                {
                    flow = os.CreateObject<Flow>();
                    flow.名称 = "单据转换流程";
                    var f = flow as IFlow;

                    //SELECT
                    //'var ',caption,'=','f.CreateNode('
                    //,[X],','
                    //,[Y],','
                    //,[Width],','
                    //,[Height],','
                    //,'"'+[Form]+'"',','
                    //,'"'+Caption+'");'
                    //FROM[IMatrix.ERP.R2].[dbo].[FlowNode]

                    var 采购询价 = CreateNode(f,480, 177, 64, 64, "采购管理.采购询价", "采购询价");
                    var 采购申请 = CreateNode(f,304, 177, 64, 64, "采购管理.采购申请", "采购申请");
                    var 采购退货 = CreateNode(f,866, 360, 64, 64, "采购管理.采购退货", "采购退货");
                    var 采购入库 = CreateNode(f,1080, 360, 64, 64, "仓库管理.采购入库", "采购入库");
                    var 采购订单 = CreateNode(f,866, 180, 64, 64, "采购管理.采购订单", "采购订单");
                    var 采购合同 = CreateNode(f,660, 180, 64, 64, "采购管理.采购合同", "采购合同");
                    var 采购退货出库 = CreateNode(f,660, 360, 64, 64, "仓库管理.采购退货出库", "采购退货出库");
                    var 采购到货 = CreateNode(f,1080, 180, 64, 64, "采购管理.采购到货", "采购到货");


                    var 销售报价 = CreateNode(f,304, 570, 64, 64, "销售管理.销售报价", "销售报价");
                    var 销售订单 = CreateNode(f,660, 570, 64, 64, "销售管理.销售订单", "销售订单");
                    var 销售合同 = CreateNode(f,480, 570, 64, 64, "销售管理.销售合同", "销售合同");

                    var 销售发货 = CreateNode(f,866, 570, 64, 64, "销售管理.销售发货", "销售发货");
                    var 销售出库 = CreateNode(f,1080, 570, 64, 64, "仓库管理.销售出库", "销售出库");

                    var 销售退货 = CreateNode(f,660, 780, 64, 64, "销售管理.销售退货", "销售退货");
                    var 销售退货入库 = CreateNode(f,866, 780, 64, 64, "仓库管理.销售退货入库", "销售退货入库");


                    var 调拨单 = CreateNode(f,360, 1140, 64, 64, "仓库管理.调拨单", "调拨单");
                    var 调拨出库 = CreateNode(f,600, 1140, 64, 64, "仓库管理.调拨出库", "调拨出库");
                    var 调拨入库 = CreateNode(f,120, 1140, 64, 64, "仓库管理.调拨入库", "调拨入库");

                    var 库存盘点 = CreateNode(f,360, 960, 64, 64, "仓库管理.库存盘点", "库存盘点");
                    var 报溢单 = CreateNode(f,120, 960, 64, 64, "仓库管理.报溢单", "报溢单");
                    var 报损单 = CreateNode(f,600, 960, 64, 64, "仓库管理.报损单", "报损单");

                    //FromCaption ToCaption   MC CC  Caption BeginItemPointIndex EndItemPointIndex

                    //SELECT   'CreateAction(f,' AS Expr1,
                    //FromCaption.Caption,',',
                    //ToCaption.Caption,',',
                    //FlowAction.BeginItemPointIndex,',', 
                    //FlowAction.EndItemPointIndex, 
				
                    //');' AS Expr2
                    //FROM FlowAction   LEFT OUTER JOIN
                    //FlowNode AS FromCaption ON FlowAction.[From] = FromCaption.Oid LEFT OUTER JOIN
                    //FlowNode AS ToCaption ON FlowAction.[To] = ToCaption.Oid
                    //where FlowAction.Caption not like N'%采购%'

                    CreateAction(f, 采购退货, 采购退货出库, 3, -1);
                    CreateAction(f, 采购订单, 采购退货, 2, 0);
                    CreateAction(f, 采购询价, 采购合同, 1, 3);
                    CreateAction(f, 采购订单, 采购到货, 1, 3);
                    CreateAction(f, 采购到货, 采购入库, 2, 0);
                    CreateAction(f, 采购合同, 采购订单, 1, 3);
                    CreateAction(f, 采购申请, 采购询价, 1, 3);

                    CreateAction(f, 调拨单, 调拨出库, 1, 3);
                    CreateAction(f, 销售合同, 销售订单, -1, 3);
                    CreateAction(f, 调拨单, 调拨入库, 3, 1);
                    CreateAction(f, 销售退货, 销售退货入库, 1, -1);
                    CreateAction(f, 库存盘点, 报溢单, 3, 1);
                    CreateAction(f, 销售订单, 销售发货, 1, 3);
                    CreateAction(f, 销售报价, 销售合同, 1, -1);
                    CreateAction(f, 销售发货, 销售出库, 1, 3);
                    CreateAction(f, 销售订单, 销售退货, 2, 0);
                    CreateAction(f, 库存盘点, 报损单, 1, 3);


                }
            }
        }
        public BusinessForm CreateWMSForm(string name, BusinessObject @base, NameSpace category, string nl1, bool? isOut, string nl2 = null,Action<BusinessForm> userSetup = null)
        {

            var form = CreateForm(name, @base, category, nl1, nl2,false);
            userSetup?.Invoke(form);

            string cklx = "不操作";
            if (isOut.HasValue)
                cklx = isOut.Value ? "出库" : "入库";
            var template = $@"
namespace {category.FullName}
{{
    public partial class {name}
    {{
        public override 库存操作类型 操作类型
        {{
            get {{ return 库存操作类型.{cklx}; }}
        }}
    }}
}}
";
            

            form.AddPartialLogic(template);
            var layout = $@"
namespace {form.Category.FullName}
{{
    public partial class {form.名称}_ListView : 仓库单据布局<{form.名称}, {form.ItemBusinessObject.名称}>
    {{

    }}
}}
";

            form.AddLayout(layout);
            return form;
        }

        public BusinessForm CreateForm(string name,BusinessObject @base,NameSpace category,string navl1,string navl2=null,bool createLayout = true)
        {
            var root = ModelDataSource.NavigationItemDataSources.FirstOrDefault(x => x.Name == navl1)?.Children.OfType<NavigationItem>().FirstOrDefault(x => x.Name == (navl2 ?? name));

            var form = os.CreateObject<BusinessForm>();
            form.名称 = name;
            form.Base = @base;
            form.Category = category;
            form.NavigationItem = root;
            if (createLayout)
            {
                form.AddLayout($@"
namespace {form.Category.FullName}
{{
    public partial class {form.名称}_ListView : 订单布局<{form.名称}, {form.ItemBusinessObject.名称}>
    {{

    }}
}}
");
            }
            return form;

        }

        public BusinessObject CreateNameObject(string name, NameSpace ns, string nav1 = null, string nav2 = null)
        {
            return CreateObject(name, NameObject, ns, nav1, nav2);
        }

        public BusinessObjectPartialLogic CreatePartialLogic(BusinessObject bo, string code)
        {
            var bopl = os.CreateObject<BusinessObjectPartialLogic>();
            bopl.Code = new BusinessObjects.SYS.Logic.CsharpCode(code, bo);
            bopl.BusinessObject = bo;
            return bopl;
        }

        public BusinessObject CreateObject(string name, BusinessObject baseBO, NameSpace ns, string navl1 = null, string navl2 = null)
        {
            var bo = os.CreateObject<BusinessObject>();
            bo.Category = ns;
            bo.Base = baseBO;
            bo.名称 = name;
            var root = ModelDataSource.NavigationItemDataSources.FirstOrDefault(x => x.Name == navl1)?.Children.OfType<NavigationItem>().FirstOrDefault(x => x.Name == (navl2 ?? name));
            bo.NavigationItem = root;
            return bo;
        }
        
        public NameSpace CreateNameSpace(string ns)
        {

            NameSpace find = os.FindObject<NameSpace>(new BinaryOperator("FullName", ns), true);
            
            //没找到：
            if (find == null)
            {
                var part = ns.Split('.');
                if (part.Length > 0)
                {
                    //是 “xxx.xxx”这样的多级空间。
                    find = os.CreateObject<NameSpace>();
                    find.名称 = part[part.Length - 1];
                    //创建最后一级空间
                    if (part.Length > 1)
                    {
                        //查找上级空间：
                        find.Parent = CreateNameSpace(string.Join(".", part.Take(part.Length - 1)));
                    }
                }
            }
            return find;
        }
    }
}
