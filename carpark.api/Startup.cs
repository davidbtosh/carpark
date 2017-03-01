using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.Extensions.DependencyInjection;
using carpark.api.Services;

[assembly: OwinStartup(typeof(carpark.api.Startup))]

namespace carpark.api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var services = new ServiceCollection();            
            ConfigureServices(services);  
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRatesCalculator, RatesCalculator>();

        }
    }
}
