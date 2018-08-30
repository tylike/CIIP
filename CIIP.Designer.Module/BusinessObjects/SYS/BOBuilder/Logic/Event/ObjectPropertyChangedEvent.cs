using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.SYS.Logic
{
    [XafDisplayName("属性变化")]
    public class ObjectPropertyChangedEvent : BusinessObjectEvent
    {
        public ObjectPropertyChangedEvent(Session s):base(s)
        {
            
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.CodeName = "OnChanged";
            this.AccessorModifier = AccessorModifier.Protected;
            this.MethodModifier = MethodModifier.Override;
            this.SetCode("base.OnChanged(propertyName,oldValue,newValue);");

            this.Parameters.Add(new ParameterDefine(Session)
            {
                OwnerMethod = this,
                ParameterName = "propertyName",
                ParameterType = "string",
                Index = 0
            });

            this.Parameters.Add(new ParameterDefine(Session)
            {
                OwnerMethod = this,
                ParameterName = "oldValue",
                ParameterType = "object",
                Index = 1
            });

            this.Parameters.Add(new ParameterDefine(Session)
            {
                OwnerMethod = this,
                ParameterName = "newValue",
                ParameterType = "object",
                Index = 2
            });
        }

        public override string 名称
        {
            get
            {
                return "属性变化";
            }

            set
            {
                base.名称 = value;
            }
        }
    }
}