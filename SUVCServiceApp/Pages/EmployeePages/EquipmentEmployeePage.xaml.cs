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
    /// Логика взаимодействия для EquipmentEmployeePage.xaml
    /// </summary>
    public partial class EquipmentEmployeePage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        private ResponseUsers authenticatedUser;
        private readonly EmployeeWindow employeeWindow;

        int currentPage = 1;
        int sizePage = 20;
        int maxPages = 0;
        public EquipmentEmployeePage(ResponseUsers authenticatedUser, EmployeeWindow employeeWindow)
        {
            InitializeComponent();
            this.authenticatedUser = authenticatedUser;
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadData(currentPage,sizePage);
            this.employeeWindow = employeeWindow;
        }

        private async Task LoadData(int currentPage, int sizePage)
        {
            labelPage.Content = currentPage.ToString();
            await dataGridLoader.LoadData<ResponseEquipment>(listEquipments, $"Equipments?user={authenticatedUser.ID}", currentPage, sizePage);
            var countEquipment = await apiDataProvider.GetDataFromApi<ResponseEquipment>($"Equipments?user={authenticatedUser.ID}");
            maxPages = (int)Math.Ceiling(countEquipment.Count * 1.0 / sizePage);
            if (currentPage == maxPages)
                buttonNextPage.IsEnabled = false;
            else buttonNextPage.IsEnabled = true;
            if (currentPage == 1)
                buttonPreviousPage.IsEnabled = false;
            else buttonPreviousPage.IsEnabled = true;
        }

        private async void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = textBoxSearch.Text;
            Func<ResponseEquipment, string> searchProperty = item => item.FullNameEquipment;
            await dataGridLoader.LoadData(listEquipments, $"Equipments?user={authenticatedUser.ID}", searchProperty, searchTerm);
        }

        private async void buttonPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                await LoadData(currentPage, sizePage);
            }
        }

        private async void buttonNextPage_Click(object sender, RoutedEventArgs e)
        {
            currentPage++;
            await LoadData(currentPage, sizePage);
        }
    }
}
