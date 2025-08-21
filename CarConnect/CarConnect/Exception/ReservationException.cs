using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarConnect
{
   
        public class ReservationException : Exception
        {
            public ReservationException()
                : base("An error occurred during reservation processing.")
            {
            }

            public ReservationException(string message)
                : base(message)
            {
            }

            public ReservationException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public ReservationException(int vehicleId)
                : base($"Vehicle with ID {vehicleId} is not available for reservation.")
            {
            }
        }
    }