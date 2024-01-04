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
using System.Windows.Shapes;

namespace SUVCServiceApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для InteractiveMapWindow.xaml
    /// </summary>
    public partial class InteractiveMapWindow : Window
    {
        private ResponseEquipment currentEquipment;
        string equipmentLocation;
        char equipmentFloor;

        public InteractiveMapWindow(ResponseEquipment equipment)
        {
            InitializeComponent();
            this.currentEquipment = equipment;
            equipmentLocation = currentEquipment.LocationAuditorium.ToString();
            equipmentFloor = equipmentLocation[1];
            if (IsValidLocation(equipmentLocation))
            {
                string floor = equipmentFloor.ToString() + "2";
                SetImage(secondImage, $"{floor}/{equipmentLocation}.png");
                SetImage(firstImage, $"up2.png");
            }
            else
            {
                SetImage(secondImage, $"{equipmentFloor}/{equipmentLocation}.png");
                SetImage(firstImage, $"up.png");
            }
            textBlockAction.Text = $"Подняться на {equipmentFloor} этаж";
        }
        private void SetImage(Image image, string imageName)
        {
            string imagePath = $"/SUVCServiceApp;component/Resources/InteractiveMap/" + imageName;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(imagePath, UriKind.Relative);
            bitmapImage.EndInit();
            image.Source = bitmapImage;
        }
        private bool IsValidLocation(string location)
        {
            string[] validLocations = { "1502", "1503", "1504", "1505", "1506", "1507", "1509", "1511", "1513",
                                        "1402", "1403", "1404", "1405", "1406", "1407", "1409", "1411", "1413" };

            return Array.Exists(validLocations, validLocation => validLocation == location);
        }

        private void buttonCloseMap_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonNextSlide_Click(object sender, RoutedEventArgs e)
        {
            firstImage.Visibility = Visibility.Hidden;
            secondImage.Visibility = Visibility.Visible;
            buttonNextSlide.IsEnabled = false;
            buttonPrevSlide.IsEnabled = true;
            textBlockAction.Text = $"Зайти в аудиторию {equipmentLocation}";
        }

        private void buttonPrevSlide_Click(object sender, RoutedEventArgs e)
        {
            firstImage.Visibility = Visibility.Visible;
            secondImage.Visibility = Visibility.Hidden;
            buttonNextSlide.IsEnabled = true;
            buttonPrevSlide.IsEnabled = false;
            textBlockAction.Text = $"Подняться на {equipmentFloor} этаж";
        }
    }
}
