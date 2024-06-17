using MDK._01._01_CourseProject.Models;
using MDK._01._01_CourseProject.SQL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MDK._01._01_CourseProject.Repository
{
    public static class RepositoryCar
    {
        public static List<Car> GetCars()
        {
            var cars = new List<Car>();
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Cars";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cars.Add(new Car
                                {
                                    CarID = reader.GetInt32("CarID"),
                                    CarName = reader.IsDBNull(reader.GetOrdinal("CarName")) ? null : reader.GetString("CarName"),
                                    BrandID = reader.IsDBNull(reader.GetOrdinal("BrandID")) ? (int?)null : reader.GetInt32("BrandID"),
                                    YearOfProduction = reader.IsDBNull(reader.GetOrdinal("YearOfProduction")) ? (int?)null : reader.GetInt32("YearOfProduction"),
                                    Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? null : reader.GetString("Color"),
                                    Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? null : reader.GetString("Category"),
                                    Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? (decimal?)null : reader.GetDecimal("Price")
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при получении данных автомобилей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return cars;
        }

        public static int AddCar()
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO Cars (CarName, BrandID, YearOfProduction, Color, Category, Price) VALUES (NULL, NULL, NULL, NULL, NULL, NULL); SELECT LAST_INSERT_ID();";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении автомобиля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return -1;
                }
            }
        }


        public static bool UpdateCar(Car car)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE Cars SET CarName=@CarName, BrandID=@BrandID, YearOfProduction=@YearOfProduction, Color=@Color, Category=@Category, Price=@Price WHERE CarID=@CarID";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CarID", car.CarID);
                        cmd.Parameters.AddWithValue("@CarName", car.CarName);
                        cmd.Parameters.AddWithValue("@BrandID", car.BrandID);
                        cmd.Parameters.AddWithValue("@YearOfProduction", car.YearOfProduction);
                        cmd.Parameters.AddWithValue("@Color", car.Color);
                        cmd.Parameters.AddWithValue("@Category", car.Category);
                        cmd.Parameters.AddWithValue("@Price", car.Price);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении автомобиля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        public static bool DeleteCar(Car car)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    int carID = car.CarID;
                    connection.Open();
                    string updateCarSalesQuery = "UPDATE CarSales SET CarID=NULL WHERE CarID=@CarID";
                    using (var cmd = new MySqlCommand(updateCarSalesQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@CarID", carID);
                        cmd.ExecuteNonQuery();
                    }
                    string deleteCarQuery = "DELETE FROM Cars WHERE CarID=@CarID";
                    using (var cmd = new MySqlCommand(deleteCarQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@CarID", carID);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении автомобиля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }
        public static bool DeleteAllEntries(Car car)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                try
                {
                    int carID = car.CarID;
                    connection.Open();

                    string deleteCarSalesQuery = "DELETE FROM CarSales WHERE CarID=@CarID";
                    using (var cmd = new MySqlCommand(deleteCarSalesQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@CarID", carID);
                        cmd.ExecuteNonQuery();
                    }

                    string deleteCarQuery = "DELETE FROM Cars WHERE CarID=@CarID";
                    using (var cmd = new MySqlCommand(deleteCarQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@CarID", carID);
                        cmd.ExecuteNonQuery();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении автомобиля и связанных записей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

    }
}
