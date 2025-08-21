using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarConnect
{
    
        public class AuthenticationException : Exception
        {
            public AuthenticationException() : base("Authentication failed. Invalid username or password.") { }
            public AuthenticationException(string message) : base(message) { }
        }
    }