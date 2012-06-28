using System.Configuration;

namespace OrangeTentacle.DirectDebitAlbany
{
    public class FieldConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("field")]
        public string Field
        {
            get
            {
                return (string)this["field"];
            }

            set
            {
                this["field"] = value;
            }
        }
    }
}
