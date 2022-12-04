using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApplicationCore
{
    public class Configuration : IPOCConfiguration
    {
        private const string _APPSETTINGSFILENAME = "appsettings.json";
        private string _AppSettingsFullPath = Path.Combine(Environment.CurrentDirectory, _APPSETTINGSFILENAME);

        private List<string> _validRegions { get; set; } = new List<string>();
        public Dictionary<string, List<decimal>> ValidDenominations { get; private set; } = new Dictionary<string, List<decimal>>();
        public string Region { get; set; }
        public string AvailableDemoninationsText { get; set; }

        private readonly List<decimal> _USDenominations = new List<decimal>() { 0.01M, 0.05M, 0.10M, 0.25M, 0.50M, 1.00M, 2.00M, 5.00M, 10.00M, 20.00M, 50.00M, 100.00M };
        private readonly List<decimal> _MXDenominations = new List<decimal>() { 0.05M, 0.10M, 0.20M, 0.50M, 1.00M, 2.00M, 5.00M, 10.00M, 20.00M, 50.00M, 100.00M };

        public Configuration()
        {
            _validRegions.Add("US");
            _validRegions.Add("MX");
        }

        public void SetInitialConfiguration()
        {
            if (!File.Exists(_AppSettingsFullPath))
            {
                WriteAppSettings(String.Empty);
            }
        }

        public void SetRegion()
        {
            Console.WriteLine("Please set the Region");
            string region = Console.ReadLine();
            if (ValidateRegion(region))
            {
                WriteAppSettings(region);
            }
            else
            {
                Console.WriteLine("Invalid Region, do you want to default to US (Y/N)?");
                string defaultResponse = Console.ReadLine();
                if (defaultResponse.ToUpper().Equals("Y"))
                {
                    WriteAppSettings(region);
                }
                else
                {
                    Environment.Exit(0);
                }
            }

            Console.Clear();
        }

        public void SetDenominations(string region)
        {
            SetValidDenominations(region);
        }

        public List<decimal> GetDenominations()
        {
            List<decimal> denominations;
            ValidDenominations.TryGetValue(Region, out denominations);
            return denominations;
        }

        private void SetValidDenominations(string region)
        {
            var denominationList = region switch
            {
                "US" => _USDenominations,
                "MX" => _MXDenominations,
                _ => _USDenominations
            };

            Region = region;
            ValidDenominations.Add(region, denominationList);
            GetDenominationsText();
        }

        private bool ValidateRegion(string region)
        {
            return _validRegions.Contains(region);
        }

        private void WriteAppSettings(string region)
        {
            Settings settings = new Settings
            {
                Version = "1.0",
                Region = region
            };

            string json = JsonSerializer.Serialize(settings);
            File.WriteAllText(_AppSettingsFullPath, json);
        }

        private void GetDenominationsText()
        {
            GetDenominations().ForEach(p => AvailableDemoninationsText += p + ",");
            AvailableDemoninationsText = AvailableDemoninationsText.Remove(AvailableDemoninationsText.Length - 1);
        }
    }
}
