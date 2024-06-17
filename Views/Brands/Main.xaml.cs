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

namespace MDK._01._01_CourseProject.Views.Brands
{
    public partial class Main : Page
    {
        // Поля для хранения выбранных значений фильтра
        private string _selectedCountry = "Не выбран.";
        private string _selectedManufacturer = "Не выбран.";
        private string _selectedAddress = "Не выбран.";
        private bool _filterUse = false;

        // Список всех брендов и коллекция для отображения на UI
        private List<Brand> _brands;
        private ObservableCollection<BrandUserControl> Brands { get; set; }

        // Конструктор страницы, инициализация компонентов и списков
        public Main()
        {
            InitializeComponent();
            _brands = RepositoryBrand.GetBrands();
            Brands = new ObservableCollection<BrandUserControl>();
            InitializeBrands();
            BrandList.ItemsSource = Brands;
        }

        // Метод для инициализации брендов, с учетом фильтрации
        private void InitializeBrands()
        {
            var filteredBrands = _filterUse
                ? _brands.Where(BrandMatchesFilter).ToList()
                : _brands;

            Brands.Clear();
            foreach (var brand in filteredBrands)
            {
                Brands.Add(new BrandUserControl(brand, this));
            }
        }

        // Метод для проверки соответствия бренда фильтру
        private bool BrandMatchesFilter(Brand brand)
        {
            if (_selectedCountry != "Не выбран." && _selectedCountry != brand.Country) return false;
            if (_selectedManufacturer != "Не выбран." && _selectedManufacturer != brand.Manufacturer) return false;
            if (_selectedAddress != "Не выбран." && _selectedAddress != brand.Address) return false;
            return true;
        }

        // Метод для удаления бренда
        public void RemoveBrand(BrandUserControl brandControl)
        {
            if (brandControl != null)
            {
                Brands.Remove(brandControl);
                _brands.Remove(brandControl.brand);
            }
        }

        // Метод для добавления нового бренда
        private void AddBrand_Click(object sender, RoutedEventArgs e)
        {
            var addedBrand = new Brand() { BrandID = RepositoryBrand.AddBrand()};
            _brands.Add(addedBrand);
            Brands.Add(new BrandUserControl(addedBrand, this));
        }

        // Метод для экспорта брендов в Excel
        private void ExportBrands_Click(object sender, RoutedEventArgs e)
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
                var worksheet = package.Workbook.Worksheets.Add("Бренды");
                var headers = new[] { "BrandID", "BrandName", "Country", "Manufacturer", "Address" };

                // Заполнение заголовков
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                }

                var brands = Brands.ToList();
                // Заполнение данных брендов
                for (int i = 0; i < Brands.Count; i++)
                {
                    var brand = brands[i].brand;
                    worksheet.Cells[i + 2, 1].Value = brand.BrandID;
                    worksheet.Cells[i + 2, 2].Value = brand.BrandName;
                    worksheet.Cells[i + 2, 3].Value = brand.Country;
                    worksheet.Cells[i + 2, 4].Value = brand.Manufacturer;
                    worksheet.Cells[i + 2, 5].Value = brand.Address;
                }

                package.SaveAs(new FileInfo(filePath));
            }
        }

        // Метод для обновления списка брендов
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _brands = RepositoryBrand.GetBrands();
            InitializeBrands();
        }

        // Метод для открытия окна фильтрации
        private void FilterBrand_Click(object sender, RoutedEventArgs e)
        {
            var countries = _brands.Select(b => b.Country).Distinct().ToList();
            var manufacturers = _brands.Select(b => b.Manufacturer).Distinct().ToList();
            var addresses = _brands.Select(b => b.Address).Distinct().ToList();

            var filter = new Filter(countries, manufacturers, addresses, _filterUse)
            {
                SelectedCountry = _selectedCountry,
                SelectedManufacturer = _selectedManufacturer,
                SelectedAddress = _selectedAddress
            };

            if (filter.ShowDialog() == true)
            {
                _filterUse = true;
                _selectedCountry = filter.SelectedCountry;
                _selectedManufacturer = filter.SelectedManufacturer;
                _selectedAddress = filter.SelectedAddress;
            }
            else
            {
                _filterUse = false;
            }

            InitializeBrands();
        }
    }
}
