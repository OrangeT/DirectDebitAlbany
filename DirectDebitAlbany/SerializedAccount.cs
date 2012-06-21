namespace OrangeTentacle.DirectDebitAlbany
{
    public class SerializedAccount
    {
        internal SerializedAccount()
        {}

        public string Number { get; set; }
        public string SortCode { get; set; }
        public string Name { get; set; }

        public string Line 
        {
            get { return SortCode + Number + Name; }
        }
    }
}
