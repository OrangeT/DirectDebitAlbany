using System;

namespace OrangeTentacle.DirectDebitAlbany
{
    public interface ISerializedAccount
    {
        string Number { get; }
        string SortCode { get; }
        string Name { get; }
        string Line { get; }
    }

    public class SerializedAccount : ISerializedAccount
    {
        internal SerializedAccount()
        {}

        public readonly static string[] DEFAULT_FIELDS = new [] { "SortCode", "Number", "Name" };

        public string Number { get; internal set; }
        public string SortCode { get; internal set; }
        public string Name { get; internal set; }
        public string Line { get; internal set; }
   }
}
