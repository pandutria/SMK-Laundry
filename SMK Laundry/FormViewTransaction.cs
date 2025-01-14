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
    public partial class FormViewTransaction : Form
    {
        private DatabaseDataContext db = new DatabaseDataContext();
        int currentSelectedRow = -1;
        public FormViewTransaction()
        {
            InitializeComponent();
        }

        private void loadDgvDataHeader()
        {
            dgvHeaderData.Columns.Clear();

            var query = db.DetailDeposits.Where(x => x.HeaderDeposit.Customer.Name.Contains(tbSearch.Text) || x.HeaderDeposit.Employee.Name.Contains(tbSearch.Text) || x.HeaderDeposit.TransactionDatetime.ToString().Contains(tbSearch.Text))
                .Select(x => new { ID = x.HeaderDeposit.Id, CustomerId = x.HeaderDeposit.IdCustomer, CustomerName = x.HeaderDeposit.Customer.Name, EmployeeName = x.HeaderDeposit.Employee.Name, x.HeaderDeposit.TransactionDatetime, x.HeaderDeposit.CompleteEstimationDatetime, 
                ServiceName = x.Service.Name, PrepaidPackageId = x.IdPrepaidPackage, x.PriceUnit, x.TotalUnit, x.CompletedDatetime, DetailId = x.Id });
            
            dgvHeaderData.DataSource = query;

            dgvHeaderData.Columns["ServiceName"].Visible = false;
            dgvHeaderData.Columns["PrepaidPackageId"].Visible = false;
            dgvHeaderData.Columns["PriceUnit"].Visible = false;
            dgvHeaderData.Columns["TotalUnit"].Visible = false;
            dgvHeaderData.Columns["CompletedDatetime"].Visible = false;
            dgvHeaderData.Columns["DetailId"].Visible = false;
        }

        private void dgvHeaderData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                currentSelectedRow = e.RowIndex;
                currentSelectedRow = Convert.ToInt32(dgvHeaderData.Rows[e.RowIndex].Cells["DetailId"].Value);
                loadDgvDataDetail();
            }
        }

        private void loadDgvDataDetail()
        {
            dgvDetailData.Rows.Clear();

            var query = db.DetailDeposits.Where(x => x.Id == currentSelectedRow);
            
            foreach (var d in query)
            {
                dgvDetailData.Rows.Add(d.Service.Name,d.IdPrepaidPackage, d.PriceUnit, d.TotalUnit, d.CompletedDatetime, btnComplete.Text ="Complete", d.Id);
            }
        }

        //private void loadDgvDataDetail()
        //{
        //    dgvDetailData.Rows.Clear();
        //    dgvDetailData.Columns.Clear();

        //    var query = db.DetailDeposits.Where(x => x.Id == currentSelectedRow)
        //        .Select(x => new { ServiceName = x.Service.Name, PrepaidPackageId = (int?) x.PrepaidPackage.Id, PricePerUnit = x.PriceUnit,x.TotalUnit ,x.CompletedDatetime });

        //    var btnComplete = new DataGridViewButtonColumn();
        //    btnComplete.Text = "Complete";
        //    btnComplete.HeaderText = "Action";
        //    btnComplete.Name = "btnComplete";
        //    btnComplete.UseColumnTextForButtonValue = true;

        //    dgvDetailData.Columns.Add(btnComplete);
        //    dgvDetailData.Columns["btnComplete"].DisplayIndex = dgvDetailData.Columns.Count - 1;

        //    dgvDetailData.DataSource = query;
        //}

        private void FormViewTransaction_Load(object sender, EventArgs e)
        {
            loadDgvDataHeader();  
            loadDgvDataDetail();
        }

        private void dgvDetailData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvDetailData.Columns["btnComplete"].Index && e.RowIndex >= 0)
            {
                currentSelectedRow = e.RowIndex;
                currentSelectedRow = Convert.ToInt32( dgvDetailData.Rows[e.RowIndex].Cells[6].Value);

                var query = db.DetailDeposits.FirstOrDefault(x => x.Id == currentSelectedRow);

                if (query != null)
                {
                    query.CompletedDatetime = DateTime.Now;
                    db.SubmitChanges();
                    loadDgvDataDetail();
                    loadDgvDataHeader();
                    Support.MSI("berhasil");
                }
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            loadDgvDataHeader();
        }
    }
}
