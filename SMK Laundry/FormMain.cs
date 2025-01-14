using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMK_Laundry
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void manageToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void viewTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tr = new FormViewTransaction();
            tr.MdiParent = this;
            tr.Show();

        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuLogout_Click(object sender, EventArgs e)
        {
            new FormLogin().Show();
            Hide();
            Close();
        }

        private void menuManageEmployee_Click(object sender, EventArgs e)
        {
            var fe = new FormEmployee();
            fe.MdiParent = this; 
            fe.Show();

        }

        private void menuManageService_Click(object sender, EventArgs e)
        {
            var fs = new FormService();
            fs.MdiParent = this;
            fs.Show();
        }

        private void menuManagePackage_Click(object sender, EventArgs e)
        {
            var fp = new FormPackage();
            fp.MdiParent = this;
            fp.Show();
        }

        private void menuTransactionDeposit_Click(object sender, EventArgs e)
        {
            var td = new FormTransactionDeposit();
            td.MdiParent = this;
            td.Show();
        }

        private void menuPrepaidPackage_Click(object sender, EventArgs e)
        {
            var pp = new FormPrepaidPackage();
            pp.MdiParent = this;
            pp.Show();
        }
    }
}
