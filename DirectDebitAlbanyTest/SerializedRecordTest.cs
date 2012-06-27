using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Xunit;

namespace OrangeTentacle.DirectDebitAlbany.Test
{
    public class SerializedRecordTest
    {
        public class Line
        {
            [Fact]
            public void Compose()
            {
                var originator = new Mock<IBankAccount>();
                var serializedOriginator = new Mock<ISerializedAccount>();

                serializedOriginator.Setup(x => x.Line).Returns("ORIGINATOR");
                originator.Setup(x => x.Serialize()).Returns(serializedOriginator.Object);

                var destination = new Mock<IBankAccount>();
                var serializedDestination = new Mock<ISerializedAccount>();

                serializedDestination.Setup(x => x.Line).Returns("DESTINATION");
                destination.Setup(x => x.Serialize()).Returns(serializedDestination.Object);

                var record = new Record(originator.Object, destination.Object,
                        DirectDebitAlbany.TransCode.Payment, null, "abvc");

                var serialized = record.Serialize();

                var composed = serialized.Destination + serialized.TransCode 
                    + serialized.Originator + serialized.Amount + serialized.Reference;

                Assert.Equal(composed, serialized.Line);
            }
        }
    }
}
