using System.Collections.Generic;

namespace OrangeTentacle.DirectDebitAlbany
{
    public static class BankValidation
    {
        // This class is ripe for extracting into a config file.

        public static readonly Bank[] Nine = 
            new [] { 
                Bank.AllianceAndLeicester, 
                Bank.NationalSavings, 
                Bank.NavionalAndProvincial
            };

        public static readonly Bank[] Ten =
            new [] {
                Bank.Natwest,
                Bank.Coop
            };

        public static Dictionary<TransCode, string> TransCode = 
            new Dictionary<TransCode, string> {
                { DirectDebitAlbany.TransCode.BankGiroCredit, "99" },
                { DirectDebitAlbany.TransCode.FirstPayment, "01" },
                { DirectDebitAlbany.TransCode.Payment, "17" },
                { DirectDebitAlbany.TransCode.RePresentation, "18" },
                { DirectDebitAlbany.TransCode.FinalPayment, "19" },
                { DirectDebitAlbany.TransCode.NewInstruction, "0N" },
                { DirectDebitAlbany.TransCode.CancelInstruction, "0C" },
                { DirectDebitAlbany.TransCode.ConvertInstruction, "0S" }
            };
    }
}
