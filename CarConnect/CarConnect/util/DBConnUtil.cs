using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace CarConnect
{
 
        public static class DBConnUtil
        {
            public static SqlConnection GetConnection(string connectionString)
            {
                try
                {
                    SqlConnection connection = new SqlConnection(connectionString);
                    connection.Open();
                    return connection;
                }
                catch (Exception ex)
                {
                    throw new DatabaseConnectionException("Error establishing database connection: " + ex.Message);
                }
            }
        }
    }
