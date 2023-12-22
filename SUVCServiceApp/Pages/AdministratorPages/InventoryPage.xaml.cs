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
    /// Логика взаимодействия для InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        public InventoryPage()
        {
            InitializeComponent();
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
        }

        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadData<ResponseEquipment>(listEquipments, "Equipments");
        }

        void CreateInventoryDirectory()
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string reportsFolderPath = System.IO.Path.Combine(exePath, "Inventories");

            try
            {
                if (!Directory.Exists(reportsFolderPath))
                    Directory.CreateDirectory(reportsFolderPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании папки 'Inventories': {ex.Message}");
            }
        }

        private async void buttonAddInventory_Click(object sender, RoutedEventArgs e)
        {
            
            List<ResponseEquipment> equipmentData = await apiDataProvider.GetDataFromApi<ResponseEquipment>("Equipments");
            var countEquipment = equipmentData.Count.ToString();
            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Arial", 12);
                gfx.DrawString($"Инвентаризация от {DateTime.Now.ToShortDateString()}", font, XBrushes.Black, new XRect(10, 10, page.Width, page.Height), XStringFormats.TopCenter);
                gfx.DrawString($"Количество оборудования: {countEquipment}", font, XBrushes.Black, new XRect(10, 10, page.Width, page.Height), XStringFormats.TopCenter);
                int startX = 10;
                int startY = 40;
                int rowHeight = 20;
                gfx.DrawString("Оборудование", font, XBrushes.Black, new XRect(startX + 60, startY, 150, rowHeight), XStringFormats.TopLeft);
                gfx.DrawString("Ответственный", font, XBrushes.Black, new XRect(startX + 220, startY, 150, rowHeight), XStringFormats.TopLeft);
                startY += rowHeight;
                foreach (var equipment in equipmentData)
                {
                    gfx.DrawString(equipment.FullNameEquipment, font, XBrushes.Black, new XRect(startX + 60, startY, 150, rowHeight), XStringFormats.TopLeft);
                    gfx.DrawString(equipment.OwnerName, font, XBrushes.Black, new XRect(startX + 220, startY, 150, rowHeight), XStringFormats.TopLeft);
                    startY += rowHeight;
                }

                string reportsFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inventories");
                string reportFilePath = System.IO.Path.Combine(reportsFolderPath, $"Инвентаризация за {DateTime.Now.ToShortDateString()}.pdf");
                document.Save(reportFilePath);
            }
        }
    }
}
