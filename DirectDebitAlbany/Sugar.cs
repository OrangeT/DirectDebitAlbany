using System;

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
    }
}
