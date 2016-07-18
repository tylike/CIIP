using System;
using System.ComponentModel;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    public class 会计科目 : NameObject, ITreeNode
    {
        public 会计科目(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        
        public 科目分类 科目类型 { get; set; }

        #region ITreeNode 成员

        [Association("FrpSubject_FrpSubjects")]
        public 会计科目 上级科目
        {
            get { return GetPropertyValue<会计科目>("上级科目"); }
            set { SetPropertyValue<会计科目>("上级科目", value); }
        }

        [Association("FrpSubject_FrpSubjects")]
        [DevExpress.Xpo.DisplayName("下级科目")]
        public XPCollection<会计科目> 下级科目           
        {
            get { return GetCollection<会计科目>("下级科目"); }
        }

        XPCollection<会计科目> result;
        IBindingList ITreeNode.Children
        {
            get { return 下级科目; }
        }

        ITreeNode ITreeNode.Parent
        {
            get { return this.上级科目 as ITreeNode; }
        }

        private string _科目名称;
        public string 科目名称
        {
            get { return _科目名称; }
            set { SetPropertyValue("科目名称", ref _科目名称, value); }
        }

        private string _科目编码;
        public string 科目编码
        {
            get { return _科目编码; }
            set { SetPropertyValue("科目编码", ref _科目编码, value); }
        }

        string ITreeNode.Name
        {
            get { return 科目名称; }
        }

        #endregion

        private 余额方向 _余额方向;
        public 余额方向 余额方向
        {
            get { return _余额方向; }
            set { SetPropertyValue("余额方向", ref _余额方向, value); }
        }
        
        protected override void OnSaving()
        {
            switch (科目类型)
            {
                case 科目分类.Asset:
                    if (科目编码.Substring(0, 1) != "1") throw new Exception(string.Format("科目类型【资产类】对应的科目编码不对，请核对后保存！"));
                    break;
                case 科目分类.Liabilities:
                    if (科目编码.Substring(0, 1) != "2") throw new Exception(string.Format("科目类型【负债类】对应的科目编码不对，请核对后保存！"));
                    break;
                case 科目分类.Common:
                    if (科目编码.Substring(0, 1) != "3") throw new Exception(string.Format("科目类型【共同类】对应的科目编码不对，请核对后保存！"));
                    break;
                case 科目分类.Ownerequity:
                    if (科目编码.Substring(0, 1) != "4") throw new Exception(string.Format("科目类型【所有者权益类】对应的科目编码不对，请核对后保存！"));
                    break;
                case 科目分类.Cost:
                    if (科目编码.Substring(0, 1) != "5") throw new Exception(string.Format("科目类型【成本类】对应的科目编码不对，请核对后保存！"));
                    break;
                case 科目分类.ProfitAndLoss:
                    if (科目编码.Substring(0, 1) != "6") throw new Exception(string.Format("科目类型【损益类】对应的科目编码不对，请核对后保存！"));
                    break;
            }
            base.OnSaving();
        }
        protected override void OnDeleting()
        {
            //XPCollection<FrpPayBill> PayBills = new XPCollection<FrpPayBill>(Session, CriteriaOperator.Parse("Subject=?", this.Uid));
            //XPCollection<FrpCollectionBill> CollectionBills = new XPCollection<FrpCollectionBill>(Session, CriteriaOperator.Parse("Subject = ?", this.Uid));
            //if (PayBills.Count > 0 || CollectionBills.Count > 0)
            //{
            //    throw new UserFriendlyException(string.Format("该科目有相对应的收付款单，无法删除！"));
            //}
            base.OnDeleting();
        }
    }
}