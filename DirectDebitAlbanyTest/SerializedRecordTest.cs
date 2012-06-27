using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Xunit;

namespace OrangeTentacle.DirectDebitAlbany.Test
{
    public class SerializedRecordTest
    {
        public class Line
        {
            [Fact]
            public void Compose()
            {
                var serialized = new Mock<SerializedRecord>()
                    .SetupProperty(x => x.Amount, "00000012345")
                    .SetupProperty(x => x.Reference, "               REF")
                    .SetupProperty(x => x.TransCode, "0C")
                    .Object;



            }
        }
    }
}
