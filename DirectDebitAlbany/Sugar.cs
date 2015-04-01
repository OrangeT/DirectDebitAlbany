using System;
using System.Reflection;
using System.Text;
using System.Linq;

namespace OrangeTentacle.DirectDebitAlbany
{
    public static class Sugar
    {
        public static string Format(this String str, SerializeMethod method, int length)
        {
            if (method == SerializeMethod.FixedWidth)
                return str.FixedWidth(length);
            if (method == SerializeMethod.CSV)
                return str.Truncate(length);

            throw new ArgumentOutOfRangeException("method", method, "Unknown SerializeMethod");
        }

        public static string Truncate(this String str, int length)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            var val = str.ToUpper();

            if (val.Length > length)
                val = val.Substring(0, length);

            return val;
        }

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

        public static string Format(this decimal? dec, SerializeMethod method, 
                int length, int precision)
        {
            if (method == SerializeMethod.FixedWidth)
                return dec.FixedWidth(length);
            if (method == SerializeMethod.CSV)
                return dec.ToFixedPrecision(precision);

            throw new ArgumentOutOfRangeException("method", method, "Unknown SerializeMethod");
            
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

        public static string ToFixedPrecision(this decimal? dec, int places)
        {
            dec = dec ?? 0;

            var format = places > 0
                ? "0." + string.Join("", Enumerable.Repeat("0", places))
                : "0";

            var factor = (int)Math.Pow(10, places);
            dec = Math.Floor(dec.Value * factor) / factor;

            return dec.Value.ToString(format);
        }

        public static string ComposeLine<T>(SerializeMethod method, string[] fields, object target)
        {
            if (fields.Length == 0)
                return string.Empty;

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

                var localTarget = target;

                string val = "";
                if (field.ToUpper() != "BLANK")
                {
                    string outer = "";
                    string inner = "";

                    PropertyInfo p;
                    if (field.Contains(".")) {
                        var properties = field.Split('.');

                        outer = properties[0];
                        inner = properties[1];

                        p = localTarget.GetType().GetProperty(outer,
                                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                        if (p == null)
                            throw new DirectDebitException(
                                    string.Format("Object Not Found :{0}", field));

                        localTarget = p.GetValue(localTarget, null);
                    }
                    else
                    {
                        inner = field;
                    }

                    p = localTarget.GetType().GetProperty(inner, 
                            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (p == null)
                        throw new DirectDebitException(
                                string.Format("Property Not Found {0}", inner));

                    val = p.GetValue(localTarget, null).ToString();
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
