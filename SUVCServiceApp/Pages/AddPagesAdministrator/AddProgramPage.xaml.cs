using Microsoft.Win32;
using SUVCServiceApp.Classes;
using SUVCServiceApp.Controller;
using SUVCServiceApp.ViewModel;
using SUVCServiceApp.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
using System.Reflection;

namespace SUVCServiceApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddProgramPage.xaml
    /// </summary>
    public partial class AddProgramPage : Page
    {
        private readonly AdministratorWindow administratorWindow;
        private readonly DataGridLoader dataGridLoader;
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        public AddProgramPage(AdministratorWindow administratorWindow)
        {
            InitializeComponent();
            this.administratorWindow = administratorWindow;
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
        }
        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadData<ResponseSpecialization>(comboBoxSpecialization, "Specializations");
        }

        private async void buttonAddProgram_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int specializationID = (int)comboBoxSpecialization.SelectedValue;
                ResponseRegistry program = new ResponseRegistry
                {
                    NameProgram = textBoxNameProgram.Text,
                    DescriptionProgram = textBoxDescriptionProgram.Text,
                    VersionProgram = textBoxVersionProgram.Text,
                    IDSpecialization = specializationID
                };

                bool isSuccess = await apiDataProvider.AddDataToApi("RegistryPrograms", program);
                if (isSuccess)
                {
                    MessageBox.Show($"Программа {textBoxNameProgram.Text} успешно добавлена!");
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
            administratorWindow.FrameWorkspace.Navigate(new RegsitryProgramPage(administratorWindow));
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
                    var specializations = await apiDataProvider.GetDataFromApi<ResponseSpecialization>("Specializations");

                    var programProccessor = new ExcelDataAdd<ResponseRegistry>(
                        "RegistryPrograms",
                        record =>
                        {
                            string specializationName = record["Специальность"].ToString();
                            int specializationID = GetSpecializationID(specializations, specializationName);
                            return new ResponseRegistry
                            {
                                NameProgram = record["Название"].ToString(),
                                DescriptionProgram = record["Описание"].ToString(),
                                VersionProgram = record["Версия"].ToString(),
                                IDSpecialization = specializationID
                            };
                        });

                    await programProccessor.AddDataFromExcel(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
        private int GetSpecializationID(IEnumerable<ResponseSpecialization> specializations, string specializationName)
        {
            var currentSpecialization = specializations.FirstOrDefault(spec => spec.NameSpecialization == specializationName);
            return currentSpecialization?.ID ?? 0;
        }

        private void buttonDownloadTemplate_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Скачать шаблон?",
                "Шаблон добавления программ", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx|All Files|*.*";
                saveFileDialog.Title = "Выберите место для сохранения файла";
                saveFileDialog.FileName = "программы.xlsx";
                bool? dialogResult = saveFileDialog.ShowDialog();
                if (dialogResult == true)
                {
                    try
                    {
                        string filePath = saveFileDialog.FileName;
                        string resourcePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources", "TemplateAddData", "программы.xlsx");
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
