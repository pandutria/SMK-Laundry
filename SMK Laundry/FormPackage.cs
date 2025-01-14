using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SMK_Laundry
{
    public partial class FormPackage : Form
    {
        private DatabaseDataContext db = new DatabaseDataContext();
        int currentSelectedRow = -1;
        string mode = "";
        public FormPackage()
        {
            InitializeComponent();
        }

        private void loadCboService()
        {
            cboService.DataSource = db.Services;
            cboService.ValueMember = "Id";
            cboService.DisplayMember = "Name";
        }

        private void enableButton(bool e)
        {
            btnInsert.Enabled = e;
            btnDelete.Enabled = e;
            btnUpdate.Enabled = e;

            btnCancel.Enabled = !e;
            btnSave.Enabled = !e;
        }

        private void enableField(bool e)
        {
            tbId.Enabled = !e;
            cboService.Enabled = !e;
            nupPrice.Enabled = !e;
            nupTotalUnit.Enabled = !e;

        }

        private void enableFieldAndButton(bool e)
        {
            enableButton(e);
            enableField(e);
        }

        private void loadDgv()
        {
            dgvData.Rows.Clear();
            var query = db.Packages.Where(x => x.Service.Name.Contains(tbSearch.Text) || x.TotalUnit.ToString().Contains(tbSearch.Text) || x.Price.ToString().Contains(tbSearch.Text))
                .Select(x => new { PackageId = x.Id, ServiceName = x.Service.Name, x.TotalUnit, x.Price });

            dgvData.DataSource = query;

            //var delete = new DataGridViewButtonColumn();
            //delete.Text = "Delete";
            //delete.HeaderText = "delete";
            //delete.Name = "btnDelete";
            //delete.UseColumnTextForButtonValue = true;

            //dgvData.Columns.Add(delete);
        }

        private void FormPackage_Load(object sender, EventArgs e)
        {
            loadDgv();
            loadCboService();
            enableFieldAndButton(true);
            cboService.SelectedValue = 0;
            

            nupTotalUnit.Maximum = 10000000;
            nupPrice.Maximum = 1000000;
            nupPrice.Minimum = 0;
            nupTotalUnit.Minimum = 0;
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                currentSelectedRow = e.RowIndex;
                currentSelectedRow = Convert.ToInt32(dgvData.Rows[e.RowIndex].Cells["PackageId"].Value);
                tbId.Text = dgvData.Rows[e.RowIndex].Cells["PackageId"].Value.ToString();
                cboService.Text = dgvData.Rows[e.RowIndex].Cells["ServiceName"].Value.ToString();
                nupPrice.Value = Convert.ToInt32( dgvData.Rows[e.RowIndex].Cells["Price"].Value);
                nupTotalUnit. Value= Convert.ToInt32( dgvData.Rows[e.RowIndex].Cells["TotalUnit"].Value);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Support.clearFields(this);
            enableFieldAndButton(true);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (currentSelectedRow == -1)
            {
                Support.MSI("click row");
            }
            else
            {
                mode = "update";
                enableFieldAndButton(false);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentSelectedRow == -1)
            {
                Support.MSI("click row");

            }
            else
            {
                var dialog = MessageBox.Show("are you sure want to delete this data?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                try
                {
                    if (dialog == DialogResult.Yes)
                    {
                        var queryDelete = db.Packages.FirstOrDefault(x => x.Id == currentSelectedRow);
                        db.Packages.DeleteOnSubmit(queryDelete);
                        db.SubmitChanges();
                        Support.MSI("delete data berhasil");
                        loadDgv();
                        Support.clearFields(this);
                    }
                }
                catch (Exception ex)
                {
                    Support.MSE(ex.Message);
                }
            }
        }

        private bool validation()
        {
            if (cboService.Text == string.Empty || nupPrice.Value == 0
                || nupTotalUnit.Value == 0)
            {
                Support.MSW("all input must be filled");
                return false;

            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (mode == "insert")
            {
                if (validation())
                {
                    try
                    {
                        var package = new Package();
                        package.IdService = Convert.ToInt32( cboService.SelectedValue);
                        package.Price = Convert.ToInt32(nupPrice.Value);
                        package.TotalUnit = Convert.ToInt32(nupTotalUnit.Value);

                        db.Packages.InsertOnSubmit(package);
                        db.SubmitChanges();
                        loadDgv();
                        Support.clearFields(this);
                        enableFieldAndButton(true);
                        Support.MSI("insert data succses");

                    }
                    catch (Exception ex)
                    {
                        Support.MSE(ex.Message);
                    }
                }
            }

            if (mode == "update")
            {
                try
                {
                    var queryUpdate = db.Packages.FirstOrDefault(x => x.Id == currentSelectedRow);

                    if (queryUpdate != null)
                    {
                        queryUpdate.IdService = Convert.ToInt32(cboService.SelectedValue);
                        queryUpdate.Price = Convert.ToInt32(nupPrice.Value);
                        queryUpdate.TotalUnit = Convert.ToInt32(nupTotalUnit.Value);

                        db.SubmitChanges();
                        loadDgv();
                        Support.clearFields(this);
                        enableFieldAndButton(true);
                        Support.MSI("update data succses");
                    }
                }
                catch (Exception ex)
                {
                    Support.MSE(ex.Message);
                }

            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            mode = "insert";
            enableFieldAndButton(false);
            Support.clearFields(this);
        }
    }
}
