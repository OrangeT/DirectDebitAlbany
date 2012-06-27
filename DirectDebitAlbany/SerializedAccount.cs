using System;
using System.Reflection;
using System.Text;

namespace OrangeTentacle.DirectDebitAlbany
{
    public interface ISerializedAccount
    {
        string Number { get; }
        string SortCode { get; }
        string Name { get; }
        string Line();
        string Line(string[] fields);
    }

    public class SerializedAccount : ISerializedAccount
    {
        internal SerializedAccount()
        {}

        public readonly string[] DEFAULT_FIELDS = new [] { "SortCode", "Number", "Name" };

        public string Number { get; internal set; }
        public string SortCode { get; internal set; }
        public string Name { get; internal set; }

        public string Line()
        {
            return Line(DEFAULT_FIELDS);
        }

        public string Line(string[] fields)
        {
            var composed = new StringBuilder();
            
            foreach(var field in fields)
            {
                if (field.ToUpper() == "LINE")
                    throw new DirectDebitException("Parameters may not contain Line");

                var p = typeof(SerializedAccount).GetProperty(field, 
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (p == null)
                    throw new DirectDebitException("Property Not Found");

                composed.Append(p.GetValue(this, null).ToString());
            }

            return composed.ToString();
        }
    }
}
