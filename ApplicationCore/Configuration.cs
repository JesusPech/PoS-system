using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApplicationCore
{
    public class Configuration : IPOCConfiguration
    {
        private const string _APPSETTINGSFILENAME = "appsettings.json";
        private string _AppSettingsFullPath = Path.Combine(Environment.CurrentDirectory, _APPSETTINGSFILENAME);
        private string _DefaultRegion = "US";

        private List<string> _validRegions { get; set; } = new List<string>();
        public Dictionary<string, List<decimal>> ValidDenominations { get; private set; } = new Dictionary<string, List<decimal>>();
        public string ValidDenominationsString { get; private set; }
        public string Region { get; private set; }

        private readonly List<decimal> _USDenominations = new List<decimal>() { 0.01M, 0.05M, 0.10M, 0.25M, 0.50M, 1.00M, 2.00M, 5.00M, 10.00M, 20.00M, 50.00M, 100.00M };
        private readonly List<decimal> _MXDenominations = new List<decimal>() { 0.05M, 0.10M, 0.20M, 0.50M, 1.00M, 2.00M, 5.00M, 10.00M, 20.00M, 50.00M, 100.00M };

        public Configuration()
        {
            _validRegions.Add("US");
            _validRegions.Add("MX");
            _validRegions.Add("Default");
        }

        public void SetInitialConfiguration()
        {
            if (!File.Exists(_AppSettingsFullPath))
            {
                WriteAppSettings(String.Empty);
            }
        }

        public void SetRegion(string region, bool defaultRegion)
        {
            if (defaultRegion)
            {
                WriteAppSettings(_DefaultRegion);
            }
            else
            {
                WriteAppSettings(region);
            }

            Region = defaultRegion ? _DefaultRegion : region;
        }

        public void SetDenominations(string region)
        {
            SetValidDenominations(region);
        }

        public List<decimal> GetDenominations()
        {
            List<decimal> denominations = new List<decimal>();
            ValidDenominations.TryGetValue(Region, out denominations);

            if(!denominations.Any())
                ValidDenominations.TryGetValue("Default", out denominations);

            return denominations;
        }

        private void SetValidDenominations(string region)
        {
            List<decimal> denominationList;
            string denominationRegion;           

            switch(region)
            {
                case "US":
                    denominationRegion = "US";
                    denominationList = _USDenominations;
                   break ;

                case "MX":
                    denominationRegion = "MX";
                    denominationList = _MXDenominations;
                    break;

                default:
                    denominationRegion = "US";
                    denominationList = _USDenominations;
                        break;

            }
         
            ValidDenominations.Add(denominationRegion, denominationList);

            denominationList.ForEach(p => ValidDenominationsString += p + ",");
            ValidDenominationsString = ValidDenominationsString.Remove(ValidDenominationsString.Length - 1);

        }

        public bool ValidateRegion(string region)
        {
            return _validRegions.Contains(region);
        }

        private void WriteAppSettings(string region)
        {
            Settings settings = new Settings
            {
                Version = "1.0",
                Region = region.ToUpper()
            };

            string json = JsonSerializer.Serialize(settings);
            File.WriteAllText(_AppSettingsFullPath, json);
        }

        public string GetDenominationsText()
        {
            return ValidDenominationsString;
        }

        public string GetRegion()
        {
            return Region;
        }
    }
}
