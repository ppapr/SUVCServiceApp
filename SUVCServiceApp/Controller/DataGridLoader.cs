using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SUVCServiceApp.Controller
{
    public class DataGridLoader
    {
        private readonly ApiDataProvider apiDataProvider;

        public DataGridLoader(ApiDataProvider apiDataProvider)
        {
            this.apiDataProvider = apiDataProvider;
        }

        public async Task LoadData<T>(DataGrid dataGrid, string apiEndpoint)
        {
            List<T> data = await apiDataProvider.GetDataFromApi<T>(apiEndpoint);
            if (data != null)
            {
                dataGrid.ItemsSource = data;
            }
        }

        public async Task LoadData<T>(ListView listView, string apiEndpoint)
        {
            List<T> data = await apiDataProvider.GetDataFromApi<T>(apiEndpoint);
            if (data != null)
            {
                listView.ItemsSource = data;
            }
        }
        public async Task LoadData<T>(ListView listView, string apiEndpoint, int currentPage, int sizePage)
        {
            List<T> data = await apiDataProvider.GetDataFromApi<T>(apiEndpoint);
            if (data != null)
            {
                listView.ItemsSource = data.Skip((currentPage - 1) * sizePage).Take(sizePage);
            }
        }

        public async Task LoadData<T>(ComboBox comboBox, string apiEndpoint)
        {
            List<T> data = await apiDataProvider.GetDataFromApi<T>(apiEndpoint);
            if (data != null)
            {
                comboBox.ItemsSource = data;
            }
        }
        public async Task LoadFilteredData<T>(ListView listView, string apiEndpoint, Func<T, bool> filterCondition, int currentPage, int sizePage)
        {
            List<T> data = await apiDataProvider.GetDataFromApi<T>(apiEndpoint);
            if (data != null)
            {
                var filteredData = data.Where(filterCondition).ToList();
                listView.ItemsSource = filteredData.Skip((currentPage - 1) * sizePage).Take(sizePage);
            }
        }
        public async Task LoadFilteredData<T>(ListView listView, string apiEndpoint, Func<T, bool> filterCondition)
        {
            List<T> data = await apiDataProvider.GetDataFromApi<T>(apiEndpoint);
            if (data != null)
            {
                var filteredData = data.Where(filterCondition).ToList();
                listView.ItemsSource = filteredData;
            }
        }
        public async Task LoadData<T>(ListView listView, string apiEndpoint, Func<T, string> searchProperty, string searchTerm)
        {
            List<T> data = await apiDataProvider.SearchData<T>(apiEndpoint, searchProperty, searchTerm);
            if (data != null)
            {
                listView.ItemsSource = data;
            }
        }

        public async Task LoadData<T>(DataGrid dataGrid, string apiEndpoint, Func<T, string> searchProperty, string searchTerm)
        {
            List<T> data = await apiDataProvider.SearchData<T>(apiEndpoint, searchProperty, searchTerm);
            if (data != null)
            {
                dataGrid.ItemsSource = data;
            }
        }
    }
}
