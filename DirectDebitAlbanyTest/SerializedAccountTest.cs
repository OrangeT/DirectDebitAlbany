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
                public void Takes_Properties(string prop1, string prop2, string prop3)
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
                public void Ignores_Case()
                {
                    var properties = new [] { "NUMBER" };
                    var account = new BankAccount(BankAccountTest.NUMBER, 
                            BankAccountTest.SORTCODE, BankAccountTest.NAME);
                    var serialize = account.Serialize();

                    var line = serialize.Line(properties);

                    Assert.Equal(serialize.Number, line);
                }

                [Fact]
                public void Invalid_Property_Throws_Exception()
                {
                    var properties = new [] { "Number", "Name", "SortCode", "Bob" };

                    var account = new BankAccount(BankAccountTest.NUMBER, 
                            BankAccountTest.SORTCODE, BankAccountTest.NAME);

                    var serialize = account.Serialize();
                    Assert.Throws<DirectDebitException>(() => serialize.Line(properties));
                }

                [Fact]
                public void Cannot_Specify_Line()
                {
                    var properties = new [] { "Line" };

                    var account = new BankAccount(BankAccountTest.NUMBER, 
                            BankAccountTest.SORTCODE, BankAccountTest.NAME);

                    var serialize = account.Serialize();
                    Assert.Throws<DirectDebitException>(() => serialize.Line(properties));
                }
            }
 
    }
}
