using System.Configuration;

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

        protected override ConfigurationElement CreateNewElement()
        {
            return new FieldConfiguration();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FieldConfiguration)element).Field;
        }
    }
}