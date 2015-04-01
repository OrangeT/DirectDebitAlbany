using System.Text.RegularExpressions;
using OrangeTentacle.DirectDebitAlbany;
using Xunit;
using Xunit.Extensions;

namespace OrangeTentacle.DirectDebitAlbany.Test
{
    public class BankAccountTest
    {
        public const string NUMBER = "12345678";
        public const string SORTCODE = "123456";
        public const string NAME = "Sample Account NAME";

        public class Ctor_No_Bank
        {
            [Theory]
            [InlineData("123456")]
            [InlineData("1234567")]
            [InlineData("12345678")]
            public void Number_Must_Be_Six_To_Eight_Digits(string number)
            {
                var account = new BankAccount(number, SORTCODE, NAME);
                Assert.Equal(number, account.Number);
            }

            [Theory]
            [InlineData("Acc12345")]
            [InlineData("12345")]
            [InlineData("12345678901")]
            [InlineData("123456789")]
            [InlineData("1234567890")]
            public void Invalid_Number(string number)
            {
                Assert.Throws<DirectDebitException>(
                        () => new BankAccount(number, SORTCODE, NAME));
            }

            [Fact]
            public void SortCode_Must_Be_Six_Digits()
            {
                var account = new BankAccount(NUMBER, SORTCODE, NAME);
                Assert.Equal(SORTCODE, account.SortCode);
            }

            [Theory]
            [InlineData("12345")]
            [InlineData("1234567")]
            [InlineData("1234a6")]
            public void Invalid_SortCode(string sortcode)
            {
                Assert.Throws<DirectDebitException>(
                        () => new BankAccount(NUMBER, sortcode, NAME));
            }

            [Fact]
            public void Account_Name_Must_AlphaNumeric()
            {
                var account = new BankAccount(NUMBER, SORTCODE, NAME);
                Assert.Equal(NAME, account.Name);
            }

            [Theory]
            [InlineData("Mr_Bob_Hoskins")]
            [InlineData("Arnie $%@")]
            [InlineData("")]
            public void Invalid_Name(string name)
            {
                Assert.Throws<DirectDebitException>(
                        () => new BankAccount(NUMBER, SORTCODE, name));
            }

            [Theory]
            [InlineData("Mr Bob & Benetta")]
            [InlineData("This. Is. Valid.")]
            [InlineData("Chrissy/Janet")]
            [InlineData("Bob - and - Janet")]
            public void Valid_Special_Chars(string name)
            {
                var account = new BankAccount(NUMBER, SORTCODE, name);
                Assert.Equal(name, account.Name);
            }
        }

        public class Ctor_With_Bank
        {
            [Theory]
            [InlineData("123456")]
            [InlineData("1234567")]
            [InlineData("12345678")]
            public void All_Banks_6_Eight_Digits(string number)
            {
                var account = new BankAccount(number, SORTCODE, NAME, Bank.Natwest);
                Assert.Equal(number, account.Number);
                Assert.Equal(Bank.Natwest, account.Bank);
            }

            [Theory]
            [InlineData(Bank.AllianceAndLeicester)]
            [InlineData(Bank.NationalSavings)]
            [InlineData(Bank.NavionalAndProvincial)]
            public void Valid_Banks_For_Nine_Number(Bank bank)
            {
                var number = "123456789";
                var account = new BankAccount(number, SORTCODE, NAME, bank);

                Assert.Equal(number, account.Number);
                Assert.Equal(bank, account.Bank);
            }

            [Theory]
            [InlineData(Bank.Natwest)]
            [InlineData(Bank.Coop)]
            [InlineData(Bank.Other)]
            public void Invalid_Banks_For_Nine_Number(Bank bank)
            {
                var number = "123456789";

                Assert.Throws<DirectDebitException>(
                        () => new BankAccount(number, SORTCODE, NAME, bank));
            }

            [Theory]
            [InlineData(Bank.Natwest)]
            [InlineData(Bank.Coop)]
            public void Valid_Banks_For_Ten_Number(Bank bank)
            {
                var number = "1324567890";
                var account = new BankAccount(number, SORTCODE, NAME, bank);

                Assert.Equal(number, account.Number);
                Assert.Equal(bank, account.Bank);
            }

            [Theory]
            [InlineData(Bank.AllianceAndLeicester)]
            [InlineData(Bank.NationalSavings)]
            [InlineData(Bank.NavionalAndProvincial)]
            [InlineData(Bank.Other)]
            public void Invalid_Banks_For_Ten_Number(Bank bank)
            {
                var number = "1234567890";

                Assert.Throws<DirectDebitException>(
                        () => new BankAccount(number, SORTCODE, NAME, bank));
            }

            [Theory]
            [InlineData("12345")]
            [InlineData("12345678900")]
            [InlineData("1234567a")]
            public void Invalid_Number(string number)
            {
                Assert.Throws<DirectDebitException>(
                        () => new BankAccount(number, SORTCODE, NAME, Bank.Natwest));
            }
        }

