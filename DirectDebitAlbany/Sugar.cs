using System;
using System.Reflection;
using System.Text;

namespace OrangeTentacle.DirectDebitAlbany
{
    public static class Sugar
    {
        public static string FixedWidth(this String str, int length)
        {
             if (string.IsNullOrEmpty(str))
                return string.Empty.PadLeft(length);

             var val = str.ToUpper();
            
             if (val.Length > length)
                 val = val.Substring(0, length);
             else if (val.Length < length)
                 val = val.PadRight(length);

             return val;
        }

        public static string FixedWidth(this decimal? dec, int length)
        {
            var val = "0";

            if (dec.HasValue)
            {
                var pence = dec.Value*100;
                if (pence % 1 != 0)
                    throw new DirectDebitException("Amount Not Expressed In Pence");

                val = (dec.Value*100).ToString("0");
            }

            if (val.Length > length)
                throw new DirectDebitException("Length exceeded by amount");

            return val.PadLeft(length, '0');
        }

        public static string ComposeLine<T>(SerializeMethod method, string[] fields, object target)
        {
            var composed = new StringBuilder();
            
            var lastIndex = fields.Length - 1;
            var lastField = fields[lastIndex];

            foreach(var field in fields)
            {
                if (field.ToUpper() == "LINE")
                    throw new DirectDebitException("Parameters may not contain Line");

                //if (field.ToUpper() == "DESTINATION" || field.ToUpper() == "ORIGINATOR")
                    //continue;
                    //

                string val = "";
                if (field.ToUpper() != "BLANK")
                {
                    var p = typeof(T).GetProperty(field, 
                            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (p == null)
                        throw new DirectDebitException("Property Not Found");

                    val = p.GetValue(target, null).ToString();
                }

                if (method == SerializeMethod.CSV) {
                    if (val.Contains(","))
                    {
                        val = string.Format("\"{0}\"", val);
                    }

                    if (field != lastField) {
                        val = val + ",";
                    }
                }

                composed.Append(val);
            }

            return composed.ToString();
        }

    }
}
