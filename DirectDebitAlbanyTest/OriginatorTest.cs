using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrangeTentacle.DirectDebitAlbany.Test
{
    class OriginatorTest
    {
        public static Originator SampleOriginator()
        {
            var bankAccount = "12345678";
            var sortCode = "123456";
            var name = "Account Name";

            return new Originator(bankAccount, sortCode, name);
        }

    }
}
