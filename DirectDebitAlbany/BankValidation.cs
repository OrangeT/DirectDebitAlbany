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
    }
}
