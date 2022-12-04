using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Interfaces
{
    public interface IProcessTransactions
    {
        void StartProcessingTransactions();
        void ProcessTransaction();

    }
}
