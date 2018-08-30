using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace IMatrix.ERP.Module.BusinessObjects.SYS.Logic
{
    [XafDisplayName("遍历集合")]
    [CreatableItem]
    [ChildrenType(typeof(MethodCode))]
    public class ForEachStatement : MethodCode
    {
        public ForEachStatement(Session s) : base(s)
        {

        }

        private string _Items;

        public string Items
        {
            get { return _Items; }
            set { SetPropertyValue("Items", ref _Items, value); }
        }

        private string _CodeName;

        public string CodeName
        {
            get { return _CodeName; }
            set { SetPropertyValue("CodeName", ref _CodeName, value); }
        }
        
        protected override void OnSaving()
        {
            base.OnSaving();
            this.名称 = string.Format("foreach(var {0} in {1})", CodeName, Items);
        }

    }

    public class ForEachStatement_ListView : MethodCodeListView<ForEachStatement>
    {
        public override void LayoutDetailView()
        {
            base.LayoutDetailView();
            HGroup(10, x => x.CodeName, x => x.Items);
            HGroup(20, x => x.Index);
        }
    }
}