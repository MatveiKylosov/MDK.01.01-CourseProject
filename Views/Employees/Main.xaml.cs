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

namespace MDK._01._01_CourseProject.Views.Employees
{
    public partial class Main : Page
    {
        private bool filterUse = false;
        private string enteredFullName;
        private int? enteredWorkExperienceFrom;
        private int? enteredWorkExperienceTo;
        private decimal? enteredSalaryFrom;
        private decimal? enteredSalaryTo;
        private string enteredContactDetails;
        private List<Employee> _employees;
        private ObservableCollection<EmployeeUserControl> Employees { get; set; }

        public Main()
        {
            InitializeComponent();
            _employees = RepositoryEmployee.GetEmployees();
            Employees = new ObservableCollection<EmployeeUserControl>();
            InitializeEmployees();
            CarSaleList.ItemsSource = Employees;
        }

        private void InitializeEmployees()
        {
            Employees.Clear();
            var filteredEmployees = _employees;

            if (filterUse)
            {
                filteredEmployees = filteredEmployees.Where(employee =>
                    (string.IsNullOrEmpty(enteredFullName) || employee.FullName.Contains(enteredFullName)) &&
                    (enteredWorkExperienceFrom == null || employee.WorkExperience >= enteredWorkExperienceFrom) &&
                    (enteredWorkExperienceTo == null || employee.WorkExperience <= enteredWorkExperienceTo) &&
                    (enteredSalaryFrom == null || employee.Salary >= enteredSalaryFrom) &&
                    (enteredSalaryTo == null || employee.Salary <= enteredSalaryTo) &&
                    (string.IsNullOrEmpty(enteredContactDetails) || employee.ContactDetails.Contains(enteredContactDetails))
                ).ToList();
            }

            foreach (var employee in filteredEmployees)
            {
                Employees.Add(new EmployeeUserControl(employee, this));
            }
        }

        private void FilterEmployees_Click(object sender, RoutedEventArgs e)
        {
            var filter = new Filter(filterUse)
            {
                EnteredFullName = enteredFullName,
                EnteredWorkExperienceFrom = enteredWorkExperienceFrom,
                EnteredWorkExperienceTo = enteredWorkExperienceTo,
                EnteredSalaryFrom = enteredSalaryFrom,
                EnteredSalaryTo = enteredSalaryTo,
                EnteredContactDetails = enteredContactDetails
            };

            filterUse = filter.ShowDialog();
            enteredFullName = filter.EnteredFullName;
            enteredWorkExperienceFrom = filter.EnteredWorkExperienceFrom;
            enteredWorkExperienceTo = filter.EnteredWorkExperienceTo;
            enteredSalaryFrom = filter.EnteredSalaryFrom;
            enteredSalaryTo = filter.EnteredSalaryTo;
            enteredContactDetails = filter.EnteredContactDetails;

            InitializeEmployees();
        }

        private void AddEmployee_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var addedEmployee = new Employee() { EmployeeID = RepositoryEmployee.AddEmployee() };
            _employees.Add(addedEmployee);
            Employees.Add(new EmployeeUserControl(addedEmployee, this));
        }

        public void RemoveEmployee(EmployeeUserControl employeeControl)
        {
            if (employeeControl != null)
            {
                Employees.Remove(employeeControl);
                _employees.Remove(employeeControl.Employee);
            }
        }

        private void Update_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _employees = RepositoryEmployee.GetEmployees();
            InitializeEmployees();
        }

        private void ExportEmployees_Click(object sender, System.Windows.RoutedEventArgs e)
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
                var worksheet = package.Workbook.Worksheets.Add("Employees");

                // Заголовки столбцов
                worksheet.Cells[1, 1].Value = "№";
                worksheet.Cells[1, 2].Value = "ФИО";
                worksheet.Cells[1, 3].Value = "Опыт работы";
                worksheet.Cells[1, 4].Value = "Зарплата";
                worksheet.Cells[1, 5].Value = "Контактные данные";

                var employees = Employees.ToList();
                // Заполнение данными
                for (int i = 0; i < employees.Count; i++)
                {
                    var employee = employees[i].Employee;
                    worksheet.Cells[i + 2, 1].Value = employee.EmployeeID;
                    worksheet.Cells[i + 2, 2].Value = employee.FullName;

                    if(employee.WorkExperience.HasValue)
                        worksheet.Cells[i + 2, 3].Value = employee.WorkExperience.Value;

                    if (employee.Salary.HasValue)
                        worksheet.Cells[i + 2, 4].Value = employee.Salary.Value;

                    worksheet.Cells[i + 2, 5].Value = employee.ContactDetails;
                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                // Сохранение в файл
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }
        }
    }
}