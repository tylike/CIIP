using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.StateMachine.Utils;
using DevExpress.ExpressApp.StateMachine.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using CIIP;
using DevExpress.ExpressApp.Utils;

namespace CIIP.StateMachine
{
    [ImageName("BO_State"), DefaultProperty("Caption"), 
    RuleIsReferenced("AdmiralStateIsReferencedXpo", 
        DefaultContexts.Delete, 
        typeof(XpoTransition), "TargetState", InvertResult = true, MessageTemplateMustBeReferenced = "If you want to delete this State, you must be sure it is not referenced in any Transition as TargetState.", FoundObjectMessageFormat = "{0:SourceState.Caption}"), System.ComponentModel.DisplayName("State")]
    public class CIIPXpoState : StateMachineObjectBase, IState, IStateAppearancesProvider,IFlowNode
    {
        private CIIPXpoStateMachine machine;
        private IObjectSpace objectSpace;

        private int _X;

        public int X
        {
            get { return _X; }
            set { SetPropertyValue("X", ref _X, value); }

        }

        private int _Y;

        public int Y
        {
            get { return _Y; }
            set { SetPropertyValue("Y", ref _Y, value); }
        }


        private int _Height;

        public int Height
        {
            get { return _Height; }
            set { SetPropertyValue("Height", ref _Height, value); }
        }


        private int _Width;

        public int Width
        {
            get { return _Width; }
            set { SetPropertyValue("Width", ref _Width, value); }
        }

        public CIIPXpoState(Session session)
            : base(session)
        {

        }



        public void AddTransition(CIIPXpoState targetState)
        {
            var newObject = new CIIPXpoTransition(base.Session)
            {
                TargetState = targetState
            };
            this.Transitions.Add(newObject);
        }

        public void AddTransition(CIIPXpoState targetState, string caption, int index)
        {
            var newObject = new CIIPXpoTransition(base.Session)
            {
                TargetState = targetState,
                Caption = caption,
                Index = index
            };
            this.Transitions.Add(newObject);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.objectSpace = XPObjectSpace.FindObjectSpaceByObject(this);
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            this.objectSpace = XPObjectSpace.FindObjectSpaceByObject(this);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Caption, (this.StateMachine != null) ? this.StateMachine.Name : string.Empty);
        }


        Image IFlowNode.GetImage()
        {
            return null;
            //var cls = CaptionHelper.ApplicationModel.BOModel.GetClass(ReflectionHelper.FindType(Form));
            //if (cls != null)
            //{
            //    if (!string.IsNullOrEmpty(cls.ImageName))
            //    {
            //        return ImageLoader.Instance.GetLargeImageInfo(cls.ImageName).Image;

            //    }
            //}
            //return null;
        }

        [Association("State-AppearanceDescriptions"), DevExpress.ExpressApp.DC.Aggregated]
        [XafDisplayName("外观")]
        public XPCollection<CIIPXpoStateAppearance> Appearance
        {
            get
            {
                return base.GetCollection<CIIPXpoStateAppearance>("Appearance");
            }
        }

        [Browsable(false)]
        private IList<MarkerObject> AvailableMarkerObjects
        {
            get
            {
                return new StateMachineLogic(this.objectSpace).GetAvailableMarkerObjects(this, this.objectSpace);
            }
        }

        [RuleRequiredField("GepXpoState.Caption", DefaultContexts.Save)]
        [XafDisplayName("标题")]
        public string Caption
        {
            get
            {
                string propertyValue = base.GetPropertyValue<string>("Caption");
                if (string.IsNullOrEmpty(propertyValue) && (this.Marker != null))
                {
                    return this.Marker.DisplayName;
                }
                return propertyValue;
            }
            set
            {
                base.SetPropertyValue("Caption", value);
            }
        }

        object IState.Marker
        {
            get
            {
                if (this.Marker == null)
                {
                    return this;
                }
                return this.Marker.Marker;
            }
        }

        IStateMachine IState.StateMachine
        {
            get
            {
                return this.StateMachine;
            }
        }

        IList<ITransition> IState.Transitions
        {
            get
            {
                List<ITransition> list = new List<ITransition>();
                foreach (CIIPXpoTransition transition in this.Transitions)
                {
                    list.Add(transition);
                }
                return list;
            }
        }

        IList<IAppearanceRuleProperties> IStateAppearancesProvider.Appearances
        {
            get
            {
                List<IAppearanceRuleProperties> list = new List<IAppearanceRuleProperties>();
                foreach (CIIPXpoStateAppearance appearance in this.Appearance)
                {
                    list.Add(appearance);
                }
                return list;
            }
        }

        [NonPersistent, DataSourceProperty("AvailableMarkerObjects"), ImmediatePostData]
        public MarkerObject Marker
        {
            get
            {
                return new StateMachineLogic(this.objectSpace).GetMarkerObjectFromMarkerValue(this.MarkerValue, this, this.objectSpace);
            }
            set
            {
                this.MarkerValue = new StateMachineLogic(this.objectSpace).GetMarkerValueFromMarkerObject(value, this, this.objectSpace);
                base.OnChanged("Caption");
            }
        }

        [Browsable(false)]
        public string MarkerValue
        {
            get
            {
                return base.GetPropertyValue<string>("MarkerValue");
            }
            set
            {
                base.SetPropertyValue<string>("MarkerValue", value);
            }
        }

        [Association("StateMachine-States")]
        [XafDisplayName("所属状态机"),VisibleInListView(false),VisibleInDetailView(false)]
        public CIIPXpoStateMachine StateMachine
        {
            get
            {
                return this.machine;
            }
            set
            {
                base.SetPropertyValue("Machine", ref this.machine, value);
            }
        }

        [Size(-1), CriteriaOptions("StateMachine.TargetObjectType")]
        [XafDisplayName("生效条件")]
        public string TargetObjectCriteria
        {
            get
            {
                return base.GetPropertyValue<string>("TargetObjectCriteria");
            }
            set
            {
                base.SetPropertyValue<string>("TargetObjectCriteria", value);
            }
        }

        [Association("State-Transitions"), DevExpress.ExpressApp.DC.Aggregated]
        [XafDisplayName("转换")]
        public XPCollection<CIIPXpoTransition> Transitions
        {
            get
            {
                return base.GetCollection<CIIPXpoTransition>("Transitions");
            }
        }


        private CIIPXpoStateValue _Value;
        [XafDisplayName("值")]
        public CIIPXpoStateValue Value
        {
            get { return _Value; }
            set { SetPropertyValue("Value", ref _Value, value); }
        }




        //string IFlowNode.Form
        //{
        //    get
        //    {
        //        return null;
        //    }

        //    set
        //    {

        //    }
        //}
    }

    [ImageName("BO_State"), DefaultProperty("Caption")]
    public class CIIPXpoStateValue : XPLiteObject
    {
        public CIIPXpoStateValue(Session s):base(s)
        {

        }

        private string _Oid;
        [Key(false)]
        public string Oid
        {
            get { return _Oid; }
            set { SetPropertyValue("Oid", ref _Oid, value); }
        }

        private string _Caption;

        public string Caption
        {
            get { return _Caption; }
            set { SetPropertyValue("Caption", ref _Caption, value); }
        }


    }
}