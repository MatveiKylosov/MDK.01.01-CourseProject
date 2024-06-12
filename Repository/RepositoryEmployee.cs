using MDK._01._01_CourseProject.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using MDK._01._01_CourseProject.SQL;

namespace MDK._01._01_CourseProject.Repository
{
    static public class RepositoryEmployee
    {
        static public List<Employee> GetEmployees()
        {
            var employees = new List<Employee>();
            using (var connection = new MySqlConnection(Config.connectionString))
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
                                FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? string.Empty : reader.GetString("FullName"),
                                WorkExperience = reader.IsDBNull(reader.GetOrdinal("WorkExperience")) ? (int?)null : reader.GetInt32("WorkExperience"),
                                Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? (decimal?)null : reader.GetDecimal("Salary"),
                                ContactDetails = reader.IsDBNull(reader.GetOrdinal("ContactDetails")) ? string.Empty : reader.GetString("ContactDetails")
                            });
                        }
                    }
                }
            }
            return employees;
        }


        static public void AddEmployee(Employee employee)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Employees (FullName, WorkExperience, Salary, ContactDetails) VALUES (@FullName, @WorkExperience, @Salary, @ContactDetails)";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@FullName", employee.FullName);
                    cmd.Parameters.AddWithValue("@WorkExperience", employee.WorkExperience);
                    cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                    cmd.Parameters.AddWithValue("@ContactDetails", employee.ContactDetails);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static public void UpdateEmployee(Employee employee)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
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
            }
        }

        static public void DeleteEmployee(int employeeID)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Employees WHERE EmployeeID=@EmployeeID";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
