using OrangeTentacle.DirectDebitAlbany;
using Xunit;

namespace OrangeTentacle.DirectDebitAlbany.Test
{
    public class BankAccountTest
    {
        public class Equals
        {
            public const string NUMBER = "12345678";
            public const string SORTCODE = "123456";
            public const string NAME = "Sample Account NAME";

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
