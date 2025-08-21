using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarConnect
{
    
        public class Admin
        {
            public int AdminId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
            public DateTime JoinDate { get; set; }

            public Admin() { }

            public bool Authenticate(string password)
            {
                return Password == password;
            }
        }
    }
