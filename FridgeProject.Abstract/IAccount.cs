using FridgeProject.Abstract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeProject.Abstract
{
    public interface IAccount
    {
        public Task<AuthorizationInfo> LogIn(LogInInfo logIn);
        public Task LogOut();
    }
}
