using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace CIIP.Designer
{
    [NonPersistent]
    public class BatchInputBusinessObject : BaseObject
    {
        public BatchInputBusinessObject(Session s) : base(s)
        {

        }

        [Size(-1)]
        [XafDisplayName("快速代码")]
        public string Code
        {
            get { return GetPropertyValue<string>(nameof(Code)); }
            set { SetPropertyValue(nameof(Code), value); }
        }

        //规则1:名称1,名称2,名称3,名称4 为4个业务对象名称.建立出4张表.
        //规则2:名称{ 名称1,名称2,名称3 } 为一个类中包含了3个string属性
        //规则3:名称{ string 名称1,int 名称2,datetime 名称3} 为一个类中指定了三个有类型的属性.
        [RuleFromBoolProperty]
        public bool Error
        {
            get
            {
                return false;
            }
        }


        [XafDisplayName("说明")]
        public string Memo
        {
            get
            {
                return "";
            }
        }
    }
    public class QuickAddBusinessObject : ObjectViewController<ListView, BusinessObject>
    {
        public QuickAddBusinessObject()
        {
            var batchCreate = new PopupWindowShowAction(this, "BatchCreate", PredefinedCategory.Unspecified);
            batchCreate.Execute += BatchCreate_Execute;
            batchCreate.CustomizePopupWindowParams += BatchCreate_CustomizePopupWindowParams;

        }

        private void BatchCreate_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace();
            var para = os.CreateObject<BatchInputBusinessObject>();
            var view = Application.CreateDetailView(os, para);
            //Application.ShowViewStrategy.ShowViewInPopupWindow(view, () => { });

            e.View = view;
        }

        private void BatchCreate_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {

        }
    }

#warning 需要验证属性名称不可以重名的情况.


#warning 此功能可以后续实现,当前可以使用复制功能直接copy已有布局
    // 业务类型上面,使用Attribute指定使用哪个布局模板
    // 系统起动时,检查所有使用了Attribute的类,遍历并进行更新

    //[LayoutTemplate(typeof(布局模板)] 
    //泛型参数类型应该是: 某单据,单据明细 两个类型.
    //这种情况,只支持两种类型,如果基类中有多个类型,就按顺序传入,反射取得,无需处理.
    //public class 某单据 :  ......
    //{
    //}
}