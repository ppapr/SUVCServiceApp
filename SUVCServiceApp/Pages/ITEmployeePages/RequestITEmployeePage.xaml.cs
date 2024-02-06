using Org.BouncyCastle.Asn1.Ocsp;
using SUVCServiceApp.Controller;
using SUVCServiceApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MessageBox = System.Windows.MessageBox;

namespace SUVCServiceApp.Pages.ITEmployeePages
{
    /// <summary>
    /// Логика взаимодействия для RequestITEmployeePage.xaml
    /// </summary>
    public partial class RequestITEmployeePage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;

        private ResponseUsers authenticatedUser;
        private ResponseRequests currentRequest;

        int currentPage = 1;
        int sizePage = 20;
        int maxPages = 0;

        private NotifyIcon notifyIcon;
        private List<ResponseRequests> requests;
        private List<ResponseRequests> requestsAll;
        private int lastKnownRequestsCount;

        private DispatcherTimer timer;

        public RequestITEmployeePage(ResponseUsers authenticatedUser)
        {
            InitializeComponent();
            this.authenticatedUser = authenticatedUser;
            dataGridLoader = new DataGridLoader(apiDataProvider);
            notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Information,
                Visible = true
            };

            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Start();
            LoadData(currentPage, sizePage);
        }
        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (currentRequest == null)
                await LoadData(currentPage, sizePage);
        }
        private void ShowNotification(ResponseRequests request)
        {
            notifyIcon.ShowBalloonTip(3000, "Назначена заявка", $"Имя пользователя: {request.UserRequestName}\nОписание заявки: {request.Description}", ToolTipIcon.Info);
        }

        private async Task LoadData(int currentPage, int sizePage)
        {
            labelPage.Content = currentPage.ToString();
            requests = await apiDataProvider.GetDataFromApi<ResponseRequests>($"Requests?userExecutor={authenticatedUser.ID}");
            requestsAll = await apiDataProvider.GetDataFromApi<ResponseRequests>("Requests");
            requestsAll = requestsAll.Where(r => r.IDEquipment == 42 && r.IDExecutorRequest == 10).ToList();
            if (requests != null && requestsAll != null)
            {
                var request = requests.Concat(requestsAll).ToList();
                request = request
                    .Where(r => r.IDStatus == 1 || r.IDStatus == 2)
                    .OrderByDescending(r => r.DateCreateRequest)
                    .ToList();

                dataGridLoader.LoadData(listRequests, request, currentPage, sizePage);
                maxPages = (int)Math.Ceiling(request.Count * 1.0 / sizePage);
                lastKnownRequestsCount = Properties.Settings.Default.LastRequestCountIT;
                if (lastKnownRequestsCount != request.Count)
                {
                    try
                    {
                        lastKnownRequestsCount = request.Count;
                        ShowNotification(request[request.Count]);
                        Properties.Settings.Default.LastRequestCountIT = lastKnownRequestsCount;
                        Properties.Settings.Default.Save();
                    }
                    catch { }
                }
                if (currentPage == maxPages)
                    buttonNextPage.IsEnabled = false;
                else buttonNextPage.IsEnabled = true;
                if (currentPage == 1)
                    buttonPreviousPage.IsEnabled = false;
                else buttonPreviousPage.IsEnabled = true;
            }
        }

        public ResponseEquipment currentEquipment;

        private void buttonShowMap_Click(object sender, RoutedEventArgs e)
        {
            if (currentEquipment != null)
            {
                new Windows.InteractiveMapWindow(currentEquipment).ShowDialog();
            }
            else
                MessageBox.Show("Выберите оборудование!");
        }

        private async Task GetCurrentEqipment()
        {
            if (currentRequest != null)
            {
                List<ResponseEquipment> data = await apiDataProvider.GetDataFromApi<ResponseEquipment>("Equipments");
                if (data != null)
                {
                    var countEquipments = data.Where(equipment => equipment.ID != 42 && equipment.ID != 61 && equipment.ID == currentRequest.IDEquipment).ToList();
                    currentEquipment = countEquipments.FirstOrDefault();
                }
            }
        }
        private async void listRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentRequest = (ResponseRequests)listRequests.SelectedItem;
            await GetCurrentEqipment();
        }

        async Task UpdateStatusRequest(int statusID)
        {
            if (currentRequest != null)
            {
                try
                {
                    ApiDataProvider apiDataProvider = new ApiDataProvider();
                    int currentRequestID = currentRequest.ID;
                    ResponseRequests request = new ResponseRequests
                    {
                        ID = currentRequest.ID,
                        Description = currentRequest.Description,
                        DateCreateRequest = currentRequest.DateCreateRequest,
                        DateExecuteRequest = currentRequest.DateExecuteRequest,
                        IDStatus = statusID,
                        IDPriority = currentRequest.IDPriority,
                        IDEquipment = currentRequest.IDEquipment,
                        IDUserRequest = currentRequest.IDUserRequest,
                        IDExecutorRequest = authenticatedUser.ID
                    };

                    bool isSuccess = await apiDataProvider.UpdateDataToApi("Requests", currentRequestID, request);
                    if (isSuccess)
                    {
                        MessageBox.Show($"Вы изменили статус заявки сотрудника {currentRequest.UserRequestName}");
                        await LoadData(currentPage, sizePage);
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка! Проверьте поля ввода данных!");
                    }
                }
                catch
                {
                    MessageBox.Show("Проверьте корректность данных и соеднинение с интернетом!");
                }
            }
            else MessageBox.Show("Выберите заявку!");
        }

        private async void buttonStartExecute_Click(object sender, RoutedEventArgs e)
        {
            await UpdateStatusRequest(2);
        }

        private async void buttonEndExecute_Click(object sender, RoutedEventArgs e)
        {
            await UpdateStatusRequest(3);
        }

        private async void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = textBoxSearch.Text;
            Func<ResponseRequests, string> searchProperty = item => item.Description;
            await dataGridLoader.LoadData(listRequests, "Requests", searchProperty, searchTerm);
        }

        private async void buttonPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                await LoadData(currentPage, sizePage);
            }
        }

        private async void buttonNextPage_Click(object sender, RoutedEventArgs e)
        {
            currentPage++;
            await LoadData(currentPage, sizePage);
        }
    }
}
