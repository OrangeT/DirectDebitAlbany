using System;
using Xunit;
using Xunit.Extensions;
using Moq;

namespace OrangeTentacle.DirectDebitAlbany.Test
{
    public class RecordTest
    {
        public class Ctor
        {
            [Fact]
            public void Valid_Ctor()
            {
                var originator = BankAccountTest.SampleOriginator();
                var destination = BankAccountTest.SampleDestination();
                var record = new Record(originator, destination, TransCode.Payment, 100.00m, "Ref");

                Assert.Equal(TransCode.Payment, record.TransCode);
                Assert.Equal(100.00m, record.Amount);
                Assert.Equal("Ref", record.Reference);
                Assert.Equal(originator, record.Originator);
                Assert.Equal(destination, record.Destination);
            }

            [Fact]
            public void Must_Have_Originator()
            {
                var destination = BankAccountTest.SampleDestination();
                Assert.Throws<DirectDebitException>(
                        () => new Record(null, destination, TransCode.Payment, 100.00m, "Ref"));
            }

            [Fact]
            public void Must_Have_Destination()
            {
                var originator = BankAccountTest.SampleOriginator();
                Assert.Throws<DirectDebitException>(
                        () => new Record(originator, null, TransCode.Payment, 100.00m, "Ref"));
            }

            [Fact]
            public void Orginator_Destination_Must_Be_Distinct()
            {
                var bankAccount = "12345678";
                var sortCode = "123456";
                var name = "Sample Bank Account";

                var originator = new BankAccount(bankAccount, sortCode, name);
                var destination = new BankAccount(bankAccount, sortCode, name);

                Assert.Throws<DirectDebitException>(
                       () => new Record(originator, destination, TransCode.Payment, 100.00m, "Ref"));
            }

            [Fact]
            public void Must_Have_Ref()
            {
                var originator = BankAccountTest.SampleOriginator();
                var destination = BankAccountTest.SampleDestination();
                Assert.Throws<DirectDebitException>(
                        () => new Record(originator, destination, TransCode.Payment, 100.00m, null));
            }
        }

        public class Serialize
        {
            public class TransCode
            {
                [Theory]
                [InlineData(DirectDebitAlbany.TransCode.BankGiroCredit, "99")]
                [InlineData(DirectDebitAlbany.TransCode.FirstPayment, "01")]
                [InlineData(DirectDebitAlbany.TransCode.Payment, "17")]
                [InlineData(DirectDebitAlbany.TransCode.RePresentation, "18")]
                [InlineData(DirectDebitAlbany.TransCode.FinalPayment, "19")]
                [InlineData(DirectDebitAlbany.TransCode.NewInstruction, "0N")]
                [InlineData(DirectDebitAlbany.TransCode.CancelInstruction, "0C")]
                [InlineData(DirectDebitAlbany.TransCode.ConvertInstruction, "0S")]
                public void CorrectCode(DirectDebitAlbany.TransCode tCode, string code)
                {
                    var record = new Record(BankAccountTest.SampleOriginator(), 
                            BankAccountTest.SampleDestination(), tCode, 100.00m, "Ref");

                    var serialized = record.Serialize(SerializeMethod.FixedWidth);

                    Assert.Equal(code, serialized.TransCode);
                }
            }

            public class Amount
            {
                [Fact]
                public void Pad_Left()
                {
                    var record = new Record(BankAccountTest.SampleOriginator(), 
                            BankAccountTest.SampleDestination(), DirectDebitAlbany.TransCode.Payment, 
                            12.34m, "Ref");

                    var serialized = record.Serialize(SerializeMethod.FixedWidth);

                    Assert.Equal("00000001234", serialized.Amount);
                }

                [Fact]
                public void In_Pence()
                {
                    var record = new Record(BankAccountTest.SampleOriginator(), 
                            BankAccountTest.SampleDestination(), DirectDebitAlbany.TransCode.Payment,
                            1234m, "Ref");

                    var serialized = record.Serialize(SerializeMethod.FixedWidth);

                    Assert.Equal("00000123400", serialized.Amount);
                }

                [Fact]
                public void CSV_Fixed_Dp()
                {
                    var record = new Record(BankAccountTest.SampleOriginator(), 
                            BankAccountTest.SampleDestination(), DirectDebitAlbany.TransCode.Payment,
                            1234m, "Ref");

                    var serialized = record.Serialize(SerializeMethod.CSV);

                    Assert.Equal("1234.00", serialized.Amount);
                }
            }

            public class Reference
            {
                [Fact]    
                public void Uppercase()
                {
                    var reference = "abcdefghijk mnopqr";

                    var record = new Record(BankAccountTest.SampleOriginator(), 
                            BankAccountTest.SampleDestination(), DirectDebitAlbany.TransCode.Payment,
                            null, reference);

                    var serialized = record.Serialize(SerializeMethod.FixedWidth);

                    Assert.Equal("ABCDEFGHIJK MNOPQR", serialized.Reference);
                }

