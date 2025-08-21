using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarConnect
{
   
        public class Reservation
        {
            public int ReservationId { get; set; }
            public int CustomerId { get; set; }
            public int VehicleId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal TotalCost { get; set; }
            public string Status { get; set; }

            public Reservation() { }

        public decimal CalculateTotalCost(int days, decimal dailyRate)
        {
            return dailyRate * days;
        }
    }
    }