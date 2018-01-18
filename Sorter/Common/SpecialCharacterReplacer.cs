using System;
using System.Text.RegularExpressions;

namespace Sorter.Common
{
    public class SpecialCharacterReplacer
    {
        public static string ReplaceDanishChar(string originalString)
        {
            string pattern = @"[æøå]";
            if(Regex.IsMatch(originalString, pattern))
            {
                originalString = originalString.Replace("æ", "ae");
                originalString = originalString.Replace("ø", "oe");
                originalString = originalString.Replace("å", "aa");
            }

            return originalString;
        }
    }
}