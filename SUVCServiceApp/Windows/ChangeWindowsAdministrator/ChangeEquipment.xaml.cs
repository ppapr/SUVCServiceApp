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
using System.Windows.Shapes;

namespace SUVCServiceApp.Windows.ChangeWindowsAdministrator
{
    /// <summary>
    /// Логика взаимодействия для ChangeEquipment.xaml
    /// </summary>
    public partial class ChangeEquipment : Window
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        ResponseEquipment responseEquipment;
        public ChangeEquipment(ResponseEquipment responseEquipment)
        {
            InitializeComponent();
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
            this.responseEquipment = responseEquipment;
            textBoxAuditoriumName.Text = responseEquipment.LocationAuditorium.ToString();
            textBoxDescriptionEquipment.Text=responseEquipment.EquipmentDescription;
            textBoxInventoryName.Text = responseEquipment.InventoryName;
            textBoxNameEquipment.Text = responseEquipment.EquipmentName;
            textBoxNetworkName.Text = responseEquipment.NetworkName;
        }
        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadData<ResponseStatusEqipment>(comboBoxStatus, "StatusEquipments");
            await dataGridLoader.LoadData<ResponseUsers>(comboBoxOwner, "Users");
            comboBoxOwner.SelectedItem = comboBoxOwner.Items.Cast<ResponseUsers>().FirstOrDefault(u => u.FullName == responseEquipment.OwnerName);
            comboBoxStatus.SelectedItem = comboBoxStatus.Items.Cast<ResponseStatusEqipment>().FirstOrDefault(s => s.NameStatus == responseEquipment.StatusName);
        }
        private async void buttonSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ApiDataProvider apiDataProvider = new ApiDataProvider();
                int ownerID = (int)comboBoxOwner.SelectedValue;
                int statusID = (int)comboBoxStatus.SelectedValue;
                int location = Convert.ToInt32(textBoxAuditoriumName.Text);
                ResponseEquipment equipment = new ResponseEquipment
                {
                    ID = responseEquipment.ID,
                    EquipmentName = textBoxNameEquipment.Text,
                    EquipmentDescription = textBoxDescriptionEquipment.Text,
                    InventoryName = textBoxInventoryName.Text,
                    NetworkName = textBoxNetworkName.Text,
                    IDStatus = statusID,
                    IDOwnerEquipment = ownerID,
                    LocationAuditorium = location
                };

                bool isSuccess = await apiDataProvider.UpdateDataToApi("Equipments", responseEquipment.ID, equipment);
                if (isSuccess)
                {
                    MessageBox.Show($"Оборудование {textBoxNameEquipment.Text} {textBoxNetworkName.Text} успешно изменено!");
                }
                else
                {
                    MessageBox.Show("Произошла ошибка при добавлении данных! Проверьте поля ввода данных!");

                }
            }
            catch 
            {
                MessageBox.Show("Проверьте заполненность данных и соеднинение с интернетом!");
            }
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
