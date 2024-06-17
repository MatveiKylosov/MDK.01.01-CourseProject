using MDK._01._01_CourseProject.Models;
using MDK._01._01_CourseProject.Repository;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MDK._01._01_CourseProject.Views.Employees
{
    public partial class EmployeeUserControl : UserControl
    {
        public Employee Employee;
        Main main;
        bool edit
        {
            get { return FullName.IsEnabled; }
            set
            {
                FullName.IsEnabled = value;
                WorkExperience.IsEnabled = value;
                Salary.IsEnabled = value;
                ContactDetails.IsEnabled = value;
                DeleteButton.IsEnabled = value;
                EditButton.Content = value ? "Сохранить" : "Изменить";
            }
        }

        public EmployeeUserControl(Employee employee, Main main)
        {
            InitializeComponent();
            this.Employee = employee;
            this.main = main;
            this.edit = false;
            if (employee == null) return;
            FullName.Text = employee.FullName;
            WorkExperience.Text = employee.WorkExperience?.ToString();
            Salary.Text = employee.Salary?.ToString();
            ContactDetails.Text = employee.ContactDetails;
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
            if (String.IsNullOrEmpty(WorkExperience.Text) || !int.TryParse(WorkExperience.Text, out int workExperience))
            {
                MessageBox.Show("Опыт работы должен быть числом.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrEmpty(Salary.Text) || !decimal.TryParse(Salary.Text, out decimal salary))
            {
                MessageBox.Show("Зарплата должна быть числом.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrEmpty(ContactDetails.Text))
            {
                MessageBox.Show("Контактные данные не должны быть пустыми.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Employee.FullName = this.FullName.Text;
            Employee.WorkExperience = workExperience;
            Employee.Salary = salary;
            Employee.ContactDetails = this.ContactDetails.Text;
            RepositoryEmployee.UpdateEmployee(Employee);
            edit = false;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool hasRelatedCarSales = RepositoryCarSale.GetCarSales().Any(x => x.EmployeeID == Employee.EmployeeID);
            MessageBoxResult result;
            if (hasRelatedCarSales)
            {
                result = MessageBox.Show("Данная запись участвует в других таблицах. Удалить эту запись вместе с другими?", "Внимание!", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                    RepositoryEmployee.DeleteAllEntries(Employee);
                else if (result == MessageBoxResult.No)
                    RepositoryEmployee.DeleteEmployee(Employee);
            }
            else
            {
                result = MessageBox.Show("Продолжить удаление этой записи?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                    RepositoryEmployee.DeleteEmployee(Employee);
                else
                    result = MessageBoxResult.Cancel;
            }
            if (result == MessageBoxResult.Yes || result == MessageBoxResult.No)
                main.RemoveEmployee(this);
        }
    }
}