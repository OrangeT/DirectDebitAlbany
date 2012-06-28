using System;
using Xunit;
using Xunit.Extensions;

namespace OrangeTentacle.DirectDebitAlbany.Test
{
    public class SerializedAccountTest
    {
        public class Line
        {
            [Fact]
            public void Default()
            {
                var account = new BankAccount(BankAccountTest.NUMBER, 
                        BankAccountTest.SORTCODE, BankAccountTest.NAME);

                var serialize = account.Serialize();
                var line = serialize.Line();

                var composed = serialize.SortCode + serialize.Number + serialize.Name;

                Assert.Equal(composed, line);
            }

            [Theory]
            [InlineData("Number", "Name", "SortCode")]
            [InlineData("Name", "SortCode", "Number")]
            public void Properties(string prop1, string prop2, string prop3)
            {
                var properties = new [] { prop1, prop2, prop3 };

                var account = new BankAccount(BankAccountTest.NUMBER, 
                        BankAccountTest.SORTCODE, BankAccountTest.NAME);

                var serialize = account.Serialize();
                var line = serialize.Line(properties);

                var composed = "";
                foreach(var prop in properties)
                {
                    var p = typeof(ISerializedAccount).GetProperty(prop);
                    composed += p.GetValue(serialize, null).ToString();
                }

                Assert.Equal(composed, line);
            }

            [Fact]
            public void Configuration()
            {
                var configuration = new DirectDebitConfiguration();
                configuration.BankAccount.Add(new FieldConfiguration { Field = "Number" });

                var account = new BankAccount(BankAccountTest.NUMBER,
                        BankAccountTest.SORTCODE, BankAccountTest.NAME);

                var serialize = account.Serialize();
                var line = serialize.Line(configuration);

                Assert.Equal(serialize.Number, line);
            }
        }
    }
}
