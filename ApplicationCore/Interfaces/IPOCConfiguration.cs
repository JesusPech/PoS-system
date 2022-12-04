using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Interfaces
{
    public interface IPOCConfiguration
    {
        //string Region { get; set; }

        //string AvailableDemoninationsText { get; set; }

        string GetRegion();

        string GetDenominationsText();


        void SetInitialConfiguration();

        void SetRegion();

        void SetDenominations(string region);

        List<decimal> GetDenominations();
    }
}
