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

namespace SUVCServiceApp.Pages.ITEmployeePages
{
    /// <summary>
    /// Логика взаимодействия для SparePartsITEmployeePage.xaml
    /// </summary>
    public partial class SparePartsITEmployeePage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        private readonly Windows.EmployeeITWindow employeeITWindow;

        int currentPage = 1;
        int sizePage = 20;
        int maxPages = 0;
        public SparePartsITEmployeePage(Windows.EmployeeITWindow employeeITWindow)
        {
            InitializeComponent();
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadData(currentPage, sizePage);
            this.employeeITWindow = employeeITWindow;
        }

        private async Task LoadData(int currentPage, int sizePage)
        {
            labelPage.Content = currentPage.ToString();
            await dataGridLoader.LoadData<ResponseSpare>(listSpares, "SparesEquipments", currentPage, sizePage);
            var countSpares = await apiDataProvider.GetDataFromApi<ResponseSpare>($"SparesEquipments");
            maxPages = (int)Math.Ceiling(countSpares.Count * 1.0 / sizePage);
            if (currentPage == maxPages)
                buttonNextPage.IsEnabled = false;
            else buttonNextPage.IsEnabled = true;
            if (currentPage == 1)
                buttonPreviousPage.IsEnabled = false;
            else buttonPreviousPage.IsEnabled = true;
        }

        private void buttonAddSpare_Click(object sender, RoutedEventArgs e)
        {
            employeeITWindow.FrameWorkspace.Navigate(new Pages.AddPagesITEmployee.AddSpareITEmployee(employeeITWindow));
        }

        private async void textBoxSearchSpares_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = textBoxSearchSpares.Text;
            Func<ResponseSpare, string> searchProperty = item => item.SpareName;
            await dataGridLoader.LoadData(listSpares, "SparesEquipments", searchProperty, searchTerm);
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
