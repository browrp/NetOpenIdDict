using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetOpenIdDict.Data;
using OpenIddict.Abstractions;
using OpenIddict.EntityFrameworkCore.Models;

namespace NetOpenIdDict
{
    public class TestData : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public TestData(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("postman", cancellationToken) is null)
            {
                /*
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "postman",
                    ClientSecret = "postman-secret",
                    DisplayName = "Postman",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,

                        OpenIddictConstants.Permissions.Prefixes.Scope + "api"


                    }
                }, cancellationToken);
                */


                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "postman",
                    ClientSecret = "postman-secret",
                    DisplayName = "Postman",
                    RedirectUris = { new Uri("https://oauth.pstmn.io/v1/callback") },
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,

                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,

                        // Enabling Refresh Token Flow
                        // This requires the client to pass offline_access scope
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

                        OpenIddictConstants.Permissions.Prefixes.Scope + "api",

                        OpenIddictConstants.Permissions.ResponseTypes.Code,

                        // Allow Password Grant
                        OpenIddictConstants.Permissions.GrantTypes.Password


                    }
                }, cancellationToken);

                
            }
            // Me trying to pull the values and update them just to see if it works...
            // This totally doesn't work.  How do we pull back settings for a particular client,
            // update those settings, and then save them back to the DB? 
            else 
            {

                //{ OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreApplication`1[System.Int32]}

                /*
                var openIddictApplicationDescriptor = await manager.FindByClientIdAsync("postman", cancellationToken);
                if(openIddictApplicationDescriptor != null)
                {
                    
                    //openIddictApplicationDescriptor["Permissions"].Add()
                    var casted = (OpenIddictEntityFrameworkCoreApplication)openIddictApplicationDescriptor;
                    //casted.Permissions.Add(OpenIddictConstants.GrantTypes.RefreshToken);
                    var t = casted.Properties;

                }*/
            }
            
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    }
}
