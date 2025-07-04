﻿using Gwenchana.BussinessLogic;
using Gwenchana.DataAccess;
using Gwenchana.DataAccess.DBConnect;
using Gwenchana.DataAccess.DTO;
using Gwenchana.DataAccess.ViewModel;
using Gwenchana.LanguagePack;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//using CuaHangMayTinh.UI.Authentication;

namespace Gwenchana
{
    public partial class information: Form
    {
        public int id { get; set; }
        public string temp { get; set; }

        public information(int employe)
        {
            InitializeComponent();
            UpdateComponent(LanguageClass.Language);
            id = employe;
            LoadData();

        }
        private void UpdateComponent(string language)
        {
            Resource.Culture = string.IsNullOrEmpty(language) ? null : new CultureInfo(language);
            lb_personalInformation.Text = Resource.lb_personalInformation;
            lb_Name.Text = Resource.lb_Name;
            lb_Age.Text = Resource.lb_employeeAge;
            lb_Phonenumber.Text = Resource.lb_employeePhonenumber;
            lb_accountUsername.Text = Resource.lb_employeeUsername;
            lb_accountPassword.Text = Resource.lb_employeePassword;
            btn_changeAccount.Text = Resource.btn_changeAccount;
            btn_changeInformation.Text = Resource.btn_changeInformation;
        }

        void LoadData()
        {
            txt_Name.Enabled = true;
            txt_Age.Enabled = true;
            txt_PhoneNumber.Enabled = true;
            txt_Username.Enabled = true;
            txt_Password.Enabled = true;
            btn_changeAccount.Visible = true;
            CurrentEmployee employee = new CurrentEmployee();
            employee.GetCurrentEmployee(id);
            txt_Name.Text = employee.employeeName;
            txt_Age.Text = employee.Age.ToString();
            txt_PhoneNumber.Text = employee.phoneNumber;
            txt_Username.Text = employee.username;
            txt_Password.Text = employee.password;
            txt_Username.Enabled = false;
            txt_Password.Enabled = false;
        }

        private void information_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void ChangePass_Click(object sender, EventArgs e)
        {
            temp = "ChangeAccount";

            txt_Age.Enabled = false;
            txt_Name.Enabled = false;
            txt_PhoneNumber.Enabled = false;
            txt_Username.Enabled = true;
            txt_Password.Enabled = true;
            btn_changeAccount.Visible = false;
    
           
        }

        private void txt_Name_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CurrentEmployee employee = new CurrentEmployee();
            employee.GetCurrentEmployee(id);
            if(txt_Age.Text == "" || txt_Name.Text == "" || txt_PhoneNumber.Text == "")
            {
                MessageBox.Show(Resource.Validation_Error_MissingFields);
                return;
            }
            if (txt_Age.Text.Length > 3 || txt_Age.Text.Length < 1)
            {
                MessageBox.Show(Resource.Validation_Error_InvalidAge);
                return;
            }
            if (txt_PhoneNumber.Text.Length > 11 || txt_PhoneNumber.Text.Length < 9)
            {
                MessageBox.Show(Resource.Validation_Error_InvalidPhone);
                return;
            }
            if (txt_Username.Text == "" || txt_Password.Text == "")
            {
                MessageBox.Show(Resource.Validation_Error_AccountInfoRequired);
                return;
            }


            if (temp == "ChangeAccount")
            {
                AccountBLL accountBLL = new AccountBLL();
                Account account = new Account()
                {
                    Id = employee.Account_Id,
                    Username = txt_Username.Text,
                    Password = txt_Password.Text,
                    Role = "Employee"
                };
                if (accountBLL.UpdateAccount(account))
                {
                    MessageBox.Show(Resource.Account_Edit_Success);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(Resource.Account_Edit_Fail);
                }
            }
            else 
            {

                EmployeeBLL employeeBLL = new EmployeeBLL();
                Employee employee1 = new Employee()
                {
                    Empolyee_Id = employee.Employee_Id,
                    employeeName = txt_Name.Text,
                    Age = int.Parse(txt_Age.Text),
                    phoneNumber = txt_PhoneNumber.Text,
                    Account_Id = employee.Account_Id,
                    TrangThai = employee.TrangThai
                };

                if (employeeBLL.UpdateEmployee(employee1))
                {
                    MessageBox.Show(Resource.Employee_Update_Success);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(Resource.Employee_Update_Fail);
                }
            }
        }

        private void btn_ChangeAccount_Click(object sender, EventArgs e)
        {

        }
    }
}
