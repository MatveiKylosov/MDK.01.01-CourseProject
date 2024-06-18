using MDK._01._01_CourseProject.Models;
using MDK._01._01_CourseProject.Repository;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MDK._01._01_CourseProject.Views.CarSales
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        private bool filterUse = false;

        private int SelectedBrandID = -1;
        private int SelectedCarID = -1;
        private int SelectedEmployeeID = -1;
        private int SelectedCustomerID = -1;
        private DateTime? EnteredFirstSaleDate;
        private DateTime? EnteredSecondSaleDate;
        Customer customer = null;

        private List<CarSale> _carSales;
        private ObservableCollection<CarSaleUserControl> CarSale { get; set; }
        public Main(Customer customer = null)
        {
            InitializeComponent();
            _carSales = RepositoryCarSale.GetCarSales();
            CarSale = new ObservableCollection<CarSaleUserControl>();
            CarSaleList.ItemsSource = CarSale;

            if(null != customer)
            {
                AddCarSale.Visibility = Visibility.Hidden;
                this.customer = customer;
            }

            InitializeCarSales();
        }

        public void InitializeCarSales()
        {
            CarSale.Clear();
            var filteredCarSales = _carSales;

            if(customer != null)
                filteredCarSales = _carSales.Where(x => x.CustomerID == customer.CustomerID).ToList();

            if (filterUse)
            {
                filteredCarSales = filteredCarSales.Where(IsCarSaleMatchingFilters).ToList();
            }

            foreach (var carSale in filteredCarSales)
                CarSale.Add(new CarSaleUserControl((customer != null),carSale, this));
        }

        private bool IsCarSaleMatchingFilters(CarSale carSale)
        {
            var Cars = RepositoryCar.GetCars().Where(x => x.BrandID == SelectedBrandID);
            if(SelectedBrandID != -1 && !Cars.Any(x => x.CarID == carSale.CarID))
                return false;

            if (SelectedCarID != -1 && carSale.CarID != SelectedCarID)
            {
                return false;
            }

            if (SelectedEmployeeID != -1 && carSale.EmployeeID != SelectedEmployeeID)
            {
                return false;
            }

            if (SelectedCustomerID != -1 && carSale.CustomerID != SelectedCustomerID)
            {
                return false;
            }

            if (EnteredFirstSaleDate != null && carSale.SaleDate < EnteredFirstSaleDate)
            {
                return false;
            }

            if (EnteredSecondSaleDate != null && carSale.SaleDate > EnteredSecondSaleDate)
            {
                return false;
            }

            return true;
        }

        public void RemoveCarSale(CarSaleUserControl carSaleUserControl)
        {
            if (carSaleUserControl != null)
            {
                CarSale.Remove(carSaleUserControl);
                _carSales.Remove(carSaleUserControl.CarSale);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _carSales = customer == null ? RepositoryCarSale.GetCarSales() : 
                                           RepositoryCarSale.GetCarSales().Where(x => x.CustomerID == customer.CustomerID).ToList();
            InitializeCarSales();
        }

        private void ExportCarSales_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Файлы Excel (*.xlsx)|*.xlsx",
                Title = "Сохранить файл Excel"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    ExportToExcel(saveFileDialog.FileName);
                    MessageBox.Show("Данные успешно экспортированы!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при экспорте данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Метод для записи данных в Excel файл
        private void ExportToExcel(string filePath)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("CarSales");

                // Заголовки столбцов
                worksheet.Cells[1, 1].Value = "№";
                worksheet.Cells[1, 2].Value = "Дата продажи";
                worksheet.Cells[1, 3].Value = "Сотрудник";
                worksheet.Cells[1, 4].Value = "Машина";
                worksheet.Cells[1, 5].Value = "Клиент";

                var carSales = CarSale.ToList();

                // Заполнение данными
                for (int i = 0; i < carSales.Count; i++)
                {
                    var carSale = carSales[i].CarSale;
                    var FindEmployee = RepositoryEmployee.GetEmployees().FirstOrDefault(x => x.EmployeeID == carSale.EmployeeID);
                    var FindCar = RepositoryCar.GetCars().FirstOrDefault(x => x.CarID == carSale.CarID);
                    var FindCustomer = RepositoryCustomer.GetCustomers().FirstOrDefault(x => x.CustomerID == carSale.CustomerID);
                    worksheet.Cells[i + 2, 1].Value = carSale.SaleID;

                    if (carSale.SaleDate.HasValue)
                        worksheet.Cells[i + 2, 2].Value = carSale.SaleDate.Value.ToString("dd.MM.yyyy HH:mm:ss", new CultureInfo("ru-RU"));
                    if (FindEmployee != null)
                        worksheet.Cells[i + 2, 3].Value = FindEmployee.FullName;
                    if (FindCar != null)
                        worksheet.Cells[i + 2, 4].Value = FindCar.CarName;
                    if (FindCustomer != null)
                        worksheet.Cells[i + 2, 5].Value = FindCustomer.FullName;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                // Сохранение в файл
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }
        }

        private void FilterCarSales_Click(object sender, RoutedEventArgs e)
        {
            var filter = new Filter(filterUse)
            {
                SelectedBrandID = this.SelectedBrandID,
                SelectedCarID = this.SelectedCarID,
                SelectedEmployeeID = this.SelectedEmployeeID,
                SelectedCustomerID = this.SelectedCustomerID,
                EnteredFirstSaleDate = this.EnteredFirstSaleDate,
                EnteredSecondSaleDate = this.EnteredSecondSaleDate,
                customer = this.customer
            };

            filterUse = filter.ShowDialog();
            SelectedBrandID = filter.SelectedBrandID;
            SelectedCarID = filter.SelectedCarID;
            SelectedEmployeeID = filter.SelectedEmployeeID;
            SelectedCustomerID = filter.SelectedCustomerID;
            EnteredFirstSaleDate = filter.EnteredFirstSaleDate;
            EnteredSecondSaleDate = filter.EnteredSecondSaleDate;

            InitializeCarSales();
        }

        private void AddCarSale_Click(object sender, RoutedEventArgs e)
        {
            var addedCarSale = new CarSale() { SaleID = RepositoryCarSale.AddCarSale() };
            _carSales.Add(addedCarSale);
            CarSale.Add(new CarSaleUserControl((customer != null), addedCarSale, this));
        }
    }
}
