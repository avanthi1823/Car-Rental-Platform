using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarConnect
{
   
        public class Vehicle
        {
            public int VehicleId { get; set; }
            public string Model { get; set; }
            public string Make { get; set; }
            public int Year { get; set; }
            public string Color { get; set; }
            public string RegistrationNumber { get; set; }
            public bool Availability { get; set; }
            public decimal DailyRate { get; set; }

            public Vehicle() { }
        }
    }