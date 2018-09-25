using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace CIIP.Designer
{
    public class InterfaceViewController : ObjectViewController<ObjectView,Interface>
    {
        public InterfaceViewController()
        {
            var createDefaultInterfaceImplement = new SimpleAction(this, "CreateDefaultInterfaceImplement", PredefinedCategory.Unspecified);
            createDefaultInterfaceImplement.Caption = "生成";
            createDefaultInterfaceImplement.Execute += CreateDefaultInterfaceImplement_Execute;
        }

        private void CreateDefaultInterfaceImplement_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var obj = ObjectSpace.CreateObject<BusinessObject>();
            obj.Name = this.ViewCurrentObject.Name + "_Impl";
            var ii = ObjectSpace.CreateObject<ImplementInterface>();
            ii.ImplementInterfaceInfo = ViewCurrentObject;
            obj.ImplementInterfaces.Add(ii);
            ViewCurrentObject.DefaultImplement = obj;
            //foreach (var item in this.ViewCurrentObject.Properties)
            //{

            //}

            //接口有对应默认实现类
            //如何生成代码?
            //遍历,接口属性,生成默认的方法,即自动实现,类似于vs中的 快速按钮,点击后,实现了接口.
            //
        }
    }
}