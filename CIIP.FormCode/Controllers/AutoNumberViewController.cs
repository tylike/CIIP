using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using CIIP.FormCode;

namespace ERP.Module.Controllers
{
    public class AutoNumberViewController : ViewController
    {
        // Fields
        private IContainer components = null;

        // Methods
        public AutoNumberViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
        }

        private void ObjectSpace_Committing(object sender, CancelEventArgs e)
        {
            var source = from p in ObjectSpace.ModifiedObjects.OfType<I单据编号>()
                where ObjectSpace.IsNewObject(p)
                select p;
            if (source.Any())
            {
                var dictionary = ObjectSpace.GetObjects<单据编号方案>(null, true).ToDictionary(p => p.应用单据);
                foreach (I单据编号 i单据编号 in source)
                {
                    if (string.IsNullOrEmpty(i单据编号.编号))
                    {
                        var key = i单据编号.GetType();
                        单据编号方案 solution = null;
                        if (dictionary.ContainsKey(key))
                        {
                            solution = dictionary[key];
                            i单据编号.编号 = solution.生成编号(i单据编号 as XPBaseObject);

                        }
                        else
                        {
                            //throw new Exception("错误，没有找到编号方案!");
                        }
                        
                    }
                }
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (ObjectSpace != null)
            {
                ObjectSpace.Committing += ObjectSpace_Committing;
            }
        }

        protected override void OnDeactivated()
        {
            if (ObjectSpace != null)
            {
                ObjectSpace.Committing -= ObjectSpace_Committing;
            }
            base.OnDeactivated();
        }
    }


}
