using System;
using Xunit;

namespace OrangeTentacle.DirectDebitAlbany.Test
{
    public class RecordTest
    {
        public class Ctor
        {
            [Fact]
            public void Valid_Ctor()
            {
                var originator = OriginatorTest.SampleOriginator();
                var destination = DestinationTest.SampleDestination();
                var record = new Record(originator, destination, TransCode.Payment, 100.00m, "Ref");
            }

            [Fact]
            public void Must_Have_Originator()
            {
                var destination = DestinationTest.SampleDestination();
                Assert.Throws<DirectDebitException>(
                        () => new Record(null, destination, TransCode.Payment, 100.00m, "Ref"));
            }

            [Fact]
            public void Must_Have_Destination()
            {
                var originator = OriginatorTest.SampleOriginator();
                Assert.Throws<DirectDebitException>(
                        () => new Record(originator, null, TransCode.Payment, 100.00m, "Ref"));
            }

            [Fact]
            public void Orginator_Destination_Must_Be_Distinct()
            {
                var bankAccount = "12345678";
                var sortCode = "123456";
                var name = "Sample Bank Account";

                var originator = new Originator(bankAccount, sortCode, name);
                var destination = new Destination(bankAccount, sortCode, name);

                Assert.Throws<DirectDebitException>(
                       () => new Record(originator, destination, TransCode.Payment, 100.00m, "Ref"));
            }

            [Fact]
            public void Must_Have_Ref()
            {
                var originator = OriginatorTest.SampleOriginator();
                var destination = DestinationTest.SampleDestination();
                Assert.Throws<DirectDebitException>(
                        () => new Record(originator, destination, TransCode.Payment, 100.00m, null));
            }
        }
    }
}
