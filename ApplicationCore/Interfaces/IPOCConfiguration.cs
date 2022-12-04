using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Interfaces
{
    public interface IPOCConfiguration
    {
        string GetRegion();

        string GetDenominationsText();

        void SetInitialConfiguration();

        void SetRegion(string region, bool defaultRegion);

        void SetDenominations(string region);

        List<decimal> GetDenominations();

        bool ValidateRegion(string region);
    }
}
