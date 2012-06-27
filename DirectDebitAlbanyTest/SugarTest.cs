using OrangeTentacle.DirectDebitAlbany;
using Xunit;

namespace OrangeTentacle.DirectDebitAlbany.Test
{
    public class SugarTest
    {
        public class FixedWidth_String
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

        public class FixedWidth_Decimal
        {
            [Fact]
            public void Null()
            {
                decimal? dec = null;

                var result = dec.FixedWidth(4);

                Assert.Equal("0000", result);
            }

            [Fact]
            public void InPence()
            {
                decimal? dec = 123.45m;

                var result = dec.FixedWidth(5);

                Assert.Equal("12345", result);
            }

            [Fact]
            public void InPounds()
            {
                decimal? dec = 123m;

                var result = dec.FixedWidth(5);

                Assert.Equal("12300", result);
            }

            [Fact]
            public void Precision_Exception()
            {
                decimal? dec = 123.456m;

                Assert.Throws<DirectDebitException>(
                        () => dec.FixedWidth(5));
            }

            [Fact]
            public void TooLarge_Exception()
            {
                decimal? dec = 1234.56m;

                Assert.Throws<DirectDebitException>(
                        () => dec.FixedWidth(5));
            }


        }
    }
}
