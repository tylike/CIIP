using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CIIP.Module.BusinessObjects.Flow
{
    public class FormConvertRule : BaseObject
    {
        private FormConverter _Converter;
        [Association]
        public FormConverter Converter
        {
            get { return _Converter; }
            set { SetPropertyValue("Converter", ref _Converter, value); }
        }


        public IModelMember From { get; set; }

        public IModelMember To { get; set; }
    }
}