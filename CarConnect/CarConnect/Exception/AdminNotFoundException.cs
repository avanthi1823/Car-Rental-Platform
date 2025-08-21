using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarConnect
{
   
        public class AdminNotFoundException : Exception
        {
            public AdminNotFoundException()
                : base("Admin not found with the specified criteria.")
            {
            }

            public AdminNotFoundException(string message)
                : base(message)
            {
            }

            public AdminNotFoundException(string message, Exception innerException)
                : base(message, innerException)
            {
            }
        }
    }