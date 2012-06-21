using System;

namespace OrangeTentacle.DirectDebitAlbany
{
   public class Record
   {
        public Record(Originator originator, Destination destination, TransCode code,
                decimal amount, string reference)
        {
            if (originator == null)
                throw new DirectDebitException("Originator must not be null");
            if (destination == null)
                throw new DirectDebitException("Destination must not be null");
            if (string.IsNullOrEmpty(reference))
                throw new DirectDebitException("Reference must not be null or empty");
            if (originator.Equals(destination))
                throw new DirectDebitException("Originator and Destination must not be the same");
        }
   } 
}
