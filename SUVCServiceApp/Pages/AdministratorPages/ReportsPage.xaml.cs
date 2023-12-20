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
        }
        //public async Task<List<ResponseEquipment>> GetEquipmentForDisposal()
        //{
        //    //string apiEndpoint = "Equipments";
        //    //Func<ResponseEquipment, bool> searchCondition = equipment => equipment.StatusName.Equals("Требуется списание", StringComparison.OrdinalIgnoreCase);

        //    //return await apiDataProvider.SearchData<ResponseEquipment>(apiEndpoint, searchCondition);
        //}
    }
}
