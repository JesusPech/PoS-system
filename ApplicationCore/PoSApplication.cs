using ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCore
{
    internal class PoSApplication
    {
        static void Main(string[] args)
        {            
            //Inject the services needed for the application
            var ServiceProvider = new ServiceCollection()
                .AddSingleton<IProcessTransactions, ProcessTransactions>()
                .AddSingleton<IPOCConfiguration, Configuration>()
                .BuildServiceProvider();

            var processTransactions = ServiceProvider.GetService<IProcessTransactions>();
            var configurations = ServiceProvider.GetService<IPOCConfiguration>();

            configurations.SetInitialConfiguration();

            Microsoft.Extensions.Configuration.IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();

            if(string.IsNullOrWhiteSpace(config.GetSection("Region").Value))
            {
                configurations.SetRegion();
            }

            configurations.SetDenominations(config.GetSection("Region").Value);

            processTransactions.StartProcessingTransactions();
        }
    }
}
