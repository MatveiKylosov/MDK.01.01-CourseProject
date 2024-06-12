using MDK._01._01_CourseProject.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using MDK._01._01_CourseProject.SQL;

namespace MDK._01._01_CourseProject.Repository
{
    static public class RepositoryCarSale
    {
        static public List<CarSale> GetCarSales()
        {
            var carSales = new List<CarSale>();
            using (var connection = new MySqlConnection(Config.connectionString))
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
            return carSales;
        }

        static public void AddCarSale(CarSale carSale)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
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
            }
        }

        static public void UpdateCarSale(CarSale carSale)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
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
            }
        }

        static public void DeleteCarSale(int saleID)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                connection.Open();
                string query = "DELETE FROM CarSales WHERE SaleID=@SaleID";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SaleID", saleID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

}
