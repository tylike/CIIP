namespace CIIP.Module.BusinessObjects.SYS
{
    public static class BusinessObjectCodeGenerateExtendesion
    {
        public static string CommonUsing()
        {
            return @"using System;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using System.Linq;
using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using 常用基类;
using 基础信息;
using DevExpress.Persistent.Validation;
";
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