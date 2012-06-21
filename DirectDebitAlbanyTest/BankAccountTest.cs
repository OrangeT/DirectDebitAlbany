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
        }

        public class Ctor_With_Bank
        {
            [Theory]
            [InlineData("123456")]
            [InlineData("1234567")]
            [InlineData("12345678")]
            [InlineData("123456789")]
            [InlineData("1234567890")]
            public void Number_Between_Six_And_Ten_Digits(string number)
            {
                var account = new BankAccount(number, SORTCODE, NAME, Bank.Natwest);
                Assert.Equal(number, account.Number);
            }

            [Theory]
            [InlineData("12345")]
            [InlineData("12345678900")]
            [InlineData("1234567a")]
            public void Invalid_Number(string number)
            {
                Assert.Throws<DirectDebitException>(
                        () => new BankAccount(number, SORTCODE, NAME));
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
    }
}
