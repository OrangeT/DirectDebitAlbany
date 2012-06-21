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

    }
}
