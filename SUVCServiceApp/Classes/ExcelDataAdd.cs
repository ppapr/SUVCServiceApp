using SUVCServiceApp.Controller;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SUVCServiceApp.Classes
{
    public class ExcelDataAdd<T>
    {
        private readonly string _apiEndpoint;
        private readonly Func<IDataRecord, T> _dataMapper;

        public ExcelDataAdd(string apiEndpoint, Func<IDataRecord, T> dataMapper)
        {
            _apiEndpoint = apiEndpoint;
            _dataMapper = dataMapper;
        }

        public async Task<bool> AddDataFromExcel(string filePath)
        {
            try
            {
                List<T> dataList = ReadDataFromExcel(filePath);
                ApiDataProvider apiDataProvider = new ApiDataProvider();

                foreach (var dataItem in dataList)
                {
                    bool isSuccess = await apiDataProvider.AddDataToApi(_apiEndpoint, dataItem);

                    if (!isSuccess)
                    {
                        MessageBox.Show($"Произошла ошибка при добавлении данных!");
                        return false;
                    }
                }

                MessageBox.Show("Данные успешно добавлены!");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
                return false;
            }
        }

        private List<T> ReadDataFromExcel(string filePath)
        {
            List<T> dataList = new List<T>();

            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand($"SELECT * FROM [Лист1$]", connection);

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T dataItem = _dataMapper(reader);
                        dataList.Add(dataItem);
                    }
                }
            }

            return dataList;

        }
    }
}
