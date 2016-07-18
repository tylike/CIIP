using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.StateMachine.Xpo;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using CIIP;
using System.Linq;
using DevExpress.ExpressApp.SystemModule;

namespace CIIP.StateMachine
{
    [ImageName("BO_StateMachine"),
     DefaultProperty("Name"),
     XafDisplayName("流程状态"),
     RuleCriteria("CIIPXpoStateMachine.StartState", DefaultContexts.Save, "Active = false Or (StartState is not null And Active = true)", SkipNullOrEmptyValues = false)
    ]
    [NavigationItem("审批流程")]
    public class CIIPXpoStateMachine : StateMachineObjectBase, IStateMachine, IStateMachineUISettings,IFlow
    {
        public CIIPXpoStateMachine(Session s) : base(s)
        {

        }



        [VisibleInListView(false)]
        public CIIPXpoStateMachine Flow
        {
            get { return this; }
        }

        [Browsable(false)]
        public bool IsDesignMode
        {
            get { return true; }
        }
        
        private IObjectSpace objectSpace;
        
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.objectSpace = XPObjectSpace.FindObjectSpaceByObject(this);
        }

        public void ExecuteTransition(object targetObject, IState targetState)
        {
            new StateMachineLogic(this.objectSpace).ExecuteTransition(targetObject, targetState);
        }

        public IState FindCurrentState(object targetObject)
        {
            return new StateMachineLogic(this.objectSpace).FindCurrentState(this, targetObject, this.StartState);
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            this.objectSpace = XPObjectSpace.FindObjectSpaceByObject(this);
        }

        IFlowAction IFlow.CreateAction(IFlowNode from, IFlowNode to)
        {
            var obj = new CIIPXpoTransition(Session);

            obj.SourceState = from as CIIPXpoState;
            obj.TargetState = to as CIIPXpoState;
            obj.Caption = obj.TargetState.Caption;
            return obj;
        }

        IFlowNode IFlow.CreateNode(int x, int y, int width, int height, string form, string caption)
        {
            var obj = new CIIPXpoState(Session);
            obj.StateMachine = this;
            obj.X = x;
            obj.Y = y;
            obj.Width = width;
            obj.Height = height;
            obj.Caption = caption;
            return obj;
        }

        void IFlow.RemoveNode(IFlowNode node)
        {
            States.Remove((CIIPXpoState)node);
        }

        void IFlow.RemoveAction(IFlowAction action)
        {
            ((CIIPXpoState)action.From).Transitions.Remove((CIIPXpoTransition)action);
        }

        void IFlow.ShowNodesView(ShowNodesEventParameter p)
        {
            CIIPXpoState obj;
            if (p.Shape == null)
            {

                obj = (CIIPXpoState)(this as IFlow).CreateNode((int)p.MouseClickPoint.X, (int)p.MouseClickPoint.Y, 64, 64, "", "");
            }
            else
            {
                obj = p.SelectedNode as CIIPXpoState;
            }

            if (obj == null)
                throw new Exception("没有状态对象！");

            var view = p.Application.CreateDetailView(p.ObjectSpace, obj, false);
            p.ViewParameter.CreatedView = view;
            p.ViewParameter.TargetWindow = TargetWindow.NewModalWindow;

            var dc = new DialogController();
            dc.Accepting += (s, p1) =>
            {
                var sp = p.Shape;
                if (p.Shape == null)
                {
                    sp = p.CreateShape(obj);
                }

                p.UpdateShape(obj, sp);
                Flow.States.Add(obj);
                obj.Save();
                if (Flow.StartState == null)
                {
                    Flow.StartState = obj;
                }
            };

            dc.Cancelling += (s, p1) =>
            {
                obj.Delete();
                p.DeletSelectedNode();
                //_diagram.DeleteSelectedItems();
            };
            dc.SaveOnAccept = false;
            p.ViewParameter.Controllers.Add(dc);
        }

        [XafDisplayName("有效")]
        public bool Active
        {
            get
            {
                return base.GetPropertyValue<bool>("Active");
            }
            set
            {
                base.SetPropertyValue<bool>("Active", value);
            }
        }

        [Browsable(false)]
        public IList<StringObject> AvailableStatePropertyNames
        {
            get
            {
                List<StringObject> list = new List<StringObject>();
                if (this.TargetObjectType != null)
                {
                    foreach (string str in new StateMachineLogic(this.objectSpace).FindAvailableStatePropertyNames(this.TargetObjectType))
                    {
                        list.Add(new StringObject(str));
                    }
                }
                return list;
            }
        }

        string IStateMachine.StatePropertyName
        {
            get
            {
                if (this.StatePropertyName == null)
                {
                    return "";
                }
                return this.StatePropertyName.Name;
            }
        }

        IList<IState> IStateMachine.States
        {
            get
            {
                List<IState> list = new List<IState>();
                foreach (var state in this.States)
                {
                    list.Add(state);
                }
                return list;
            }
        }
        [XafDisplayName("在详细视图中显示按钮")]
        public bool ExpandActionsInDetailView
        {
            get
            {
                return base.GetPropertyValue<bool>("ExpandActionsInDetailView");
            }
            set
            {
                base.SetPropertyValue<bool>("ExpandActionsInDetailView", value);
            }
        }

        [XafDisplayName("名称")]
        public string Name
        {
            get
            {
                return base.GetPropertyValue<string>("Name");
            }
            set
            {
                base.SetPropertyValue<string>("Name", value);
            }
        }

        [DataSourceProperty("States")]
        [XafDisplayName("初始状态")]
        public CIIPXpoState StartState
        {
            get
            {
                return base.GetPropertyValue<CIIPXpoState>("StartState");
            }
            set
            {
                base.SetPropertyValue("StartState", value);
            }
        }

        [DataSourceProperty("AvailableStatePropertyNames"), ValueConverter(typeof(StringObjectToStringConverter)), RuleRequiredField("GepXpoStateMachine.StatePropertyName", DefaultContexts.Save)]
        [XafDisplayName("状态属性")]
        public StringObject StatePropertyName
        {
            get
            {
                return base.GetPropertyValue<StringObject>("StatePropertyName");
            }
            set
            {
                base.SetPropertyValue<StringObject>("StatePropertyName", value);
            }
        }

        [Association("StateMachine-States"), RuleUniqueValue("GepXpoStateMachine.UniqueStateMarker", DefaultContexts.Save, TargetPropertyName = "MarkerValue"), DevExpress.ExpressApp.DC.Aggregated]
        [XafDisplayName("状态")]
        public XPCollection<CIIPXpoState> States
        {
            get
            {
                return base.GetCollection<CIIPXpoState>("States");
            }
        }

        [RuleRequiredField("GepXpoStateMachine.TargetObjectType", DefaultContexts.Save), ValueConverter(typeof(TypeToStringConverter)), TypeConverter(typeof(StateMachineTypeConverter)), ImmediatePostData]
        [XafDisplayName("目标类型")]
        public Type TargetObjectType
        {
            get
            {
                return base.GetPropertyValue<Type>("TargetObjectType");
            }
            set
            {
                base.SetPropertyValue<Type>("TargetObjectType", value);
            }
        }

        IFlowNode IFlow.SelectedNode
        {
            get
            {
                return null;
            }

            set
            {
                
            }
        }

        IEnumerable<IFlowNode> IFlow.Nodes
        {
            get
            {
                return this.States;
            }
        }

        IEnumerable<IFlowAction> IFlow.Actions
        {
            get
            {
                return this.States.SelectMany(x => x.Transitions);
            }
        }
    }
}