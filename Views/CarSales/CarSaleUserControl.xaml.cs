using Google.Protobuf;
using MDK._01._01_CourseProject.Models;
using MDK._01._01_CourseProject.Repository;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
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
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace MDK._01._01_CourseProject.Views.CarSales
{
    /// <summary>
    /// Логика взаимодействия для CarSaleUserControl.xaml
    /// </summary>
    public partial class CarSaleUserControl : UserControl
    {
        public CarSale CarSale;
        Main main;

        bool edit
        {
            get { return CustomerComboBox.IsEnabled; }
            set
            {
                CustomerComboBox.IsEnabled = value;
                SaleDate.IsEnabled = value;
                CarComboBox.IsEnabled = value;
                EmployeeComboBox.IsEnabled = value;
                CustomerComboBox.IsEnabled = value;
                DeleteButton.IsEnabled = value;

                EditButton.Content = value ? "Сохранить" : "Изменить";
            }
        }

        public CarSaleUserControl(CarSale CarSale, Main main)
        {
            InitializeComponent();
            this.CarSale = CarSale;
            this.main = main;
            this.edit = false;

            if (CarSale == null) return;

            foreach (var car in RepositoryCar.GetCars())
            {
                var comboBoxItem = new ComboBoxItem
                {
                    Content = car.CarName,
                    Tag = car.CarID
                };
                CarComboBox.Items.Add(comboBoxItem);
            }

            foreach (ComboBoxItem item in CarComboBox.Items)
            {
                if ((int)item.Tag == CarSale.CarID)
                {
                    CarComboBox.SelectedItem = item;
                    break;
                }
            }


            foreach (var customer in RepositoryCustomer.GetCustomers())
            {
                var comboBoxItem = new ComboBoxItem
                {
                    Content = customer.FullName,
                    Tag = customer.CustomerID
                };
                CustomerComboBox.Items.Add(comboBoxItem);
            }

            foreach (ComboBoxItem item in CustomerComboBox.Items)
            {
                if ((int)item.Tag == CarSale.CustomerID)
                {
                    CustomerComboBox.SelectedItem = item;
                    break;
                }
            }


            foreach (var Employee in RepositoryEmployee.GetEmployees())
            {
                var comboBoxItem = new ComboBoxItem
                {
                    Content = Employee.FullName,
                    Tag = Employee.EmployeeID
                };
                EmployeeComboBox.Items.Add(comboBoxItem);
            }

            foreach (ComboBoxItem item in EmployeeComboBox.Items)
            {
                if ((int)item.Tag == CarSale.EmployeeID)
                {
                    EmployeeComboBox.SelectedItem = item;
                    break;
                }
            }

            SaleDate.Text = CarSale.SaleDate.Value.ToString("dd.MM.yyyy HH:mm:ss", new CultureInfo("ru-RU"));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!edit)
            {
                edit = true;
                EditButton.Content = "Сохранить";
                return;
            }
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

            if (string.IsNullOrEmpty(SaleDate.Text) ||
                !DateTime.TryParseExact(SaleDate.Text, russianDateFormats, new CultureInfo("ru-RU"), DateTimeStyles.None, out DateTime parsedDate))
            {
                MessageBox.Show("Укажите дату продажи в правильном формате.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CustomerComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Должен быть выбран клиент.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (EmployeeComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Должен быть выбран сотрудник.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CarComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Должен быть выбран автомобиль.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CarSale.CarID      = (int)((ComboBoxItem)CarComboBox.SelectedItem)     .Tag;
            CarSale.EmployeeID = (int)((ComboBoxItem)EmployeeComboBox.SelectedItem).Tag;
            CarSale.CustomerID = (int)((ComboBoxItem)CustomerComboBox.SelectedItem).Tag;
            CarSale.SaleDate = parsedDate;

            RepositoryCarSale.UpdateCarSale(CarSale);
            edit = false;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Продолжить удаление этой записи?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                RepositoryCarSale.DeleteCarSale(CarSale);
                main.RemoveCarSale(this);
            }
        }
    }
}
