using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarConnect
{
    
    
        public class AuthenticationService
        {
            private readonly ICustomerService _customerService;
            private readonly IAdminService _adminService;

            public AuthenticationService(ICustomerService customerService, IAdminService adminService)
            {
                _customerService = customerService;
                _adminService = adminService;
            }

            public Customer AuthenticateCustomer(string username, string password)
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    throw new AuthenticationException("Username and password are required.");
                }

                Customer customer = _customerService.GetCustomerByUsername(username);

                if (customer == null)
                {
                    throw new AuthenticationException("Invalid username or password.");
                }

                if (customer.Password != password) // In real applications, use password hashing
                {
                    throw new AuthenticationException("Invalid username or password.");
                }

                return customer;
            }

            public Admin AuthenticateAdmin(string username, string password)
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    throw new AuthenticationException("Username and password are required.");
                }

                Admin admin = _adminService.GetAdminByUsername(username);

                if (admin == null)
                {
                    throw new AuthenticationException("Invalid username or password.");
                }

                if (admin.Password != password) // In real applications, use password hashing
                {
                    throw new AuthenticationException("Invalid username or password.");
                }

                return admin;
            }
        }
    }