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
    public partial class FormTransactionDeposit : Form
    {
        private DatabaseDataContext db = new DatabaseDataContext();
        private Timer timer = new Timer();
        //int customerId = 0;

        int currentSelectedRow = -1;
        public FormTransactionDeposit()
        {
            InitializeComponent();
        }

        private void loadCboService()
        {
            cboService.DataSource = db.Services;
            cboService.ValueMember = "Id";
            cboService.DisplayMember = "Name";
        }

        private void tbPhoneNumber_TextChanged(object sender, EventArgs e)
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

        private void linkAddNewCustomer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new FormAddNewCustomer().ShowDialog();
        }

        private void FormTransactionDeposit_Load(object sender, EventArgs e)
        {
            loadCboService();
            lblCurrentTime.Text = DateTime.Now.ToString("dd - MMM - yy hh:mm:ss");
            lblTotalPay.Visible = false;
            //label1.Text = customerId.ToString();
            
            
        }

        private void cboService_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboService.SelectedValue != null)
            {
                var query = db.Services.FirstOrDefault(x => x.Id.Equals(cboService.SelectedValue));
                if (query != null )
                {
                    tbPricePerUnit.Text = query.PriceUnit.ToString();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var query = db.Services.FirstOrDefault(x => x.Id.Equals(cboService.SelectedValue));

            if (query != null)
            {
                if (cbUsePrepaid.Checked)
                {
                    var subTotal = query.PriceUnit * Convert.ToDecimal(nupTotalUnit.Value);
                    dgvData.Rows.Add(query.Name, query.Name, query.PriceUnit, Convert.ToDecimal( nupTotalUnit.Value), 0, btnDelete.Text = "Delete", query.Category.Name, query.EstimationDuration, query.Id, subTotal);
                    loadEstimationTime();
                }   
                
                else
                {
                    var subTotal = query.PriceUnit * Convert.ToDecimal(nupTotalUnit.Value);
                    dgvData.Rows.Add(query.Name, null, query.PriceUnit, Convert.ToDecimal(nupTotalUnit.Value), subTotal, btnDelete.Text = "Delete", query.Category.Name, query.EstimationDuration, query.Id, null);
                    loadTotalPay();
                    loadEstimationTime();
                }


            }
            
        }

        private void loadTotalPay()
        {
            var totalPay = 0;

            foreach (DataGridViewRow row in dgvData.Rows)
            {    
                if (row.Cells[4].Value != null)
                {
                    totalPay += Convert.ToInt32(row.Cells[4].Value);
                }
            }

            lblTotalPay.Text = totalPay.ToString(); 
            lblPay.Text = "Total Pay:                  Rp." + totalPay.ToString();
        }

        private void loadEstimationTime()
        {
            var totalMinutes = 0;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.Cells[6].Value != null  && row.Cells[7].Value != null)
                {
                    if (row.Cells[6].Value.ToString() == "Laundry Kiloan")
                    {
                        totalMinutes += Convert.ToInt32(row.Cells[7].Value) * 60;
                    }  

                    else
                    {
                        if (row.Cells[3].Value != null)
                        {
                            totalMinutes += (Convert.ToInt32(row.Cells[3].Value) * Convert.ToInt32(row.Cells[7].Value) * 60);
                        }
                    }

                } 
            }
            var timeSpan = TimeSpan.FromMinutes(totalMinutes);
            var estimateDatetime = DateTime.Now.Add(timeSpan).ToString("yyyy-MM-dd HH:mm:ss");
            lblEstimateTime.Text = estimateDatetime;

        }

        private bool validation()
        {
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }
                if (row.Cells[0].Value == null || tbPhoneNumber.Text == string.Empty)
                {
                    Support.MSE("all data must be fieled");
                    return false;
                }
            }

            return true;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (validation())
                {
                    foreach (DataGridViewRow row in dgvData.Rows)
                    {
                        if (row.Cells[1].Value != null && row.Cells[8].Value != null)
                        {
                            var deposit = new HeaderDeposit();
                            deposit.IdCustomer = Convert.ToInt32(lblCustomerId.Text);
                            deposit.IdEmployee = DataStorage.employeeId;
                            deposit.TransactionDatetime = DateTime.Now;
                            deposit.CompleteEstimationDatetime = Convert.ToDateTime(lblEstimateTime.Text);
                            db.HeaderDeposits.InsertOnSubmit(deposit);
                            db.SubmitChanges();

                            var package = new Package();
                            package.IdService = Convert.ToInt32(row.Cells[8].Value);
                            package.TotalUnit = Convert.ToInt32(row.Cells[3].Value);
                            package.Price = Convert.ToInt32(row.Cells[4].Value);

                            db.Packages.InsertOnSubmit(package);
                            db.SubmitChanges();


                            var prepaidPackage = new PrepaidPackage();
                            prepaidPackage.IdPackage = package.Id;
                            prepaidPackage.IdCustomer = Convert.ToInt32(lblCustomerId.Text);
                            prepaidPackage.Price = Convert.ToInt32(row.Cells[9].Value);
                            prepaidPackage.StartDateTime = DateTime.Now;
                            db.PrepaidPackages.InsertOnSubmit(prepaidPackage);
                            db.SubmitChanges();

                            var detailDeposit = new DetailDeposit();
                            detailDeposit.IdDeposit = deposit.Id;
                            detailDeposit.IdService = Convert.ToInt32(row.Cells[8].Value);
                            detailDeposit.CompletedDatetime = null;
                            detailDeposit.PriceUnit = Convert.ToInt32(row.Cells[2].Value);
                            detailDeposit.TotalUnit = Convert.ToInt32(row.Cells[3].Value);
                            detailDeposit.IdPrepaidPackage = prepaidPackage.Id;
                            db.DetailDeposits.InsertOnSubmit(detailDeposit);
                        }

                        else if (row.Cells[1].Value == null && row.Cells[8].Value != null)
                        {

                            var deposit = new HeaderDeposit();
                            deposit.IdCustomer = Convert.ToInt32(lblCustomerId.Text);
                            deposit.IdEmployee = DataStorage.employeeId;
                            deposit.TransactionDatetime = DateTime.Now;
                            deposit.CompleteEstimationDatetime = Convert.ToDateTime(lblEstimateTime.Text);
                            db.HeaderDeposits.InsertOnSubmit(deposit);
                            db.SubmitChanges();

                            var detailDeposit = new DetailDeposit();
                            detailDeposit.IdDeposit = deposit.Id;
                            detailDeposit.IdService = Convert.ToInt32(row.Cells[8].Value);
                            detailDeposit.PriceUnit = Convert.ToInt32(row.Cells[2].Value);
                            detailDeposit.TotalUnit = Convert.ToInt32(row.Cells[3].Value);
                            detailDeposit.IdPrepaidPackage = null;
                            detailDeposit.CompletedDatetime = null;
                            db.DetailDeposits.InsertOnSubmit(detailDeposit);
                            db.SubmitChanges();
                        }

                        db.SubmitChanges();

                    }
                    Support.MSI("berhasil");

                    db.SubmitChanges();
                }
            } catch (Exception ex)
            {
                Support.MSE(ex.Message);
            }
            
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvData.Columns["btnDelete"].Index && e.RowIndex >= 0)
            {
                dgvData.Rows.RemoveAt(e.RowIndex);
                loadTotalPay();
            }
        }
    }
}
