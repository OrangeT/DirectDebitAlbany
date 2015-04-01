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
        ISerializedRecord Serialize(DirectDebitConfiguration config);
        ISerializedRecord Serialize(string[] accountFields, string[] recordFields);
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

        public ISerializedRecord Serialize(SerializeMethod method)
        {
            var accountFields = SerializedAccount.DEFAULT_FIELDS;
            var recordFields = SerializedRecord.DEFAULT_FIELDS;

            return Serialize(method, accountFields, recordFields);
        }

        public ISerializedRecord Serialize(SerializeMethod method, DirectDebitConfiguration config)
        {
            var accountFields = config.BankAccount.GetProperties();
            var recordFields = config.Record.GetProperties();

            return Serialize(method, accountFields, recordFields);
        }

        public ISerializedRecord Serialize(SerializeMethod method, string[] accountFields, 
                string[] recordFields)
        {
            var record = new SerializedRecord();

            record.TransCode = BankValidation.TransCode[this.TransCode];
            record.Amount = Amount.FixedWidth(11);

            record.Reference = Reference.FixedWidth(18);

            record.Originator = Originator.Serialize(method, accountFields).Line; 
            record.Destination = Destination.Serialize(method, accountFields).Line; 

            record.Line = Sugar.ComposeLine<SerializedRecord>(method, recordFields, record);

            return record;
        }
   } 
}
