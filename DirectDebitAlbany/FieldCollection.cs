using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace OrangeTentacle.DirectDebitAlbany
{
    [ConfigurationCollection(typeof(FieldConfiguration),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class FieldCollection : ConfigurationElementCollection
    {
        public FieldConfiguration this[int index]
        { 
            get { return (FieldConfiguration) BaseGet(index); }
        }
        
        public void Add(FieldConfiguration field)
        {
            BaseAdd(field);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FieldConfiguration();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FieldConfiguration)element).Field;
        }

        public string[] GetProperties()
        {
            return (from FieldConfiguration p in this select p.Field).ToArray();
        }
    }
}
