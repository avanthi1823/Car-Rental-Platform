using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;


public class Customer
{
    public int Id;
    public string Username;
    public string Password;
    public string Email;
    public string Phone;
}

public class Vehicle
{
    public int Id;
    public string Model;
    public string Brand;
    public bool IsAvailable;
}

// Services
public class CustomerService
{
    private List<Customer> customers = new List<Customer>
    {
        new Customer
        {
            Id = 1,
            Username = "arunk",
            Password = "Arun@123",
            Email = "arun.kumar@gmail.com",
            Phone = "9876543210"
        }
    };

    public bool Authenticate(string username, string password)
    {
        return customers.Any(c => c.Username == username && c.Password == password);
    }

    public bool UpdateCustomerInfo(int id, string newEmail, string newPhone)
    {
        var customer = customers.FirstOrDefault(c => c.Id == id);
        if (customer == null) return false;

        customer.Email = newEmail;
        customer.Phone = newPhone;
        return true;
    }
}

public class VehicleService
{
    private List<Vehicle> vehicles = new List<Vehicle>
    {
        new Vehicle { Id = 1, Model = "Swift", Brand = "Maruti", IsAvailable = true },
        new Vehicle { Id = 2, Model = "i20", Brand = "Hyundai", IsAvailable = true }
    };

    public bool AddVehicle(Vehicle vehicle)
    {
        vehicles.Add(vehicle);
        return true;
    }

    public bool UpdateVehicle(int id, string newModel, string newBrand, bool newAvailability)
    {
        var vehicle = vehicles.FirstOrDefault(v => v.Id == id);
        if (vehicle == null) return false;

        vehicle.Model = newModel;
        vehicle.Brand = newBrand;
        vehicle.IsAvailable = newAvailability;
        return true;
    }

    public List<Vehicle> GetAvailableVehicles()
    {
        return vehicles.Where(v => v.IsAvailable).ToList();
    }

    public List<Vehicle> GetAllVehicles()
    {
        return vehicles;
    }
}

// Test Class
[TestFixture]
public class CarRentalSystemTests
{
    private CustomerService customerService;
    private VehicleService vehicleService;

    [SetUp]
    public void Setup()
    {
        customerService = new CustomerService();
        vehicleService = new VehicleService();
    }

    [Test]
    public void Test_CustomerAuthentication_InvalidCredentials()
    {
        bool result = customerService.Authenticate("invalidUser", "wrongPass");
        Assert.IsFalse(result);
    }

    [Test]
    public void Test_UpdateCustomerInformation_Success()
    {
        bool result = customerService.UpdateCustomerInfo(1, "newemail@example.com", "9876543210");
        Assert.IsTrue(result);
    }

    [Test]
    public void Test_AddNewVehicle_Success()
    {
        var vehicle = new Vehicle { Id = 3, Model = "City", Brand = "Honda", IsAvailable = false };
        bool result = vehicleService.AddVehicle(vehicle);
        Assert.IsTrue(result);
    }

    [Test]
    public void Test_UpdateVehicleDetails_Success()
    {
        bool result = vehicleService.UpdateVehicle(1, "Swift Dzire", "Maruti", false);
        Assert.IsTrue(result);
    }

    [Test]
    public void Test_GetAvailableVehicles_ReturnsOnlyAvailable()
    {
        var availableVehicles = vehicleService.GetAvailableVehicles();
        Assert.IsTrue(availableVehicles.All(v => v.IsAvailable));
    }

    [Test]
    public void Test_GetAllVehicles_ReturnsAll()
    {
        var allVehicles = vehicleService.GetAllVehicles();
        Assert.IsTrue(allVehicles.Count >= 2);
    }
}