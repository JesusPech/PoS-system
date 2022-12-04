using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationCore
{
    public class ProcessTransactions : IProcessTransactions
    {
        private decimal _priceItemParsed;
        private decimal _moneyProvided;
        private decimal _moneyToReturn;
        private const string _exitConditionString = "X";       
        private readonly IPOCConfiguration _configuration;
        public ProcessTransactions(IPOCConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void StartProcessingTransactions()
        {           
            bool exitChoice = true;
            while (exitChoice)
            {
                Console.Clear();
                Console.WriteLine("Welcome to POS terminal!");
                Console.WriteLine("IMPORTANT: you are working with {0} configuration", _configuration.GetRegion()); ;
                Console.WriteLine("Available denominations for this configuration are: {0}", _configuration.GetDenominationsText());
                Console.WriteLine("Enter price of the item or type 'X' to exit.");
                string priceItem = Console.ReadLine();

                if (priceItem.ToUpper().Equals("X"))
                {
                    exitChoice = false;
                }
                else
                {
                    if (decimal.TryParse(priceItem, out _priceItemParsed))
                    {
                        ProcessTransaction();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Welcome to POS terminal!");                       
                        Console.WriteLine(ErrorRepository.InvalidInputPrice);
                    }

                };
            }
        }

        public void ProcessTransaction()
        {
            SubmitPayment();
            CalculateChange();
        }

        private void SubmitPayment()
        {
            bool exitChoice = false;
            _moneyProvided = 0M;
            GeneratePaymentScreen(false);
            while (!exitChoice)
            {
                string denomination = Console.ReadLine();
                if (decimal.TryParse(denomination, out decimal denominationParsed) && denominationParsed >= 0 && ValidateDenomination(denominationParsed))
                {
                    Console.WriteLine("Please insert the quantity you want to use or type 'X' to exit.");
                    string quantity = Console.ReadLine();
                    if (int.TryParse(quantity, out int quantityParsed) && quantityParsed >= 0)
                    {
                        _moneyProvided += (denominationParsed * quantityParsed);
                        exitChoice = (_moneyProvided >= _priceItemParsed);
                        GeneratePaymentScreen(false);
                    }
                    else
                    {
                        GeneratePaymentScreen(true);
                        exitChoice = ExitCondition(quantity);
                    }
                }
                else
                {
                    GeneratePaymentScreen(true);
                    exitChoice = ExitCondition(denomination);
                }
            }
            _moneyToReturn = _moneyProvided - _priceItemParsed;
        }

        private void CalculateChange()
        {
            Dictionary<decimal, int> neededChange = new Dictionary<decimal, int>();
            decimal moneyToReturn = _moneyToReturn;

            var denominationsAvailable = _configuration.GetDenominations().Where(x => x <= moneyToReturn).OrderByDescending(x => x).ToList();

            if(moneyToReturn > 0)
            {
                foreach (decimal denomination in denominationsAvailable)
                {
                    int billsToReturn = NumberOfBillsToReturn(denomination, moneyToReturn);
                    if (billsToReturn > 0)
                    {
                        neededChange.Add(denomination, billsToReturn);
                        moneyToReturn -= (billsToReturn * denomination);
                        if (moneyToReturn == 0) break;
                    }
                }

                Console.Clear();
                Console.WriteLine("**************************************");
                foreach (var change in neededChange)
                {
                    Console.WriteLine($"Denomination: {change.Key:#########0.00}, Quantity: {change.Value:#########0.00}");
                }
                Console.WriteLine("**************************************");
                Console.WriteLine("Total change is: ${0} ", _moneyToReturn);
                Console.WriteLine("**************************************");

                if (moneyToReturn > 0)
                {                  
                    Console.WriteLine($"{moneyToReturn:#########0.00} {0}",  ErrorRepository.InvalidReturnChange);
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("**************************************");
                Console.WriteLine("No Money to Return");
                Console.WriteLine("**************************************");
            }
            
            Console.WriteLine("Press any key to continue.");
            string quantity = Console.ReadLine();
        }

        private int NumberOfBillsToReturn(decimal denominations, decimal moneyToReturn)
        {
            return (int)(moneyToReturn / denominations);
        }

        private void GeneratePaymentScreen(bool invalidInput)
        {
            Console.Clear();
            Console.WriteLine("IMPORTANT: you are working with {0} configuration", _configuration.GetRegion());
            Console.WriteLine("Available denominations for this configuration are: {0}", _configuration.GetDenominationsText());
            Console.WriteLine("");
            Console.WriteLine("**************************************");
            Console.WriteLine("Processing Transaction:");
            Console.WriteLine($"Product Value: ${_priceItemParsed:#########0.00}");
            Console.WriteLine($"Ammount Provided: ${_moneyProvided:#########0.00}");
            Console.WriteLine("**************************************");
            if (invalidInput) {               
                Console.WriteLine(ErrorRepository.InvalidInputDenomination);
            }
            else
            {
                Console.WriteLine("Please insert the denomination you want to use or type 'X' to exit.");
            }

        }

        private bool ExitCondition(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && value.ToUpper().Equals(_exitConditionString);
        }

        private bool ValidateDenomination(decimal denomination)
        {
            return _configuration.GetDenominations().Contains(denomination);
        }       
    }
}
