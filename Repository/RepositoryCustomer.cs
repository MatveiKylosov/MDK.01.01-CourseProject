using MDK._01._01_CourseProject.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using MDK._01._01_CourseProject.SQL;
using System.Windows;

namespace MDK._01._01_CourseProject.Repository
{
    public static class RepositoryCustomer
    {
        public static List<Customer> GetCustomers()
        {
            var customers = new List<Customer>();
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Customers";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customers.Add(new Customer
                                {
                                    CustomerID = reader.GetInt32("CustomerID"),
                                    FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? null : reader.GetString("FullName"),
                                    PassportData = reader.IsDBNull(reader.GetOrdinal("PassportData")) ? null : reader.GetString("PassportData"),
                                    Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString("Address"),
                                    BirthDate = reader.IsDBNull(reader.GetOrdinal("BirthDate")) ? (DateTime?)null : reader.GetDateTime("BirthDate"),
                                    Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? (bool?)null : reader.GetBoolean("Gender"),
                                    ContactDetails = reader.IsDBNull(reader.GetOrdinal("ContactDetails")) ? null : reader.GetString("ContactDetails")
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при получении данных клиентов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return customers;
        }

        public static int AddCustomer()
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO Customers (FullName, PassportData, Address, BirthDate, Gender, ContactDetails) VALUES (NULL, NULL, NULL, NULL, NULL, NULL); SELECT LAST_INSERT_ID();";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return -1;
                }
            }
        }


        public static bool UpdateCustomer(Customer customer)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE Customers SET FullName=@FullName, PassportData=@PassportData, Address=@Address, BirthDate=@BirthDate, Gender=@Gender, ContactDetails=@ContactDetails WHERE CustomerID=@CustomerID";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                        cmd.Parameters.AddWithValue("@FullName", customer.FullName);
                        cmd.Parameters.AddWithValue("@PassportData", customer.PassportData);
                        cmd.Parameters.AddWithValue("@Address", customer.Address);
                        cmd.Parameters.AddWithValue("@BirthDate", customer.BirthDate);
                        cmd.Parameters.AddWithValue("@Gender", customer.Gender);
                        cmd.Parameters.AddWithValue("@ContactDetails", customer.ContactDetails);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении данных клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        public static bool DeleteCustomer(int customerID)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    connection.Open();
                    string deleteCarSalesQuery = "DELETE FROM CarSales WHERE CustomerID=@CustomerID";
                    using (var cmd = new MySqlCommand(deleteCarSalesQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", customerID);
                        cmd.ExecuteNonQuery();
                    }
                    string deleteCustomerQuery = "DELETE FROM Customers WHERE CustomerID=@CustomerID";
                    using (var cmd = new MySqlCommand(deleteCustomerQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", customerID);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }
    }
}
