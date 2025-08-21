using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarConnect
{
    
        public interface IAdminService
        {
            Admin GetAdminById(int adminId);
            Admin GetAdminByUsername(string username);
            List<Admin> GetAllAdmins();
            bool RegisterAdmin(Admin adminData);
            bool UpdateAdmin(Admin adminData);
            bool DeleteAdmin(int adminId);
        }
    }