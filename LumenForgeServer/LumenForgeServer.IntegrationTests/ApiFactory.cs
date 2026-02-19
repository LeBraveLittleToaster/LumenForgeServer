using System;
using System.Collections.Generic;
using System.Text;

namespace LumenForgeServer.IntegrationTests;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public class ApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Example: replace a real external client with a fake
                //services.RemoveAll<IClock>();
                //services.AddSingleton<IClock>(new FakeClock(DateTimeOffset.Parse("2026-02-19T00:00:00Z")));

                // Example: swap DB registration here (see next section)
            });
        }
    }
