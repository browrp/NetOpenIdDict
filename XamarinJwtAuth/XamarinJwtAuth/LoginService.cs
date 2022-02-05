using System;
using System.Security.Cryptography;
using System.Text;

namespace XamarinJwtAuth
{
    public class LoginService
    {
        private string codeVerifier;
        private const string IDToken = "id_token";
        private const string CodeChallengeMethod = "S256";

        public string BuildAuthenticationUrl()
        {
            var state = CreateCryptoGuid();
            var nonce = CreateCryptoGuid();
            var codeChallenge = CreateCodeChallenge();
            
            return $"{Constants.AuthorizeUri}?response_type={IDToken}&scope=profile&redirect_uri=xamarinWebAuth%3A%2F%2F&client_id={Constants.ClientId}&code_challenge{codeChallenge}&code_challenge_method={CodeChallengeMethod}&nonce={nonce}";
        }

        private string CreateCryptoGuid()
        {
            using (var generator = RandomNumberGenerator.Create())
            {
                var bytes = new byte[16];
                generator.GetBytes(bytes);
                return new Guid(bytes).ToString("N");
            }
        }

        private string CreateCodeChallenge()
        {
            codeVerifier = CreateCryptoGuid();
            using (var sha256 = SHA256.Create())
            {
                var codeChallengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
                return Convert.ToBase64String(codeChallengeBytes);
            }
        }

        //public JwtSecurityToken ParseAuthenticationResult(WebAuthenticatorResultauthenticationResult)
        //{
        //    var handler = new JwtSecurityTokenHandler();
        //    var token = handler.ReadJwtToken(authenticationResult.IdToken);
        //    return token;
        //}
    }
}
