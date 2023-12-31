﻿using SUVCServiceApp.Controller;
using SUVCServiceApp.ViewModel;
using SUVCServiceApp.Windows;
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
    /// Логика взаимодействия для SparePartsPages.xaml
    /// </summary>
    public partial class SparePartsPages : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        private readonly AdministratorWindow administratorWindow;
        public SparePartsPages(AdministratorWindow administratorWindow)
        {
            InitializeComponent();
            this.administratorWindow = administratorWindow;
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
        }

        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadData<ResponseSpare>(listSpares, "SparesEquipments");
        }

        private void buttonAddSpare_Click(object sender, RoutedEventArgs e)
        {
            administratorWindow.FrameWorkspace.Navigate(new AddSparePartsPage(administratorWindow));
        }

        private async void textBoxSearchSpares_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = textBoxSearchSpares.Text;
            Func<ResponseSpare, string> searchProperty = item => item.SpareName;
            await dataGridLoader.LoadData(listSpares, "SparesEquipments", searchProperty, searchTerm);
        }
        private ResponseSpare currentSpare;
        private async void buttonDeleteSpare_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить запчасть {currentSpare.SpareName}?", 
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                await apiDataProvider.DeleteDataFromApi<ResponseSpare>("SparesEquipments", currentSpare.ID);
                MessageBox.Show("Удаление завершено!");
                LoadDataGrid();
            }
        }

        private void listSpares_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentSpare = (ResponseSpare)listSpares.SelectedItem;
        }
    }
}
