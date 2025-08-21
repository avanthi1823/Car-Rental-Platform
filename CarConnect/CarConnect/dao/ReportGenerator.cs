using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::CarConnect.dao;

   

    namespace CarConnect.util
    {
        public class ReportGenerator
        {
            private IReservationService reservationService;
            private IVehicleService vehicleService;
            private ICustomerService customerService;

        public string ConnectionString { get; }

        public ReportGenerator()
            {
                reservationService = new ReservationService();
                vehicleService = new VehicleService();
                customerService = new CustomerService();
            }

        public ReportGenerator(string connectionString)
        {
            ConnectionString = connectionString;
            reservationService = new ReservationService(connectionString);
            vehicleService = new VehicleService(connectionString);
            customerService = new CustomerService(connectionString);
        }


        public void GenerateReservationHistoryReport(DateTime startDate, DateTime endDate)
            {
                try
                {
                    var reservations = reservationService.GetAllReservations()
                        .Where(r => r.StartDate >= startDate && r.EndDate <= endDate)
                        .OrderBy(r => r.StartDate)
                        .ToList();

                    var vehicles = vehicleService.GetAllVehicles();
                    var customers = customerService.GetAllCustomers();

                    Console.WriteLine("\n=== Reservation History Report ===");
                    Console.WriteLine($"Period: {startDate:d} to {endDate:d}");
                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine("{0,-12} {1,-20} {2,-15} {3,-10} {4,-10} {5,-10}",
                        "Reservation", "Customer", "Vehicle", "Start Date", "End Date", "Total");
                    Console.WriteLine("------------------------------------------------------------");

                    decimal totalRevenue = 0;
                    foreach (var reservation in reservations)
                    {
                        var customer = customers.FirstOrDefault(c => c.CustomerId == reservation.CustomerId);
                        var vehicle = vehicles.FirstOrDefault(v => v.VehicleId == reservation.VehicleId);

                        string customerName = customer != null ? $"{customer.FirstName} {customer.LastName}" : "N/A";
                        string vehicleInfo = vehicle != null ? $"{vehicle.Make} {vehicle.Model}" : "N/A";

                        Console.WriteLine("{0,-12} {1,-20} {2,-15} {3,-10:d} {4,-10:d} {5,-10}",
                            reservation.ReservationId,
                            customerName,
                            vehicleInfo,
                            reservation.StartDate,
                            reservation.EndDate,
                            reservation.TotalCost);

                        totalRevenue += reservation.TotalCost;
                    }

                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine($"Total Reservations: {reservations.Count}");
                    Console.WriteLine($"Total Revenue: {totalRevenue}");
                    Console.WriteLine($"Average Revenue per Reservation: {totalRevenue / (reservations.Count > 0 ? reservations.Count : 1)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error generating report: {ex.Message}");
                }
            }

        public void GenerateVehicleUtilizationReport()
        {
            try
            {
                var vehicles = vehicleService.GetAllVehicles();
                var reservations = reservationService.GetAllReservations();

                Console.WriteLine("\n=== Vehicle Utilization Report ===");
                Console.WriteLine("--------------------------------------------------------------------------");
                Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15}",
                    "Vehicle ID", "Make/Model", "Days Rented", "Utilization %", "Revenue Generated");
                Console.WriteLine("--------------------------------------------------------------------------");

                // Use earliest and latest reservation dates to calculate the full period
                DateTime? earliestStart = reservations.Min(r => (DateTime?)r.StartDate);
                DateTime? latestEnd = reservations.Max(r => (DateTime?)r.EndDate);

                // Default to 1 day if no reservations
                int totalDaysInPeriod = (earliestStart != null && latestEnd != null)
                    ? (latestEnd.Value - earliestStart.Value).Days
                    : 1;

                foreach (var vehicle in vehicles)
                {
                    var vehicleReservations = reservations
                        .Where(r => r.VehicleId == vehicle.VehicleId)
                        .ToList();

                    int daysRented = vehicleReservations.Sum(r =>
                        Math.Max((r.EndDate - r.StartDate).Days, 1)); // Avoid zero-day rentals

                    decimal utilizationRate = (decimal)daysRented / totalDaysInPeriod * 100;
                    decimal revenue = vehicleReservations.Sum(r => r.TotalCost);

                    Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15:P0} {4,-15}",
                        vehicle.VehicleId,
                        $"{vehicle.Make} {vehicle.Model}",
                        daysRented,
                        utilizationRate / 100, // because :P0 expects 0.00–1.00 range
                        revenue);
                }

                Console.WriteLine("--------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }
        public void GenerateCustomerActivityReport()
            {
                try
                {
                    var customers = customerService.GetAllCustomers();
                    var reservations = reservationService.GetAllReservations();

                    Console.WriteLine("\n=== Customer Activity Report ===");
                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine("{0,-15} {1,-20} {2,-15} {3,-15}",
                        "Customer ID", "Name", "Reservations", "Total Spent");
                    Console.WriteLine("-------------------------------------------------------");

                    foreach (var customer in customers)
                    {
                        var customerReservations = reservations
                            .Where(r => r.CustomerId == customer.CustomerId)
                            .ToList();

                        decimal totalSpent = customerReservations.Sum(r => r.TotalCost);

                        Console.WriteLine("{0,-15} {1,-20} {2,-15} {3,-15}",
                            customer.CustomerId,
                            $"{customer.FirstName} {customer.LastName}",
                            customerReservations.Count,
                            totalSpent);
                    }

                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine($"Total Customers: {customers.Count}");
                    Console.WriteLine($"Total Active Customers: {customers.Count(c => reservations.Any(r => r.CustomerId == c.CustomerId))}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error generating report: {ex.Message}");
                }
            }
        }
    }