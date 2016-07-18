using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.StateMachine.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace CIIP.StateMachine
{
    [ImageName("BO_Appearance"), XafDisplayName("外观控制"),
     Appearance("GepXpoStateAppearance.AppearanceForAction", TargetItems = "BackColor; FontColor; FontStyle", Enabled = false, Criteria = "AppearanceItemType='Action'")]
    public class CIIPXpoStateAppearance : StateMachineObjectBase, IAppearanceRuleProperties, IAppearance
    {
        private string appearanceItemType;
        private Color? backColor;
        private string context;
        private bool? enabled;
        private Color? fontColor;
        private System.Drawing.FontStyle? fontStyle;
        private string method;
        private int priority;
        private CIIPXpoState state;
        private string targetItems;
        private ViewItemVisibility? visibility;

        public CIIPXpoStateAppearance(Session session)
            : base(session)
        {
            this.appearanceItemType = "ViewItem";
            this.context = "Any";
        }

        [ImmediatePostData]
        [XafDisplayName("类型")]
        public string AppearanceItemType
        {
            get
            {
                return this.appearanceItemType;
            }
            set
            {
                base.SetPropertyValue<string>("AppearanceItemType", ref this.appearanceItemType, value);
            }
        }

        [ValueConverter(typeof(NullableColorConverter)), Persistent]
        [XafDisplayName("背景颜色")]
        public Color? BackColor
        {
            get
            {
                return this.backColor;
            }
            set
            {
                base.SetPropertyValue<Color?>("BackColor", ref this.backColor, value);
            }
        }
        [XafDisplayName("生效环境")]
        public string Context
        {
            get
            {
                return this.context;
            }
            set
            {
                base.SetPropertyValue<string>("Context", ref this.context, value);
            }
        }

        [Browsable(false), Size(-1)]
        [XafDisplayName("生效条件")]
        public string Criteria
        {
            get
            {
                if ((((this.State != null) && (this.State.StateMachine != null)) && ((this.State.StateMachine.StatePropertyName != null) && (this.State.Marker != null))) && (this.State.Marker.Marker != null))
                {
                    return new BinaryOperator(this.State.StateMachine.StatePropertyName.Name, this.State.Marker.Marker).ToString();
                }
                return "0=1";
            }
            set
            {
            }
        }

        [Browsable(false)]
        public Type DeclaringType
        {
            get
            {
                return this.TargetObjectType;
            }
        }
        [XafDisplayName("是否启用")]
        public bool? Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                base.SetPropertyValue<bool?>("Enabled", ref this.enabled, value);
            }
        }

        [Persistent, ValueConverter(typeof(NullableColorConverter))]
        [XafDisplayName("字体颜色")]
        public Color? FontColor
        {
            get
            {
                return this.fontColor;
            }
            set
            {
                base.SetPropertyValue<Color?>("FontColor", ref this.fontColor, value);
            }
        }
        [XafDisplayName("字体样式")]
        public FontStyle? FontStyle
        {
            get
            {
                return this.fontStyle;
            }
            set
            {
                base.SetPropertyValue("FontStyle", ref this.fontStyle, value);
            }
        }

        [Browsable(false)]
        public string Method
        {
            get
            {
                return this.method;
            }
            set
            {
                base.SetPropertyValue<string>("Method", ref this.method, value);
            }
        }
        [XafDisplayName("优先级")]
        public int Priority
        {
            get
            {
                return this.priority;
            }
            set
            {
                base.SetPropertyValue("Priority", ref this.priority, value);
            }
        }

        [Association("State-AppearanceDescriptions"), Browsable(false)]
        public CIIPXpoState State
        {
            get
            {
                return this.state;
            }
            set
            {
                base.SetPropertyValue("State", ref this.state, value);
            }
        }

        [RuleRequiredField("GepXpoStateAppearance.TargetItems", DefaultContexts.Save)]
        [XafDisplayName("目标字段")]
        public string TargetItems
        {
            get
            {
                return this.targetItems;
            }
            set
            {
                base.SetPropertyValue<string>("TargetItems", ref this.targetItems, value);
            }
        }

        [Browsable(false), MemberDesignTimeVisibility(false)]
        public Type TargetObjectType
        {
            get
            {
                if ((this.State != null) && (this.State.StateMachine != null))
                {
                    return this.State.StateMachine.TargetObjectType;
                }
                return null;
            }
        }
        [XafDisplayName("可见")]
        public ViewItemVisibility? Visibility
        {
            get
            {
                return this.visibility;
            }
            set
            {
                base.SetPropertyValue("Visibility", ref this.visibility, value);
            }
        }
    }
}