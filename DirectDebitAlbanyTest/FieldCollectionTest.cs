using System;
using OrangeTentacle.DirectDebitAlbany;
using Xunit;

namespace OrangeTentacle.DirectDebitAlbany.Test
{
    public class FieldCollectionTest
    {
        public class GetProperties
        {
            [Fact]
            public void FromCollection()
            {
                var fieldCollection = new FieldCollection();
                fieldCollection.Add(new FieldConfiguration { Field = "Bob" });
                fieldCollection.Add(new FieldConfiguration { Field = "Marley" });

                var properties = fieldCollection.GetProperties();

                Assert.Equal(new [] { "Bob", "Marley" }, properties);
            }

            [Fact]
            public void EmptyCollection()
            {
                var fieldCollection = new FieldCollection();

                var properties = fieldCollection.GetProperties();

                Assert.Equal(new string[] {}, properties);
            }
        }
    }
}
