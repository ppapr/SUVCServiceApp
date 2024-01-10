using SUVCServiceApp.Controller;
using SUVCServiceApp.ViewModel;
using SUVCServiceApp.Windows;
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
        public EquipmentPage(AdministratorWindow administratorWindow)
        {
            InitializeComponent();
            this.administratorWindow = administratorWindow;
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
        }

        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadData<ResponseEquipment>(listEqipments, "Equipments");
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
                LoadDataGrid();
            }
        }

        ResponseEquipment currentEquipment;
        private void listEqipments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentEquipment = (ResponseEquipment)listEqipments.SelectedItem;
        }
    }
}
