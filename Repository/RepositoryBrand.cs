using MDK._01._01_CourseProject.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using MDK._01._01_CourseProject.SQL;

namespace MDK._01._01_CourseProject.Repository
{
    static public class RepositoryBrand
    {
        static public List<Brand> GetBrands()
        {
            var brands = new List<Brand>();
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Brands";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            brands.Add(new Brand
                            {
                                BrandID = reader.GetInt32("BrandID"),
                                BrandName = reader.IsDBNull(1) ? string.Empty : reader.GetString("BrandName"),
                                Country = reader.IsDBNull(2) ? string.Empty : reader.GetString("Country"),
                                Manufacturer = reader.IsDBNull(3) ? string.Empty : reader.GetString("Manufacturer"),
                                Address = reader.IsDBNull(4) ? string.Empty : reader.GetString("Address")
                            });
                        }
                    }
                }
            }
            return brands;
        }

        static public void AddBrand(Brand brand)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Brands (BrandName, Country, Manufacturer, Address) VALUES (@BrandName, @Country, @Manufacturer, @Address)";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@BrandName", brand.BrandName);
                    cmd.Parameters.AddWithValue("@Country", brand.Country);
                    cmd.Parameters.AddWithValue("@Manufacturer", brand.Manufacturer);
                    cmd.Parameters.AddWithValue("@Address", brand.Address);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static public void UpdateBrand(Brand brand)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                connection.Open();
                string query = "UPDATE Brands SET BrandName=@BrandName, Country=@Country, Manufacturer=@Manufacturer, Address=@Address WHERE BrandID=@BrandID";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@BrandID", brand.BrandID);
                    cmd.Parameters.AddWithValue("@BrandName", brand.BrandName);
                    cmd.Parameters.AddWithValue("@Country", brand.Country);
                    cmd.Parameters.AddWithValue("@Manufacturer", brand.Manufacturer);
                    cmd.Parameters.AddWithValue("@Address", brand.Address);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static public void DeleteAllEntries(Brand brand)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                int brandID = brand.BrandID;
                connection.Open();

                string deleteCarSalesQuery = "DELETE FROM CarSales WHERE CarID IN (SELECT CarID FROM Cars WHERE BrandID=@BrandID)";
                using (var cmd = new MySqlCommand(deleteCarSalesQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@BrandID", brandID);
                    cmd.ExecuteNonQuery();
                }

                string deleteCarsQuery = "DELETE FROM Cars WHERE BrandID=@BrandID";
                using (var cmd = new MySqlCommand(deleteCarsQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@BrandID", brandID);
                    cmd.ExecuteNonQuery();
                }

                string deleteBrandQuery = "DELETE FROM Brands WHERE BrandID=@BrandID";
                using (var cmd = new MySqlCommand(deleteBrandQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@BrandID", brandID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static public void DeleteBrand(Brand brand)
        {
            using (var connection = new MySqlConnection(Config.connectionString))
            {
                int brandID = brand.BrandID;
                connection.Open();

                string updateCarSalesQuery = "UPDATE CarSales SET BrandID=NULL WHERE CarID IN (SELECT CarID FROM Cars WHERE BrandID=@BrandID)";
                using (var cmd = new MySqlCommand(updateCarSalesQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@BrandID", brandID);
                    cmd.ExecuteNonQuery();
                }

                string updateCarsQuery = "UPDATE Cars SET BrandID=NULL WHERE BrandID=@BrandID";
                using (var cmd = new MySqlCommand(updateCarsQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@BrandID", brandID);
                    cmd.ExecuteNonQuery();
                }

                string deleteBrandQuery = "DELETE FROM Brands WHERE BrandID=@BrandID";
                using (var cmd = new MySqlCommand(deleteBrandQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@BrandID", brandID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
