using carpark.api.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;

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
