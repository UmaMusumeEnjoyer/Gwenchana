﻿//using DashboardApp.Models;
using Gwenchana.DataAccess.ViewModel;
using Gwenchana.LanguagePack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Gwenchana
{
    public partial class Dashboard : Form
    {
        //Fields
        private AdminDashboard model;

        //Constructor
        public Dashboard()
        {
            InitializeComponent();
            //Default - Last 7 days
            dtpStartDate.Value = DateTime.Today.AddDays(-7);
            dtpEndDate.Value = DateTime.Now;
            btn_last7Days.Select();

            model = new AdminDashboard();
            UpdateComponent(LanguageClass.Language);
            LoadData();
        }

        private void UpdateComponent(string language)
        {
            Resource.Culture = string.IsNullOrEmpty(language) ? null : new CultureInfo(language);
            lb_Statistics.Text = Resource.lb_Statistics;
            btn_customDate.Text = Resource.btn_customDate;
            btn_Today.Text = Resource.btn_Today;
            btn_last7Days.Text = Resource.btn_last7Days;
            btn_last30Days.Text = Resource.btn_last30Days;
            btn_thisMonth.Text = Resource.btn_thisMonth;
            lb_TotalSalesOrders.Text = Resource.lb_TotalSalesOrders;
            lb_TotalRevenue.Text = Resource.lb_TotalRevenue;
            lb_TotalProfit.Text = Resource.lb_TotalProfit;
            lb_Counter.Text = Resource.lb_Counter;
            lb_CustomerCounter.Text = Resource.lb_CustomerCounter;
            lb_SupplierCounter.Text = Resource.lb_SupplierCounter;
            lb_ProductCounter.Text = Resource.lb_ProductCounter;
            lb_lowIventory.Text = Resource.lb_lowIventory;
            chartTopProducts.Titles[0].Text = Resource.txt_Top5product;
            chartGrossRevenue.Titles[0].Text = Resource.txt_Totalrevenue;
        }

        private void LoadData()
        {
            var refreshData = model.LoadData(dtpStartDate.Value, dtpEndDate.Value);
            if (refreshData == true)
            {
                lblNumOrders.Text = model.numReceipts.ToString();
                lblTotalRevenue.Text = model.TotalRevenue.ToString() + "đ";
                lblTotalProfit.Text = model.TotalProfit.ToString() + "đ";

                lblNumCustomers.Text = model.numCustomers.ToString();
                lblNumSuppliers.Text = model.numSuppliers.ToString();
                lblNumProducts.Text = model.numProducts.ToString();

                chartGrossRevenue.DataSource = model.GrossRevenueList;
                chartGrossRevenue.Series[0].XValueMember = "Date";
                chartGrossRevenue.Series[0].YValueMembers = "TotalAmount";
                chartGrossRevenue.DataBind();

                chartTopProducts.DataSource = model.TopProducts;
                chartTopProducts.Series[0].XValueMember = "Key";
                chartTopProducts.Series[0].YValueMembers = "Value";
                chartTopProducts.DataBind();

                dgvUnderstock.DataSource = model.Understock;
                dgvUnderstock.Columns[0].HeaderText = Resource.dvg_Item;
                dgvUnderstock.Columns[1].HeaderText = Resource.dvg_Units;
            }
            
        }
        private void DisableCustomDates()
        {
            dtpStartDate.Enabled = false;
            dtpEndDate.Enabled = false;
            btn_OK.Visible = false;
        }

        //Event methods
        private void btnToday_Click(object sender, EventArgs e)
        {
            dtpStartDate.Value = DateTime.Today;
            dtpEndDate.Value = DateTime.Now;
            LoadData();
            DisableCustomDates();
        }

        private void btnLast7Days_Click(object sender, EventArgs e)
        {
            dtpStartDate.Value = DateTime.Today.AddDays(-7);
            dtpEndDate.Value = DateTime.Now;
            LoadData();
            DisableCustomDates();
        }

        private void btnLast30Days_Click(object sender, EventArgs e)
        {
            dtpStartDate.Value = DateTime.Today.AddDays(-30);
            dtpEndDate.Value = DateTime.Now;
            LoadData();
            DisableCustomDates();
        }

        private void btnThisMonth_Click(object sender, EventArgs e)
        {
            dtpStartDate.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpEndDate.Value = DateTime.Now;
            LoadData();
            DisableCustomDates();
        }

        private void btnCustomDate_Click(object sender, EventArgs e)
        {
            dtpStartDate.Enabled = true;
            dtpEndDate.Enabled = true;
            btn_OK.Visible = true;
        }

        private void btnOkCustomDate_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void lblNumOrders_Click(object sender, EventArgs e)
        {

        }

        private void chartGrossRevenue_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblTotalProfit_Click(object sender, EventArgs e)
        {

        }

        private void dgvUnderstock_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
