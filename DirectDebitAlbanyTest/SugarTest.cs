using OrangeTentacle.DirectDebitAlbany;
using Xunit;

namespace OrangeTentacle.DirectDebitAlbany.Test
{
    public class SugarTest
    {
        public class FixedWidth
        {
            [Fact]
            public void Null()
            {
                string str = null;
                
                var result = str.FixedWidth(4);

                Assert.Equal("    ", result);
            }

            [Fact]
            public void EmptyString()
            {
                var result = string.Empty.FixedWidth(4);

                Assert.Equal("    ", result);
            }

            [Fact]
            public void Uppercase()
            {
                var str = "abcd";

                var result = str.FixedWidth(4);

                Assert.Equal("ABCD", result);
            }

            [Fact]
            public void Truncate()
            {
                var str = "ABCDE";

                var result = str.FixedWidth(4);

                Assert.Equal("ABCD", result);
            }

            [Fact]
            public void Pad()
            {
                var str = "ABC";

                var result = str.FixedWidth(4);

                Assert.Equal("ABC ", result);
            }
        }
    }
}
