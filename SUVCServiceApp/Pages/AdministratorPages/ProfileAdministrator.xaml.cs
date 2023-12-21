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
using MessageBox = System.Windows.Forms.MessageBox;

namespace SUVCServiceApp.Pages.AdministratorPages
{
    /// <summary>
    /// Логика взаимодействия для ProfileAdministrator.xaml
    /// </summary>
    public partial class ProfileAdministrator : Page
    {
        public ProfileAdministrator()
        {
            InitializeComponent();
            LoadReportsToListView();
        }

        private void LoadReportsToListView()
        {
            string reportsFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            if (Directory.Exists(reportsFolderPath))
            {
                string[] reportFiles = Directory.GetFiles(reportsFolderPath, "*.pdf");
                listViewReports.Items.Clear();
                foreach (string reportFile in reportFiles)
                {
                    listViewReports.Items.Add(System.IO.Path.GetFileName(reportFile));
                }
            }
        }
        private string selectedReport;

        private void listViewReports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedReport = listViewReports.SelectedItem as string;
        }

        private void buttonCreateReport_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedReport))
            {
                string reportFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", selectedReport);
                Process.Start(reportFilePath);
            }
            else
            {
                MessageBox.Show("Выберите отчет из списка.");
            }
        }
    }
}
