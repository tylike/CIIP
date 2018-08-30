using DevExpress.ExpressApp.DC;

namespace CIIP.Module.BusinessObjects
{
    public enum 科目分类
    {
        [XafDisplayName("资产类")]
        Asset = 0,
        [XafDisplayName("负债类")]
        Liabilities = 1,
        [XafDisplayName("所有者权益类")]
        Ownerequity = 2,
        [XafDisplayName("成本类")]
        Cost = 3,
        [XafDisplayName("损益类")]
        ProfitAndLoss = 4,
        [XafDisplayName("共同类")]
        Common = 5
    }
}