using MDK._01._01_CourseProject.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using MDK._01._01_CourseProject.SQL;
using System.Windows;

namespace MDK._01._01_CourseProject.Repository
{
    public static class RepositoryCarSale
    {
        public static List<CarSale> GetCarSales()
        {
            var carSales = new List<CarSale>();
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM CarSales";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                carSales.Add(new CarSale
                                {
                                    SaleID = reader.GetInt32("SaleID"),
                                    SaleDate = reader.IsDBNull(reader.GetOrdinal("SaleDate")) ? (DateTime?)null : reader.GetDateTime("SaleDate"),
                                    EmployeeID = reader.IsDBNull(reader.GetOrdinal("EmployeeID")) ? (int?)null : reader.GetInt32("EmployeeID"),
                                    CarID = reader.IsDBNull(reader.GetOrdinal("CarID")) ? (int?)null : reader.GetInt32("CarID"),
                                    CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? (int?)null : reader.GetInt32("CustomerID")
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при получении данных о продажах автомобилей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return carSales;
        }

        public static bool AddCarSale(CarSale carSale)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO CarSales (SaleDate, EmployeeID, CarID, CustomerID) VALUES (@SaleDate, @EmployeeID, @CarID, @CustomerID)";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@SaleDate", carSale.SaleDate);
                        cmd.Parameters.AddWithValue("@EmployeeID", carSale.EmployeeID);
                        cmd.Parameters.AddWithValue("@CarID", carSale.CarID);
                        cmd.Parameters.AddWithValue("@CustomerID", carSale.CustomerID);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении продажи автомобиля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        public static bool UpdateCarSale(CarSale carSale)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE CarSales SET SaleDate=@SaleDate, EmployeeID=@EmployeeID, CarID=@CarID, CustomerID=@CustomerID WHERE SaleID=@SaleID";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@SaleID", carSale.SaleID);
                        cmd.Parameters.AddWithValue("@SaleDate", carSale.SaleDate);
                        cmd.Parameters.AddWithValue("@EmployeeID", carSale.EmployeeID);
                        cmd.Parameters.AddWithValue("@CarID", carSale.CarID);
                        cmd.Parameters.AddWithValue("@CustomerID", carSale.CustomerID);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении продажи автомобиля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        public static bool DeleteCarSale(int saleID)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM CarSales WHERE SaleID=@SaleID";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@SaleID", saleID);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении продажи автомобиля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }
    }
}
