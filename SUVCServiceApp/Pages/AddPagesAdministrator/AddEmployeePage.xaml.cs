using Microsoft.Win32;
using Newtonsoft.Json;
using SUVCServiceApp.Classes;
using SUVCServiceApp.Controller;
using SUVCServiceApp.ViewModel;
using SUVCServiceApp.Windows;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
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
    /// Логика взаимодействия для AddEmployeePage.xaml
    /// </summary>
    public partial class AddEmployeePage : Page
    {
        private readonly AdministratorWindow administratorWindow;
        public AddEmployeePage(AdministratorWindow administratorWindow)
        {
            InitializeComponent();
            this.administratorWindow = administratorWindow;
        }

        private async void buttonAddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int role = comboBoxRole.Text == "Сотрудник" ? 3 : 2;
                ApiDataProvider apiDataProvider = new ApiDataProvider();
                ResponseUsers user = new ResponseUsers
                {
                    Name = textBoxName.Text,
                    Surname = textBoxSurName.Text,
                    MiddleName = textBoxMiddleName.Text,
                    Login = textBoxLogin.Text,
                    Password = textBoxPassword.Text,
                    IDRole = role,
                };

                bool isSuccess = await apiDataProvider.AddDataToApi("Users", user);
                if (isSuccess)
                {
                    MessageBox.Show($"Сотрудник {textBoxSurName.Text} {textBoxName.Text} {textBoxMiddleName.Text} успешно добавлен!");
                }
                else
                {
                    MessageBox.Show("Произошла ошибка при добавлении данных! Проверьте поля ввода данных!");
                }
            }

            catch
            {
                MessageBox.Show("Проверьте заполненность данных и соединение с интернетом!");
            }
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            administratorWindow.FrameWorkspace.Navigate(new EmployeePage(administratorWindow));
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
                    var userProcessor = new ExcelDataAdd<ResponseUsers>(
                            "Users",
                            record => new ResponseUsers
                            {
                            Name = record["Имя"].ToString(),
                            Surname = record["Фамилия"].ToString(),
                            MiddleName = record["Отчество"].ToString(),
                            Login = record["Логин"].ToString(),
                            Password = record["Пароль"].ToString(),
                            IDRole = GetRoleIdByDepartment(record["Отдел"].ToString())
                            });

                    await userProcessor.AddDataFromExcel(filePath); ;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
        private int GetRoleIdByDepartment(string department)
        => department == "Учебный" ? 3 : department == "ИТ" ? 2 : 0;

        private void buttonDownloadTemplate_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Скачать шаблон?",
                "Шаблон добавления программ", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx|All Files|*.*";
                saveFileDialog.Title = "Выберите место для сохранения файла";
                saveFileDialog.FileName = "сотрудники.xlsx";
                bool? dialogResult = saveFileDialog.ShowDialog();
                if (dialogResult == true)
                {
                    try
                    {
                        string filePath = saveFileDialog.FileName;
                        string resourcePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", 
                                                                    "Resources", "TemplateAddData", "сотрудники.xlsx");
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
