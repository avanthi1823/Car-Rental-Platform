using System;

using System.Collections.Generic;
using System.Data.SqlClient;

namespace CarConnect.dao
{
    public class CustomerService : ICustomerService
    {
        private readonly string connectionString;

        // Constructor accepting a connection string  
        public CustomerService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public CustomerService()
        {
        }

        public Customer GetCustomerById(int customerId)
        {
            try
            {
                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    string query = "SELECT  * FROM Customer WHERE CustomerId = @CustomerId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Customer
                                {
                                    CustomerId = (int)reader["CustomerId"],
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Username = reader["Username"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    RegistrationDate = (DateTime)reader["RegistrationDate"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseConnectionException("Error retrieving customer: " + ex.Message);
            }

            throw new CustomerNotFoundException();
        }

        public Customer GetCustomerByUsername(string username)
        {
            try
            {
                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    string query = "SELECT  * FROM Customer WHERE Username = @Username";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Customer
                                {
                                    CustomerId = (int)reader["CustomerId"],
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Username = reader["Username"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    RegistrationDate = (DateTime)reader["RegistrationDate"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseConnectionException("Error retrieving customer: " + ex.Message);
            }

            return null;
        }

        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            try
            {
                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    string query = "SELECT  * FROM Customer ORDER BY CustomerId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customers.Add(new Customer
                                {
                                    CustomerId = (int)reader["CustomerId"],
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Username = reader["Username"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    RegistrationDate = (DateTime)reader["RegistrationDate"]
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseConnectionException("Error retrieving customers: " + ex.Message);
            }

            return customers;
        }

        public bool RegisterCustomer(Customer customerData)
        {
            try
            {
                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    string query = @"INSERT INTO Customer 
                                    (CustomerId, FirstName, LastName, Email, PhoneNumber, Address, Username, Password, RegistrationDate) 
                                    VALUES (@CustomerId, @FirstName, @LastName, @Email, @PhoneNumber, @Address, @Username, @Password, @RegistrationDate)";

                    // Add this before using the RegistrationDate parameter
                    if (customerData.RegistrationDate < new DateTime(1753, 1, 1) || customerData.RegistrationDate > new DateTime(9999, 12, 31))
                    {
                        customerData.RegistrationDate = DateTime.Now;
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerData.CustomerId);
                        command.Parameters.AddWithValue("@FirstName", customerData.FirstName);
                        command.Parameters.AddWithValue("@LastName", customerData.LastName);
                        command.Parameters.AddWithValue("@Email", customerData.Email);
                        command.Parameters.AddWithValue("@PhoneNumber", customerData.PhoneNumber);
                        command.Parameters.AddWithValue("@Address", customerData.Address);
                        command.Parameters.AddWithValue("@Username", customerData.Username);
                        command.Parameters.AddWithValue("@Password", customerData.Password);
                        command.Parameters.AddWithValue("@RegistrationDate", customerData.RegistrationDate);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                throw new InvalidInputException("Username or Customer ID already exists.");
            }
            catch (Exception ex)
            {
                throw new DatabaseConnectionException("Error registering customer: " + ex.Message);
            }
        }

        public bool UpdateCustomer(Customer customerData)
        {
            try
            {
                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    string query = @"UPDATE Customer SET 
                                    FirstName = @FirstName, 
                                    LastName = @LastName, 
                                    Email = @Email, 
                                    PhoneNumber = @PhoneNumber, 
                                    Address = @Address, 
                                    Password = @Password 
                                    WHERE CustomerId = @CustomerId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerData.CustomerId);
                        command.Parameters.AddWithValue("@FirstName", customerData.FirstName);
                        command.Parameters.AddWithValue("@LastName", customerData.LastName);
                        command.Parameters.AddWithValue("@Email", customerData.Email);
                        command.Parameters.AddWithValue("@PhoneNumber", customerData.PhoneNumber);
                        command.Parameters.AddWithValue("@Address", customerData.Address);
                        command.Parameters.AddWithValue("@Password", customerData.Password);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseConnectionException("Error updating customer: " + ex.Message);
            }
        }

        public bool DeleteCustomer(int customerId)
        {
            try
            {
                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    string query = "DELETE FROM Customer WHERE CustomerId = @CustomerId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseConnectionException("Error deleting customer: " + ex.Message);
            }
        }
    }
}