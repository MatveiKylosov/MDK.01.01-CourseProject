using MDK._01._01_CourseProject.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using MDK._01._01_CourseProject.SQL;

namespace MDK._01._01_CourseProject.Repository
{
    static public class RepositoryCar
    {
        static public List<Car> GetCars()
        {
            var cars = new List<Car>();
            using (var connection = new MySqlConnection(Config.connectionString))
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
            return cars;
        }

        static public void AddCar(Car car)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Cars (CarName, BrandID, YearOfProduction, Color, Category, Price) VALUES (@CarName, @BrandID, @YearOfProduction, @Color, @Category, @Price)";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CarName", car.CarName);
                    cmd.Parameters.AddWithValue("@BrandID", car.BrandID);
                    cmd.Parameters.AddWithValue("@YearOfProduction", car.YearOfProduction);
                    cmd.Parameters.AddWithValue("@Color", car.Color);
                    cmd.Parameters.AddWithValue("@Category", car.Category);
                    cmd.Parameters.AddWithValue("@Price", car.Price);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static public void UpdateCar(Car car)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
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
            }
        }

        static public void DeleteCar(int carID)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
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
            }
        }
    }
}
