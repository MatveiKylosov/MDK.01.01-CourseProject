using MDK._01._01_CourseProject.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using MDK._01._01_CourseProject.SQL;
using System.Windows;

namespace MDK._01._01_CourseProject.Repository
{
    public static class RepositoryEmployee
    {
        public static List<Employee> GetEmployees()
        {
            var employees = new List<Employee>();
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Employees";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employees.Add(new Employee
                                {
                                    EmployeeID = reader.GetInt32("EmployeeID"),
                                    FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? null : reader.GetString("FullName"),
                                    WorkExperience = reader.IsDBNull(reader.GetOrdinal("WorkExperience")) ? (int?)null : reader.GetInt32("WorkExperience"),
                                    Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? (decimal?)null : reader.GetDecimal("Salary"),
                                    ContactDetails = reader.IsDBNull(reader.GetOrdinal("ContactDetails")) ? null : reader.GetString("ContactDetails")
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при получении данных сотрудников: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return employees;
        }

        public static int AddEmployee()
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO Employees (FullName, WorkExperience, Salary, ContactDetails) VALUES (NULL, NULL, NULL, NULL); SELECT LAST_INSERT_ID();";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return -1;
                }
            }
        }

        public static bool UpdateEmployee(Employee employee)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE Employees SET FullName=@FullName, WorkExperience=@WorkExperience, Salary=@Salary, ContactDetails=@ContactDetails WHERE EmployeeID=@EmployeeID";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
                        cmd.Parameters.AddWithValue("@FullName", employee.FullName);
                        cmd.Parameters.AddWithValue("@WorkExperience", employee.WorkExperience);
                        cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                        cmd.Parameters.AddWithValue("@ContactDetails", employee.ContactDetails);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении данных сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        public static bool DeleteEmployee(int employeeID)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    connection.Open();
                    string deleteCarSalesQuery = "DELETE FROM CarSales WHERE EmployeeID=@EmployeeID";
                    using (var cmd = new MySqlCommand(deleteCarSalesQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                        cmd.ExecuteNonQuery();
                    }
                    string deleteEmployeeQuery = "DELETE FROM Employees WHERE EmployeeID=@EmployeeID";
                    using (var cmd = new MySqlCommand(deleteEmployeeQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }
    }
}
