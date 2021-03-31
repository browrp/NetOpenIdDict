using System;
using System.Threading.Tasks;

namespace XamarinJwtAuth.TokenManagement
{
    public interface ITokenManager
    {
        Task<LoginResult> Login(string username, string password, bool rememberMe = true);
    }
}
