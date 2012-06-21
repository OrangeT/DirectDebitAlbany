using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrangeTentacle.DirectDebitAlbany.Test
{
    class DestinationTest
    {
        public static Destination SampleDestination()
        {
            var bankAccount = "87654321";
            var sortCode = "654321";
            var name = "Account Name";

            return new Destination(bankAccount, sortCode, name);
        }
    }
}
