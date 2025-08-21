using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CarConnect
{
   
   public class ReservationService : IReservationService
        {
            //private string connectionString;
            private IVehicleService vehicleService;

		
		private readonly string connectionString;

		public ReservationService(string connectionString)
		{
			this.connectionString = connectionString;
            this.vehicleService = new VehicleService(connectionString);
        }

        public ReservationService()
        {
        }

        public Reservation GetReservationById(int reservationId)
            {
                try
                {
                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = "SELECT  * FROM Reservation WHERE ReservationId = @ReservationId";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ReservationId", reservationId);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    return new Reservation
                                    {
                                        ReservationId = (int)reader["ReservationId"],
                                        CustomerId = (int)reader["CustomerId"],
                                        VehicleId = (int)reader["VehicleId"],
                                        StartDate = (DateTime)reader["StartDate"],
                                        EndDate = (DateTime)reader["EndDate"],
                                        TotalCost = (decimal)reader["TotalCost"],
                                        Status = reader["Status"].ToString()
                                    };
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error retrieving reservation: " + ex.Message);
                }

                throw new ReservationException("Reservation not found");
            }

            public List<Reservation> GetReservationsByCustomerId(int customerId)
            {
                List<Reservation> reservations = new List<Reservation>();
                try
                {
                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = "SELECT  * FROM Reservation WHERE CustomerId = @CustomerId ORDER BY StartDate";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@CustomerId", customerId);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    reservations.Add(new Reservation
                                    {
                                        ReservationId = (int)reader["ReservationId"],
                                        CustomerId = (int)reader["CustomerId"],
                                        VehicleId = (int)reader["VehicleId"],
                                        StartDate = (DateTime)reader["StartDate"],
                                        EndDate = (DateTime)reader["EndDate"],
                                        TotalCost = (decimal)reader["TotalCost"],
                                        Status = reader["Status"].ToString()
                                    });
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error retrieving reservations: " + ex.Message);
                }

                return reservations;
            }

            public List<Reservation> GetAllReservations()
            {
                List<Reservation> reservations = new List<Reservation>();
                try
                {
                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = "SELECT  * FROM Reservation ORDER BY StartDate";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    reservations.Add(new Reservation
                                    {
                                        ReservationId = (int)reader["ReservationId"],
                                        CustomerId = (int)reader["CustomerId"],
                                        VehicleId = (int)reader["VehicleId"],
                                        StartDate = (DateTime)reader["StartDate"],
                                        EndDate = (DateTime)reader["EndDate"],
                                        TotalCost = (decimal)reader["TotalCost"],
                                        Status = reader["Status"].ToString()
                                    });
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error retrieving reservations: " + ex.Message);
                }

                return reservations;
            }

            public bool CreateReservation(Reservation reservationData)
            {
                try
                {
                    // Check vehicle availability
                    Vehicle vehicle = vehicleService.GetVehicleById(reservationData.VehicleId);
                    if (!vehicle.Availability)
                    {
                        throw new ReservationException("Vehicle is not available for reservation");
                    }

                    // Calculate duration and total cost
                    TimeSpan duration = reservationData.EndDate - reservationData.StartDate;
                    reservationData.TotalCost = vehicle.DailyRate * duration.Days;
                    reservationData.Status = "Confirmed";

                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = @"INSERT INTO Reservation 
                                    (ReservationId, CustomerId, VehicleId, StartDate, EndDate, TotalCost, Status) 
                                    VALUES (@ReservationId, @CustomerId, @VehicleId, @StartDate, @EndDate, @TotalCost, @Status)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ReservationId", reservationData.ReservationId);
                            command.Parameters.AddWithValue("@CustomerId", reservationData.CustomerId);
                            command.Parameters.AddWithValue("@VehicleId", reservationData.VehicleId);
                            command.Parameters.AddWithValue("@StartDate", reservationData.StartDate);
                            command.Parameters.AddWithValue("@EndDate", reservationData.EndDate);
                            command.Parameters.AddWithValue("@TotalCost", reservationData.TotalCost);
                            command.Parameters.AddWithValue("@Status", reservationData.Status);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Update vehicle availability
                                vehicle.Availability = false;
                                vehicleService.UpdateVehicle(vehicle);
                                return true;
                            }
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error creating reservation: " + ex.Message);
                }
            }

            public bool UpdateReservation(Reservation reservationData)
            {
                try
                {
                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = @"UPDATE Reservation SET 
                                    CustomerId = @CustomerId, 
                                    VehicleId = @VehicleId, 
                                    StartDate = @StartDate, 
                                    EndDate = @EndDate, 
                                    TotalCost = @TotalCost, 
                                    Status = @Status 
                                    WHERE ReservationId = @ReservationId";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ReservationId", reservationData.ReservationId);
                            command.Parameters.AddWithValue("@CustomerId", reservationData.CustomerId);
                            command.Parameters.AddWithValue("@VehicleId", reservationData.VehicleId);
                            command.Parameters.AddWithValue("@StartDate", reservationData.StartDate);
                            command.Parameters.AddWithValue("@EndDate", reservationData.EndDate);
                            command.Parameters.AddWithValue("@TotalCost", reservationData.TotalCost);
                            command.Parameters.AddWithValue("@Status", reservationData.Status);

                            int rowsAffected = command.ExecuteNonQuery();
                            return rowsAffected > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error updating reservation: " + ex.Message);
                }
            }

            public bool CancelReservation(int reservationId)
            {
                try
                {
                    // First get the reservation to find the vehicle
                    Reservation reservation = GetReservationById(reservationId);

                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = "DELETE FROM Reservation WHERE ReservationId = @ReservationId";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ReservationId", reservationId);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Make the vehicle available again
                                Vehicle vehicle = vehicleService.GetVehicleById(reservation.VehicleId);
                                vehicle.Availability = true;
                                vehicleService.UpdateVehicle(vehicle);
                                return true;
                            }
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error canceling reservation: " + ex.Message);
                }
            }
        }
    }