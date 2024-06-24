using ExpressVoitures.ViewModels;
using System.Threading.Tasks;

namespace ExpressVoitures.Services
{
    public interface IAccountService
    {
        Task<bool> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
    }
}
