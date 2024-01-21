using NPOI.XWPF.UserModel;
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
using System.Drawing;

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

        void CreateNewParagraph(XWPFDocument doc, string textParagraph, double spacing, ParagraphAlignment paragraphAlignment)
        {
            XWPFParagraph reportParagraph = doc.CreateParagraph();
            reportParagraph.Alignment = paragraphAlignment;
            reportParagraph.setSpacingBetween(spacing, LineSpacingRule.EXACT);
            XWPFRun reportRun = reportParagraph.CreateRun();
            reportRun.SetText(textParagraph);
        }
        public async Task<List<ResponseRequests>> GetDataFromApiForPeriod(DateTime startDate, DateTime endDate)
        {
            string apiEndpoint = $"Requests";
            List<ResponseRequests> data = await apiDataProvider.GetDataFromApi<ResponseRequests>(apiEndpoint);
            return data.Where(r => r.DateCreateRequest.Date >= startDate && r.DateCreateRequest.Date <=endDate).ToList();
        }

        public async Task<ReportData> GetReportData(DateTime selectedStartDate, DateTime selectedEndDate)
        {
            List<ResponseRequests> data = await GetDataFromApiForPeriod(selectedStartDate, selectedEndDate);
            return new ReportData(data);
        }


        async Task<bool> ReportForRequests()
        {
            DateTime firstStartDate = dateStartPeriodFirst.SelectedDate ?? DateTime.MinValue;
            DateTime firstEndDate = dateEndPeriodFirst.SelectedDate ?? DateTime.MinValue;

            DateTime secondStartDate = dateStartPeriodSecond.SelectedDate ?? DateTime.MinValue;
            DateTime secondEndDate = dateEndPeriodSecond.SelectedDate ?? DateTime.MinValue;

            ReportData reportDataFirst = await GetReportData(firstStartDate, firstEndDate);
            ReportData reportDataSecond = await GetReportData(secondStartDate, secondEndDate);

            using (var doc = new XWPFDocument())
            {
                CreateNewParagraph(doc, "Государственное бюджетное профессиональное образовательное учреждение " +
                                    "\"Южно-Уральский многопрофильный колледж\"", 0, ParagraphAlignment.CENTER);
                CreateNewParagraph(doc, "Отчет информационного отдела", 30, ParagraphAlignment.CENTER);
                CreateNewParagraph(doc, $"Период с {firstStartDate} по {firstEndDate}", 15, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"Количество выполненных заявок информационным отделом: {reportDataFirst.CompletedRequestsCount}", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"ИТ-специалист, выполнивший максимальное число заявок: {reportDataFirst.TopExecutor} ({reportDataFirst.TopExecutorRequestCount.ToString()})", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"ИТ-специалист, выполнивший минимальное число заявок: {reportDataFirst.LeastBusyExecutor} ({reportDataFirst.LeastBusyExecutortCount.ToString()})", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"Сотрудник, оставивший наибольшее число заявок за период: {reportDataFirst.MostIncidentUser} ({reportDataFirst.MostIncidentUserCount.ToString()})", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"Заявок в обработке: {reportDataFirst.CountRequestProcessed}", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"Заявок выполняется: {reportDataFirst.CountRequestExecuted}", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"Заявок выполнено: {reportDataFirst.CountRequestCompleted}", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"Заявок отказано: {reportDataFirst.CountRequestRejected}", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"Всего заявок: {reportDataFirst.CountRequestTotal}", 0, ParagraphAlignment.LEFT);


                CreateNewParagraph(doc, $"Период с {secondStartDate} по {secondEndDate}", 15, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"Количество выполненных заявок информационным отделом: {reportDataSecond.CompletedRequestsCount}", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"ИТ-специалист, выполнивший максимальное число заявок: {reportDataSecond.TopExecutor} ({reportDataSecond.TopExecutorRequestCount.ToString()})", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"ИТ-специалист, выполнивший минимальное число заявок: {reportDataSecond.LeastBusyExecutor} ({reportDataSecond.LeastBusyExecutortCount.ToString()})", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"Сотрудник, оставивший наибольшее число заявок за период: {reportDataSecond.MostIncidentUser} ({reportDataSecond.MostIncidentUserCount.ToString()})", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"Заявок в обработке: {reportDataSecond.CountRequestProcessed}", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"Заявок выполняется: {reportDataSecond.CountRequestExecuted}", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"Заявок выполнено: {reportDataSecond.CountRequestCompleted}", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"Заявок отказано: {reportDataSecond.CountRequestRejected}", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"Всего заявок: {reportDataSecond.CountRequestTotal}", 0, ParagraphAlignment.LEFT);

                

                CreateNewParagraph(doc, "Заведующий отделом ИТ Чухарев В.М. ____________", 80, ParagraphAlignment.RIGHT);

                string reportsFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                string reportFilePath = System.IO.Path.Combine(reportsFolderPath, $"[{DateTime.Now.ToShortDateString()}] Отчет по заявкам.docx");
                if (File.Exists(reportFilePath))
                {
                    MessageBoxResult result = MessageBox.Show("Файл уже существует. Заменить предыдущий файл?", "Подтверждение",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.No)
                    {
                        MessageBox.Show("Создание отчета отменено!");
                        return false;
                    }
                }
                using (FileStream fs = new FileStream(reportFilePath, FileMode.Create, FileAccess.Write))
                {
                    doc.Write(fs);

                    return true;
                }
            }
        }


        async Task<bool> CreateSpareEquipmentReport()
        {
            List<ResponseSpare> sparesData = await apiDataProvider.GetDataFromApi<ResponseSpare>("SparesEquipments");
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports",
                $"[{DateTime.Now.ToShortDateString()}] Отчет по запчастям.docx");
            if (File.Exists(filePath))
            {
                MessageBoxResult result = MessageBox.Show("Файл уже существует. Заменить предыдущий файл?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    MessageBox.Show("Создание отчета отменено!");
                    return false;
                }
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                XWPFDocument doc = new XWPFDocument();
                XWPFParagraph titleParagraph = doc.CreateParagraph();
                XWPFRun titleRun = titleParagraph.CreateRun();
                titleRun.SetText("Отчет по запчастям");
                titleRun.FontSize = 14;
                titleParagraph.Alignment = ParagraphAlignment.CENTER;
                XWPFTable table = doc.CreateTable(sparesData.Count + 1, 2);
                table.GetRow(0).GetCell(0).SetText("Наименование");
                table.GetRow(0).GetCell(1).SetText("Оборудование");
                for (int i = 0; i < sparesData.Count; i++)
                {
                    table.GetRow(i + 1).GetCell(0).SetText(sparesData[i].SpareName);
                    table.GetRow(i + 1).GetCell(1).SetText(sparesData[i].Equipment);
                }
                doc.Write(fs);

                return true;
            }
        }
        async Task<bool> CreateWriteOffEquipmentReport()
        {

            List<ResponseEquipment> writeOffEquipmentData = await apiDataProvider.GetDataFromApi<ResponseEquipment>("Equipments");
            writeOffEquipmentData = writeOffEquipmentData.Where(equipment => equipment.IDStatus == 4).ToList();
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports",
                $"[{DateTime.Now.ToShortDateString()}] Отчет по оборудованию.docx");

            if (File.Exists(filePath))
            {
                MessageBoxResult result = MessageBox.Show("Файл уже существует. Заменить предыдущий файл?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    MessageBox.Show("Создание отчета отменено!");
                    return false;
                }
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                XWPFDocument doc = new XWPFDocument();
                XWPFParagraph titleParagraph = doc.CreateParagraph();
                XWPFRun titleRun = titleParagraph.CreateRun();
                titleRun.SetText("Отчет по списанию оборудования");
                titleRun.FontSize = 14;
                titleParagraph.Alignment = ParagraphAlignment.CENTER;
                XWPFTable table = doc.CreateTable(writeOffEquipmentData.Count + 1, 7);
                string[] headers = { "Наименование", "Описание", "Имя в сети", "Инвентарный номер", "Владелец", "Расположение", "Статус" };
                for (int i = 0; i < headers.Length; i++)
                {
                    table.GetRow(0).GetCell(i).SetText(headers[i]);
                }
                for (int i = 0; i < writeOffEquipmentData.Count; i++)
                {
                    table.GetRow(i + 1).GetCell(0).SetText(writeOffEquipmentData[i].EquipmentName);
                    table.GetRow(i + 1).GetCell(1).SetText(writeOffEquipmentData[i].EquipmentDescription);
                    table.GetRow(i + 1).GetCell(2).SetText(writeOffEquipmentData[i].NetworkName);
                    table.GetRow(i + 1).GetCell(3).SetText(writeOffEquipmentData[i].InventoryName);
                    table.GetRow(i + 1).GetCell(4).SetText(writeOffEquipmentData[i].OwnerName);
                    table.GetRow(i + 1).GetCell(5).SetText(writeOffEquipmentData[i].LocationAuditorium.ToString());
                    table.GetRow(i + 1).GetCell(6).SetText(writeOffEquipmentData[i].StatusName);
                }
                doc.Write(fs);

                return true;
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

        private async void buttonRequestReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateReportDirectory();
                if (await ReportForRequests())
                    MessageBox.Show("Отчет создан!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось создать отчет! Возможно произошла ошибка, " +
                    "проверьте соединение с интернетом или попробуйте позже!" + ex.ToString());
            }
        }

        private async void buttonSpareReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateReportDirectory();
                if (await CreateSpareEquipmentReport())
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
                if (await CreateWriteOffEquipmentReport())
                    MessageBox.Show("Отчет создан!");
            }
            catch
            {
                MessageBox.Show("Не удалось создать отчет! Возможно произошла ошибка, " +
                    "проверьте соединение с интернетом или попробуйте позже!");
            }
        }
    }
    public class ReportData
    {
        private static ApiDataProvider apiDataProvider = new ApiDataProvider();
        public int CompletedRequestsCount { get; set; }
        public string MostIncidentUser { get; set; }
        public int MostIncidentUserCount { get; set; }
        public string TopExecutor { get; set; }
        public int TopExecutorRequestCount { get; set; }
        public string LeastBusyExecutor { get; set; }
        public int LeastBusyExecutortCount { get; set; }
        public int CountRequestExecuted { get; set; }
        public int CountRequestProcessed { get; set; }
        public int CountRequestCompleted { get; set; }
        public int CountRequestRejected { get; set; }
        public int CountRequestTotal { get; set; }

        public ReportData(List<ResponseRequests> data)
        {
            this.CompletedRequestsCount = data
                .Where(request => request.IDStatus == 3)
                .Count();
            this.MostIncidentUser = data
                .GroupBy(request => request.UserRequestName)
                .OrderByDescending(group => group.Count())
                .FirstOrDefault()?.Key;
            this.MostIncidentUserCount = data
                .Count(request => request.UserRequestName == this.MostIncidentUser);
            this.TopExecutor = data
                .Where(user => user.IDExecutorRequest != 10)
                .GroupBy(request => request.UserExecutorName)
                .OrderByDescending(group => group.Count())
                .FirstOrDefault()?.Key;
            this.TopExecutorRequestCount = data
                .Count(request => request.UserExecutorName == this.TopExecutor);
            this.LeastBusyExecutor = data
                .GroupBy(request => request.UserExecutorName)
                .OrderBy(group => group.Count())
                .FirstOrDefault()?.Key;
            this.LeastBusyExecutortCount = data
                .Count(request => request.UserExecutorName == this.LeastBusyExecutor);
            this.CountRequestExecuted = data
                .Count(request => request.IDStatus == 2); // выполняется
            this.CountRequestCompleted = data
                .Count(request => request.IDStatus == 3); // выполнено
            this.CountRequestProcessed = data
                .Count(request => request.IDStatus == 1); // В обработке
            this.CountRequestRejected = data
                .Count(request => request.IDStatus == 4); // Отказано
            this.CountRequestTotal = data 
                .Count();                               // Всего
        }
    }

}
