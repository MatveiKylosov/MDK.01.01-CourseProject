using MDK._01._01_CourseProject.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using MDK._01._01_CourseProject.SQL;

namespace MDK._01._01_CourseProject.Repository
{
    public static class RepositoryCustomer
    {
        public static List<Customer> GetCustomers()
        {
            var customers = new List<Customer>();
            using (var connection = new MySqlConnection(Config.connectionString))
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
                                FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? string.Empty : reader.GetString("FullName"),
                                PassportData = reader.IsDBNull(reader.GetOrdinal("PassportData")) ? string.Empty : reader.GetString("PassportData"),
                                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? string.Empty : reader.GetString("Address"),
                                BirthDate = reader.IsDBNull(reader.GetOrdinal("BirthDate")) ? (DateTime?)null : reader.GetDateTime("BirthDate"),
                                Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? (bool?)null : reader.GetBoolean("Gender"),
                                ContactDetails = reader.IsDBNull(reader.GetOrdinal("ContactDetails")) ? string.Empty : reader.GetString("ContactDetails")
                            });
                        }
                    }
                }
            }
            return customers;
        }

        public static void AddCustomer(Customer customer)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Customers (FullName, PassportData, Address, BirthDate, Gender, ContactDetails) " +
                               "VALUES (@FullName, @PassportData, @Address, @BirthDate, @Gender, @ContactDetails)";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@FullName", customer.FullName);
                    cmd.Parameters.AddWithValue("@PassportData", customer.PassportData);
                    cmd.Parameters.AddWithValue("@Address", customer.Address);
                    cmd.Parameters.AddWithValue("@BirthDate", customer.BirthDate);
                    cmd.Parameters.AddWithValue("@Gender", customer.Gender);
                    cmd.Parameters.AddWithValue("@ContactDetails", customer.ContactDetails);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateCustomer(Customer customer)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                connection.Open();
                string query = "UPDATE Customers SET FullName=@FullName, PassportData=@PassportData, Address=@Address, " +
                               "BirthDate=@BirthDate, Gender=@Gender, ContactDetails=@ContactDetails WHERE CustomerID=@CustomerID";
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
            }
        }

        public static void DeleteCustomer(int customerID)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                connection.Open();
                // Удаляем все продажи, связанные с этим клиентом
                string deleteCarSalesQuery = "DELETE FROM CarSales WHERE CustomerID=@CustomerID";
                using (var cmd = new MySqlCommand(deleteCarSalesQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    cmd.ExecuteNonQuery();
                }

                // Удаляем самого клиента
                string deleteCustomerQuery = "DELETE FROM Customers WHERE CustomerID=@CustomerID";
                using (var cmd = new MySqlCommand(deleteCustomerQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
