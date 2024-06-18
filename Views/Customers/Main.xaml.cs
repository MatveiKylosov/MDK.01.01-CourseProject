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

namespace MDK._01._01_CourseProject.Views.Customers
{
    public partial class Main : Page
    {
        private bool filterUse = false;
        private string enteredFullName;
        private string enteredPassportData;
        private string enteredAddress;
        private DateTime? enteredBirthDateFrom;
        private DateTime? enteredBirthDateTo;
        private bool? selectedGender;
        private string enteredContactDetails;
        private List<Customer> _customers;
        private Customer customer;
        private ObservableCollection<CustomerUserControl> Customers { get; set; }

        public Main(Customer customer = null)
        {
            InitializeComponent();
            _customers = RepositoryCustomer.GetCustomers();
            Customers = new ObservableCollection<CustomerUserControl>();
            CarSaleList.ItemsSource = Customers;
            this.customer = customer;
            if(customer != null)
                ExportCustomers.Visibility = AddCustomers.Visibility = FilterCustomers.Visibility = UpdateCustomers.Visibility = Visibility.Hidden;

            InitializeCustomers();
        }

        private void FilterCustomers_Click(object sender, RoutedEventArgs e)
        {
            var filter = new Filter(filterUse)
            {
                EnteredFullName = enteredFullName,
                EnteredPassportData = enteredPassportData,
                EnteredAddress = enteredAddress,
                EnteredBirthDateFrom = enteredBirthDateFrom,
                EnteredBirthDateTo = enteredBirthDateTo,
                SelectedGender = selectedGender,
                EnteredContactDetails = enteredContactDetails
            };

            filterUse = filter.ShowDialog();
            enteredFullName = filter.EnteredFullName;
            enteredPassportData = filter.EnteredPassportData;
            enteredAddress = filter.EnteredAddress;
            enteredBirthDateFrom = filter.EnteredBirthDateFrom;
            enteredBirthDateTo = filter.EnteredBirthDateTo;
            selectedGender = filter.SelectedGender;
            enteredContactDetails = filter.EnteredContactDetails;

            InitializeCustomers();
        }

        private void InitializeCustomers()
        {
            Customers.Clear();
            var filteredCustomers = _customers;

            if(customer != null)
            {
                Customers.Add(new CustomerUserControl(true, customer, this));
                return;
            }

            if (filterUse)
            {
                filteredCustomers = filteredCustomers.Where(customer =>
                    (string.IsNullOrEmpty(enteredFullName) || customer.FullName.Contains(enteredFullName)) &&
                    (string.IsNullOrEmpty(enteredPassportData) || customer.PassportData.Contains(enteredPassportData)) &&
                    (string.IsNullOrEmpty(enteredAddress) || customer.Address.Contains(enteredAddress)) &&
                    (enteredBirthDateFrom == null || customer.BirthDate >= enteredBirthDateFrom) &&
                    (enteredBirthDateTo == null || customer.BirthDate <= enteredBirthDateTo) &&
                    (selectedGender == null || customer.Gender == selectedGender) &&
                    (string.IsNullOrEmpty(enteredContactDetails) || customer.ContactDetails.Contains(enteredContactDetails))
                ).ToList();
            }

            foreach (var customer in filteredCustomers)
                Customers.Add(new CustomerUserControl((customer != null),customer, this));
        }

        private void AddCustomer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var addedCustomer = new Customer() { CustomerID = RepositoryCustomer.AddCustomer() };
            _customers.Add(addedCustomer);
            Customers.Add(new CustomerUserControl((customer != null), addedCustomer, this));
        }

        public void RemoveCustomer(CustomerUserControl customerControl)
        {
            if (customerControl != null)
            {
                Customers.Remove(customerControl);
                _customers.Remove(customerControl.Customer);
            }
        }

        private void Update_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _customers = RepositoryCustomer.GetCustomers();
            InitializeCustomers();
        }

        private void ExportCustomers_Click(object sender, System.Windows.RoutedEventArgs e)
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
                var worksheet = package.Workbook.Worksheets.Add("Customers");

                // Заголовки столбцов
                worksheet.Cells[1, 1].Value = "FullName";
                worksheet.Cells[1, 2].Value = "PassportData";
                worksheet.Cells[1, 3].Value = "Address";
                worksheet.Cells[1, 4].Value = "BirthDate";
                worksheet.Cells[1, 5].Value = "Gender";
                worksheet.Cells[1, 6].Value = "ContactDetails";

                var customers = Customers.ToList();

                // Заполнение данными
                for (int i = 0; i < customers.Count; i++)
                {
                    var customer = customers[i].Customer;
                    worksheet.Cells[i + 2, 1].Value = customer.FullName;
                    worksheet.Cells[i + 2, 2].Value = customer.PassportData;
                    worksheet.Cells[i + 2, 3].Value = customer.Address;
                    worksheet.Cells[i + 2, 4].Value = customer.BirthDate;
                    worksheet.Cells[i + 2, 5].Value = customer.Gender.Value ? "Male" : "Female";
                    worksheet.Cells[i + 2, 6].Value = customer.ContactDetails;
                }

                // Сохранение в файл
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }
        }
    }
}