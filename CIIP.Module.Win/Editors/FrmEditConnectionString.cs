using CIIP.Module.BusinessObjects.SYS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CIIP.Module.Win.Editors
{
    public partial class FrmEditConnectionString : Form
    {
        public FrmEditConnectionString()
        {
            InitializeComponent();

            this.richTextBox1.Text = AdmiralEnvironment.ReadConnectionString();
            if (string.IsNullOrEmpty(this.richTextBox1.Text))
            {
                if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
                {
                    //winApplication.ConnectionString = 
                    this.richTextBox1.Text = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    this.Text += "**本次必须按下保存按钮才能应用配置！";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdmiralEnvironment.WriteConnectionString(this.richTextBox1.Text);
            Thread.Sleep(500);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
