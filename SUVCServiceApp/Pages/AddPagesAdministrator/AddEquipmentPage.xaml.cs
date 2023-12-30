using Microsoft.Win32;
using SUVCServiceApp.Classes;
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
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        private readonly AdministratorWindow administratorWindow;
        public AddEquipmentPage(AdministratorWindow administratorWindow)
        {
            InitializeComponent();
            this.administratorWindow = administratorWindow;
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
        }
        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadData<ResponseStatusEqipment>(comboBoxStatus, "StatusEquipments");
            await dataGridLoader.LoadData<ResponseUsers>(comboBoxOwner, "Users");
        }

        private async void buttonAddEquipment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ApiDataProvider apiDataProvider = new ApiDataProvider();
                int ownerID = (int)comboBoxOwner.SelectedValue;
                int statusID = (int)comboBoxStatus.SelectedValue;
                int location = Convert.ToInt32(textBoxAuditoriumName.Text);
                ResponseEquipment equipment = new ResponseEquipment
                {
                    EquipmentName = textBoxNameEquipment.Text,
                    EquipmentDescription = textBoxDescriptionEquipment.Text,
                    InventoryName = textBoxInventoryName.Text,
                    NetworkName = textBoxNetworkName.Text,
                    IDStatus = statusID,
                    IDOwnerEquipment = ownerID,
                    LocationAuditorium = location
                };

                bool isSuccess = await apiDataProvider.AddDataToApi("Equipments", equipment);
                if (isSuccess)
                {
                    MessageBox.Show($"Оборудование {textBoxNameEquipment.Text} {textBoxInventoryName.Text} {textBoxNetworkName.Text} успешно добавлен!");
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

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            administratorWindow.FrameWorkspace.Navigate(new EquipmentPage(administratorWindow));
        }

        private async void buttonAddFromExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Excel Files|*.xlsx;*.xls",
                    Title = "Выберите файл Excel"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    var users = await apiDataProvider.GetDataFromApi<ResponseUsers>("Users");

                    var equipmentProcessor = new ExcelDataAdd<ResponseEquipment>(
                        "Equipments",
                        record =>
                        {
                            string ownerFullName = record["Владелец"].ToString();
                            int ownerId = GetUserID(users, ownerFullName);
                            return new ResponseEquipment
                            {
                                EquipmentName = record["Название"].ToString(),
                                EquipmentDescription = record["Описание"].ToString(),
                                InventoryName = record["Инвентарный номер"].ToString(),
                                NetworkName = record["Сетевое имя"].ToString(),
                                IDStatus = GetStatusID(record["Статус"].ToString()),
                                IDOwnerEquipment = ownerId,
                                LocationAuditorium = Convert.ToInt32(record["Аудитория"].ToString())
                            };
                        });

                    await equipmentProcessor.AddDataFromExcel(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
        private int GetUserID(IEnumerable<ResponseUsers> users, string fullName)
        {
            var currentUser = users.FirstOrDefault(user => user.FullName == fullName);
            return currentUser?.ID ?? 0;
        }
        private int GetStatusID(string status)
        => status == "Отличное" ? 1 : status == "Среднее" ? 2 : status == "Ремонт" ? 3 : status == "Списание" ? 4 : 0;

        private void buttonDownloadTemplate_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Скачать шаблон?",
                "Шаблон добавления оборудования", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx|All Files|*.*";
                saveFileDialog.Title = "Выберите место для сохранения файла";
                saveFileDialog.FileName = "оборудование.xlsx";
                bool? dialogResult = saveFileDialog.ShowDialog();
                if (dialogResult == true)
                {
                    try
                    {
                        string filePath = saveFileDialog.FileName;
                        string resourcePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources", "TemplateAddData", "оборудование.xlsx");
                        System.IO.File.Copy(resourcePath, filePath, true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
