using Google.Protobuf.WellKnownTypes;
using MDK._01._01_CourseProject.Models;
using MDK._01._01_CourseProject.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MDK._01._01_CourseProject.Views.CarSales
{
    /// <summary>
    /// Логика взаимодействия для Filter.xaml
    /// </summary>
    public partial class Filter : Window
    {
        public int SelectedBrandID
        {
            get => BrandComboBox.SelectedItem is ComboBoxItem selectedItem ? (int)selectedItem.Tag : -1;
            set
            {
                foreach (ComboBoxItem item in BrandComboBox.Items)
                {
                    if ((int)item.Tag == value)
                    {
                        BrandComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        public int SelectedCarID
        {
            get => CarComboBox.SelectedItem is ComboBoxItem selectedItem ? (int)selectedItem.Tag : -1;
            set
            {
                foreach (ComboBoxItem item in CarComboBox.Items)
                {
                    if ((int)item.Tag == value)
                    {
                        CarComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        public int SelectedEmployeeID
        {
            get => EmployeeComboBox.SelectedItem is ComboBoxItem selectedItem ? (int)selectedItem.Tag : -1;
            set
            {
                foreach (ComboBoxItem item in EmployeeComboBox.Items)
                {
                    if ((int)item.Tag == value)
                    {
                        EmployeeComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        public int SelectedCustomerID
        {
            get => CustomerComboBox.SelectedItem is ComboBoxItem selectedItem ? (int)selectedItem.Tag : -1;
            set
            {
                foreach (ComboBoxItem item in CustomerComboBox.Items)
                {
                    if ((int)item.Tag == value)
                    {
                        CustomerComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        public DateTime? EnteredFirstSaleDate
        {
            get
            {
                string[] russianDateFormats = new string[]
                {
                    "dd.MM.yyyy",
                    "dd.MM.yyyy HH:mm",
                    "dd.MM.yyyy HH:mm:ss",
                    "dd/MM/yyyy",
                    "dd/MM/yyyy HH:mm",
                    "dd/MM/yyyy HH:mm:ss",
                    "dd-MM-yyyy",
                    "dd-MM-yyyy HH:mm",
                    "dd-MM-yyyy HH:mm:ss"
                };
                bool isValidDate = DateTime.TryParseExact(FirstSaleDate.Text, russianDateFormats, new CultureInfo("ru-RU"), DateTimeStyles.None, out DateTime parsedDate);
                return isValidDate ? parsedDate : (DateTime?)null;
            }
            set => FirstSaleDate.Text = value?.ToString();
        }

        public DateTime? EnteredSecondSaleDate
        {
            get
            {
                string[] russianDateFormats = new string[]
                {
                    "dd.MM.yyyy",
                    "dd.MM.yyyy HH:mm",
                    "dd.MM.yyyy HH:mm:ss",
                    "dd/MM/yyyy",
                    "dd/MM/yyyy HH:mm",
                    "dd/MM/yyyy HH:mm:ss",
                    "dd-MM-yyyy",
                    "dd-MM-yyyy HH:mm",
                    "dd-MM-yyyy HH:mm:ss"
                };
                bool isValidDate = DateTime.TryParseExact(SecondSaleDate.Text, russianDateFormats, new CultureInfo("ru-RU"), DateTimeStyles.None, out DateTime parsedDate);
                return isValidDate ? parsedDate : (DateTime?)null;
            }
            set => SecondSaleDate.Text = value?.ToString();
        }


        public Filter(bool filterUse)
        {
            InitializeComponent();
            ActiveFilter.IsChecked = filterUse;
            ToggleFilterControls(filterUse);
            InitializeComboBoxes();
            BrandComboBox.SelectionChanged += BrandComboBox_SelectionChanged;
        }

        // Инициализация комбо-боксов
        private void InitializeComboBoxes()
        {
            var brands = RepositoryBrand.GetBrands();
            var employees = RepositoryEmployee.GetEmployees();
            var customers = RepositoryCustomer.GetCustomers();

            BrandComboBox.Items.Clear();
            EmployeeComboBox.Items.Clear();
            CustomerComboBox.Items.Clear();


            BrandComboBox.Items.Add(new ComboBoxItem { Content = "Не выбран.", Tag = -1 });
            EmployeeComboBox.Items.Add(new ComboBoxItem { Content = "Не выбран.", Tag = -1 });
            CustomerComboBox.Items.Add(new ComboBoxItem { Content = "Не выбран.", Tag = -1 });

            foreach (var brand in brands)
            {
                var brandItem = new ComboBoxItem { Content = brand.BrandName, Tag = brand.BrandID };
                BrandComboBox.Items.Add(brandItem);
            }
            foreach (var employee in employees)
            {
                var employeeItem = new ComboBoxItem { Content = employee.FullName, Tag = employee.EmployeeID };
                EmployeeComboBox.Items.Add(employeeItem);
            }
            foreach (var customer in customers)
            {
                var customerItem = new ComboBoxItem { Content = customer.FullName, Tag = customer.CustomerID };
                CustomerComboBox.Items.Add(customerItem);
            }
        }
        
        private void PopulateCarComboBox(int? brandId)
        {
            CarComboBox.Items.Clear();
            CarComboBox.Items.Add(new ComboBoxItem { Content = "Не выбран.", Tag = -1 });

            if (brandId.HasValue)
            {
                var cars = RepositoryCar.GetCars().Where(x => x.BrandID == brandId.Value);

                if (cars.Any())
                {
                    foreach (var car in cars)
                    {
                        var carItem = new ComboBoxItem { Content = car.CarName, Tag = car.CarID };
                        CarComboBox.Items.Add(carItem);
                    }
                }
                else
                    // Если машины отсутствуют, устанавливаем "Не выбран." в качестве выбранного элемента
                    CarComboBox.SelectedIndex = 0;
            }
            else
                // Если бренд не выбран, устанавливаем "Не выбран." в качестве выбранного элемента
                CarComboBox.SelectedIndex = 0;

            // Если машина не выбрана, устанавливаем "Не выбран." в качестве выбранного элемента
            if (SelectedCarID == -1)
                CarComboBox.SelectedIndex = 0;
            
        }

        // Показать диалоговое окно
        public bool ShowDialog()
        {
            base.ShowDialog();
            return ActiveFilter.IsChecked == true;
        }

        // Обработчик изменения состояния чекбокса
        private void ActiveFilter_Checked(object sender, RoutedEventArgs e)
        {
            ToggleFilterControls(ActiveFilter.IsChecked == true);
        }

        // Применение фильтра
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        // Отмена фильтрации
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        // Переключение состояния контролов фильтра
        private void ToggleFilterControls(bool isEnabled)
        {
            BrandComboBox.IsEnabled = CarComboBox.IsEnabled = EmployeeComboBox.IsEnabled = CustomerComboBox.IsEnabled = FirstSaleDate.IsEnabled = SecondSaleDate.IsEnabled = isEnabled;
        }
        private void BrandComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int? selectedBrandId = SelectedBrandID == -1 ? null : (int?)SelectedBrandID;
            PopulateCarComboBox(selectedBrandId);
        }
    }
}
