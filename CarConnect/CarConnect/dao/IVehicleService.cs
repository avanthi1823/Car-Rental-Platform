using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarConnect
{
  
        public interface IVehicleService
        {
            Vehicle GetVehicleById(int vehicleId);
            List<Vehicle> GetAvailableVehicles();
            List<Vehicle> GetAllVehicles();
            bool AddVehicle(Vehicle vehicleData);
            bool UpdateVehicle(Vehicle vehicleData);
            bool RemoveVehicle(int vehicleId);
        }
    }
