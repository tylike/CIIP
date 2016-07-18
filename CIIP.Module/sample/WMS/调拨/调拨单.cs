using System;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using CIIP.Module.BusinessObjects;

namespace WMS
{
    //选择仓库、库位后，录入要调拨的产品内容
    //确认后，生成对应的入库单，出库单，调拨出入库单产品、数量、需要与调拨单一致。
    //入库操作人员从入库单审核确认，出库同理
    [NavigationItem("仓库管理")]
    public class 调拨单 : 仓库单据基类<调拨明细>
    {
        public 调拨单(Session s ):base(s)
        {

        }

        #region 快捷操作
        //以下快捷操枪属性：
        //填写后，则使用，不填写，则可以在明细中指定。
        private 仓库 _调出仓库;
        public 仓库 调出仓库
        {
            get { return _调出仓库; }
            set { SetPropertyValue("调出仓库", ref _调出仓库, value); }
        }

        private 库位 _调出库位;
        [DataSourceProperty("调出仓库.库位", DataSourcePropertyIsNullMode.SelectAll)]
        public 库位 调出库位
        {
            get { return _调出库位; }
            set { SetPropertyValue("调出库位", ref _调出库位, value); }
        }

        private 库位 _目标库位;
        [DataSourceProperty("目标仓库.库位", DataSourcePropertyIsNullMode.SelectAll)]
        public 库位 目标库位
        {
            get { return _目标库位; }
            set { SetPropertyValue("目标库位", ref _目标库位, value); }
        }
        #endregion

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading)
            {
                if (propertyName == "调出仓库")
                {
                    foreach (var i in this.明细项目)
                    {
                        i.调出仓库 = this.调出仓库;
                    }
                    this.调出库位 = null;
                }
                if (propertyName == "调出库位")
                {
                    foreach (var i in this.明细项目)
                    {
                        i.调出库位 = this.调出库位;
                    }
                }

                if (propertyName == "目标库位")
                {
                    foreach (var i in this.明细项目)
                    {
                        i.库位 = this.目标库位;
                    }
                }

                if (propertyName == "目标仓库")
                {
                    foreach (var i in this.明细项目)
                    {
                        i.目标仓库 = this.目标仓库;
                    }
                    this.目标库位 = null;
                }
            }
        }

        

        //public override 库存操作类型 操作类型
        //{
        //    get
        //    {
        //        return 库存操作类型.不操作;
        //    }
        //}

        public override void OnChecked()
        {
            base.OnChecked();
            //生成调拨出库与入库
        }

        public override void OnUnChecked()
        {
            base.OnUnChecked();
            //删除出库入与入库
            //如果出库与入库与经审核了呢？
            //应该做对应的单据修正？
        }

    }
}