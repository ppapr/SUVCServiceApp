using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SUVCServiceApp.Controller;
using SUVCServiceApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
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
            // Общее количество заявок за период
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
            // Сотрудник, минимально выполнивший заявки
            string leastBusyExecutor = filteredData
                .GroupBy(request => request.UserExecutorName)
                .OrderBy(group => group.Count())
                .FirstOrDefault()?.Key;
            using (var document = new PdfDocument())
            {
                document.Info.Title = "Отчет о заявках";
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Times New Roman", 14);

                gfx.DrawString($"Отчетность отдела информационных технологий", font, XBrushes.Black,
                    new XRect(0, 20, page.Width, page.Height), XStringFormats.TopCenter);
                gfx.DrawString($"Отчет с {selectedStartDate} по {selectedEndDate}", font, XBrushes.Black,
                    new XRect(15, 50, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString($"Количество выполненных заявок: {completedRequestsCount}", font, XBrushes.Black,
                    new XRect(15, 80, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString($"Сотрудник, выполнивший максимальное число заявок: {topExecutor}", font, XBrushes.Black,
                    new XRect(15, 110, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString($"Сотрудник, выполнивший минимальное число заявок: {leastBusyExecutor}", font, XBrushes.Black,
                    new XRect(15, 170, page.Width, page.Height), XStringFormats.TopLeft);

                string reportsFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                string reportFilePath = System.IO.Path.Combine(reportsFolderPath, $"Отчет по заявкам за {DateTime.Now.ToShortDateString()}.pdf");
                document.Save(reportFilePath);
            }
        }

        void CreateReportDirectory()
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string reportsFolderPath = System.IO.Path.Combine(exePath, "Reports");

            try
            {
                if (!Directory.Exists(reportsFolderPath))
                    Directory.CreateDirectory(reportsFolderPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании папки 'Reports': {ex.Message}");
            }
        }

        private void buttonRequestReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateReportDirectory();
                ReportForRequests();
                MessageBox.Show("Отчет создан!");
            }
            catch
            {
                MessageBox.Show("Не удалось создать отчет! Возможно произошла ошибка, " +
                    "проверьте соединение с интернетом или попробуйте позже!");
            }
        }
    }
}
