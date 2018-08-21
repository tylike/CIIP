using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using System.ComponentModel;
using CIIP.Module.BusinessObjects;
using 常用基类;
using DevExpress.ExpressApp.Model;

namespace CIIP.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class FlowViewController : ViewController<DetailView>
    {
        public FlowViewController()
        {

            InitializeComponent();
            TargetObjectType = typeof(单据);
            //FlowToNext.TargetObjectsCriteria = "审核状态='已审核'";
            FlowToNext.Execute += FlowToNext_Execute;
            //单据创建时执行：在commiting时执行规则
            //单据满足条件时执行：在commiting时，判断是否满足条件，再执行
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        
        private void FlowToNext_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            ObjectSpace.CommitChanges();
            var newFormOs = Application.CreateObjectSpace();
            
            var fromObject = newFormOs.GetObject(this.View.CurrentObject as 单据);
            
            var action = e.SelectedChoiceActionItem?.Data as FlowAction;
            if (action != null)
            {
                ExecuteAction(newFormOs, fromObject, action);
            }
        }

        private void ExecuteAction(IObjectSpace newFormOs, 单据 fromObject, FlowAction action)
        {
            //action数据是来自于 初始化按钮时的os,它可能不是当前objectspace
            action = ObjectSpace.GetObject(action);
            var os = new NonPersistentObjectSpace(null);

            
            var toType = ReflectionHelper.FindType(action.To.Form);

            #region 生成目标单据主表属性
            单据 toObject = null;
            //查找当前单据是从那张单据生成过来的
            var record = ObjectSpace.FindObject<单据流程状态记录>(CriteriaOperator.Parse("目标单据=?", CurrentObject.GetMemberValue("Oid").ToString()));

            if (action.目标类型 == 目标类型.生成单据)
            {
                toObject = newFormOs.CreateObject(toType) as 单据;
            }
            else if (action.目标类型 == 目标类型.更新单据)
            {
                var history = ObjectSpace.FindObject<单据流程状态记录>(CriteriaOperator.Parse("来源单据=? and 执行动作.Oid=?", CurrentObject.GetMemberValue("Oid").ToString(), action.Oid));
                if (history != null)
                {
                    //更新动作已经执行过了，不要再执行。
                    return;
                }
                    
                //如果是更新单据，那必然是有已经生成过的单据。
                if (record != null){
                    toObject = newFormOs.GetObject( record.来源单据);// newFormOs.GetObjectByStringKey(action.ToType, record.来源单据) as 单据;
                }
                else
                {
                    return;
                }
                if (toObject == null)
                    return;
            }

            var clsname = action.GetFlowLogicClassFullName();
            var actionLogicType = ReflectionHelper.FindType(clsname);
            IExecuteFlowAction actionLogic = null;
            if (actionLogicType != null)
            {
                actionLogic = Activator.CreateInstance(actionLogicType) as IExecuteFlowAction;
                if (actionLogic != null)
                {
                    actionLogic.ExecuteMasterCore(fromObject, toObject);
                }
            }

            #endregion
            
            #region 生成目标单据子表记录
            var fromItems = fromObject.GetMemberValue("明细项目") as XPBaseCollection;
            var toItems = toObject.GetMemberValue("明细项目") as XPBaseCollection;
            var toItemType = toItems.GetObjectClassInfo().ClassType;
            //来源单据的明细,当前单据
            var detailRecords = new List<Tuple<string, string>>();
            foreach (XPBaseObject item in fromItems)
            {
                //查找记录？
                XPBaseObject toItem = null;
                if (action.目标类型 == 目标类型.更新单据)
                {
                    //如果是更新单据（反写）时，没有找到反写目标明细，则继续。忽略当前条目。
                    //recs可以查到来源明细
                    //
                    var recs = record.明细.FirstOrDefault(x => x.目标 == item.GetMemberValue("Oid").ToString());
                    toItem = (toObject.GetMemberValue("明细项目") as IList).OfType<BaseObject>().FirstOrDefault(x => x.Oid.ToString() == recs.来源);
                    if (toItem == null)
                        continue;
                }
                else if (action.目标类型 == 目标类型.生成单据)
                {
                    toItem = newFormOs.CreateObject(toItemType) as XPBaseObject;
                }
                else
                {
                    throw new Exception("未处理的目标类型！");
                }
                toItems.BaseAdd(toItem);

                if (actionLogic != null)
                    actionLogic.ExecuteChildrenCore(item, toItem);
                //foreach (var f in action.ItemsMapping)
                //{
                //    var fromValue = item.Evaluate(f.FromProperty.Name);
                //    toItem.SetMemberValue(f.ToProperty.Name, fromValue);
                //}

                detailRecords.Add(new Tuple<string, string>(item.GetMemberValue("Oid").ToString(), toItem.GetMemberValue("Oid").ToString()));
            }

            #region 生成主表记录
            EventHandler act = null;
            act = (snd, evt) =>
            {

                newFormOs.Committed -= act;
                var process = ObjectSpace.CreateObject<单据流程状态记录>();
                process.来源单据 = this.CurrentObject as 单据;
                process.目标单据 = this.ObjectSpace.GetObject(toObject);

                process.执行动作 = action;
                process.业务项目 = process.来源单据.业务项目;
                foreach (var item in detailRecords)
                {
                    var det = ObjectSpace.CreateObject<单据流程记录明细>();
                    det.来源 = item.Item1;
                    det.目标 = item.Item2;
                    process.明细.Add(det);
                }

                ObjectSpace.CommitChanges();
            };
            newFormOs.Committed += act;
            #endregion

            #endregion

            if (action.自动保存)
            {
                newFormOs.CommitChanges();
            }

            if (action.显示编辑界面)
            {
                var para = new ShowViewParameters();
                para.CreatedView = Application.CreateDetailView(newFormOs, toObject, true);
                para.TargetWindow = TargetWindow.Default;

                Application.ShowViewStrategy.ShowView(para, new ShowViewSource(this.Frame, this.FlowToNext));
                //e.ShowViewParameters.CreatedView = 
            }
        }

        static List<FlowAction> _actions;
        public static List<FlowAction> ActionInfos
        {
            get
            {
                return _actions;
            }
            set
            {
                _actions = value;
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            if (ActionInfos == null)
            {
                ActionInfos = ObjectSpace.GetObjects<FlowAction>().ToList();
            }

            this.ObjectSpace.Committed += ObjectSpace_Committed;
            
            var currentObject = CurrentObject as 单据;

            this.FlowToNext.Items.Clear();
            //生成按钮
            var currentTypeName = this.View.Model.ModelClass.TypeInfo.FullName;
            foreach (var item in ActionInfos.Where(x => x.动作类型 == 动作类型.点击按钮执行 && x.From.Form == currentTypeName))
            {
                var choiceItem = new ChoiceActionItem(item.To.Caption, item);
                var type = Application.Model.BOModel.GetClass(ReflectionHelper.FindType(item.To.Form));
                choiceItem.ImageName = type.ImageName;

                if (!string.IsNullOrEmpty(item.生效条件))
                {
                    choiceItem.Enabled.SetItemValue("流程按钮生效条件",
                        (bool)(this.View.CurrentObject as XPBaseObject).Evaluate(item.生效条件));

                    currentObject.Changed += (s, e) =>
                    {
                        choiceItem.Enabled.SetItemValue("流程按钮生效条件",
                            (bool) (this.View.CurrentObject as XPBaseObject).Evaluate(item.生效条件));
                    };
                }

                //判断是否可以生成：
                //1.是否为不限制生成次数
                //2.判断数量是否足够用
                var 已生成单据记录 = ObjectSpace.GetObjects<单据流程状态记录>(new BinaryOperator("来源单据", this.CurrentObject.GetMemberValue("Oid")));

                if (item.MultiGenerate == 单据转换多次生成控制.不允许多次生成 && 已生成单据记录.Count > 0)
                {
                    choiceItem.Enabled.SetItemValue("不允许多次生成", false);
                }
                else if (item.MultiGenerate == 单据转换多次生成控制.允许多次生判断条件)
                {
                    var gen = false;
                    //允许多次生成
                    var items = currentObject.GetMemberValue("明细项目") as IEnumerable;
                    foreach (XPBaseObject i in items)
                    {
                        //if i.数量-i.已改库数量 > 0 then 可以处理
                        //并且写入一张表中，写：什么时间反写什么字段为什么内容
                        if ((bool)i.Evaluate(item.明细生效条件))
                        {
                            gen = true;
                            break;
                        }
                    }
                    choiceItem.Enabled.SetItemValue("明细满足多次生成条件", gen);
                }

                FlowToNext.Items.Add(choiceItem);
            }

            // Perform various tasks depending on the target View.
        }

        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            //检查规则，并执行。有条件、无条件情况
            var actions = ActionInfos.Where(x => x.动作类型 == 动作类型.单据创建时执行 || x.动作类型 == 动作类型.单据满足条件时执行);
            foreach (var item in actions)
            {
                var isCondition = true;
                if (item.动作类型 == 动作类型.单据满足条件时执行)
                {
                    isCondition = (bool)CurrentObject.Evaluate(item.生效条件);
                }
                if (isCondition)
                {
                    var newOs = Application.CreateObjectSpace();
                    ExecuteAction(newOs, CurrentObject as 单据, item);
                }
            }
        }

        XPBaseObject CurrentObject
        {
            get { return this.View.CurrentObject as XPBaseObject; }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            this.ObjectSpace.Committed -= this.ObjectSpace_Committed;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
