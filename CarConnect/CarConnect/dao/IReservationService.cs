using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarConnect
{
    
        public interface IReservationService
        {
            Reservation GetReservationById(int reservationId);
            List<Reservation> GetReservationsByCustomerId(int customerId);
            List<Reservation> GetAllReservations();
            bool CreateReservation(Reservation reservationData);
            bool UpdateReservation(Reservation reservationData);
            bool CancelReservation(int reservationId);
        }
    }