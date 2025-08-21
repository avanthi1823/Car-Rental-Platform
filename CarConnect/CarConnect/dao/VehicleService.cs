using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CarConnect
{


        public class VehicleService : IVehicleService
        {
        private readonly string connectionString;

        public VehicleService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public VehicleService()
        {
        }

        public Vehicle GetVehicleById(int vehicleId)
            {
                try
                {
                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = "SELECT  * FROM Vehicle WHERE VehicleId = @VehicleId";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@VehicleId", vehicleId);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    return new Vehicle
                                    {
                                        VehicleId = (int)reader["VehicleId"],
                                        Model = reader["Model"].ToString(),
                                        Make = reader["Make"].ToString(),
                                        Year = (int)reader["Year"],
                                        Color = reader["Color"].ToString(),
                                        RegistrationNumber = reader["RegistrationNumber"].ToString(),
                                        Availability = (bool)reader["Availability"],
                                        DailyRate = (decimal)reader["DailyRate"]
                                    };
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error retrieving vehicle: " + ex.Message);
                }

                throw new VehicleNotFoundException();
            }

            public List<Vehicle> GetAvailableVehicles()
            {
                List<Vehicle> vehicles = new List<Vehicle>();
                try
                {
                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = "SELECT * FROM Vehicle WHERE Availability = 1 ORDER BY VehicleId";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    vehicles.Add(new Vehicle
                                    {
                                        VehicleId = (int)reader["VehicleId"],
                                        Model = reader["Model"].ToString(),
                                        Make = reader["Make"].ToString(),
                                        Year = (int)reader["Year"],
                                        Color = reader["Color"].ToString(),
                                        RegistrationNumber = reader["RegistrationNumber"].ToString(),
                                        Availability = (bool)reader["Availability"],
                                        DailyRate = (decimal)reader["DailyRate"]
                                    });
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error retrieving available vehicles: " + ex.Message);
                }

                return vehicles;
            }

            public List<Vehicle> GetAllVehicles()
            {
                List<Vehicle> vehicles = new List<Vehicle>();
                try
                {
                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = "SELECT * FROM Vehicle ORDER BY VehicleId";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    vehicles.Add(new Vehicle
                                    {
                                        VehicleId = (int)reader["VehicleId"],
                                        Model = reader["Model"].ToString(),
                                        Make = reader["Make"].ToString(),
                                        Year = (int)reader["Year"],
                                        Color = reader["Color"].ToString(),
                                        RegistrationNumber = reader["RegistrationNumber"].ToString(),
                                        Availability = (bool)reader["Availability"],
                                        DailyRate = (decimal)reader["DailyRate"]
                                    });
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error retrieving vehicles: " + ex.Message);
                }

                return vehicles;
            }

            public bool AddVehicle(Vehicle vehicleData)
            {
                try
                {
                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = @"INSERT INTO Vehicle 
                                    (VehicleId, Model, Make, Year, Color, RegistrationNumber, Availability, DailyRate) 
                                    VALUES (@VehicleId, @Model, @Make, @Year, @Color, @RegistrationNumber, @Availability, @DailyRate)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@VehicleId", vehicleData.VehicleId);
                            command.Parameters.AddWithValue("@Model", vehicleData.Model);
                            command.Parameters.AddWithValue("@Make", vehicleData.Make);
                            command.Parameters.AddWithValue("@Year", vehicleData.Year);
                            command.Parameters.AddWithValue("@Color", vehicleData.Color);
                            command.Parameters.AddWithValue("@RegistrationNumber", vehicleData.RegistrationNumber);
                            command.Parameters.AddWithValue("@Availability", vehicleData.Availability);
                            command.Parameters.AddWithValue("@DailyRate", vehicleData.DailyRate);

                            int rowsAffected = command.ExecuteNonQuery();
                            return rowsAffected > 0;
                        }
                    }
                }
                catch (SqlException ex) when (ex.Number == 2627)
                {
                    throw new InvalidInputException("Vehicle ID or Registration Number already exists.");
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error adding vehicle: " + ex.Message);
                }
            }

            public bool UpdateVehicle(Vehicle vehicleData)
            {
                try
                {
                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = @"UPDATE Vehicle SET 
                                    Model = @Model, 
                                    Make = @Make, 
                                    Year = @Year, 
                                    Color = @Color, 
                                    RegistrationNumber = @RegistrationNumber, 
                                    Availability = @Availability, 
                                    DailyRate = @DailyRate 
                                    WHERE VehicleId = @VehicleId";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@VehicleId", vehicleData.VehicleId);
                            command.Parameters.AddWithValue("@Model", vehicleData.Model);
                            command.Parameters.AddWithValue("@Make", vehicleData.Make);
                            command.Parameters.AddWithValue("@Year", vehicleData.Year);
                            command.Parameters.AddWithValue("@Color", vehicleData.Color);
                            command.Parameters.AddWithValue("@RegistrationNumber", vehicleData.RegistrationNumber);
                            command.Parameters.AddWithValue("@Availability", vehicleData.Availability);
                            command.Parameters.AddWithValue("@DailyRate", vehicleData.DailyRate);

                            int rowsAffected = command.ExecuteNonQuery();
                            return rowsAffected > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error updating vehicle: " + ex.Message);
                }
            }

            public bool RemoveVehicle(int vehicleId)
            {
                try
                {
                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = "DELETE FROM Vehicle WHERE VehicleId = @VehicleId";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@VehicleId", vehicleId);

                            int rowsAffected = command.ExecuteNonQuery();
                            return rowsAffected > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error removing vehicle: " + ex.Message);
                }
            }
        }
    }