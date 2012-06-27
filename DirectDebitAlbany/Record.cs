using System;

namespace OrangeTentacle.DirectDebitAlbany
{
   public interface IRecord
   {
        TransCode TransCode { get; }
        decimal? Amount { get; }
        string Reference { get; }

        IBankAccount Originator { get; }
        IBankAccount Destination { get; }

        ISerializedRecord Serialize();
   }

   public class Record
   {
       public TransCode TransCode { get; protected set; }
       public decimal? Amount { get; protected set; }
       public string Reference { get; protected set; }

       public IBankAccount Originator { get; protected set; }
       public IBankAccount Destination { get; protected set; }

        public Record(IBankAccount originator, IBankAccount destination, TransCode code,
                decimal? amount, string reference)
        {
            if (originator == null)
                throw new DirectDebitException("Originator must not be null");
            if (destination == null)
                throw new DirectDebitException("Destination must not be null");
            if (string.IsNullOrEmpty(reference))
                throw new DirectDebitException("Reference must not be null or empty");
            if (originator.Equals(destination))
                throw new DirectDebitException("Originator and Destination must not be the same");

            TransCode = code;
            Amount = amount;
            Reference = reference;
            Originator = originator;
            Destination = destination;
        }

        public ISerializedRecord Serialize()
        {
            var record = new SerializedRecord();

            record.TransCode = BankValidation.TransCode[this.TransCode];
            record.Amount = Amount.FixedWidth(11);

            record.Reference = Reference.FixedWidth(18);

            record.Originator = Originator.Serialize().Line();
            record.Destination = Destination.Serialize().Line();

            return record;
        }
   } 
}
