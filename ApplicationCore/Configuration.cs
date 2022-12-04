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

        /// <summary>
        /// Set supported regions and default region       
        /// </summary>
        public Configuration()
        {
            _validRegions.Add("US");
            _validRegions.Add("MX");
            _validRegions.Add("Default");
        }

        /// <summary>
        /// Create config file       
        /// </summary>
        public void SetInitialConfiguration()
        {
            if (!File.Exists(_AppSettingsFullPath))
            {
                WriteAppSettings(String.Empty);
            }
        }

        /// <summary>
        /// It write region to configuration file
        /// If defaultRegion is true then uses default Region
        /// </summary>
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

        /// <summary>
        /// Set denomination using region parameter       
        /// </summary>
        public void SetDenominations(string region)
        {
            SetValidDenominations(region);
        }

        /// <summary>
        /// It gets and returns configured denominations based on region configured. 
        /// It returns default denominations if region configured is not supported
        /// </summary>
        public List<decimal> GetDenominations()
        {
            List<decimal> denominations = new List<decimal>();
            ValidDenominations.TryGetValue(Region, out denominations);

            if(!denominations.Any()) //region configured is not supported
                ValidDenominations.TryGetValue("Default", out denominations);

            return denominations;
        }

        /// <summary>
        /// Set denomination using region parameter
        /// If region is not supported then system configured default denominations
        /// </summary>
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

        /// <summary>
        /// Returns if input region is supported      
        /// </summary>
        public bool ValidateRegion(string region)
        {
            return _validRegions.Contains(region);
        }

        /// <summary>
        /// Write configuration file       
        /// </summary>
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

        /// <summary>
        /// Returns configured denominations as string      
        /// </summary>
        public string GetDenominationsText()
        {
            return ValidDenominationsString;
        }

        /// <summary>
        /// Returns configured region       
        /// </summary>
        public string GetRegion()
        {
            return Region;
        }
    }
}
