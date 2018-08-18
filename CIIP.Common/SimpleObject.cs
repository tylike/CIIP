using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.Linq;
using DevExpress.Persistent.Base;
using CIIP;

namespace 常用基类
{
    //[NonPersistent]
    //public abstract class SimpleObject<T> : IMatrixBaseObject
    //{
    //    public SimpleObject(Session s) : base(s)
    //    {

    //    }
    //    private T _Oid;
    //    [Key]
    //    public T Oid
    //    {
    //        get { return _Oid; }
    //        set { SetPropertyValue("Oid", ref _Oid, value); }
    //    }
    //}

    public class Logic<T> : Logic
        where T : SimpleObject
    {
        public virtual void AfterConstructionCore(T instance, Session s)
        {

        }
        [Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
        public override void AfterConstruction(object instance, Session s)
        {
            AfterConstructionCore((T) instance, s);
        }

        public virtual void OnSavingCore(T instance, Session s)
        {

        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override void OnSaving(object instance, Session s)
        {
            OnSavingCore((T) instance, s);
        }

        public virtual void OnsavedCore(T instance, Session s)
        {

        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override void OnDeleted(object instance, Session s)
        {
            OnDeletedCore((T) instance, s);
        }

        public virtual void OnDeletingCore(T instance, Session s)
        {

        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override void OnDeleting(object instance, Session s)
        {
            OnDeletingCore((T) instance, s);
        }
        
        public virtual void OnDeletedCore(T instance, Session s)
        {

        }

        public virtual void OnChangedCore(T instance, Session s, string propertyName, object oldValue, object newValue)
        {
           
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override void OnChanged(SimpleObject simpleObject, Session session, string propertyName, object oldValue,
            object newValue)
        {
            OnChangedCore((T) simpleObject, session, propertyName, oldValue, newValue);
        }

    }
    
    [NonPersistent]
    public abstract class SimpleObject : BaseObject
    {
        public static void RegisterLogic<T>(Logic logic)
        {
            if (!Logics.ContainsKey(typeof(T)))
            {
                Logics.Add(typeof(T), new List<Logic>());
            }
            Logics[typeof(T)].Add(logic);
        }

        public static Dictionary<Type, List<Logic>> Logics { get; } = new Dictionary<Type, List<Logic>>();
        private List<Logic> _logics;
        private List<Logic> logics
        {
            get
            {
                if (_logics == null)
                {
                    var t = this.GetType();
                    if (Logics.ContainsKey(t))
                        _logics = Logics[t];
                }
                return _logics;
            }
        }

        public SimpleObject(Session s) : base(s)
        {

        }       

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            创建者 = SecuritySystem.CurrentUserName;
            创建时间 = DateTime.Now;
            if (logics != null)
            {
                foreach (var logic in logics)
                {
                    logic.AfterConstruction(this, Session);
                }
            }
        }

        protected override void OnSaved()
        {
            base.OnSaved();
            if (logics != null)
            {
                foreach (var logic in logics)
                {
                    logic.Onsaved(this, Session);
                }
            }
        }

        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (logics != null)
            {
                foreach (var logic in logics)
                {
                    logic.OnDeleting(this, Session);
                }
            }
        }

        protected override void OnDeleted()
        {
            base.OnDeleted();
            if (logics != null)
            {
                foreach (var logic in logics)
                {
                    logic.OnDeleted(this, Session);
                }
            }
        }

        protected override void OnSaving()
        {
            修改者 = SecuritySystem.CurrentUserName;
            修改时间 = DateTime.Now;
            base.OnSaving();
            if (logics != null)
            {
                foreach (var logic in logics)
                {
                    logic.OnSaving(this, Session);
                }
            }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (logics != null)
            {
                foreach (var logic in logics)
                {
                    logic.OnChanged(this, Session,propertyName,oldValue,newValue);
                }
            }
        }
        
        private string _创建者;

        [ModelDefault("AllowEdit", "False")]
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

        [ModelDefault("DisplayFormat", "yyyy-MM-dd hh:mm:ss")]
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