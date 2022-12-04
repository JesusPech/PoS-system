using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ApplicationCore
{
    public static class ErrorRepository
    {
        public const string InvalidRegion  = "Invalid Region, do you want to default to US (Y / N)? ";

        public const string InvalidInputPrice = "INVALID INPUT. Enter price of the item or type 'X' to exit.";

        public const string InvalidReturnChange = "Invalid Quantity to Return";

        public const string InvalidInputDenomination = "INVALID INPUT Please insert the denomination you want to use or type 'X' to exit.";

    }
}
