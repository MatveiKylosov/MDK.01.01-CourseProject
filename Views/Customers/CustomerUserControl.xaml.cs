using MDK._01._01_CourseProject.Models;
using MDK._01._01_CourseProject.Repository;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MDK._01._01_CourseProject.Views.Customers
{
    public partial class CustomerUserControl : UserControl
    {
        bool UserMode;
        public Customer Customer;
        Main main;
        bool edit
        {
            get { return ContactDetails.IsEnabled; }
            set
            {
                if (!UserMode)
                {
                    FullName.IsEnabled = value;
                    PassportData.IsEnabled = value;
                    Address.IsEnabled = value;
                    BirthDate.IsEnabled = value;
                    Gender.IsEnabled = value;
                }
                ContactDetails.IsEnabled = value;
                DeleteButton.IsEnabled = value;
                EditButton.Content = value ? "Сохранить" : "Изменить";
            }
        }

        public CustomerUserControl(bool UserMode, Customer customer, Main main)
        {
            InitializeComponent();
            this.Customer = customer;
            this.main = main;
            this.edit = false;
            if (customer == null) return;

            FullName.Text = customer.FullName;
            PassportData.Text = customer.PassportData;
            Address.Text = customer.Address;
            BirthDate.SelectedDate = customer.BirthDate;
            ContactDetails.Text = customer.ContactDetails;
            Gender.IsChecked = customer.Gender;

            if (UserMode)
                DeleteButton.Visibility = Visibility.Hidden;

            this.UserMode = UserMode;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!edit)
            {
                edit = true;
                EditButton.Content = "Сохранить";
                return;
            }
            if (String.IsNullOrEmpty(FullName.Text))
            {
                MessageBox.Show("ФИО не должно быть пустым.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrEmpty(PassportData.Text))
            {
                MessageBox.Show("Паспортные данные не должны быть пустыми.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrEmpty(Address.Text))
            {
                MessageBox.Show("Адрес не должен быть пустым.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrEmpty(ContactDetails.Text))
            {
                MessageBox.Show("Контактные данные не должны быть пустыми.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Customer.FullName = this.FullName.Text;
            Customer.PassportData = this.PassportData.Text;
            Customer.Address = this.Address.Text;
            Customer.BirthDate = BirthDate.SelectedDate.Value;
            Customer.ContactDetails = this.ContactDetails.Text;
            Customer.Gender = Gender.IsChecked.Value;
            RepositoryCustomer.UpdateCustomer(Customer);
            edit = false;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool hasRelatedCarSales = RepositoryCarSale.GetCarSales().Any(x => x.CustomerID == Customer.CustomerID);
            MessageBoxResult result;
            if (hasRelatedCarSales)
            {
                result = MessageBox.Show("Данная запись участвует в других таблицах. Удалить эту запись вместе с другими?", "Внимание!", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                    RepositoryCustomer.DeleteAllEntries(Customer);
                else if (result == MessageBoxResult.No)
                    RepositoryCustomer.DeleteCustomer(Customer);
            }
            else
            {
                result = MessageBox.Show("Продолжить удаление этой записи?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                    RepositoryCustomer.DeleteCustomer(Customer);
                else
                    result = MessageBoxResult.Cancel;
            }
            if (result == MessageBoxResult.Yes || result == MessageBoxResult.No)
                main.RemoveCustomer(this);
        }
    }
}