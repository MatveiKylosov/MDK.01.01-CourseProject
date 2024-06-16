using Google.Protobuf;
using MDK._01._01_CourseProject.Models;
using MDK._01._01_CourseProject.Repository;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
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
            get { return SaleDate.IsEnabled; }
            set
            {
                SaleDate.IsEnabled = value;
                CarComboBox.IsEnabled = value;
                CustomerComboBox.IsEnabled = value;
                EmployeeComboBox.IsEnabled = value;

                EditButton.Content = value ? "Сохранить" : "Изменить";
            }
        }
        public CarSaleUserControl(CarSale carSale, Main main)
        {
            InitializeComponent();
            this.edit = false;
            this.main = main;
            this.CarSale = carSale;


            if (CarSale == null) return;


            var cars = RepositoryCar.GetCars();

            foreach (var car in cars)
            {
                var comboBoxItem = new ComboBoxItem
                {
                    Content = car.CarName,
                    Tag = car.CarID
                };
                CarComboBox.Items.Add(comboBoxItem);
            }

            var customers = RepositoryCustomer.GetCustomers();

            foreach (var customer in customers)
            {
                var comboBoxItem = new ComboBoxItem
                {
                    Content = $"{customer.FullName}",
                    Tag = customer.CustomerID
                };
                CustomerComboBox.Items.Add(comboBoxItem);
            }

            var employees = RepositoryEmployee.GetEmployees();

            foreach (var employee in employees)
            {
                var comboBoxItem = new ComboBoxItem
                {
                    Content = employee.FullName,
                    Tag = employee.EmployeeID
                };
                EmployeeComboBox.Items.Add(comboBoxItem);
            }


        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            string[] formats = { "dd.MM.yyyy HH:mm:ss", "dd.MM.yyyy H:mm:ss", "dd.MM.yyyy HH:mm:ss", "dd.MM.yyyy H:mm:ss" };

            if (!DateTime.TryParseExact(SaleDate.Text, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                return;
                if (CarComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Должен быть выбрана машина.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
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

            CarSale.CarID = (int)((ComboBoxItem)CarComboBox.SelectedItem).Tag;
            CarSale.CustomerID = (int)((ComboBoxItem)CustomerComboBox.SelectedItem).Tag;
            CarSale.EmployeeID = (int)((ComboBoxItem)EmployeeComboBox.SelectedItem).Tag;
            CarSale.SaleDate = parsedDate;

            RepositoryCarSale.UpdateCarSale(CarSale);
            edit = false;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Продолжить удаление этой записи?","Внимание!",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                //main.Remove
                RepositoryCarSale.DeleteCarSale(CarSale);
            }
        }
    }
}
