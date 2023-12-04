﻿using SUVCServiceApp.Controller;
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
    /// Логика взаимодействия для AddProgramPage.xaml
    /// </summary>
    public partial class AddProgramPage : Page
    {
        public AddProgramPage()
        {
            InitializeComponent();
        }

        private async void buttonAddProgram_Click(object sender, RoutedEventArgs e)
        {
            ApiDataProvider apiDataProvider = new ApiDataProvider();
            ResponseRegistry program = new ResponseRegistry
            {
                NameProgram = textBoxNameProgram.Text,
                DescriptionProgram = textBoxDescriptionProgram.Text,
                VersionProgram = textBoxVersionProgram.Text,
                IDSpecialization = Convert.ToInt16(comboBoxSpecialization.Text)
            };

            bool isSuccess = await apiDataProvider.AddDataToApi("RegistryProgram", program);
            if (isSuccess)
            {
                MessageBox.Show($"Программа {textBoxNameProgram.Text} успешно добавлен!");
            }
            else
            {
                MessageBox.Show("Произошла ошибка при добавлении данных! Проверьте поля ввода данных!");
            }
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