                [Fact]
                public void Truncate()
                {
                    var reference = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                    var record = new Record(BankAccountTest.SampleOriginator(), 
                            BankAccountTest.SampleDestination(), DirectDebitAlbany.TransCode.Payment,
                            null, reference);

                    var serialized = record.Serialize(SerializeMethod.FixedWidth);

                    Assert.Equal("ABCDEFGHIJKLMNOPQR", serialized.Reference);
                }

                [Fact]
                public void No_Pad_Csv()
                {
                    var reference = "abc";
                    
                    var record = new Record(BankAccountTest.SampleOriginator(), 
                            BankAccountTest.SampleDestination(), DirectDebitAlbany.TransCode.Payment,
                            null, reference);

                    var serialized = record.Serialize(SerializeMethod.CSV);

                    Assert.Equal("ABC", serialized.Reference);
                }
            }

            public class Originator
            {
                [Fact]
                public void Serialized()
                {
                    var originator = new Mock<IBankAccount>();
                    var serializedAccount = new Mock<ISerializedAccount>();

                    serializedAccount.Setup(x => x.Line).Returns("TESTTEST");
                    originator.Setup(x => x.Serialize(
                                It.IsAny<SerializeMethod>(), It.IsAny<string[]>()))
                        .Returns(serializedAccount.Object);

                    var record = new Record(originator.Object, BankAccountTest.SampleDestination(), 
                            DirectDebitAlbany.TransCode.Payment, null, "abvc");

                    var serialized = record.Serialize(SerializeMethod.FixedWidth);

                    Assert.Equal("TESTTEST", serialized.Originator);
                }
            }

            public class Destination
            {
                [Fact]
                public void Serialized()
                {
                    var destination = new Mock<IBankAccount>();
                    var serializedAccount = new Mock<ISerializedAccount>();

                    serializedAccount.Setup(x => x.Line).Returns("TESTTEST");
                    destination.Setup(x => x.Serialize(
                                It.IsAny<SerializeMethod>(), It.IsAny<string[]>()))
                        .Returns(serializedAccount.Object);

                    var record = new Record(BankAccountTest.SampleOriginator(), 
                            destination.Object, DirectDebitAlbany.TransCode.Payment, null, "abvc");

                    var serialized = record.Serialize(SerializeMethod.FixedWidth);

                    Assert.Equal("TESTTEST", serialized.Destination);
                }
            }

            public class Line
            {
                [Fact]
                public void Default()
                {
                    var record = SampleRecord();
                    var serialized = record.Serialize(SerializeMethod.FixedWidth);

                    var composed = serialized.Destination + serialized.TransCode 
                        + serialized.Originator + serialized.Amount + serialized.Reference;

                    Assert.Equal(composed, serialized.Line);
                }

                [Theory]
                [InlineData("TransCode", "Amount", "Reference", "Originator", "Destination")]
                [InlineData("TransCode", "Originator", "Destination", "Amount", "Reference")]
                [InlineData("Originator", "Destination", "Reference", "Amount", "TransCode")]
                public void Takes_Properties(string prop1, string prop2, string prop3, 
                        string prop4, string prop5)
                {
                    var properties = new string[] { prop1, prop2, prop3, prop4, prop5 };

                    var record = SampleRecord();
                    var serialized = record.Serialize(SerializeMethod.FixedWidth,
                            SerializedAccount.DEFAULT_FIELDS, properties);

                    var composed = "";
                    foreach(var prop in properties)
                    {
                        var p = typeof(ISerializedRecord).GetProperty(prop);
                        composed += p.GetValue(serialized, null).ToString();
                    }

                    Assert.Equal(composed, serialized.Line);
                }

                [Fact]
                public void Configuration()
                {
                    var configuration = new DirectDebitConfiguration();
                    configuration.Record.Add(new FieldConfiguration { Field = "TransCode" });

                    var record = SampleRecord();
                    var serialized = record.Serialize(SerializeMethod.FixedWidth, 
                            configuration);

                    Assert.Equal(serialized.TransCode, serialized.Line);
                }

                public Record SampleRecord()
                {
                    var originator = new Mock<IBankAccount>();
                    var serializedOriginator = new Mock<ISerializedAccount>();

                    serializedOriginator.Setup(x => x.Line).Returns("ORIGINATOR");
                    originator.Setup(x => x.Serialize(It.IsAny<SerializeMethod>(), 
                                It.IsAny<string[]>()))
                        .Returns(serializedOriginator.Object);

                    var destination = new Mock<IBankAccount>();
                    var serializedDestination = new Mock<ISerializedAccount>();

                    serializedDestination.Setup(x => x.Line).Returns("DESTINATION");
                    destination.Setup(x => x.Serialize(It.IsAny<SerializeMethod>(),
                                It.IsAny<string[]>()))
                        .Returns(serializedDestination.Object);

                    var record = new Record(originator.Object, destination.Object,
                            DirectDebitAlbany.TransCode.Payment, null, "abvc");

                    return record;
                }
            }
        }
    }
}
