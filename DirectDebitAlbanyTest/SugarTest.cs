using OrangeTentacle.DirectDebitAlbany;
using Xunit;

namespace OrangeTentacle.DirectDebitAlbany.Test
{
    public class SugarTest
    {
        public class Truncate_string
        {
            [Fact]
            public void Null()
            {
                string str = null;

                var result = str.Truncate(4);

                Assert.Equal("", result);
            }

            [Fact]
            public void EmptyString()
            {
                string str = null;

                var result = str.Truncate(4);

                Assert.Equal("", result);
            }

            [Fact]
            public void Uppercase()
            {
                string str = "bob";

                var result = str.Truncate(4);

                Assert.Equal("BOB", result);
            }

            [Fact]
            public void Truncate()
            {
                var str = "ABCDE";

                var result = str.Truncate(4);

                Assert.Equal("ABCD", result);
            }
        }

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

        public class ToFixedPrecision
        {
            [Fact]
            public void Null()
            {
                decimal? dec = null;

                var result = dec.ToFixedPrecision(2);

                Assert.Equal("0.00", result);
            }

            [Fact]
            public void Zero_Places()
            {
                decimal? dec = 123.54m;

                var result = dec.ToFixedPrecision(0);

                Assert.Equal("123", result);
            }

            [Fact]
            public void Two_Places()
            {
                decimal? dec = 123.545m;

                var result = dec.ToFixedPrecision(2);

                Assert.Equal("123.54", result);
            }

            [Fact]
            public void Longer_Precision()
            {
                decimal? dec = 123.54m;

                var result = dec.ToFixedPrecision(5);

                Assert.Equal("123.54000", result);
            }
        }

        public class ComposeLine
        {
            public class FixedWidth
            {
                [Fact]
                public void Takes_Properties()
                {
                    var properties = new [] { "Property1", "Property2" };
                    var target = new SimpleClass { Property1 = "Property1", Property2 = 32 };

                    var line = Sugar.ComposeLine<SimpleClass>(SerializeMethod.FixedWidth, 
                            properties, target);

                    Assert.Equal("Property132", line);
                }

                [Fact]
                public void Ignores_Case()
                {
                    var properties = new [] { "PROPERTY1" };
                    var target = new SimpleClass { Property1 = "Property1", Property2 = 32 };

                    var line = Sugar.ComposeLine<SimpleClass>(SerializeMethod.FixedWidth, 
                            properties, target);

                    Assert.Equal("Property1", line);
                }

                [Fact]
                public void Properties_Are_Ordered()
                {
                    var properties = new [] { "Property2", "Property1" };
                    var target = new SimpleClass { Property1 = "Property1", Property2 = 32 };

                    var line = Sugar.ComposeLine<SimpleClass>(SerializeMethod.FixedWidth,
                            properties, target);

                    Assert.Equal("32Property1", line);
                }

                [Fact]
                public void Rejects_Line()
                {
                    var properties = new [] { "Line" };
                    var target = new SimpleClass { Property1 = "Property1", Property2 = 32 };

                    Assert.Throws<DirectDebitException>(
                            () => Sugar.ComposeLine<SimpleClass>(SerializeMethod.FixedWidth,
                                                                 properties, target));
                }

                [Fact]
                public void Rejects_Null_Properties()
                {
                    var properties = new [] { "Property30" };
                    var target = new SimpleClass { Property1 = "Property1", Property2 = 32 };

                    Assert.Throws<DirectDebitException>(
                            () => Sugar.ComposeLine<SimpleClass>(SerializeMethod.FixedWidth,
                                                                 properties, target));
                }
            }

            public class CSV
            {
                [Fact]
                public void Takes_Properties()
                {
                    var properties = new [] { "Property1", "Property2" };
                    var target = new SimpleClass { Property1 = "Property1", Property2 = 32 };

                    var line = Sugar.ComposeLine<SimpleClass>(SerializeMethod.CSV, 
                            properties, target);

                    Assert.Equal("Property1,32", line);
                }

                [Fact]
                public void Inner_Properties()
                {
                    var properties = new [] { "Property3.Property1" };
                    var target = new SimpleClass { Property1 = "Property1", Property2 = 32,
                        Property3 = new InnerClass { Property1 = "Property3" } };

                    var line = Sugar.ComposeLine<SimpleClass>(SerializeMethod.CSV, 
                            properties, target);

                    Assert.Equal("Property3", line);
                }

                [Fact]
                public void Escapes_Commas()
                {
                    var properties = new [] { "Property1", "Property2" };
                    var target = new SimpleClass { Property1 = "Proper,ty1", Property2 = 32 };

                    var line = Sugar.ComposeLine<SimpleClass>(SerializeMethod.CSV, 
                            properties, target);

                    Assert.Equal("\"Proper,ty1\",32", line);
                }

                [Fact]
                public void Generate_Blank_Entries()
                {
                    var properties = new [] { "Property1", "Blank", "Property2"};
                    var target = new SimpleClass { Property1 = "Property1", Property2 = 32 };

                    var line = Sugar.ComposeLine<SimpleClass>(SerializeMethod.CSV, 
                            properties, target);

                    Assert.Equal("Property1,,32", line);
                }
            }


            public class SimpleClass
            {
                public string Line { get; set; }
                public string Property1 { get; set; }
                public int Property2 { get; set; }

                public InnerClass Property3 { get; set; }
            }

            public class InnerClass
            {
                public string Property1 { get; set; }
            }
        }
    }
}
