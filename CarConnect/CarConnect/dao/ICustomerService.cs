using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarConnect
{
   
        public interface ICustomerService
        {
            Customer GetCustomerById(int customerId);
            Customer GetCustomerByUsername(string username);
            List<Customer> GetAllCustomers();
            bool RegisterCustomer(Customer customerData);
            bool UpdateCustomer(Customer customerData);
            bool DeleteCustomer(int customerId);
        }
    }