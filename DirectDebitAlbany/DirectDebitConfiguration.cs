using System;
using System.Configuration;

namespace OrangeTentacle.DirectDebitAlbany
{
    public class DirectDebitConfiguration : ConfigurationSection
    {
        public const string SECTION_NAME = "DirectDebit";

        public static DirectDebitConfiguration GetSection()
        {
            var section = ConfigurationManager.GetSection(SECTION_NAME) 
                as DirectDebitConfiguration;

            return section;
        }

        [ConfigurationProperty("BankAccount")]
        public FieldCollection BankAccount
        {
            get
            {
                return (FieldCollection)this["BankAccount"];
            }
        }

        [ConfigurationProperty("Record")]
        public FieldCollection Record
        {
            get
            {
                return (FieldCollection)this["Record"];
            }
        }

    }
}
