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
        private string _selectedCountry = "";
        private string _selectedManufacturer = "";
        private string _selectedAddress = "";
        private bool _filterUse = false;
        private List<Brand> _brands;
        private ObservableCollection<BrandUserControl> Brands { get; set; }

        public Main(List<Brand> brands)
        {
            InitializeComponent();
            _brands = brands ?? new List<Brand>();
            Brands = new ObservableCollection<BrandUserControl>();
            InitializeBrands();
            BrandList.ItemsSource = Brands;
        }

        private void InitializeBrands(string selectedCountry = null, string selectedManufacturer = null, string selectedAddress = null)
        {
            var filteredBrands = _brands.Where(x =>
            {
                if ((selectedCountry == null || selectedCountry == "Не выбран.") && (selectedManufacturer == null || selectedManufacturer == "Не выбран.") && (selectedManufacturer == null|| selectedCountry == "Не выбран."))
                    return true;
                if (selectedCountry != "Не выбран." && selectedCountry != null && selectedCountry != x.Country)
                    return false;
                if (selectedManufacturer != "Не выбран." && selectedManufacturer != null && selectedManufacturer != x.Manufacturer)
                    return false;
                if (selectedAddress != "Не выбран." && selectedAddress != null && selectedAddress != x.Address)
                    return false;
                return true;
            }).ToList();

            Brands.Clear();
            foreach (var brand in filteredBrands)
                Brands.Add(new BrandUserControl(brand, this));
        }

        public void RemoveBrand(BrandUserControl brandControl)
        {
            if (brandControl != null)
            {
                Brands.Remove(brandControl);
                _brands.Remove(brandControl.brand);
            }
        }

        private void AddBrand_Click(object sender, RoutedEventArgs e)
        {
            RepositoryBrand.AddBrand(new Brand());
            var addedBrand = RepositoryBrand.GetBrands().LastOrDefault();
            if (addedBrand != null)
            {
                _brands.Add(addedBrand);
                Brands.Add(new BrandUserControl(addedBrand, this));
            }
        }

        private void ExportBrands_Click(object sender, RoutedEventArgs e)
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
                var worksheet = package.Workbook.Worksheets.Add("Бренды");
                var headers = new[] { "BrandID", "BrandName", "Country", "Manufacturer", "Address" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                }
                for (int i = 0; i < _brands.Count; i++)
                {
                    var brand = _brands[i];
                    worksheet.Cells[i + 2, 1].Value = brand.BrandID;
                    worksheet.Cells[i + 2, 2].Value = brand.BrandName;
                    worksheet.Cells[i + 2, 3].Value = brand.Country;
                    worksheet.Cells[i + 2, 4].Value = brand.Manufacturer;
                    worksheet.Cells[i + 2, 5].Value = brand.Address;
                }
                package.SaveAs(new FileInfo(filePath));
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _brands = RepositoryBrand.GetBrands();
            InitializeBrands();
        }

        private void FilterBrand_Click(object sender, RoutedEventArgs e)
        {
            var filter = new Filter();
            var countries = _brands.Select(b => b.Country).Distinct().ToList();
            var manufacturers = _brands.Select(b => b.Manufacturer).Distinct().ToList();
            var addresses = _brands.Select(b => b.Address).Distinct().ToList();

            _filterUse = filter.ShowDialog(_filterUse, countries, manufacturers, addresses, _selectedCountry, _selectedManufacturer, _selectedAddress);
            if (_filterUse)
            {
                _selectedCountry = filter.SelectedCountry;
                _selectedManufacturer = filter.SelectedManufacturer;
                _selectedAddress = filter.SelectedAddress;
                InitializeBrands(_selectedCountry, _selectedManufacturer, _selectedAddress);
            }
            else
            {
                InitializeBrands();
            }
        }
    }
}
