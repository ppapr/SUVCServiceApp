using SUVCServiceApp.Controller;
using SUVCServiceApp.ViewModel;
using SUVCServiceApp.Windows;
using SUVCServiceApp.Windows.ChangeWindowsAdministrator;
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

namespace SUVCServiceApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для EquipmentPage.xaml
    /// </summary>
    public partial class EquipmentPage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        private readonly AdministratorWindow administratorWindow;

        int currentPage = 1;
        int sizePage = 20;
        int maxPages = 0;
        public EquipmentPage(AdministratorWindow administratorWindow)
        {
            InitializeComponent();
            this.administratorWindow = administratorWindow;
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadData(currentPage, sizePage);
        }

        private async Task LoadData(int currentPage, int sizePage)
        {
            labelPage.Content = currentPage.ToString();
            await dataGridLoader.LoadFilteredData<ResponseEquipment>(listEqipments, "Equipments", 
                equipment => equipment.ID != 42 && equipment.ID != 61, currentPage, sizePage);
            var countEquipments = await apiDataProvider.GetDataFromApi<ResponseEquipment>("Equipments");
            maxPages = (int)Math.Ceiling(countEquipments.Count * 1.0 / sizePage);
            if (currentPage == maxPages)
                buttonNextPage.IsEnabled = false;
            else buttonNextPage.IsEnabled = true;
            if (currentPage == 1)
                buttonPreviousPage.IsEnabled = false;
            else buttonPreviousPage.IsEnabled = true;
        }

        private void buttonAddEquipment_Click(object sender, RoutedEventArgs e)
        {
            administratorWindow.FrameWorkspace.Navigate(new AddEquipmentPage(administratorWindow));
        }

        private async void textBoxSearchEquipment_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = textBoxSearchEquipment.Text;
            Func<ResponseEquipment, string> searchProperty = item => item.FullNameEquipment;
            await dataGridLoader.LoadData(listEqipments, "Equipments", searchProperty, searchTerm);
        }

        private async void buttonDeleteEqipment_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить запчасть {currentEquipment.EquipmentName}?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                await apiDataProvider.DeleteDataFromApi<ResponseEquipment>("Equipments", currentEquipment.ID);
                MessageBox.Show("Удаление завершено!");
                await LoadData(currentPage, sizePage);
            }
        }

        ResponseEquipment currentEquipment;
        private void listEqipments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentEquipment = (ResponseEquipment)listEqipments.SelectedItem;
        }

        private async void buttonNextPage_Click(object sender, RoutedEventArgs e)
        {
            currentPage++;
            await LoadData(currentPage, sizePage);
        }

        private async void buttonPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                await LoadData(currentPage, sizePage);
            }
        }

        private void buttonChangeEquipment_Click(object sender, RoutedEventArgs e)
        {
            if (currentEquipment != null)
            {
                ChangeEquipment changeWindow = new ChangeEquipment(currentEquipment);
                changeWindow.Closed += (s, args) =>
                {
                    LoadData(currentPage, sizePage);
                };
                changeWindow.ShowDialog();
            }
        }
    }
}
