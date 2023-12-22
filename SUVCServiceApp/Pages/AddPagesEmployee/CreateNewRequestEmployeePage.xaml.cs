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

namespace SUVCServiceApp.Pages.AddPagesEmployee
{
    /// <summary>
    /// Логика взаимодействия для CreateNewRequestEmployeePage.xaml
    /// </summary>
    public partial class CreateNewRequestEmployeePage : Page
    {
        private readonly EmployeeWindow employeeWindow;
        private int authenticatedUserId;
        private readonly DataGridLoader dataGridLoader;
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        public CreateNewRequestEmployeePage(int authenticatedUserId, EmployeeWindow employeeWindow)
        {
            InitializeComponent();
            this.authenticatedUserId = authenticatedUserId;
            this.employeeWindow = employeeWindow;
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadData();
        }

        private async void LoadData()
        {
            await dataGridLoader.LoadData<ResponseEquipment>(comboBoxEquipment, $"Equipments?user={authenticatedUserId}");
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            employeeWindow.FrameWorkspace.Navigate(new Pages.EmployeePages.RequestsEmployeePage(authenticatedUserId, employeeWindow));
        }
        bool isRequest;
        private async void buttonCreateRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime executeDefault = DateTime.Parse("01-01-0001");
                var equipment = (ResponseEquipment)comboBoxEquipment.SelectedItem;
                int equipmentID = 0;
                if (isRequest)
                {
                    equipmentID = (int)equipment.ID;
                    MessageBox.Show("Заявка");
                }
                else
                {
                    equipmentID = 61;
                    MessageBox.Show("Обращение");
                }
                ResponseRequests requests = new ResponseRequests
                {
                    Description = textBoxRequestDescription.Text,
                    DateCreateRequest = DateTime.Parse(DateTime.Now.ToShortDateString()),
                    DateExecuteRequest = executeDefault,
                    IDStatus = 1,
                    IDPriority = 2,
                    IDEquipment = equipmentID,
                    IDUserRequest = authenticatedUserId,
                    IDExecutorRequest = 10
                };
                bool isSuccess = await apiDataProvider.AddDataToApi("Requests", requests);
                if (isSuccess)
                {
                    MessageBox.Show($"Ваша заявка успешно создана!");
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButtonRequest.IsChecked == true) isRequest = true;
            else isRequest = false;
        }
    }
}
