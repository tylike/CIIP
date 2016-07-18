using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using 常用基类;

namespace CIIP.Module.BusinessObjects
{
    public class 财务记录 : SimpleObject
    {
        public 财务记录(Session s) : base(s)
        {
        }

        private decimal _金额;

        public decimal 金额
        {
            get { return _金额; }
            set { SetPropertyValue("金额", ref _金额, value); }
        }

        private IEnumerable<款项分类> _source;

        protected virtual IEnumerable<款项分类> Source
        {
            get
            {
                if (_source == null)
                {
                    var type = this.GetType().FullName;
                    _source = Session.Query<款项分类>().Where(
                        x => x.业务对象.Where(
                            t => t.Oid == type
                            ).Count() > 0
                        );
                }
                return _source;
            }
        }

        private 款项分类 _款项分类;

        public 款项分类 款项分类
        {
            get { return _款项分类; }
            set { SetPropertyValue("款项分类", ref _款项分类, value); }
        }
    }
}