        public new class Equals
        {
            [Fact]
            public void Match_On_Bank_Account_And_SORTCODE()
            {
                var account1 = new BankAccount(NUMBER, SORTCODE, NAME);
                var account2 = new BankAccount(NUMBER, SORTCODE, NAME);

                Assert.Equal(account1, account2);
            }

            [Fact]
            public void Different_Bank_Account()
            {
                var account1 = new BankAccount("87654321", SORTCODE, NAME);
                var account2 = new BankAccount(NUMBER, SORTCODE, NAME);

                Assert.NotEqual(account1, account2);
            }

            [Fact]
            public void Difference_Sort_Code()
            {
                var account1 = new BankAccount(NUMBER, SORTCODE, NAME);
                var account2 = new BankAccount(NUMBER, "654321", NAME);

                Assert.NotEqual(account1, account2);
            }

            [Fact]
            public void Account_Name_Irrelevant()
            {
                var account1 = new BankAccount(NUMBER, SORTCODE, "Another Account Name");
                var account2 = new BankAccount(NUMBER, SORTCODE, NAME);

                Assert.Equal(account1, account2);
            }
        }

        public class Serialize
        {
            public class Number
            {
                [Theory]
                [InlineData("123456", "00123456", Bank.Other)]
                [InlineData("1234567", "01234567", Bank.Other)]
                [InlineData("12345678", "12345678", Bank.Other)]
                [InlineData("123456789", "23456789", Bank.AllianceAndLeicester)]
                [InlineData("123456789", "23456789", Bank.NationalSavings)]
                [InlineData("123456789", "23456789", Bank.NavionalAndProvincial)]
                [InlineData("1234567890", "34567890", Bank.Natwest)]
                [InlineData("1234567890", "12345678", Bank.Coop)]
                public void Valid_Data(string number, string expected, Bank bank)
                {
                    var account = new BankAccount(number, SORTCODE, NAME, bank);

                    var serialize = account.Serialize(SerializeMethod.FixedWidth);

                    Assert.Equal(expected, serialize.Number);
                }
            }

            public class SortCode
            {
                [Fact]
                public void SixDigits()
                {
                    var account = new BankAccount(NUMBER, SORTCODE, NAME);

                    var serialize = account.Serialize(SerializeMethod.FixedWidth);

                    Assert.Equal(SORTCODE, serialize.SortCode);
                }
            }

            public class AccountName
            {
                [Fact]
                public void Uppercase()
                {
                    var account = new BankAccount(NUMBER, SORTCODE, NAME);

                    var serialize = account.Serialize(SerializeMethod.FixedWidth);

                    Assert.True(Regex.IsMatch(serialize.Name, @"^[A-Z\s]+$"));
                }

                [Fact]
                public void Long_Names_Truncate()
                {
                    var longname = "Mr Bob Johnny Martin Horrocks III";
                    var account = new BankAccount(NUMBER, SORTCODE, longname);
                    var serialize = account.Serialize(SerializeMethod.FixedWidth);

                    Assert.Equal("MR BOB JOHNNY MART", serialize.Name);
                }

                [Fact]
                public void Short_Name_Pad()
                {
                    var shortname = "Bob";
                    var account = new BankAccount(NUMBER, SORTCODE, shortname);
                    var serialize = account.Serialize(SerializeMethod.FixedWidth);

                    Assert.Equal("BOB               ", serialize.Name);
                }
            }

            public class Line
            {
                [Fact]
                public void Default()
                {
                    var account = new BankAccount(BankAccountTest.NUMBER, 
                            BankAccountTest.SORTCODE, BankAccountTest.NAME);

                    var serialize = account.Serialize(SerializeMethod.FixedWidth);

                    var composed = serialize.SortCode + serialize.Number + serialize.Name;

                    Assert.Equal(composed, serialize.Line);
                }

                [Theory]
                [InlineData("Number", "Name", "SortCode")]
                [InlineData("Name", "SortCode", "Number")]
                public void Properties(string prop1, string prop2, string prop3)
                {
                    var properties = new [] { prop1, prop2, prop3 };

                    var account = new BankAccount(BankAccountTest.NUMBER, 
                            BankAccountTest.SORTCODE, BankAccountTest.NAME);

                    var serialize = account.Serialize(SerializeMethod.FixedWidth, properties);

                    var composed = "";
                    foreach(var prop in properties)
                    {
                        var p = typeof(ISerializedAccount).GetProperty(prop);
                        composed += p.GetValue(serialize, null).ToString();
                    }

                    Assert.Equal(composed, serialize.Line);
                }

                [Fact]
                public void Configuration()
                {
                    var configuration = new DirectDebitConfiguration();
                    configuration.BankAccount.Add(new FieldConfiguration { Field = "Number" });

                    var account = new BankAccount(BankAccountTest.NUMBER,
                            BankAccountTest.SORTCODE, BankAccountTest.NAME);

                    var serialize = account.Serialize(SerializeMethod.FixedWidth, configuration);

                    Assert.Equal(serialize.Number, serialize.Line);
                }

            }

       }

        public static BankAccount SampleOriginator()
        {
            var number = "12345678";
            var sortCode = "123456";
            var name = "Account Name";

            return new BankAccount(number, sortCode, name);
        }

        public static BankAccount SampleDestination()
        {
            var number = "87654321";
            var sortCode = "654321";
            var name = "Account Name";

            return new BankAccount(number, sortCode, name);
        }
    }
}
