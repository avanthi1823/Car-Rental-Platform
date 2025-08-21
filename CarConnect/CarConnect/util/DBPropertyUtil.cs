using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CarConnect
{

    
        public static class DBPropertyUtil
        {
        public static string GetConnectionString()
        {
            return "Data Source=DESKTOP-LQ66VHC;Initial Catalog=CarconnectRentDB;Integrated Security=True;TrustServerCertificate=True";
        }
    }
    }
