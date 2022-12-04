using ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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

            if (string.IsNullOrWhiteSpace(config.GetSection("Region").Value))
            {
                Console.WriteLine("Please set the Region");
                string region = Console.ReadLine();
                string validRegion = string.Empty;
                bool defaultRegion = false;
                if (configurations.ValidateRegion(region))
                {
                    validRegion = region;
                }
                else
                {
                    Console.WriteLine(ErrorRepository.InvalidRegion);
                    string defaultResponse = Console.ReadLine();
                    if (defaultResponse.ToUpper().Equals("Y"))
                    {
                        validRegion = "";
                        defaultRegion = true;
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }

                Console.Clear();
                configurations.SetRegion(validRegion, defaultRegion);
            }

            configurations.SetDenominations(config.GetSection("Region").Value);

            processTransactions.StartProcessingTransactions();
        }
    }
}
