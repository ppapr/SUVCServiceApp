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

namespace SUVCServiceApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReportsPage.xaml
    /// </summary>
    public partial class ReportsPage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        public ReportsPage()
        {
            InitializeComponent();
            LoadQuery();

        }
        async void LoadQuery()
        {
            var spares = await apiDataProvider.GetDataFromApi<ResponseSpare>("SparesEquipments");
            textBlockCountSpares.Text += spares.Count.ToString();

            List<ResponseEquipment> data = await apiDataProvider.GetDataFromApi<ResponseEquipment>("Equipments");
            if (data != null)
            {
                var countEquipments = data.Where(equipment => equipment.IDStatus == 4).ToList();
                textBlockCountEquipments.Text += countEquipments.Count.ToString();
            }
        }

        public async Task<List<ResponseRequests>> GetDataFromApiForPeriod(DateTime startDate, DateTime endDate)
        {
            string apiEndpoint = $"Requests?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
            return await apiDataProvider.GetDataFromApi<ResponseRequests>(apiEndpoint);
        }

        async void ReportForRequests()
        {
            DateTime selectedStartDate = dateStartPeriod.SelectedDate ?? DateTime.MinValue;
            DateTime selectedEndDate = dateEndPeriod.SelectedDate ?? DateTime.MaxValue; 
            List<ResponseRequests> dataForPeriod = await GetDataFromApiForPeriod(selectedStartDate, selectedEndDate);
            var filteredData = dataForPeriod
                .Where(request => request.DateCreateRequest >= selectedStartDate && request.DateCreateRequest < selectedEndDate)
                .ToList();

            int completedRequestsCount = filteredData.Count(request => request.IDStatus == 3);
            // Пользователь, у которого наиболее часто возникают проблемы
            string mostIncidentUser = filteredData 
                .GroupBy(request => request.UserRequestName)
                .OrderByDescending(group => group.Count())
                .FirstOrDefault()?.Key;
            // Сотрудник, максимально выполнивший заявки
            string topExecutor = filteredData
                .GroupBy(request => request.UserExecutorName)
                .OrderByDescending(group => group.Count())
                .FirstOrDefault()?.Key;
            // Количество заявок каждого пользователя
            var requestsByUser = filteredData
                .GroupBy(request => request.UserRequestName)
                .ToDictionary(group => group.Key, group => group.Count());
            // Сотрудник, минимально выполнивший заявки
            string leastBusyExecutor = filteredData
                .GroupBy(request => request.UserExecutorName)
                .OrderBy(group => group.Count())
                .FirstOrDefault()?.Key;

        }

    }
}
