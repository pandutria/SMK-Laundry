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
    public partial class FormAddNewCustomer : Form
    {
        public FormAddNewCustomer()
        {
            InitializeComponent();
        }

        private bool isValidPhoneNumber(string phoneNumber)
        {
            var regex = @"^\+\d+$";
            return Regex.IsMatch(phoneNumber, regex);
        }

        private bool validation()
        {
            if (tbCustomerName.Text == string.Empty || tbPhoneNumber.Text == string.Empty || tbAddress.Text == string.Empty)
            {
                Support.MSW("all input must be filled");
                return false;
            }

            if (!isValidPhoneNumber(tbPhoneNumber.Text))
            {
                Support.MSW("phone number must be unique");
                return false;
            }

            return true;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var db = new DatabaseDataContext();
            try
            {
                if (validation())
                {
                    var cust = new Customer();
                    cust.Name = tbCustomerName.Text;
                    cust.PhoneNumber = tbPhoneNumber.Text;
                    cust.Address = tbAddress.Text;

                    db.Customers.InsertOnSubmit(cust);
                    db.SubmitChanges();
                    Support.MSI("add new customer berhasil");
                    Close();
                }
                

            } catch (Exception ex)
            {
                Support.MSE(ex.Message);
            }
            
        }
    }
}
