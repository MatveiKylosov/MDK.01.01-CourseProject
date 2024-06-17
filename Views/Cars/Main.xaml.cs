using MDK._01._01_CourseProject.Models;
using MDK._01._01_CourseProject.Repository;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MDK._01._01_CourseProject.Views.Cars
{
    public partial class Main : Page
    {
        private bool filterUse = false;
        private int selectedBrandID = -1;
        private int enteredFirstDate;
        private int enteredSecondDate;
        private decimal enteredFirstPrice;
        private decimal enteredSecondPrice;
        private string selectedColor = "Не выбран.";
        private string selectedCategory = "Не выбран.";
        private List<Car> _cars;
        private ObservableCollection<CarUserControl> Cars { get; set; }

        public Main()
        {
            InitializeComponent();
            _cars = RepositoryCar.GetCars();
            Cars = new ObservableCollection<CarUserControl>();
            InitializeCars();
            CarList.ItemsSource = Cars;
        }

        // Инициализация списка машин
        public void InitializeCars()
        {
            var filteredCars = _cars;

            if (filterUse)
            {
                filteredCars = filteredCars.Where(car =>
                    (selectedBrandID == -1 || car.BrandID == selectedBrandID) &&
                    (enteredFirstDate == 0 || car.YearOfProduction >= enteredFirstDate) &&
                    (enteredSecondDate == 0 || car.YearOfProduction <= enteredSecondDate) &&
                    (enteredFirstPrice == 0 || car.Price >= enteredFirstPrice) &&
                    (enteredSecondPrice == 0 || car.Price <= enteredSecondPrice) &&
                    (selectedColor == "Не выбран." || selectedColor == car.Color) &&
                    (selectedCategory == "Не выбран." || selectedCategory == car.Category)
                ).ToList();
            }

            Cars.Clear();
            foreach (var car in filteredCars)
            {
                Cars.Add(new CarUserControl(car, this));
            }
        }

        // Добавление новой машины
        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            var addedCar = new Car() { CarID = RepositoryCar.AddCar() };
            _cars.Add(addedCar);
            Cars.Add(new CarUserControl(addedCar, this));
        }

        // Удаление машины
        public void RemoveCar(CarUserControl carControl)
        {
            if (carControl != null)
            {
                Cars.Remove(carControl);
                _cars.Remove(carControl.Car);
            }
        }

        // Обновление списка машин
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _cars = RepositoryCar.GetCars();
            InitializeCars();
        }

        // Экспорт данных в Excel
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

        // Метод экспорта данных в Excel
        private void ExportToExcel(string filePath)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Cars");
                worksheet.Cells[1, 1].Value = "CarID";
                worksheet.Cells[1, 2].Value = "CarName";
                worksheet.Cells[1, 3].Value = "Brand";
                worksheet.Cells[1, 4].Value = "YearOfProduction";
                worksheet.Cells[1, 5].Value = "Color";
                worksheet.Cells[1, 6].Value = "Category";
                worksheet.Cells[1, 7].Value = "Price";

                var cars = Cars.ToList();

                for (int i = 0; i < cars.Count; i++)
                {
                    var car = cars[i].Car;
                    worksheet.Cells[i + 2, 1].Value = car.CarID;
                    worksheet.Cells[i + 2, 2].Value = car.CarName;

                    var BrandFind = RepositoryBrand.GetBrands().FirstOrDefault(x => x.BrandID == car.BrandID);
                    if (BrandFind != null)
                        worksheet.Cells[i + 2, 3].Value = BrandFind.BrandName;
                    worksheet.Cells[i + 2, 4].Value = car.YearOfProduction;
                    worksheet.Cells[i + 2, 5].Value = car.Color;
                    worksheet.Cells[i + 2, 6].Value = car.Category;
                    worksheet.Cells[i + 2, 7].Value = car.Price;
                }

                package.SaveAs(new FileInfo(filePath));
            }
        }

        // Фильтрация машин
        private void FilterCars_Click(object sender, RoutedEventArgs e)
        {
            var colors = RepositoryCar.GetCars().Select(b => b.Color).Distinct().ToList();
            var categories = RepositoryCar.GetCars().Select(b => b.Category).Distinct().ToList();
            var filter = new Filter(colors, categories, filterUse)
            {
                SelectedBrandID = selectedBrandID,
                EnteredFirstDate = enteredFirstDate,
                EnteredSecondDate = enteredSecondDate,
                EnteredFirstPrice = enteredFirstPrice,
                EnteredSecondPrice = enteredSecondPrice,
                SelectedColor = selectedColor,
                SelectedCategory = selectedCategory
            };

            filterUse = filter.ShowDialog();
            selectedBrandID = filter.SelectedBrandID;
            enteredFirstDate = filter.EnteredFirstDate;
            enteredSecondDate = filter.EnteredSecondDate;
            enteredFirstPrice = filter.EnteredFirstPrice;
            enteredSecondPrice = filter.EnteredSecondPrice;
            selectedColor = filter.SelectedColor;
            selectedCategory = filter.SelectedCategory;
            InitializeCars();
        }
    }
}
