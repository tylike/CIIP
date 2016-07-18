using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.Flow
{
    public class FormConverter : BaseObject
    {
        public FormConverter(Session s) : base(s)
        {

        }

        //[EditorAlias(AdmiralEditors.BusinessObjectSelector)]
        public IModelClass From { get; set; }

        //[EditorAlias(AdmiralEditors.BusinessObjectSelector)]
        public IModelClass To { get; set; }

        /// <summary>
        /// 可以应用到哪些视图
        /// </summary>
        public string ApplyToViews { get; set; }

        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<FormConvertRule> MasterMapping
        {
            get { return GetCollection<FormConvertRule>("MasterMapping"); }
        }

        //[Association,Agg]
        //public XPCollection<FormConvertRule> DetailMapping
        //{
        //    get { return GetCollection<FormConvertRule>("DetailMapping"); }
        //}
    }
}