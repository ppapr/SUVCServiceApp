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
    /// Логика взаимодействия для ChangeExecutor.xaml
    /// </summary>
    public partial class ChangeExecutor : Window
    {
        ResponseRequests response;
        private readonly DataGridLoader gridLoader;
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        public ChangeExecutor(ResponseRequests responseRequests)
        {
            InitializeComponent();
            gridLoader = new DataGridLoader(apiDataProvider);
            this.response = responseRequests;
            textBoxRequest.Text = response.UserRequestName + " " + response.EquipmentName;
            LoadData();
        }

        async void LoadData() => await gridLoader.LoadData<ResponseUsers>(comboBoxExecutors, "Users?idrole=2");

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void buttonSetExecutor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ApiDataProvider apiDataProvider = new ApiDataProvider();
                int currentRequestID = response.ID;
                var selectedExecutor = (ResponseUsers)comboBoxExecutors.SelectedItem;
                int executorID = selectedExecutor.ID;
                ResponseRequests request = new ResponseRequests
                {
                    ID = response.ID,
                    Description = response.Description,
                    DateCreateRequest = response.DateCreateRequest,
                    DateExecuteRequest = response.DateExecuteRequest,
                    IDStatus = 1,
                    IDPriority = response.IDPriority,
                    IDEquipment = response.IDEquipment,
                    IDUserRequest = response.IDUserRequest,
                    IDExecutorRequest = executorID
                };

                bool isSuccess = await apiDataProvider.UpdateDataToApi("Requests", currentRequestID, request);
                if (isSuccess)
                {
                    MessageBox.Show($"На заявку {response.UserRequestName + " " + response.EquipmentName} успешно назначен исполнитель" +
                        $" {selectedExecutor.FullName}!");
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
    }
}
