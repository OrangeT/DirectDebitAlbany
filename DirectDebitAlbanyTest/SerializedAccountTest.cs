using System;
using Xunit;

namespace OrangeTentacle.DirectDebitAlbany.Test
{
    public class SerializedAccountTest
    {
            public class Line
            {
                [Fact]
                public void Composed()
                {
                    var account = new BankAccount(BankAccountTest.NUMBER, BankAccountTest.SORTCODE, BankAccountTest.NAME);

                    var serialize = account.Serialize();

                    Assert.Equal(serialize.SortCode, serialize.Line.Substring(0, 6));
                    Assert.Equal(serialize.Number, serialize.Line.Substring(6, 8));
                    Assert.Equal(serialize.Name, serialize.Line.Substring(14, 18));
                }
            }
 
    }
}
