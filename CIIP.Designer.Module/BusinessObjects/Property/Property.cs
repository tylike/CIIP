using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace CIIP.Designer
{
    public class Property : PropertyBase
    {
        private string _Expression;

        [XafDisplayName("计算公式")]
        [ToolTip("填正了公式后，此属性将为只读，使用公式进行计算")]
        public string Expression
        {
            get { return _Expression; }
            set { SetPropertyValue("Expression", ref _Expression, value); }
        }
        
        private int _Size;
        [XafDisplayName("长度")]
        public int Size
        {
            get { return _Size; }
            set { SetPropertyValue("Size", ref _Size, value); }
        }
        
        

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Size = 100;
        }

        public Property(Session s) : base(s)
        {

        }
    }
}