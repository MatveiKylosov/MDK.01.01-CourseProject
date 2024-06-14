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
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace MDK._01._01_CourseProject.Views.Brands
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
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

        private void InitializeBrands()
        {
            foreach (var brand in _brands)
                Brands.Add(new BrandUserControl(brand, this));
        }

        public void RemoveBrand(BrandUserControl brand)
        {
            if (brand != null)
            {
                Brands.Remove(brand);
                _brands.Remove(brand.brand);
            }
        }

        private void AddBrand_Click(object sender, RoutedEventArgs e)
        {
            var newBrand = new Brand();
            RepositoryBrand.AddBrand(newBrand);

            var addedBrand = Repository.RepositoryBrand.GetBrands().LastOrDefault();
            if (addedBrand != null)
            {
                newBrand.BrandID = addedBrand.BrandID;
                _brands.Add(newBrand);
                Brands.Add(new BrandUserControl(newBrand, this));
            }
        }

        private void ExportBrands_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Файлы Excel (*.xlsx)|*.xlsx",
                Title = "Сохранить файл Excel"
            };

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

                // Заголовки столбцов
                var headers = new[] { "BrandID", "BrandName", "Country", "Manufacturer", "Address" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                }

                // Заполнение данными
                for (int i = 0; i < _brands.Count; i++)
                {
                    var brand = _brands[i];
                    worksheet.Cells[i + 2, 1].Value = brand.BrandID;
                    worksheet.Cells[i + 2, 2].Value = brand.BrandName;
                    worksheet.Cells[i + 2, 3].Value = brand.Country;
                    worksheet.Cells[i + 2, 4].Value = brand.Manufacturer;
                    worksheet.Cells[i + 2, 5].Value = brand.Address;
                }

                // Сохранение в файл
                package.SaveAs(new FileInfo(filePath));
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _brands = Repository.RepositoryBrand.GetBrands();
            Brands.Clear();
            InitializeBrands();
        }
    }
}
