using System;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.SYS.Logic
{
    [XafDisplayName("对象创建")]
    public class ObjectAfterConstruction : BusinessObjectEvent
    {
        public ObjectAfterConstruction(Session s) : base(s)
        {
            
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.CodeName = "AfterConstruction";
            this.AccessorModifier = AccessorModifier.Public;
            this.MethodModifier = MethodModifier.Override;
            this.SetCode("base.AfterConstruction();");
        }

        public override string Name 
        {
            get { return "对象创建"; }

            set { base.Name = value; }
        }
        
    }

    //目标:对事件类的详细视图都做统一排版

    //public class MethodDefine_ListView : ListViewObject<MethodDefine>
    //{
    //    public override void LayoutListView()
    //    {

    //    }

    //    public override void LayoutDetailView()
    //    {
    //        DetailViewLayout.ClearNodes();
    //        this.HGroup(10, x => x.Code);
    //        base.LayoutDetailView();
    //    }
    //}
}