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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SUVCServiceApp.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Drawing;

namespace SUVCServiceApp.Pages.EmployeePages
{
    /// <summary>
    /// Логика взаимодействия для RequestsEmployeePage.xaml
    /// </summary>
    public partial class RequestsEmployeePage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        private ResponseUsers authenticatedUser;
        private readonly EmployeeWindow employeeWindow;

        int currentPage = 1;
        int sizePage = 20;
        int maxPages = 0;

        private NotifyIcon notifyIcon;
        private List<ResponseRequests> requests;
        private DispatcherTimer timer;

        private List<ResponseRequests> previousRequests;
        public RequestsEmployeePage(ResponseUsers authenticatedUser, EmployeeWindow employeeWindow)
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

            LoadData(currentPage,sizePage);
            this.employeeWindow = employeeWindow;
        }
        private async void Timer_Tick(object sender, EventArgs e)
        {
            await LoadData(currentPage, sizePage);
            CheckStatusChange();
        }
        private void ShowNotification(ResponseRequests request)
        {
            notifyIcon.ShowBalloonTip(3000, "Изменен статус заявки", $"Заявка: {request.Description}", ToolTipIcon.Info);
        }

        private async Task LoadData(int currentPage, int sizePage)
        {
            labelPage.Content = currentPage.ToString();
            requests = await apiDataProvider.GetDataFromApi<ResponseRequests>($"Requests?userRequest={authenticatedUser.ID}");
            if (requests != null)
            {
                requests = requests.Where(r => r.IDStatus == 1 || r.IDStatus == 2).OrderByDescending(r => r.DateCreateRequest).ToList();
                dataGridLoader.LoadData(listRequests, requests, currentPage, sizePage);

                maxPages = (int)Math.Ceiling(requests.Count * 1.0 / sizePage);
                previousRequests = new List<ResponseRequests>(listRequests.Items.OfType<ResponseRequests>());
                if (currentPage == maxPages)
                    buttonNextPage.IsEnabled = false;
                else buttonNextPage.IsEnabled = true;
                if (currentPage == 1)
                    buttonPreviousPage.IsEnabled = false;
                else buttonPreviousPage.IsEnabled = true;
            }
        }

        private void CheckStatusChange()
        {
            foreach (ResponseRequests currentRequest in listRequests.Items)
            {
                ResponseRequests previousRequest = previousRequests.FirstOrDefault(x => x.ID == currentRequest.ID);

                if (previousRequest != null && currentRequest.IDStatus != previousRequest.IDStatus)
                {
                    ShowNotification(currentRequest);
                }
            }
        }

        private void buttonAddRequest_Click(object sender, RoutedEventArgs e)
        {
            employeeWindow.FrameWorkspace.Navigate(new Pages.AddPagesEmployee.CreateNewRequestEmployeePage(authenticatedUser, employeeWindow));
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
