using NPOI.XWPF.UserModel;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SUVCServiceApp.Controller;
using SUVCServiceApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public InventoryPage()
        {
            InitializeComponent();
            LoadInventoryListView();
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
        private void LoadInventoryListView()
        {
            string inventoryFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inventories");
            if (Directory.Exists(inventoryFolder))
            {
                string[] inventoryFiles = Directory.GetFiles(inventoryFolder, "*.docx");
                listViewInventory.Items.Clear();
                foreach (string file in inventoryFiles)
                {
                    listViewInventory.Items.Add(System.IO.Path.GetFileName(file));
                }
            }
        }
        private void buttonAddInventory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateInventoryDirectory();
                CreateInventory();

                MessageBox.Show("Инвентаризация создана!");
            }
            catch
            {
                MessageBox.Show("Не удалось создать инвентарный отчет! Возможно произошла ошибка, " +
                    "проверьте соединение с интернетом или попробуйте позже!");
            }
        }

        async Task CreateInventory()
        {
            List<ResponseEquipment> equipmentData = await apiDataProvider.GetDataFromApi<ResponseEquipment>("Equipments");
            var countEquipment = equipmentData.Count.ToString();
            using (FileStream fs = new FileStream(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inventories", $"Инвентаризация за {DateTime.Now.ToShortDateString()}.docx"), FileMode.Create, FileAccess.Write))
            {
                XWPFDocument doc = new XWPFDocument();
                XWPFParagraph paragraph1 = doc.CreateParagraph();
                paragraph1.Alignment = ParagraphAlignment.CENTER;
                paragraph1.CreateRun().SetText($"Инвентаризация от {DateTime.Now.ToShortDateString()}");

                XWPFParagraph paragraph2 = doc.CreateParagraph();
                paragraph2.Alignment = ParagraphAlignment.CENTER;
                paragraph2.CreateRun().SetText($"Количество оборудования: {countEquipment}");
                XWPFTable table = doc.CreateTable(equipmentData.Count + 1, 2);
                table.GetRow(0).GetCell(0).SetText("Оборудование");
                table.GetRow(0).GetCell(1).SetText("Ответственный");
                for (int i = 0; i < equipmentData.Count; i++)
                {
                    table.GetRow(i + 1).GetCell(0).SetText(equipmentData[i].FullNameEquipment);
                    table.GetRow(i + 1).GetCell(1).SetText(equipmentData[i].OwnerName);
                }
                doc.Write(fs);
            }
        }
        private string selectedInventory;
        private void listViewInventory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedInventory = listViewInventory.SelectedItem as string;
        }

        private void buttonCheckInventory_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedInventory))
            {
                string reportFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inventories", selectedInventory);
                Process.Start(reportFilePath);
            }
            else
            {
                MessageBox.Show("Выберите инвентаризацию из списка.");
            }
        }
    }
}

