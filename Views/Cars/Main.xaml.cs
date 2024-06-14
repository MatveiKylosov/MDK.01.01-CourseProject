using MDK._01._01_CourseProject.Models;
using MDK._01._01_CourseProject.Repository;
using MDK._01._01_CourseProject.Views.Brands;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MDK._01._01_CourseProject.Views.Cars
{
    public partial class Main : Page
    {
        private List<Car> _cars;
        private ObservableCollection<CarUserControl> Cars { get; set; }
        public Main(List<Car> cars)
        {
            InitializeComponent();
            _cars = cars ?? new List<Car>();
            Cars = new ObservableCollection<CarUserControl>();
            InitializeCars();
            CarList.ItemsSource = Cars;
        }

        public void InitializeCars()
        {
            var filteredCars = _cars; 

            Cars.Clear();
            foreach (var car in _cars)
                Cars.Add(new CarUserControl(car, this));
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            RepositoryCar.AddCar(new Car());
            var addedCar = RepositoryCar.GetCars().LastOrDefault();
            if (addedCar != null)
            {
                _cars.Add(addedCar);
                Cars.Add(new CarUserControl(addedCar, this));
            }
        }
        public void RemoveCar(CarUserControl carControl)
        {
            if (carControl != null)
            {
                Cars.Remove(carControl);
                _cars.Remove(carControl.Car);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _cars = RepositoryCar.GetCars();
            InitializeCars();
        }


        private void ExportCars_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog { Filter = "Файлы Excel (*.xlsx)|*.xlsx", Title = "Сохранить файл Excel" };
            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                try
                {
                    ExportToExcel(filePath);
                    MessageBox.Show("Данные успешно экспортированы!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при экспорте данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportToExcel(string filePath)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Cars");

                // Заголовки столбцов
                worksheet.Cells[1, 1].Value = "CarID";
                worksheet.Cells[1, 2].Value = "CarName";
                worksheet.Cells[1, 3].Value = "BrandID";
                worksheet.Cells[1, 4].Value = "YearOfProduction";
                worksheet.Cells[1, 5].Value = "Color";
                worksheet.Cells[1, 6].Value = "Category";
                worksheet.Cells[1, 7].Value = "Price";

                // Заполнение данными
                for (int i = 0; i < _cars.Count; i++)
                {
                    var car = _cars[i];
                    worksheet.Cells[i + 2, 1].Value = car.CarID;
                    worksheet.Cells[i + 2, 2].Value = car.CarName;
                    worksheet.Cells[i + 2, 3].Value = car.BrandID;
                    worksheet.Cells[i + 2, 4].Value = car.YearOfProduction;
                    worksheet.Cells[i + 2, 5].Value = car.Color;
                    worksheet.Cells[i + 2, 6].Value = car.Category;
                    worksheet.Cells[i + 2, 7].Value = car.Price;
                }

                // Сохранение в файл
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }
        }

        private void FilterCars_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
