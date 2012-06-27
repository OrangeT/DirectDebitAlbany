using System;
using Xunit;

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
            }
 
    }
}
