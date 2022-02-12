using System;
using System.Diagnostics;  //Added for Debug.WriteLine
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetOpenIdDict.Data;

[assembly: HostingStartup(typeof(NetOpenIdDict.Areas.Identity.IdentityHostingStartup))]
namespace NetOpenIdDict.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                Debug.WriteLine("");
                int i = 0;
                i += 1;
            });
        }
    }
}