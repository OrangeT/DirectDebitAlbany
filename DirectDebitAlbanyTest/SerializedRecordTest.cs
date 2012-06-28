using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace OrangeTentacle.DirectDebitAlbany.Test
{
    public class SerializedRecordTest
    {
        public class Line
        {
            [Fact]
            public void Default()
            {
                var serialized = SampleRecord();
                var line = serialized.Line();

                var composed = serialized.Destination + serialized.TransCode 
                    + serialized.Originator + serialized.Amount + serialized.Reference;

                Assert.Equal(composed, line);
            }

            [Theory]
            [InlineData("TransCode", "Amount", "Reference", "Originator", "Destination")]
            [InlineData("TransCode", "Originator", "Destination", "Amount", "Reference")]
            [InlineData("Originator", "Destination", "Reference", "Amount", "TransCode")]
            public void Takes_Properties(string prop1, string prop2, string prop3, string prop4, string prop5)
            {
                var properties = new string[] { prop1, prop2, prop3, prop4, prop5 };

                var serialized = SampleRecord();
                var line = serialized.Line(properties);

                var composed = "";
                foreach(var prop in properties)
                {
                    var p = typeof(ISerializedRecord).GetProperty(prop);
                    composed += p.GetValue(serialized, null).ToString();
                }

                Assert.Equal(composed, line);
            }

            [Fact]
            public void Configuration()
            {
                var configuration = new DirectDebitConfiguration();
                configuration.Record.Add(new FieldConfiguration { Field = "TransCode" });

                var serialize = SampleRecord();
                var line = serialize.Line(configuration);

                Assert.Equal(serialize.TransCode, line);
            }

            public ISerializedRecord SampleRecord()
            {
                var originator = new Mock<IBankAccount>();
                var serializedOriginator = new Mock<ISerializedAccount>();

                serializedOriginator.Setup(x => x.Line()).Returns("ORIGINATOR");
                originator.Setup(x => x.Serialize()).Returns(serializedOriginator.Object);

                var destination = new Mock<IBankAccount>();
                var serializedDestination = new Mock<ISerializedAccount>();

                serializedDestination.Setup(x => x.Line()).Returns("DESTINATION");
                destination.Setup(x => x.Serialize()).Returns(serializedDestination.Object);

                var record = new Record(originator.Object, destination.Object,
                        DirectDebitAlbany.TransCode.Payment, null, "abvc");

                var serialized = record.Serialize();
                return serialized;
            }
        }
    }
}
