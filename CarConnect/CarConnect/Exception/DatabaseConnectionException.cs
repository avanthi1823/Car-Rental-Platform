using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarConnect
{
    
        public class DatabaseConnectionException : Exception
        {
            public DatabaseConnectionException() : base("Database connection error occurred.") { }
            public DatabaseConnectionException(string message) : base(message) { }
        }
    }