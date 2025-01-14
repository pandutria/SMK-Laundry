using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMK_Laundry
{
    public partial class FormService : Form
    {
        private DatabaseDataContext db = new DatabaseDataContext();
        int currentSelectedRow = -1;
        string mode = "";
        public FormService()
        {
            InitializeComponent();
        }

        private void loadDgv()
        {
            dgvData.Rows.Clear();
            var query = db.Services.Where(x => x.Name.Contains(tbSearch.Text) || x.Category.Name.Contains(tbSearch.Text) || x.Unit.Name.Contains(tbSearch.Text))
                .Select(x => new { ServiceId = x.Id, ServiceName = x.Name, Category = x.Category.Name.Replace("Laundry", ""), Unit = x.Unit.Name,Price = x.PriceUnit, x.EstimationDuration});

            dgvData.DataSource = query;
        }

        private void loadCboCategory()
        {
            cboCategory.DataSource = db.Categories;
            cboCategory.ValueMember = "Id";
            cboCategory.DisplayMember = "Name";
        } 

        private void loadCboUnit()
        {
            cboUnit.DataSource = db.Units;
            cboUnit.ValueMember = "Id";
            cboUnit.DisplayMember = "Name";
        }

        private void FormService_Load(object sender, EventArgs e)
        {
            loadDgv();
            loadCboCategory();
            enableFieldAndButton(true);
            loadCboUnit();

            nupPrice.Maximum = 10000000;
            nupPrice.Minimum = 0;
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
            tbName.Enabled = !e;
            cboCategory.Enabled = !e;
            cboUnit.Enabled = !e;
            nupPrice.Enabled = !e;
            nupEstimateDuration.Enabled = !e;

        }

        private void enableFieldAndButton(bool e)
        {
            enableButton(e);
            enableField(e);
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {   
            loadDgv();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                currentSelectedRow = e.RowIndex;
                currentSelectedRow = Convert.ToInt32(dgvData.Rows[e.RowIndex].Cells["ServiceId"].Value);
                tbId.Text = dgvData.Rows[e.RowIndex].Cells["ServiceId"].Value.ToString();
                tbName.Text = dgvData.Rows[e.RowIndex].Cells["ServiceName"].Value.ToString();
                cboCategory.Text = dgvData.Rows[e.RowIndex].Cells["Category"].Value.ToString();
                cboUnit.Text = dgvData.Rows[e.RowIndex].Cells["Unit"].Value.ToString();
                nupPrice.Value = Convert.ToInt32( dgvData.Rows[e.RowIndex].Cells["Price"].Value);
                nupEstimateDuration.Value = Convert.ToInt32(dgvData.Rows[e.RowIndex].Cells["EstimationDuration"].Value);
            }
        }

        private bool validation()
        {
            if (tbName.Text == string.Empty || cboCategory.Text == string.Empty
                || cboUnit.Text == string.Empty || nupPrice.Value == 0)
            {
                Support.MSW("all input must be filled");
                return false;

            }

            return true;
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
                        var queryDelete = db.Services.FirstOrDefault(x => x.Id == currentSelectedRow);
                        db.Services.DeleteOnSubmit(queryDelete);
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (mode == "insert")
            {
                if (validation())
                {
                    try
                    {
                        var service = new Service();
                        service.Name = tbName.Text;
                        service.IdCategory = Convert.ToInt32( cboCategory.SelectedValue);
                        service.IdUnit = Convert.ToInt32( cboUnit.SelectedValue);
                        service.PriceUnit = Convert.ToInt32( nupPrice.Value);
                        service.EstimationDuration = Convert.ToInt32( nupEstimateDuration.Value);

                        db.Services.InsertOnSubmit(service);
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
                    var queryUpdate = db.Services.FirstOrDefault(x => x.Id == currentSelectedRow);

                    if (queryUpdate != null)
                    {
                        queryUpdate.Name = tbName.Text;
                        queryUpdate.IdCategory = Convert.ToInt32(cboCategory.SelectedValue);
                        queryUpdate.IdUnit = Convert.ToInt32(cboUnit.SelectedValue);
                        queryUpdate.PriceUnit = Convert.ToInt32(nupPrice.Value);
                        queryUpdate.EstimationDuration = Convert.ToInt32(nupEstimateDuration.Value);

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
