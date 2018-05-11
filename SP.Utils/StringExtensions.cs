using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SP
{
    public enum TextDivisions { Line, Word, Letter }

    public static class StringExtensions
    {
        /// <summary>
        /// Creates a capitalized string.
        /// </summary>
        /// <param name="target">The extension method target.</param>
        /// <returns>A copy of this instance with the first letter capitalized.</returns>
        public static string Capitalize(this string target)
        {
            switch (target)
            {
                case null:
                    throw new NullReferenceException();
                case string s when String.IsNullOrWhiteSpace(s):
                    return s;
                case string s when s[0].IsNumeric():
                    int ix = 1;
                    while (ix < s.Length && s[ix].IsNumeric()) ix++;
                    string start = s.Substring(0, ix), end = s.Substring(ix);
                    string[] suffixes = { "st", "nd", "rd", "th", "ST", "ND", "RD", "TH" };
                    if (Array.IndexOf(suffixes, end) >= 0)
                        return start + end.ToLower();
                    return start + end.Capitalize();
                default:
                    return Char.ToUpper(target[0]) + target.Substring(1).ToLower();
            }
        }

        /// <summary>
        /// Counts the occurrences of the specified sub-string within this instance.
        /// </summary>
        /// <param name="target">The extension method target.</param>
        /// <param name="subString">The sub-string to count.</param>
        /// <returns>The number of times the sub-string is found.</returns>
        public static int Count(this string target, string subString)
        {
            if (target == null)
                throw new NullReferenceException();

            if (subString == null)
                throw new ArgumentNullException("subString");

            int result = 0, ix = 0 - subString.Length;
            do
            {
                ix = target.IndexOf(subString, ix + subString.Length);
                if (ix >= 0)
                    result++;
            } while (ix >= 0);

            return result;
        }

        /// <summary>
        /// Creates a de-capitalized string.
        /// </summary>
        /// <param name="target">The extension method target.</param>
        /// <returns>A copy of this instance with a lowercase first letter.</returns>
        public static string Decapitalize(this string target)
        {
            if (target == null)
                throw new NullReferenceException();

            if (target.Length == 0)
                return target;

            return Char.ToLower(target[0]) + target.Substring(1);
        }

        private static (int start, int length, string difference) DiffElements(string baseText, string compareText, string delimiter)
        {
            string[] baseElems = baseText.Split(new[] { delimiter }, StringSplitOptions.None),
                compareElems = compareText.Split(new[] { delimiter }, StringSplitOptions.None);
            var compareDiff = new StringBuilder();
            int index = 0, position = 0, start = -1, length = 0;
            string baseElem = "", compareElem = "";
            while (index < Math.Max(baseElems.Length, compareElems.Length))
            {
                if (index > 0)
                    position += baseElem.Length + delimiter.Length;

                (baseElem, compareElem) = NextElem(index);
                if (baseElem != compareElem)
                {
                    start = position;
                    while (index < Math.Max(baseElems.Length, compareElems.Length) && baseElem != compareElem)
                    {
                        if (length > 0 && baseElem.Length > 0)
                            length += delimiter.Length;
                        length += baseElem.Length;

                        if (compareDiff.Length > 0 && compareElem.Length > 0)
                            compareDiff.Append(delimiter);
                        compareDiff.Append(compareElem);

                        (baseElem, compareElem) = NextElem(++index);
                    }
                    break;
                }
                index++;
            }

            return (start, length, start >= 0 ? compareDiff.ToString() : null);

            (string, string) NextElem(int ix) => (ix < baseElems.Length ? baseElems[ix] : "", ix < compareElems.Length ? compareElems[ix] : "");
        }

        /// <summary>
        /// Determines the difference between the target instance and another string.
        /// </summary>
        /// <param name="target">The extension method target.</param>
        /// <param name="other">The string to compare to.</param>
        /// <param name="type">The type of comparison to perform.</param>
        /// <returns>The start index and length within the target instance and the content from the comparison string that differs 
        /// from the target instance; or if no difference is found, an index of <c>-1</c>.</returns>
        public static (int start, int length, string difference) DiffWith(this string target, string other, TextDivisions type = TextDivisions.Line)
        {
            if (target == null)
                throw new NullReferenceException();

            if (other == null)
                throw new ArgumentNullException("other");

            if (type == TextDivisions.Line || target.IndexOf('\n') >= 0)
                return LineDiff(target, other, type);
            if (type == TextDivisions.Word || target.IndexOf(' ') >= 0)
                return WordDiff(target, other, type);
            return LetterDiff(target, other);
        }

        private static (int start, int length, string difference) LetterDiff(string word1, string word2)
        {
            if (word1 == "" && word2 != "")
                return (0, 0, word2); //special case, the difference occurs at position 0 but also for 0 characters

            int start = -1, length = 0;
            char letter1 = Char.MinValue, letter2 = Char.MinValue;
            string difference = null;

            for (int ix = 0; ix < word1.Length; ix++)
            {
                letter1 = word1[ix];
                if (ix < word2.Length)
                    letter2 = word2[ix];

                if (letter1 != letter2)
                {
                    start = ix; //get the index of where the words differ
                    length = word1.Length - start;
                    difference = word2.Substring(start);
                    break;
                }
            }
            return (start, length, difference);
        }

        private static (int start, int length, string difference) LineDiff(string text1, string text2, TextDivisions type)
        {
            int start = -1, length = 0, s;
            string diff = null;
            (start, length, diff) = DiffElements(text1, text2, "\r\n");
            if (start >= 0 && type != TextDivisions.Line)
            {
                (s, length, diff) = WordDiff(text1.Substring(start, length), text2.Substring(start), type);
                start += s;
            }
            return (start, length, diff);
        }

        private static (int start, int length, string difference) WordDiff(string line1, string line2, TextDivisions type)
        {
            int start = -1, length, s;
            string diff = null;
            (start, length, diff) = DiffElements(line1, line2, " ");
            if (start >= 0 && type != TextDivisions.Word)
            {
                (s, length, diff) = LetterDiff(line1.Substring(start, length), line2.Substring(start));
                start += s;
            }
            return (start, length, diff);
        }

        /// <summary>
        /// Creates patterened string.
        /// </summary>
        /// <param name="target">The extension method target.</param>
        /// <param name="count">The number of times to repeat.</param>
        /// <returns>A new string containing this instance repeated the specified number of times.</returns>
        public static string Repeat(this string target, int count)
        {
            if (target == null)
                throw new NullReferenceException();

            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            if (count == 0)
                return "";

            var sb = new StringBuilder(target);
            for (int i = 1; i < count; i++)
                sb.Append(target);

            return sb.ToString();
        }

        /// <summary>
        /// Gets the first 'word' in a string as well the remaining content.
        /// </summary>
        /// <param name="target">The extension method target.</param>
        /// <param name="delimiter">The delimiter to indicate word boundaries.</param>
        /// <returns>The first token found and the substring following it, or a blank string if the delimiter is not present.</returns>
        public static (string first, string rest) GetToken(this string target, char delimiter = ' ')
        {
            if (target == null)
                throw new NullReferenceException();
            var ix = target.IndexOf(delimiter);
            if (ix >= 0)
                return (target.Substring(0, ix), target.Substring(ix + 1));
            return ("", target);
        }

        /// <summary>
        /// Converts a phrase separated by underscores to a one with the underscores removed and first 
        /// letter of each word capitalized. The remaining letters of each word are converted to lowercase.
        /// </summary>
        /// <param name="target">The target instance.</param>
        /// <returns>The target converted to camel case.</returns>
        public static string ToCamelCase(this string target)
        {
            if (target == null)
                throw new NullReferenceException();

            if (target.IndexOf('_') < 0)
                return target.ToLower().Capitalize();

            var words = target.Split('_');
            return String.Join("", words.Select(word => word.ToLower().Capitalize()));
        }

        /// <summary>
        /// Converts a singular word to its plural form.
        /// </summary>
        /// <param name="target">The target instance.</param>
        /// <returns>The target converted to a plural form.</returns>
        public static string ToPlural(this string target)
        {
            switch (target)
            {
                case null:
                    throw new NullReferenceException();
                case "":
                    return target;
                case string _ when target.Length == 1:
                    return target + "'s";
                case string _ when target.EndsWith("es") || (target[target.Length - 1] == 's' && target[target.Length - 2] != 's'):
                    return target;
                case string _ when target.EndsWith("ey"):
                    return target.Substring(0, target.Length - 2) + "ies";
                case string _ when target.EndsWith("y"):
                    return target.Substring(0, target.Length - 1) + "ies";
                case string _ when target.EndsWith("s"):
                    return target + "es";
                default:
                    return target + "s";
            }
        }

        /// <summary>
        /// Converts a plural word to its singular form.
        /// </summary>
        /// <param name="target">The target instance.</param>
        /// <returns>The target converted to a singular form.</returns>
        public static string ToSingular(this string target)
        {
            switch (target)
            {
                case null:
                    throw new NullReferenceException();
                case string s when String.IsNullOrWhiteSpace(s) || s.Length < 2 || s.EndsWith("ss"):
                    return s;
                case string s when s.EndsWith("ies"):
                    return s.Substring(0, s.Length - 3) + "y";
                case string s when s.EndsWith("ses"):
                    return s.Substring(0, s.Length - 2);
                case string s when s.EndsWith("s"):
                    return s.Substring(0, s.Length - 1);
                case string s when s[s.Length - 1] == 's' && (s[s.Length - 2] == 'e' || s[s.Length - 2].IsConsonant()):
                    return s.Substring(0, s.Length - 1);
                default:
                    return target;
            }
        }

        /// <summary>
        /// Converts a 'camel case' phrase into one with each word separated by an underscore ('_') and
        /// the remaining letters converted to lowercase.
        /// </summary>
        /// <param name="target">The target instance.</param>
        /// <returns>The target converted to an underscore phrase if it is a camel-case phrase; otherwise,
        /// the target converted to lowercase.</returns>
        public static string ToUnderscores(this string target)
        {
            switch (target)
            {
                case null:
                    throw new NullReferenceException();
                case string s when String.IsNullOrWhiteSpace(s):
                    return s;
                case String s when s.Length < 3:
                    return s.ToLower();
                default:
                    return UnCamelCase(target, true);
            }

            string UnCamelCase(string val, bool newWord)
            {
                switch (val)
                {
                    case string s when s.Length > 1 && !newWord && Char.IsUpper(s[0]) && Char.IsLower(s[1]):
                        return "_" + Char.ToLower(s[0]) + s[1] + UnCamelCase(s.Substring(2), false);
                    case string s when s.Length > 1:
                        return Char.ToLower(s[0]) + UnCamelCase(s.Substring(1), false);
                    default:
                        return val;
                }
            }
        }

        public static bool EndsWith(this string target, char value)
        {
            if (target == null)
                throw new ArgumentNullException();
            if (target == "") return false;
            return target[target.Length - 1] == value;
        }

        public static string Escape(this string target, params char[] charsToEscape)
        {
            if (target == null)
                throw new ArgumentNullException();
            if (target == "") return target;
            var result = target;
            int ix = 0;
            while ((ix = result.IndexOfAny(charsToEscape, ix)) >= 0)
                result.Insert(ix, @"\");
            return result;
        }

        public static bool SoundsLike(this string target, string other) => new DoubleMetaphone(target).PrimaryKey == new DoubleMetaphone(other).PrimaryKey;

        public static bool StartsWith(this string target, char value)
        {
            if (target == null)
                throw new ArgumentNullException();
            if (target == "") return false;
            return target[0] == value;
        }

        public static string ToProperCase(this string target)
        {
            switch (target)
            {
                case null:
                    throw new NullReferenceException();
                case string s when String.IsNullOrWhiteSpace(s):
                    return s;
                case string s when !s.Contains(' '):
                    return s.Capitalize();
                default:
                    var sb = new StringBuilder();
                    foreach (string word in target.Split(' '))
                        if (word != "")
                        {
                            sb.Append(word.Capitalize());
                            sb.Append(' ');
                        }
                    if (sb.Length > 0 && sb[sb.Length - 1] == ' ')
                        sb.Remove(sb.Length - 1, 1);
                    return sb.ToString();
            }
        }

        public static string ToQuotedString(this string target, char quoteChar = '\'')
        {
            if (target == null)
                throw new ArgumentNullException();
            string qc = quoteChar.ToString();
            return qc + target.Replace(qc, qc + qc) + qc;
        }

        public static string ToUnQuotedString(this string target, char quoteChar = '\'')
        {
            if (target == null)
                throw new ArgumentNullException();
            if (target.Length > 1 && target.StartsWith(quoteChar) && target.EndsWith(quoteChar))
            {
                var unQuoted = target.Substring(1, target.Length - 2);
                var qc = quoteChar.ToString();
                return unQuoted.Replace(qc + qc, qc);
            }
            return target;
        }
    }
}
