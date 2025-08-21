﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarConnect
{

        public class CustomerNotFoundException : Exception
        {
            public CustomerNotFoundException() : base("Customer not found.") { }
            public CustomerNotFoundException(string message) : base(message) { }
        }
    }