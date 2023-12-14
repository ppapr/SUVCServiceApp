﻿using SUVCServiceApp.Controller;
using SUVCServiceApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SUVCServiceApp.Windows;

namespace SUVCServiceApp.Pages.EmployeePages
{
    /// <summary>
    /// Логика взаимодействия для RequestsEmployeePage.xaml
    /// </summary>
    public partial class RequestsEmployeePage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        private int authenticatedUserId;
        private readonly EmployeeWindow employeeWindow;
        public RequestsEmployeePage(int authenticatedUserId, EmployeeWindow employeeWindow)
        {
            InitializeComponent();
            this.authenticatedUserId = authenticatedUserId;
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
            this.employeeWindow = employeeWindow;
        }

        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadData<ResponseRequests>(listRequests, $"Requests?userRequest={authenticatedUserId}");
        }

        private void buttonAddRequest_Click(object sender, RoutedEventArgs e)
        {
            employeeWindow.FrameWorkspace.Navigate(new Pages.AddPagesEmployee.CreateNewRequestEmployeePage(authenticatedUserId, employeeWindow));
        }
    }
}
