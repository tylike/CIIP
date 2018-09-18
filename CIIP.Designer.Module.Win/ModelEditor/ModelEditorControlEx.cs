using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Win.Core.ModelEditor;
using DevExpress.ExpressApp.Win.Core.ModelEditor.NodesTree;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
namespace CIIP.Win.ModelEditor
{
    [ToolboxItem(false)]
    public class ModelEditorControlEx : ModelEditorControl
    {

        public ModelEditorControlEx(ModelTreeList treeList,SettingsStorage settings) : base(treeList)
        {
            var fs = typeof(ModelEditorControl).GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var field = fs.Single(x => x.Name == "settings"); //this.GetType().GetField("settings", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            
            field.SetValue(this, settings);
            //ReflectionHelper..SetMemberValue(this, "settings", settings);
        }
    }

    public class CIIPExtendModelInterfaceAdapter: ExtendModelInterfaceAdapter
    {
        public override string GetDisplayPropertyValue(object nodeObject)
        {
            var t = ((ModelTreeListNode)nodeObject).ModelNode;
            Debug.WriteLine(t);
            
            var caption = base.GetDisplayPropertyValue(nodeObject);
            switch (caption)
            {
                case "ActionDesign":
                    return "按钮设置";
                case "Actions":
                    return "按钮";

                case "ActionToContainerMapping":
                    return "容器映射";
                case "Controllers":
                    return "控制器";
                case "DisableReasons":
                    return "禁用原因";
                case "BOModel":
                    return "模型";
                case "CreatableItems":
                    return "快速创建";
                case "ImageSources":
                    return "图像来源";
                case "Localization":
                    return "本地化";
                case "NavigationItems":
                    return "导航设置";
                case "Options":
                    return "选项";
                case "Validation":
                    return "验证";
                case "ViewItems":
                    return "视图项目";
                case "Views":
                    return "视图";
                default:
                    break;
            }
  
            
            return caption;
        }
    }
}
