using System;

namespace OrangeTentacle.DirectDebitAlbany
{
    public class DirectDebitException : Exception
    {
        public DirectDebitException()
            : base()
        {
        }

        public DirectDebitException(string message)
            : base(message)
        {
        }
    }

}
