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


        async Task CreateSpareEquipmentReport(string reportTitle)
        {
            List<ResponseSpare> sparesData = await apiDataProvider.GetDataFromApi<ResponseSpare>("SparesEquipments");
            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Arial", 12);
                gfx.DrawString(reportTitle, font, XBrushes.Black, new XRect(10, 10, page.Width, page.Height), XStringFormats.TopCenter);
                int startX = 10;
                int startY = 40;
                int rowHeight = 20;
                gfx.DrawString("Наименование", font, XBrushes.Black, new XRect(startX + 60, startY, 150, rowHeight), XStringFormats.TopLeft);
                gfx.DrawString("Оборудование", font, XBrushes.Black, new XRect(startX + 220, startY, 150, rowHeight), XStringFormats.TopLeft);
                startY += rowHeight;
                foreach (var spare in sparesData)
                {
                    gfx.DrawString(spare.SpareName, font, XBrushes.Black, new XRect(startX + 60, startY, 150, rowHeight), XStringFormats.TopLeft);
                    gfx.DrawString(spare.Equipment, font, XBrushes.Black, new XRect(startX + 220, startY, 150, rowHeight), XStringFormats.TopLeft);
                    startY += rowHeight;
                }

                string reportsFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                string reportFilePath = System.IO.Path.Combine(reportsFolderPath, $"Отчет по запчастям за {DateTime.Now.ToShortDateString()}.pdf");
                document.Save(reportFilePath);
            }
        }
        async Task CreateWriteOffEquipmentReport(string reportTitle)
        {

            List<ResponseEquipment> writeOffEquipmentData = await apiDataProvider.GetDataFromApi<ResponseEquipment>("Equipments");
            writeOffEquipmentData = writeOffEquipmentData.Where(equipment => equipment.IDStatus == 4).ToList();
            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Arial", 6);
                gfx.DrawString(reportTitle, font, XBrushes.Black, new XRect(10, 10, page.Width, page.Height), XStringFormats.TopCenter);
                int startX = 10;
                int startY = 40;
                int rowHeight = 10;
                int columnWidth = 150;
                gfx.DrawString("Наименование", font, XBrushes.Black,    new XRect(startX, startY, 150, rowHeight), XStringFormats.TopLeft);
                gfx.DrawString("Описание", font, XBrushes.Black,        new XRect(startX + 80, startY, 150, rowHeight), XStringFormats.TopLeft);
                gfx.DrawString("Имя в сети", font, XBrushes.Black,      new XRect(startX + 230, startY, 150, rowHeight), XStringFormats.TopLeft);
                gfx.DrawString("Инвентарный номер", font, XBrushes.Black, new XRect(startX + 290, startY, 150, rowHeight), XStringFormats.TopLeft);
                gfx.DrawString("Владелец", font, XBrushes.Black,        new XRect(startX + 360, startY, 150, rowHeight), XStringFormats.TopLeft);
                gfx.DrawString("Расположение", font, XBrushes.Black,    new XRect(startX + 450, startY, 150, rowHeight), XStringFormats.TopLeft);
                gfx.DrawString("Статус", font, XBrushes.Black,          new XRect(startX + 500, startY, 150, rowHeight), XStringFormats.TopLeft);
                startY += rowHeight;
                foreach (var equipment in writeOffEquipmentData)
                {
                    DrawCellWithWordWrap(gfx, equipment.EquipmentName, font, startX, startY, columnWidth, rowHeight);
                    DrawCellWithWordWrap(gfx, equipment.EquipmentDescription, font, startX + 80, startY, columnWidth, rowHeight);
                    DrawCellWithWordWrap(gfx, equipment.NetworkName, font, startX + 230, startY, columnWidth, rowHeight);
                    DrawCellWithWordWrap(gfx, equipment.InventoryName, font, startX + 290, startY, columnWidth, rowHeight);
                    DrawCellWithWordWrap(gfx, equipment.OwnerName, font, startX + 360, startY, columnWidth, rowHeight);
                    DrawCellWithWordWrap(gfx, equipment.LocationAuditorium.ToString(), font, startX + 470, startY, columnWidth, rowHeight);
                    DrawCellWithWordWrap(gfx, equipment.StatusName, font, startX + 500, startY, columnWidth, rowHeight);

                    startY += rowHeight * equipment.EquipmentDescription.Split(' ').Length;
                }
                string reportsFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                string reportFilePath = System.IO.Path.Combine(reportsFolderPath, $"Отчет по оборудованию за {DateTime.Now.ToShortDateString()}.pdf");
                document.Save(reportFilePath);
            }
        }
        static void DrawCellWithWordWrap(XGraphics gfx, string text, XFont font, int x, int y, int width, int height, int maxCharactersPerLine = 40)
        {
            string[] words = text.Split(' ');
            double currentX = x;
            double currentY = y;
            string currentLine = "";

            foreach (var word in words)
            {
                if ((currentLine + " " + word).Length > maxCharactersPerLine)
                {
                    gfx.DrawString(currentLine, font, XBrushes.Black, new XRect(x, currentY, width, height), XStringFormats.TopLeft);
                    currentLine = word;
                    currentY += height;
                }
                else
                {
                    currentLine += " " + word;
                }
            }
            gfx.DrawString(currentLine.Trim(), font, XBrushes.Black, new XRect(x, currentY, width, height), XStringFormats.TopLeft);
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

        private async void buttonSpareReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateReportDirectory();
                await CreateSpareEquipmentReport($"Отчет по запчастям за {DateTime.Now.ToShortDateString()}");
                MessageBox.Show("Отчет создан!");
            }
            catch
            {
                MessageBox.Show("Не удалось создать отчет! Возможно произошла ошибка, " +
                    "проверьте соединение с интернетом или попробуйте позже!");
            }
        }

        private async void buttonEquipmentReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateReportDirectory();
                await CreateWriteOffEquipmentReport($"Отчет по списанию за {DateTime.Now.ToShortDateString()}");
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
