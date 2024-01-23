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
using System.Windows.Threading;

namespace SUVCServiceApp.Controls
{
    /// <summary>
    /// Логика взаимодействия для MapControl.xaml
    /// </summary>
    public partial class MapControl : UserControl
    {
        public static DependencyProperty UserProperty =
     DependencyProperty.Register("AuthenticatedUser", typeof(ResponseUsers), typeof(MapControl));
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();

        public ResponseUsers AuthenticatedUser
        {
            get { return (ResponseUsers)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }


        List<ResponseRequests> requestsList;
        public MapControl()
        {
            InitializeComponent();
            Loaded += MapControl_Loaded;
        }

        private void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await GetRequestAsync(); 
            FindAndProcessRectangles(this);
        }

        private void FindAndProcessRectangles(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is Rectangle rectangle && rectangle.Name.StartsWith("c"))
                {
                    string cabinetNumber = rectangle.Name.Substring(1);
                    ResponseRequests matchingRequest = requestsList.FirstOrDefault(r => r.EquipmentName.Contains(cabinetNumber));

                    if (matchingRequest != null)
                    {
                        rectangle.Fill = new SolidColorBrush(Colors.Green);
                    }
                }

                FindAndProcessRectangles(child);
            }
        }

        public async Task GetRequestAsync()
        {
            var requests = await apiDataProvider.GetDataFromApi<ResponseRequests>($"Requests?userExecutor={AuthenticatedUser.ID}");
            requestsList = requests.ToList();
        }


        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle rect)
            {
                new Windows.MapRequestsWindow(AuthenticatedUser, rect).ShowDialog();
            }
        }

        private void buttonFloorTwo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            floorFive.Visibility = Visibility.Hidden;
            floorFour.Visibility = Visibility.Hidden;
            floorThree.Visibility = Visibility.Hidden;
            floorTwo.Visibility = Visibility.Visible;
        }

        private void buttonFloorThree_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            floorFive.Visibility = Visibility.Hidden;
            floorFour.Visibility = Visibility.Hidden;
            floorThree.Visibility = Visibility.Visible;
            floorTwo.Visibility = Visibility.Hidden;
        }

        private void buttonFloorFour_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            floorFive.Visibility = Visibility.Hidden;
            floorFour.Visibility = Visibility.Visible;
            floorThree.Visibility = Visibility.Hidden;
            floorTwo.Visibility = Visibility.Hidden;
        }

        private void buttonFloorFive_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            floorFive.Visibility = Visibility.Visible;
            floorFour.Visibility = Visibility.Hidden;
            floorThree.Visibility = Visibility.Hidden;
            floorTwo.Visibility = Visibility.Hidden;
        }

        private void cabinet_Initialized(object sender, EventArgs e)
        {

        }
    }
}
