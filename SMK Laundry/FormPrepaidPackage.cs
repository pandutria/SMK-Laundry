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
    public partial class FormPrepaidPackage : Form
    {
        private DatabaseDataContext db = new DatabaseDataContext();
        int currentSelectedRow = 1;
        public FormPrepaidPackage()
        {
            InitializeComponent();
        }

        private void loadDgvData()
        {
            dgvData.Columns.Clear();
            var query = db.PrepaidPackages.Where(x => x.Customer.Name.Contains(tbSearch.Text) || x.Package.Service.Name.Contains(tbSearch.Text)) 
                .Select(x => new { PrepaidPackageID = x.Id, Customer = x.Customer.Name, PackageName = x.Package.Service.Name + " " + x.Package.TotalUnit + x.Package.Service.Unit.Name, x.Price, Service = x.Package.Service.Name, Totalunit = x.Package.TotalUnit, Unit = x.Package.Service.Unit.Name, idCustomer = x.Customer.Id});

            dgvData.DataSource = query;

            
            dgvData.Columns["Service"].Visible = false;
            dgvData.Columns["Unit"].Visible = false;
            dgvData.Columns["Totalunit"].Visible = false;
            dgvData.Columns["idCustomer"].Visible = false;
        }

        private void loadCboPackage()
        {
            var query = db.Packages.Select(x => new {x.Id, x.Service.Name} );
            cboPackage.DataSource = query;
            cboPackage.ValueMember = "Id";
            cboPackage.DisplayMember = "Name";
        }

        private void FormPrepaidPackage_Load(object sender, EventArgs e)
        {
            loadDgvData();
            loadCboPackage();
        }

        private bool validation()
        {
            if (tbPhoneNumber.Text == string.Empty || tbPrepaidPackageID.Text == string.Empty
                || cboPackage.Text == string.Empty || nupPrice.Value == 0)
            {
                Support.MSI("all field must be field");
                return false;
            }

            return true;
        }

        private void btnSubmit_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (validation())
                {
                    var pp = new PrepaidPackage();
                    //pp.Id = Convert.ToInt32(tbPrepaidPackageID.Text);
                    pp.IdPackage = Convert.ToInt32(cboPackage.SelectedValue);
                    pp.IdCustomer = Convert.ToInt32(lblCustomerId.Text);
                    pp.Price = Convert.ToInt32(nupPrice.Value);
                    pp.StartDateTime = DateTime.Now;
                    pp.CompletedDatetime = null;
                    db.PrepaidPackages.InsertOnSubmit(pp);
                    db.SubmitChanges();
                    Support.MSI("berhasillll");
                    loadDgvData();

                }

            }
            catch (Exception ex)
            {
                Support.MSI(ex.Message);
            }
        }


        private void linkAddNewCustomer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new FormAddNewCustomer().ShowDialog();
        }
   

        private void linkAddNewCustomer_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new FormAddNewCustomer().ShowDialog();
        }

        private void tbPhoneNumber_TextChanged_1(object sender, EventArgs e)
        {
            var query = db.Customers.FirstOrDefault(x => x.PhoneNumber == tbPhoneNumber.Text);

            if (query != null)
            {
                lblAddress.Text = query.Address;
                lblName.Text = query.Name;
                lblCustomerId.Text = query.Id.ToString();
                DataStorage.customerId = query.Id;
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            loadDgvData();
        }

       
    }
}
