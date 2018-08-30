using System.Diagnostics;
using DevExpress.ExpressApp.Win.Utils;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CIIP {
    public partial class WinSplashForm : SplashScreen
    {
        static WinSplashForm()
        {
            //sw = Stopwatch.StartNew();
            //sw.Start();
        }

        public WinSplashForm() {
            InitializeComponent();
            
        }
        


        public static Bitmap LoadingImage
        {
            get { return global::CIIP.Module.Win.Properties.Resources.Splash; }
        }

        private void LblDisplayText_Click(object sender, EventArgs e)
        {

        }
        
        private void WinSplashForm_Load(object sender, EventArgs e)
        {
            //UnitOfWork u = GepContext.Instance.NewUnitOfWork();
            //if (u != null)
            //{
            //    //u.UpdateSchema();
                
            //}
        }

        public override void ProcessCommand(Enum cmd, object arg)
        {
            var t = (UpdateSplashCommand)cmd;
            switch (t)
            {
                case UpdateSplashCommand.Description:
                    this.lblMessage.Text = (string)arg;
                    break;
                case UpdateSplashCommand.Caption:
                    this.lblMessage.Text = (string)arg;
                    break;
            }
            base.ProcessCommand(cmd, arg);
        }

        //private static Stopwatch sw;
        //public override void ProcessCommand(Enum cmd, object arg)
        //{
        //    //if (object.Equals(cmd , UpdateSplashCommand.Caption))
        //    //{
        //    //    caption.Text = (string)arg;
        //    //}
        //    sw.Stop();
        //    //this.richTextBox1.AppendText(sw.ElapsedMilliseconds.ToString()+"\n");
        //    //richTextBox1.AppendText(cmd.ToString()+":");
        //    //if (arg is Array)
        //    //{
        //    //    foreach (var a in (Array) arg)
        //    //    {
        //    //        richTextBox1.AppendText(a+"");
        //    //        richTextBox1.AppendText(" ");
        //    //    }
        //    //    richTextBox1.AppendText("\n");
        //    //}
        //    //else
        //    //{
        //    //    richTextBox1.AppendText(arg + "\n");
        //    //}

        //    //if (object.Equals(cmd, UpdateSplashCommand.Description))
        //    //{
        //    //    descript.Text = (string)arg;
        //    //}

        //    base.ProcessCommand(cmd, arg);
        //    sw.Restart();
        //}


    }
}
