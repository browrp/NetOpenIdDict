using System;
using System.Threading.Tasks;
using XamarinJwtAuth.Models;

namespace XamarinJwtAuth.Services
{
    public interface IIdentityService
    {
        string CreateAuthorizationRequest();
        Task<UserToken> GetTokenAsync(string code);
        Task<string> GetAsync(string uri, string accessToken);
    }
}
