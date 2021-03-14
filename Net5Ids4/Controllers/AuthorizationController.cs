using System;
using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetOpenIdDict.Controllers
{
    public class AuthorizationController : Controller
    {
        /// <summary>
        /// One action is implemented, Exchange. This action is used by all flows, not only the Client Credentials Flow, to obtain an access token.
        /// n the case of the Client Credentials Flow, the token is issued based on the client credentials.
        /// In the case of Authorization Code Flow, the same endpoint is used but then to exchange an authorization code for a token.
        /// https://dev.to/robinvanderknaap/setting-up-an-authorization-server-with-openiddict-part-iii-client-credentials-flow-55lp
        /// </summary>
        /// <returns></returns>
        [HttpPost("~/connect/token")]
        public IActionResult Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            ClaimsPrincipal claimsPrincipal;

            if (request.IsClientCredentialsGrantType())
            {
                // Note: the client credentials are automatically validated by OpenIddict:
                // if client_id or client_secret are invalid, this action won't be invoked.

                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // Subject (sub) is a required field, we use the client id as the subject identifier here.
                identity.AddClaim(OpenIddictConstants.Claims.Subject, request.ClientId ?? throw new InvalidOperationException());

                // Add some claim, don't forget to add destination otherwise it won't be added to the access token.
                identity.AddClaim("some-claim", "some-value", OpenIddictConstants.Destinations.AccessToken);

                claimsPrincipal = new ClaimsPrincipal(identity);

                claimsPrincipal.SetScopes(request.GetScopes());
            }

            else
            {
                throw new InvalidOperationException("The specified grant type is not supported.");
            }

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        }
    }
}
