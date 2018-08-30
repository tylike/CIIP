using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;

namespace IMatrix.ERP.Module.BusinessObjects.SYS.Logic
{
    /// <summary>
    /// 满足条件则执行语句块
    /// </summary>
    [XafDisplayName("分支")]
    [ChildrenType(typeof (MethodCode))]
    public class SwitchCaseStatement : LogicCodeUnit
    {
        public SwitchCaseStatement(Session s) : base(s)
        {

        }

        private string _CriteriaExpression;

        public string CriteriaExpression
        {
            get { return _CriteriaExpression; }
            set { SetPropertyValue("CriteriaExpression", ref _CriteriaExpression, value); }
        }
    }

    public class SwitchCaseStatement_ListView : MethodCodeListView<SwitchCaseStatement>
    {
        public override void LayoutDetailView()
        {
            base.LayoutDetailView();
            HGroup(10, x => x.CriteriaExpression);
            HGroup(20, x => x.Index);
        }
    }
}