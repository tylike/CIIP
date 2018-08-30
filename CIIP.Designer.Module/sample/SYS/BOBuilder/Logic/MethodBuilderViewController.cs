using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using IMatrix.ERP.Module.BusinessObjects.SYS.Logic;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;

namespace IMatrix.ERP.Module.BusinessObjects.SYS.BOBuilder.Logic
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class MethodBuilderViewController : ViewController<ListView>
    {
        public MethodBuilderViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof (MethodDefine);
            //TargetViewType = ViewType.ListView;
            TargetViewNesting = Nesting.Nested;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (this.View.CurrentObject == null)
            {
                View_SelectionChanged(null, null);
            }
            this.View.SelectionChanged += View_SelectionChanged;
            // Perform various tasks depending on the target View.
        }

        private void View_SelectionChanged(object sender, EventArgs e)
        {
            var obj = View.SelectedObjects.OfType<object>().FirstOrDefault();
            AddStatement.Items.Clear();

            if (obj == null)
            {
                obj = (ObjectSpace.Owner as DetailView).CurrentObject;
            }

            var childrenTypeAtt =
                Application.Model.BOModel.GetClass(obj.GetType()).TypeInfo.FindAttribute<ChildrenTypeAttribute>();
            if (childrenTypeAtt != null)
            {
                var type = Application.Model.BOModel.GetClass(childrenTypeAtt.ChildrenType).TypeInfo;
                var childrenTypes = type.Descendants.Where(x => x.IsPersistent && !x.IsAbstract).ToList();
                if (type.IsPersistent && !type.IsAbstract)
                {
                    childrenTypes.Add(type);
                }

                var clses = childrenTypes.Select(item => Application.Model.BOModel.GetClass(item.Type)).ToList();

                var sorted = clses.OrderBy(x => x.Index).ToArray();
                foreach (var item in sorted)
                {
                    var choiceItem = new ChoiceActionItem(item.Caption, item.TypeInfo.Type) {ImageName = item.ImageName};
                    this.AddStatement.Items.Add(choiceItem);
                    Debug.WriteLine(item.Caption, item.ImageName);
                }
            }


        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void AddStatement_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var type = e.SelectedChoiceActionItem.Data as Type;
            var newObj = ObjectSpace.CreateObject(type) as LogicCodeUnit;
            var cs = View.CurrentObject as LogicCodeUnit;
            if (cs == null)
            {
                this.View.CollectionSource.Add(newObj);
            }
            else
            {
                newObj.Index = cs.ChildrenUnits.Count*100;
                cs.ChildrenUnits.Add(newObj);
            }
            View.CurrentObject = newObj;
        }
    }
}
