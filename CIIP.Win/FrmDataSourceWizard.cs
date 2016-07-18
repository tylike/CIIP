using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.UI.Sql;

namespace CIIP.Win
{
    public partial class FrmDataSourceWizard : Form
    {
        public FrmDataSourceWizard()
        {
            InitializeComponent();
        }

        private void FrmDataSourceWizard_Load(object sender, EventArgs e)
        {
            var sds = new SqlDataSource("ConnectionString");
            var ccc = new ConfigureConnectionContext();
            
            SqlDataSourceUIHelper.ConfigureConnection(sds, ccc);
            
        }
    }
}
