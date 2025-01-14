using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMK_Laundry
{
    public partial class FormEmployee : Form
    {
        private DatabaseDataContext db = new DatabaseDataContext();
        int currentSelectedRow = -1;
        string mode = "";
        public FormEmployee()
        {
            InitializeComponent();
        }

        private void loadDgv()
        {
            dgvData.Rows.Clear();
            var query = db.Employees.Where(x => x.Name.Contains(tbSearch.Text) || x.Email.Contains(tbSearch.Text) || x.PhoneNumber.Contains(tbSearch.Text))
                .Select(x => new {EmployeeId = x.Id, x.Name, x.Email, x.PhoneNumber, x.Address, x.DateOfBirth, Job = x.Job.Name, x.Salary, x.Password});

            dgvData.DataSource = query;

            dgvData.Columns["Password"].Visible = false;
        }

        private void loadCboJob()
        {
            cboJob.DataSource = db.Jobs;
            cboJob.ValueMember = "Id";
            cboJob.DisplayMember = "Name";
        }

        private void FormEmployee_Load(object sender, EventArgs e)
        {
            loadDgv();
            loadCboJob();
            enableFieldAndButton(true);
            nupSalary.Maximum = 10000000;
            nupSalary.Minimum = 0;

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
            tbAddress.Enabled = !e;
            tbId.Enabled = !e;
            tbEmail.Enabled = !e;
            tbPhoneNumber.Enabled = !e;
            tbName.Enabled = !e;
            tbAddress.Enabled = !e;
            dtpBirth.Enabled = !e;
            cboJob.Enabled = !e;
            nupSalary.Enabled = !e;
            tbPassword.Enabled = !e;
            tbConfirmPassword.Enabled = !e;

        }

        private void enableFieldAndButton(bool e)
        {
            enableButton(e);
            enableField(e);
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                currentSelectedRow = e.RowIndex;
                currentSelectedRow = Convert.ToInt32( dgvData.Rows[e.RowIndex].Cells["EmployeeId"].Value);
                tbId.Text = dgvData.Rows[e.RowIndex].Cells["EmployeeId"].Value.ToString();
                tbName.Text = dgvData.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                tbEmail.Text = dgvData.Rows[e.RowIndex].Cells["Email"].Value.ToString();
                tbPhoneNumber.Text = dgvData.Rows[e.RowIndex].Cells["PhoneNumber"].Value.ToString();
                tbAddress.Text = dgvData.Rows[e.RowIndex].Cells["Address"].Value.ToString();
                dtpBirth.Value = Convert.ToDateTime( dgvData.Rows[e.RowIndex].Cells["DateOfBirth"].Value);
                cboJob.Text = dgvData.Rows[e.RowIndex].Cells["Job"].Value.ToString();
                nupSalary.Value = Convert.ToDecimal( dgvData.Rows[e.RowIndex].Cells["Salary"].Value);
                tbPassword.Text = dgvData.Rows[e.RowIndex].Cells["Password"].Value.ToString();
                tbConfirmPassword.Text = dgvData.Rows[e.RowIndex].Cells["Password"].Value.ToString();
            }
        }

        private bool isValidEmail(string email)
        {
            var regex = @"^\S+@\S+\.\S+$";
            return Regex.IsMatch(email, regex);
        }

        private bool isValidPhoneNumber(string phoneNumber)
        {
            var regex = @"^\+\d+$";
            return Regex.IsMatch(phoneNumber, regex);
        }

        private bool validation()
        {
            if (tbName.Text == string.Empty || tbEmail.Text == string.Empty
                || tbAddress.Text == string.Empty || tbPhoneNumber.Text == string.Empty
                || tbPassword.Text == string.Empty || cboJob.Text == string.Empty || dtpBirth.Text == string.Empty
                || nupSalary.Value == 0)
            {
                Support.MSW("all input must be fielded");
                return false;

            }

            if (tbConfirmPassword.Text != tbPassword.Text)
            {
                Support.MSW("confirm password must be same with password");
                return false;
            }

            if (!isValidEmail(tbEmail.Text))
            {
                Support.MSW("Email must be valid format.");
                return false ;
            } 

            if (!isValidPhoneNumber(tbPhoneNumber.Text))
            {
                Support.MSW("Phone number must be start with \"+\" and another with digit");
                return false;
            }

            return true;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            loadDgv();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Support.clearFields(this);
            enableFieldAndButton(true);

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            mode = "insert";
            enableFieldAndButton(false);
            Support.clearFields(this);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (currentSelectedRow == -1)
            {
                Support.MSI("click row");
            } else
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

            } else
            {
                var dialog = MessageBox.Show("are you sure want to delete this data?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                try
                {
                    if (dialog == DialogResult.Yes)
                    {
                        var queryDelete = db.Employees.FirstOrDefault(x => x.Id == currentSelectedRow);
                        db.Employees.DeleteOnSubmit(queryDelete);
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
                        var employeee = new Employee();
                        employeee.Name = tbName.Text;
                        employeee.Email = tbEmail.Text;
                        employeee.PhoneNumber = tbPhoneNumber.Text;
                        employeee.Address = tbAddress.Text;
                        employeee.DateOfBirth = dtpBirth.Value;
                        employeee.IdJob = Convert.ToInt32( cboJob.SelectedValue);
                        employeee.Salary = nupSalary.Value;
                        employeee.Password = tbPassword.Text;

                        db.Employees.InsertOnSubmit(employeee);
                        db.SubmitChanges();
                        loadDgv();
                        Support.clearFields(this);
                        enableFieldAndButton(true);
                        Support.MSI("insert data succses");

                    } catch (Exception ex )
                    {
                        Support.MSE(ex.Message);
                    }
                }
            }

            if (mode == "update")
            {
                try
                {
                    var queryUpdate = db.Employees.FirstOrDefault(x => x.Id == currentSelectedRow);

                    if (queryUpdate != null)
                    {
                        queryUpdate.Name = tbName.Text;
                        queryUpdate.Email = tbEmail.Text;
                        queryUpdate.PhoneNumber = tbPhoneNumber.Text;
                        queryUpdate.Address = tbAddress.Text;
                        queryUpdate.DateOfBirth = dtpBirth.Value;
                        queryUpdate.IdJob = Convert.ToInt32(cboJob.SelectedValue);
                        queryUpdate.Salary = nupSalary.Value;
                        queryUpdate.Password = tbPassword.Text;

                        db.SubmitChanges();
                        loadDgv();
                        Support.clearFields(this);
                        enableFieldAndButton(true);
                        Support.MSI("update data succses");
                    }
                } catch (Exception ex)
                {
                    Support.MSE(ex.Message);
                }
               
            }
        }
    }
}
