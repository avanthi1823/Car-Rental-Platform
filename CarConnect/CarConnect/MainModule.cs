using CarConnect.dao;
using CarConnect.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace CarConnect
{
    public class MainModule
    {
        private ICustomerService customerService;
        private IVehicleService vehicleService;
        private IReservationService reservationService;
        private IAdminService adminService;
        private AuthenticationService authService;
        private ReportGenerator reportGenerator;

        public MainModule()
        {
            string connectionString = DBPropertyUtil.GetConnectionString();

            customerService = new CustomerService(connectionString);
            vehicleService = new VehicleService(connectionString);
            reservationService = new ReservationService(connectionString);
            adminService = new AdminService(connectionString);

            authService = new AuthenticationService(customerService, adminService);
            reportGenerator = new ReportGenerator(connectionString);

            Console.WriteLine("Welcome to CarConnect Console Interface!");
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Welcome to CarConnect ===");
                Console.WriteLine("1. Customer Login");
                Console.WriteLine("2. Admin Login");
                Console.WriteLine("3. Register New Customer");
                Console.WriteLine("4. View All Customers ");
                Console.WriteLine("5. View All Vehicles ");
                Console.WriteLine("6. Exit");
                Console.Write("\nEnter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": CustomerLogin(); break;
                    case "2": AdminLogin(); break;
                    case "3": RegisterCustomer(); break;
                    case "4": ViewAllCustomers(); break;
                    case "5": ViewAllVehicles(); break;
                    case "6":
                        Console.WriteLine("\nThank you for using CarConnect. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("\nInvalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void CustomerLogin()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Customer Login ===");
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();

                Customer customer = authService.AuthenticateCustomer(username, password);
                if (customer != null)
                {
                    Console.WriteLine($"\nLogin successful! Welcome, {customer.FirstName}.");
                    Console.ReadKey();
                    ShowCustomerMenu(customer);
                }
                else
                {
                    Console.WriteLine("\nInvalid username or password.");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                Console.ReadKey();
            }
        }

        private void AdminLogin()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Admin Login ===");
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();

                Admin admin = authService.AuthenticateAdmin(username, password);
                if (admin != null)
                {
                    Console.WriteLine($"\nLogin successful! Welcome, {admin.FirstName}.");
                    Console.ReadKey();
                    ShowAdminMenu(admin);
                }
                else
                {
                    Console.WriteLine("\nInvalid username or password.");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                Console.ReadKey();
            }
        }

        private void RegisterCustomer()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Register New Customer ===\n");

                Customer newCustomer = new Customer();
                Console.Write("CustomerId: ");
                newCustomer.CustomerId = Convert.ToInt32(Console.ReadLine());
                Console.Write("First Name: ");
                newCustomer.FirstName = Console.ReadLine();
                Console.Write("Last Name: ");
                newCustomer.LastName = Console.ReadLine();
                Console.Write("Email: ");
                newCustomer.Email = Console.ReadLine();
                Console.Write("Phone Number: ");
                newCustomer.PhoneNumber = Console.ReadLine();
                Console.Write("Address: ");
                newCustomer.Address = Console.ReadLine();
                Console.Write("Username: ");
                newCustomer.Username = Console.ReadLine();
                Console.Write("Password: ");
                newCustomer.Password = Console.ReadLine();
                newCustomer.RegistrationDate = DateTime.Now;

                if (customerService.RegisterCustomer(newCustomer))
                    Console.WriteLine("\nRegistration successful!");
                else
                    Console.WriteLine("\nRegistration failed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ViewAllCustomers()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== All Customers ===\n");

                List<Customer> customers = customerService.GetAllCustomers().ToList();

                if (customers.Count == 0)
                {
                    Console.WriteLine("No customers found.");
                }
                else
                {
                    Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-25} {4,-15}",
                        "ID", "First Name", "Last Name", "Email", "Phone");
                    Console.WriteLine(new string('-', 75));

                    foreach (var customer in customers)
                    {
                        Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-25} {4,-15}",
                            customer.CustomerId, customer.FirstName,
                            customer.LastName, customer.Email, customer.PhoneNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ViewAllVehicles()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== All Vehicles ===\n");

                List<Vehicle> vehicles = vehicleService.GetAllVehicles().ToList();

                if (vehicles.Count == 0)
                {
                    Console.WriteLine("No vehicles found.");
                }
                else
                {
                    // Header
                    Console.WriteLine("{0,-5} {1,-10} {2,-10} {3,-6} {4,-10} {5,-12} {6,-10}",
                        "ID", "Make", "Model", "Year", "Color", "Daily Rate", "Available");
                    Console.WriteLine(new string('=', 75));

                    // Data rows
                    foreach (var vehicle in vehicles)
                    {
                        Console.WriteLine("{0,-5} {1,-10} {2,-10} {3,-6} {4,-8} {5,-10} {6,-7}",
                            vehicle.VehicleId, vehicle.Make, vehicle.Model,
                            vehicle.Year, vehicle.Color, vehicle.DailyRate,vehicle.Availability);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ShowCustomerMenu(Customer customer)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== Customer Dashboard ===\nWelcome, {customer.FirstName} {customer.LastName}\n");
                Console.WriteLine("1. View Available Vehicles");
                Console.WriteLine("2. Make Reservation");
                Console.WriteLine("3. View My Reservations");
                Console.WriteLine("4. Update Profile");
                Console.WriteLine("5. Logout");
                Console.Write("\nEnter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ViewAvailableVehicles(); break;
                    case "2": MakeReservation(customer); break;
                    case "3": ViewCustomerReservations(customer.CustomerId); break;
                    case "4": UpdateCustomerProfile(customer); break;
                    case "5":
                        Console.WriteLine("\nLogging out... Goodbye!");
                        Console.ReadKey();
                        return;
                    default:
                        Console.WriteLine("\nInvalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void ViewAvailableVehicles()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Available Vehicles ===\n");

                List<Vehicle> vehicles = vehicleService.GetAvailableVehicles();

                if (vehicles.Count == 0)
                {
                    Console.WriteLine("No available vehicles found.");
                }
                else
                {
                    Console.WriteLine("{0,-5} {1,-10} {2,-10} {3,-6} {4,-8} {5,-10}",
                        "ID", "Make", "Model", "Year", "Color", "Daily Rate");
                    Console.WriteLine(new string('-', 50));

                    foreach (var vehicle in vehicles)
                    {
                        Console.WriteLine("{0,-5} {1,-10} {2,-10} {3,-6} {4,-8} {5,-10}",
                            vehicle.VehicleId, vehicle.Make, vehicle.Model,
                            vehicle.Year, vehicle.Color, vehicle.DailyRate);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void MakeReservation(Customer customer)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Make Reservation ===\n");
                ViewAvailableVehicles();

                Console.Write("\nEnter Vehicle ID: ");
                if (!int.TryParse(Console.ReadLine(), out int vehicleId))
                {
                    Console.WriteLine("Invalid Vehicle ID.");
                    Console.ReadKey();
                    return;
                }

                Console.Write("\nEnter Reservation ID: ");
                if (!int.TryParse(Console.ReadLine(), out int reservationId))
                {
                    Console.WriteLine("Invalid Reservation ID.");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Start Date (YYYY-MM-DD): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                {
                    Console.WriteLine("Invalid start date.");
                    Console.ReadKey();
                    return;
                }

                Console.Write("End Date (YYYY-MM-DD): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                {
                    Console.WriteLine("Invalid end date.");
                    Console.ReadKey();
                    return;
                }

                Reservation newReservation = new Reservation
                {
                    CustomerId = customer.CustomerId,
                    ReservationId = reservationId, // Fixed: Use the local variable instead of Reservation.ReservationId
                    VehicleId = vehicleId,
                    StartDate = startDate,
                    EndDate = endDate,
                    Status = "Pending"
                };

                if (reservationService.CreateReservation(newReservation))
                    Console.WriteLine("\nReservation created successfully!");
                else
                    Console.WriteLine("\nFailed to create reservation.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ViewCustomerReservations(int customerId)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Your Reservations ===\n");

                List<Reservation> reservations = reservationService.GetReservationsByCustomerId(customerId);
                List<Vehicle> vehicles = vehicleService.GetAllVehicles();

                if (reservations.Count == 0)
                {
                    Console.WriteLine("No reservations found.");
                }
                else
                {
                    Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-15} {4,-15}",
                        "Reservation ID", "Vehicle", "Start Date", "End Date", "Status");
                    Console.WriteLine(new string('-', 75));

                    foreach (var reservation in reservations)
                    {
                        Vehicle vehicle = vehicles.FirstOrDefault(v => v.VehicleId == reservation.VehicleId);
                        string vehicleName = vehicle != null ? $"{vehicle.Make} {vehicle.Model}" : "N/A";

                        Console.WriteLine("{0,-15} {1,-15} {2,-15:d} {3,-15:d} {4,-15}",
                            reservation.ReservationId, vehicleName,
                            reservation.StartDate, reservation.EndDate, reservation.Status);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void UpdateCustomerProfile(Customer customer)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Update Profile ===\n");

                Console.WriteLine($"Current First Name: {customer.FirstName}");
                Console.Write("New First Name (press Enter to keep current): ");
                string firstName = Console.ReadLine();
                if (!string.IsNullOrEmpty(firstName)) customer.FirstName = firstName;

                Console.WriteLine($"\nCurrent Last Name: {customer.LastName}");
                Console.Write("New Last Name (press Enter to keep current): ");
                string lastName = Console.ReadLine();
                if (!string.IsNullOrEmpty(lastName)) customer.LastName = lastName;

                Console.WriteLine($"\nCurrent Email: {customer.Email}");
                Console.Write("New Email (press Enter to keep current): ");
                string email = Console.ReadLine();
                if (!string.IsNullOrEmpty(email)) customer.Email = email;

                Console.Write("\nNew Password (press Enter to keep current): ");
                string password = Console.ReadLine();
                if (!string.IsNullOrEmpty(password)) customer.Password = password;

                if (customerService.UpdateCustomer(customer))
                    Console.WriteLine("\nProfile updated successfully!");
                else
                    Console.WriteLine("\nFailed to update profile.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ShowAdminMenu(Admin admin)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== Admin Dashboard ===\nWelcome, {admin.FirstName} {admin.LastName} ({admin.Role})\n");
                Console.WriteLine("1. Manage Customers");
                Console.WriteLine("2. Manage Vehicles");
                Console.WriteLine("3. View All Reservations");
                Console.WriteLine("4. Generate Reports");
                Console.WriteLine("5. Logout");
                Console.Write("\nEnter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ManageCustomers(); break;
                    case "2": ManageVehicles(); break;
                    case "3": ViewAllReservations(); break;
                    case "4": GenerateReports(); break;
                    case "5":
                        Console.WriteLine("\nLogging out... Goodbye!");
                        Console.ReadKey();
                        return;
                    default:
                        Console.WriteLine("\nInvalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }



        private void ManageCustomers()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Customer Management ===");
                Console.WriteLine("1. View All Customers");
                Console.WriteLine("2. Update Customer");
                Console.WriteLine("3. Delete Customer");
                Console.WriteLine("4. Back to Admin Menu");
                Console.Write("\nEnter your choice: ");

                switch (Console.ReadLine())
                {
                    case "1": ViewAllCustomers(); break;
                    case "2": UpdateCustomerAdmin(); break;
                    case "3": DeleteCustomer(); break;
                    case "4": return;
                    default:
                        Console.WriteLine("\nInvalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void UpdateCustomerAdmin()
        {
            try
            {
                Console.Clear();
                ViewAllCustomers();
                Console.Write("\nEnter Customer ID to update: ");
                int customerId = int.Parse(Console.ReadLine());

                Customer customer = customerService.GetCustomerById(customerId);

                Console.WriteLine($"\nCurrent First Name: {customer.FirstName}");
                Console.Write("New First Name: ");
                customer.FirstName = Console.ReadLine();

                Console.WriteLine($"\nCurrent Last Name: {customer.LastName}");
                Console.Write("New Last Name: ");
                customer.LastName = Console.ReadLine();

                if (customerService.UpdateCustomer(customer))
                    Console.WriteLine("\nCustomer updated successfully!");
                else
                    Console.WriteLine("\nFailed to update customer.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void DeleteCustomer()
        {
            try
            {
                Console.Clear();
                ViewAllCustomers();
                Console.Write("\nEnter Customer ID to delete: ");
                int customerId = int.Parse(Console.ReadLine());

                Console.Write("\nAre you sure? (Y/N): ");
                if (Console.ReadLine().ToUpper() == "Y")
                {
                    if (customerService.DeleteCustomer(customerId))
                        Console.WriteLine("\nCustomer deleted successfully!");
                    else
                        Console.WriteLine("\nFailed to delete customer.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ManageVehicles()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Vehicle Management ===");
                Console.WriteLine("1. View All Vehicles");
                Console.WriteLine("2. Add New Vehicle");
                Console.WriteLine("3. Update Vehicle");
                Console.WriteLine("4. Delete Vehicle");
                Console.WriteLine("5. Back to Admin Menu");
                Console.Write("\nEnter your choice: ");

                switch (Console.ReadLine())
                {
                    case "1": ViewAllVehicles(); break;
                    case "2": AddNewVehicle(); break;
                    case "3": UpdateVehicleAdmin(); break;
                    case "4": DeleteVehicle(); break;
                    case "5": return;
                    default:
                        Console.WriteLine("\nInvalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void AddNewVehicle()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Add New Vehicle ===\n");

                Vehicle newVehicle = new Vehicle();
                Console.Write("Vehicle ID (1-5): ");
                newVehicle.VehicleId = int.Parse(Console.ReadLine());
                Console.Write("Make: ");
                newVehicle.Make = Console.ReadLine();
                Console.Write("Model: ");
                newVehicle.Model = Console.ReadLine();
                Console.Write("Year: ");
                newVehicle.Year = int.Parse(Console.ReadLine());
                Console.Write("Color: ");
                newVehicle.Color = Console.ReadLine();
                Console.Write("Registration Number: ");
                newVehicle.RegistrationNumber = Console.ReadLine();
                newVehicle.Availability = true;
                Console.Write("Daily Rate: ");
                newVehicle.DailyRate = decimal.Parse(Console.ReadLine());

                if (vehicleService.AddVehicle(newVehicle))
                    Console.WriteLine("\nVehicle added successfully!");
                else
                    Console.WriteLine("\nFailed to add vehicle.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void UpdateVehicleAdmin()
        {
            try
            {
                Console.Clear();
                ViewAllVehicles();
                Console.Write("\nEnter Vehicle ID to update: ");
                int vehicleId = int.Parse(Console.ReadLine());

                Vehicle vehicle = vehicleService.GetVehicleById(vehicleId);

                Console.WriteLine($"\nCurrent Make: {vehicle.Make}");
                Console.Write("New Make: ");
                vehicle.Make = Console.ReadLine();

                Console.WriteLine($"\nCurrent Model: {vehicle.Model}");
                Console.Write("New Model: ");
                vehicle.Model = Console.ReadLine();

                Console.WriteLine($"\nCurrent Daily Rate: {vehicle.DailyRate}");
                Console.Write("New Daily Rate: ");
                vehicle.DailyRate = decimal.Parse(Console.ReadLine());

                Console.WriteLine($"\nCurrent Availability: {(vehicle.Availability ? "Available" : "Not Available")}");
                Console.Write("Update Availability? (Y/N): ");
                if (Console.ReadLine().ToUpper() == "Y")
                {
                    vehicle.Availability = !vehicle.Availability;
                }

                if (vehicleService.UpdateVehicle(vehicle))
                    Console.WriteLine("\nVehicle updated successfully!");
                else
                    Console.WriteLine("\nFailed to update vehicle.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void DeleteVehicle()
        {
            try
            {
                Console.Clear();
                ViewAllVehicles();
                Console.Write("\nEnter Vehicle ID to delete: ");
                int vehicleId = int.Parse(Console.ReadLine());

                Console.Write("\nAre you sure? (Y/N): ");
                if (Console.ReadLine().ToUpper() == "Y")
                {
                    if (vehicleService.RemoveVehicle(vehicleId))
                        Console.WriteLine("\nVehicle deleted successfully!");
                    else
                        Console.WriteLine("\nFailed to delete vehicle.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ViewAllReservations()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== All Reservations ===\n");

                List<Reservation> reservations = reservationService.GetAllReservations();
                List<Customer> customers = customerService.GetAllCustomers();
                List<Vehicle> vehicles = vehicleService.GetAllVehicles();

                Console.WriteLine("{0,-15} {1,-20} {2,-15} {3,-15} {4,-15} {5,-10}",
                    "Reservation ID", "Customer", "Vehicle", "Start Date", "End Date", "Status");
                Console.WriteLine(new string('-', 90));

                foreach (var reservation in reservations)
                {
                    Customer customer = customers.FirstOrDefault(c => c.CustomerId == reservation.CustomerId);
                    Vehicle vehicle = vehicles.FirstOrDefault(v => v.VehicleId == reservation.VehicleId);

                    string customerName = customer != null ? $"{customer.FirstName} {customer.LastName}" : "N/A";
                    string vehicleName = vehicle != null ? $"{vehicle.Make} {vehicle.Model}" : "N/A";

                    Console.WriteLine("{0,-15} {1,-20} {2,-15} {3,-15:d} {4,-15:d} {5,-10}",
                        reservation.ReservationId, customerName, vehicleName,
                        reservation.StartDate, reservation.EndDate, reservation.Status);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void GenerateReports()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Report Generation ===");
                Console.WriteLine("1. Reservation History Report");
                Console.WriteLine("2. Vehicle Utilization Report");
                Console.WriteLine("3. Customer Activity Report");
                Console.WriteLine("4. Back to Admin Menu");
                Console.Write("\nEnter your choice: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Enter start date (YYYY-MM-DD): ");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                            {
                                Console.WriteLine("Invalid start date.");
                                break;
                            }
                            Console.Write("Enter end date (YYYY-MM-DD): ");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                            {
                                Console.WriteLine("Invalid end date.");
                                break;
                            }
                            reportGenerator.GenerateReservationHistoryReport(startDate, endDate);
                            break;
                        case "2":
                            reportGenerator.GenerateVehicleUtilizationReport();
                            break;
                        case "3":
                            reportGenerator.GenerateCustomerActivityReport();
                            break;
                        case "4":
                            return;
                        default:
                            Console.WriteLine("\nInvalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError generating report: {ex.Message}");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }
}