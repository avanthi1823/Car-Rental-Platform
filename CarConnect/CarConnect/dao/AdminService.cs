using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace CarConnect
{
    
   

  
        public class AdminService : IAdminService
        {
		private readonly string connectionString;

		public AdminService(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public Admin GetAdminById(int adminId)
            {
                try
                {
                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = "SELECT  * FROM Admin WHERE AdminId = @AdminId";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@AdminId", adminId);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    return new Admin
                                    {
                                        AdminId = (int)reader["AdminId"],
                                        FirstName = reader["FirstName"].ToString(),
                                        LastName = reader["LastName"].ToString(),
                                        Email = reader["Email"].ToString(),
                                        PhoneNumber = reader["PhoneNumber"].ToString(),
                                        Username = reader["Username"].ToString(),
                                        Password = reader["Password"].ToString(),
                                        Role = reader["Role"].ToString(),
                                        JoinDate = (DateTime)reader["JoinDate"]
                                    };
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error retrieving admin: " + ex.Message);
                }

                throw new AdminNotFoundException();
            }

            public Admin GetAdminByUsername(string username)
            {
                try
                {
                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = "SELECT  * FROM Admin WHERE Username = @Username";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Username", username);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    return new Admin
                                    {
                                        AdminId = (int)reader["AdminId"],
                                        FirstName = reader["FirstName"].ToString(),
                                        LastName = reader["LastName"].ToString(),
                                        Email = reader["Email"].ToString(),
                                        PhoneNumber = reader["PhoneNumber"].ToString(),
                                        Username = reader["Username"].ToString(),
                                        Password = reader["Password"].ToString(),
                                        Role = reader["Role"].ToString(),
                                        JoinDate = (DateTime)reader["JoinDate"]
                                    };
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error retrieving admin: " + ex.Message);
                }

                return null;
            }

            public List<Admin> GetAllAdmins()
            {
                List<Admin> admins = new List<Admin>();
                try
                {
                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = "SELECT  * FROM Admin ORDER BY AdminId";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    admins.Add(new Admin
                                    {
                                        AdminId = (int)reader["AdminId"],
                                        FirstName = reader["FirstName"].ToString(),
                                        LastName = reader["LastName"].ToString(),
                                        Email = reader["Email"].ToString(),
                                        PhoneNumber = reader["PhoneNumber"].ToString(),
                                        Username = reader["Username"].ToString(),
                                        Password = reader["Password"].ToString(),
                                        Role = reader["Role"].ToString(),
                                        JoinDate = (DateTime)reader["JoinDate"]
                                    });
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error retrieving admins: " + ex.Message);
                }

                return admins;
            }

            public bool RegisterAdmin(Admin adminData)
            {
                try
                {
                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = @"INSERT INTO Admin 
                                    (AdminId, FirstName, LastName, Email, PhoneNumber, Username, Password, Role, JoinDate) 
                                    VALUES (@AdminId, @FirstName, @LastName, @Email, @PhoneNumber, @Username, @Password, @Role, @JoinDate)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@AdminId", adminData.AdminId);
                            command.Parameters.AddWithValue("@FirstName", adminData.FirstName);
                            command.Parameters.AddWithValue("@LastName", adminData.LastName);
                            command.Parameters.AddWithValue("@Email", adminData.Email);
                            command.Parameters.AddWithValue("@PhoneNumber", adminData.PhoneNumber);
                            command.Parameters.AddWithValue("@Username", adminData.Username);
                            command.Parameters.AddWithValue("@Password", adminData.Password);
                            command.Parameters.AddWithValue("@Role", adminData.Role);
                            command.Parameters.AddWithValue("@JoinDate", adminData.JoinDate);

                            int rowsAffected = command.ExecuteNonQuery();
                            return rowsAffected > 0;
                        }
                    }
                }
                catch (SqlException ex) when (ex.Number == 2627)
                {
                    throw new InvalidInputException("Admin ID or Username already exists.");
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error registering admin: " + ex.Message);
                }
            }

            public bool UpdateAdmin(Admin adminData)
            {
                try
                {
                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = @"UPDATE Admin SET 
                                    FirstName = @FirstName, 
                                    LastName = @LastName, 
                                    Email = @Email, 
                                    PhoneNumber = @PhoneNumber, 
                                    Password = @Password, 
                                    Role = @Role 
                                    WHERE AdminId = @AdminId";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@AdminId", adminData.AdminId);
                            command.Parameters.AddWithValue("@FirstName", adminData.FirstName);
                            command.Parameters.AddWithValue("@LastName", adminData.LastName);
                            command.Parameters.AddWithValue("@Email", adminData.Email);
                            command.Parameters.AddWithValue("@PhoneNumber", adminData.PhoneNumber);
                            command.Parameters.AddWithValue("@Password", adminData.Password);
                            command.Parameters.AddWithValue("@Role", adminData.Role);

                            int rowsAffected = command.ExecuteNonQuery();
                            return rowsAffected > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error updating admin: " + ex.Message);
                }
            }

            public bool DeleteAdmin(int adminId)
            {
                try
                {
                    using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                    {
                        string query = "DELETE FROM Admin WHERE AdminId = @AdminId";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@AdminId", adminId);

                            int rowsAffected = command.ExecuteNonQuery();
                            return rowsAffected > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error deleting admin: " + ex.Message);
                }
            }
        }
    }