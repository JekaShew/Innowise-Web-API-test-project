using FridgeProject.Abstract.Data;
using System.Threading.Tasks;

namespace FridgeProject.Abstract
{
    public interface IAccountServices
    {
        public Task<AuthorizationInfo> LogIn(LogInInfo logIn);
    }
}
