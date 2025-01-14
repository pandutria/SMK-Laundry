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
    public partial class FormLogin : Form
    {
        private DatabaseDataContext db = new DatabaseDataContext();
        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            tbEmail.Text = tbPassword.Text = "pandu";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var query = db.Employees.FirstOrDefault(x => x.Email == tbEmail.Text && x.Password == tbPassword.Text);
            if (query != null)
            {
                new FormMain().Show();
                Hide();
                DataStorage.employeeId = query.Id;
                //Support.MSI("Berhasil");
            }
            else Support.MSW("Please Try Again, Your Data is not Valid!");
        }
    }
}
