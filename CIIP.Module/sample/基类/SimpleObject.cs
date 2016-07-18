using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace IMatrix.ERP.Module.BusinessObjects
{
    [NonPersistent]
    public class SimpleObject : BaseObject
    {
        public SimpleObject(Session s) : base(s)
        {
            
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            创建者 = SecuritySystem.CurrentUserName;
            创建时间 = DateTime.Now;
        }

        protected override void OnSaving()
        {
            修改者 = SecuritySystem.CurrentUserName;
            修改时间 = DateTime.Now;
            base.OnSaving();
        }

        private string _创建者;
        [ModelDefault("AllowEdit","False")]
        public string 创建者
        {
            get { return _创建者; }
            set { SetPropertyValue("创建者", ref _创建者, value); }
        }
        
        private string _修改者;
        [ModelDefault("AllowEdit", "False")]
        public string 修改者
        {
            get { return _修改者; }
            set { SetPropertyValue("修改者", ref _修改者, value); }
        }


        private DateTime _创建时间;
        [ModelDefault("DisplayFormat","yyyy-MM-dd hh:mm:ss")]
        [ModelDefault("AllowEdit", "False")]
        public DateTime 创建时间
        {
            get { return _创建时间; }
            set { SetPropertyValue("创建时间", ref _创建时间, value); }
        }


        private DateTime _修改时间;
        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd hh:mm:ss")]
        public DateTime 修改时间
        {
            get { return _修改时间; }
            set { SetPropertyValue("修改时间", ref _修改时间, value); }
        }


    }
}