using SUVCServiceApp.Controller;
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
        private ResponseUsers authenticatedUser;
        private readonly EmployeeWindow employeeWindow;
        public RequestsEmployeePage(ResponseUsers authenticatedUser, EmployeeWindow employeeWindow)
        {
            InitializeComponent();
            this.authenticatedUser = authenticatedUser;
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
            this.employeeWindow = employeeWindow;
        }

        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadData<ResponseRequests>(listRequests, $"Requests?userRequest={authenticatedUser.ID}");
        }

        private void buttonAddRequest_Click(object sender, RoutedEventArgs e)
        {
            employeeWindow.FrameWorkspace.Navigate(new Pages.AddPagesEmployee.CreateNewRequestEmployeePage(authenticatedUser, employeeWindow));
        }

        private async void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = textBoxSearch.Text;
            Func<ResponseRequests, string> searchProperty = item => item.Description;
            await dataGridLoader.LoadData(listRequests, "Requests", searchProperty, searchTerm);
        }
    }
}
