using SUVCServiceApp.Controller;
using SUVCServiceApp.ViewModel;
using SUVCServiceApp.Windows;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для AddEquipmentPage.xaml
    /// </summary>
    public partial class AddEquipmentPage : Page
    {
        private readonly AdministratorWindow administratorWindow;
        public AddEquipmentPage(AdministratorWindow administratorWindow)
        {
            InitializeComponent();
            this.administratorWindow = administratorWindow;
        }

        private async void buttonAddEquipment_Click(object sender, RoutedEventArgs e)
        {
            ApiDataProvider apiDataProvider = new ApiDataProvider();
            ResponseEquipment equipment = new ResponseEquipment
            {
                EquipmentName = textBoxNameEquipment.Text,
                EquipmentDescription = textBoxDescriptionEquipment.Text,
                InventoryName = textBoxInventoryName.Text,
                NetworkName = textBoxNetworkName.Text,
                StatusName = comboBoxStatus.Text,
                OwnerName = comboBoxOwner.Text,
                Location = textBoxAuditoriumName.Text
            };

            bool isSuccess = await apiDataProvider.AddDataToApi("Equipment", equipment);
            if (isSuccess)
            {
                MessageBox.Show($"Оборудование {textBoxNameEquipment.Text} {textBoxInventoryName.Text} {textBoxNetworkName.Text} успешно добавлен!");
            }
            else
            {
                MessageBox.Show("Произошла ошибка при добавлении данных! Проверьте поля ввода данных!");
            }
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            administratorWindow.FrameWorkspace.Navigate(new EquipmentPage(administratorWindow));
        }
    }
}
