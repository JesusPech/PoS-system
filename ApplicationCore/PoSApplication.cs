using ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ApplicationCore
{
    internal class PoSApplication
    {
        /// <summary>
        /// Main process
        /// Steps 
        /// 1. User has to configure region/country only for the first time.
        /// 2. User enter price of item
        /// 3. User enter bills and coins to pay for the item 
        /// </summary>
        static void Main(string[] args)
        {
            //Inject the services needed for the application
            var ServiceProvider = new ServiceCollection()
                .AddSingleton<IProcessTransactions, ProcessTransactions>()
                .AddSingleton<IPOCConfiguration, Configuration>()
                .BuildServiceProvider();

            var processTransactions = ServiceProvider.GetService<IProcessTransactions>();
            var configurations = ServiceProvider.GetService<IPOCConfiguration>();

            //Create configuration file if doesn't exists
            configurations.SetInitialConfiguration();

            Microsoft.Extensions.Configuration.IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();

            //Get region from file. If blank then ask for user input
            if (string.IsNullOrWhiteSpace(config.GetSection("Region").Value))
            {
                Console.WriteLine("Please set the Region");
                string region = Console.ReadLine();
                string validRegion = string.Empty;
                bool defaultRegion = false;
                if (configurations.ValidateRegion(region)) //Validate region is supported
                {
                    validRegion = region;
                }
                else
                {
                    Console.WriteLine(ErrorRepository.InvalidRegion); //If region is not supported then we ask user if he wants to use default region
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
                configurations.SetRegion(validRegion, defaultRegion); //Saves region to configuration file
            }

            configurations.SetDenominations(config.GetSection("Region").Value);

            processTransactions.StartProcessingTransactions(); //Main process
        }
    }
}
