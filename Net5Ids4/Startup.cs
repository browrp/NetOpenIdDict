using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using NetOpenIdDict.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Microsoft.IdentityModel.Tokens;


//ToDo: This explains Scopes https://stackoverflow.com/questions/48581556/oauth2-scopes-and-user-roles

namespace NetOpenIdDict
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection"));

                //https://documentation.openiddict.com/guide/getting-started.html
                // Register the entity sets needed by OpenIddict.
                // Note: use the generic overload if you need
                // to replace the default OpenIddict entities.
                //Added in int to match Identity Framework customization
                options.UseOpenIddict<int>();

            });
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
                options.SignIn.RequireConfirmedAccount = true;
                
               
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            // https://github.com/openiddict/openiddict-samples/blob/dev/samples/Hollastin/Hollastin.Server/Startup.cs
            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddOpenIddict()
                //// Register the OpenIddict core components.
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                    .UseDbContext<ApplicationDbContext>()
                    .ReplaceDefaultEntities<int>(); //Modified to match the Primary Key on the ApplicationUser https://documentation.openiddict.com/guide/getting-started.html

                })
                //// Register the OpenIddict server components.
                .AddServer(options =>
                {
                    // Enable ClientCredentials Flow
                    options.AllowClientCredentialsFlow();

                    // Allow Authorization Code Flow 
                    options.AllowAuthorizationCodeFlow();
                    options.RequireProofKeyForCodeExchange(); // with PKCE

                    // Allow Refresh Token Flow
                    options.AllowRefreshTokenFlow();

                    //Allow Password Grant flow
                    options.AllowPasswordFlow();



                    //Enable Token Endpoint
                    options.SetTokenEndpointUris("/connect/token");
                    options.SetAuthorizationEndpointUris("/connect/authorize");
                    options.SetUserinfoEndpointUris("/connect/userinfo");
                    


                    //!! D E V E L O P M E N T   O N L Y !!
                    //Hack: This should only be done on DEVELOPMENT.  For Production use X.509 certificates are recommended!!!
                    // Register the signing and encryption credentials.
                    options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();

                    // Testing to see what happens when the token is passed back.
                    //options.DisableAccessTokenEncryption();  //If not disabled you can't view your token at jwt.io

                    // Register the encryption credentials. This sample uses a symmetric
                    // encryption key that is shared between the server and the Api2 sample
                    // (that performs local token validation instead of using introspection).
                    //
                    // Note: in a real world application, this encryption key should be
                    // stored in a safe place (e.g in Azure KeyVault, stored as a secret).
                    options.AddEncryptionKey(new SymmetricSecurityKey(
                        Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));


                    //https://dev.to/robinvanderknaap/setting-up-an-authorization-server-with-openiddict-part-iii-client-credentials-flow-55lp
                    //Added this in per Robin's documentation.
                    //Is there a way we can DB Drive this?
                    options.RegisterScopes("api");
                    
                    // Register the ASP.NET Core host and configure the ASP.NET Core options.
                    options.UseAspNetCore()
                    .EnableTokenEndpointPassthrough()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough();



                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                    
                    
                });

            services.AddHostedService<TestData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
