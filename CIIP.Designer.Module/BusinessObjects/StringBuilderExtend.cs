using DevExpress.Xpo;
using System.Text;

namespace CIIP.Designer
{
    public static class StringBuilderExtend
    {
        public static void ModelDefault(this StringBuilder self, string name,string value)
        {
            self.Append($"\t\t[ModelDefault(\"{name}\",\"{value}\")]");
        }
        public static void Assocication(this StringBuilder self,string name)
        {
            self.AppendFormat("\t\t[{0}(\"{1}\")]", typeof(AssociationAttribute).FullName, name);
        }
        public static void Assocication(this StringBuilder self)
        {
            self.AppendFormat("\t\t[{0}]", typeof(AssociationAttribute).FullName);
        }

        public static void Aggregated(this StringBuilder self)
        {
            self.Append("\t\t[" + typeof(AggregatedAttribute).FullName + "]");
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