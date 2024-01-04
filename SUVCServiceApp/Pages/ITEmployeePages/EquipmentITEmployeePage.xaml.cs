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

namespace SUVCServiceApp.Pages.ITEmployeePages
{
    /// <summary>
    /// Логика взаимодействия для EquipmentITEmployeePage.xaml
    /// </summary>
    public partial class EquipmentITEmployeePage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        public EquipmentITEmployeePage()
        {
            InitializeComponent();
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
        }

        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadData<ResponseEquipment>(listEquipments, "Equipments");
        }

        private async void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = textBoxSearch.Text;
            Func<ResponseEquipment, string> searchProperty = item => item.FullNameEquipment;
            await dataGridLoader.LoadData(listEquipments, $"Equipments", searchProperty, searchTerm);
        }
        private ResponseEquipment currentEquipment;
        private void buttonCheckLocation_Click(object sender, RoutedEventArgs e)
        {
            if (currentEquipment != null)
                new Windows.InteractiveMapWindow(currentEquipment).ShowDialog();
            else
                MessageBox.Show("Выберите оборудование!");
        }

        private void listEquipments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentEquipment = (ResponseEquipment)listEquipments.SelectedItem;
        }
    }
}
