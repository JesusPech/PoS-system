using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Interfaces
{
    public interface IPOCConfiguration
    {
        void SetInitialConfiguration();

        void SetRegion();

        void SetDenominations(string region);

        List<decimal> GetDenominations();
    }
}
