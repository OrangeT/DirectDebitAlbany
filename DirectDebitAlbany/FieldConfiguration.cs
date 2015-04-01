using System;
using System.Configuration;

namespace OrangeTentacle.DirectDebitAlbany
{
    public class FieldConfiguration : ConfigurationElement
    {
        internal Guid Id { get; set; }

        public FieldConfiguration()
        {
            Id = Guid.NewGuid();
        }

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
