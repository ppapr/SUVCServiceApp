using NPOI.XWPF.UserModel;
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

        void CreateNewParagraph(XWPFDocument doc, string textParagraph, double spacing, ParagraphAlignment paragraphAlignment)
        {
            XWPFParagraph reportParagraph = doc.CreateParagraph();
            reportParagraph.Alignment = paragraphAlignment;
            reportParagraph.setSpacingBetween(spacing, LineSpacingRule.EXACT);
            XWPFRun reportRun = reportParagraph.CreateRun();
            reportRun.SetText(textParagraph);
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

            using (var doc = new XWPFDocument())
            {
                CreateNewParagraph(doc, "Государственное бюджетное профессиональное образовательное учреждение " +
                                    "\"Южно-Уральский многопрофильный колледж\"", 0, ParagraphAlignment.CENTER);
                CreateNewParagraph(doc, "Отчет информационного отдела", 30, ParagraphAlignment.CENTER);
                CreateNewParagraph(doc, $"Отчет с {selectedStartDate} по {selectedEndDate}", 15, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"Количество выполненных заявок информационным отделом: {completedRequestsCount}", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"ИТ-специалист, выполнивший максимальное число заявок: {topExecutor}", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"ИТ-специалист, выполнивший минимальное число заявок: {leastBusyExecutor}", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, $"Сотрудник, оставивший наибольшее число заявок за период: {mostIncidentUser}", 0, ParagraphAlignment.LEFT);
                CreateNewParagraph(doc, "Заведующий отделом ИТ Чухарев В.М. ____________", 80, ParagraphAlignment.RIGHT);

                string reportsFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                string reportFilePath = System.IO.Path.Combine(reportsFolderPath, $"Отчет по заявкам за {DateTime.Now.ToShortDateString()}.docx");

                using (FileStream fs = new FileStream(reportFilePath, FileMode.Create, FileAccess.Write))
                {
                    doc.Write(fs);
                }
            }
        }


        async Task CreateSpareEquipmentReport()
        {
            List<ResponseSpare> sparesData = await apiDataProvider.GetDataFromApi<ResponseSpare>("SparesEquipments");

            // Создание документа Word
            using (FileStream fs = new FileStream(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", 
                $"Отчет по запчастям за {DateTime.Now.ToShortDateString()}.docx"), FileMode.Create, FileAccess.Write))
            {
                XWPFDocument doc = new XWPFDocument();

                // Добавление заголовка
                XWPFParagraph titleParagraph = doc.CreateParagraph();
                XWPFRun titleRun = titleParagraph.CreateRun();
                titleRun.SetText("Отчет по запчастям");
                titleRun.FontSize = 14;
                titleParagraph.Alignment = ParagraphAlignment.CENTER;

                // Добавление таблицы
                XWPFTable table = doc.CreateTable(sparesData.Count + 1, 2);

                // Добавление заголовков таблицы
                table.GetRow(0).GetCell(0).SetText("Наименование");
                table.GetRow(0).GetCell(1).SetText("Оборудование");

                // Заполнение таблицы данными
                for (int i = 0; i < sparesData.Count; i++)
                {
                    table.GetRow(i + 1).GetCell(0).SetText(sparesData[i].SpareName);
                    table.GetRow(i + 1).GetCell(1).SetText(sparesData[i].Equipment);
                }

                // Сохранение документа
                doc.Write(fs);
            }
        }
        async Task CreateWriteOffEquipmentReport()
        {

            List<ResponseEquipment> writeOffEquipmentData = await apiDataProvider.GetDataFromApi<ResponseEquipment>("Equipments");
            writeOffEquipmentData = writeOffEquipmentData.Where(equipment => equipment.IDStatus == 4).ToList();

            // Создание документа Word
            using (FileStream fs = new FileStream(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", 
                $"Отчет по оборудованию за {DateTime.Now.ToShortDateString()}.docx"), FileMode.Create, FileAccess.Write))
            {
                XWPFDocument doc = new XWPFDocument();

                // Добавление заголовка
                XWPFParagraph titleParagraph = doc.CreateParagraph();
                XWPFRun titleRun = titleParagraph.CreateRun();
                titleRun.SetText("Отчет по списанию оборудования");
                titleRun.FontSize = 14;
                titleParagraph.Alignment = ParagraphAlignment.CENTER;

                // Добавление таблицы
                XWPFTable table = doc.CreateTable(writeOffEquipmentData.Count + 1, 7);

                // Добавление заголовков таблицы
                string[] headers = { "Наименование", "Описание", "Имя в сети", "Инвентарный номер", "Владелец", "Расположение", "Статус" };
                for (int i = 0; i < headers.Length; i++)
                {
                    table.GetRow(0).GetCell(i).SetText(headers[i]);
                }

                // Заполнение таблицы данными
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

                // Сохранение документа
                doc.Write(fs);
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
                await CreateSpareEquipmentReport();
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
                await CreateWriteOffEquipmentReport();
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
