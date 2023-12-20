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
    /// Логика взаимодействия для AddSparePartsPage.xaml
    /// </summary>
    public partial class AddSparePartsPage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        private readonly AdministratorWindow administratorWindow;
        public AddSparePartsPage(AdministratorWindow administratorWindow)
        {
            InitializeComponent();
            this.administratorWindow = administratorWindow;
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
        }

        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadData<ResponseEquipment>(comboBoxEquipment, "Equipments");
        }

        private async void buttonAddSpare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ApiDataProvider apiDataProvider = new ApiDataProvider();
                var equipment = (ResponseEquipment)comboBoxEquipment.SelectedItem;
                int equipmentID = (int)equipment.ID;
                ResponseSpare spare = new ResponseSpare
                {
                    SpareName = textBoxSpareName.Text,
                    IDEquipment = equipmentID
                };

                bool isSuccess = await apiDataProvider.AddDataToApi("SparesEquipments", spare);
                if (isSuccess)
                {
                    MessageBox.Show($"Запасная часть {textBoxSpareName.Text} успешно добавлена!");
                }
                else
                {
                    MessageBox.Show("Произошла ошибка при добавлении данных! Проверьте поля ввода данных!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Проверьте заполненность данных и соеднинение с интернетом!" + ex.ToString());
            }
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            administratorWindow.FrameWorkspace.Navigate(new SparePartsPages(administratorWindow));
        }
    }
}
