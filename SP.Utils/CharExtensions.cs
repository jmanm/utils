using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP
{
    public static class CharExtensions
    {
        /// <summary>
        /// Indicates whether a character is a consonant or not.
        /// </summary>
        /// <param name="target">The target <see cref="Char"/>.</param>
        /// <returns><c>true</c> if the target is a consonant, otherwise <c>false</c>.</returns>
        public static bool IsConsonant(this char target) => "bcdfghjklmnpqrstvwxyz".Contains(target);
        public static bool IsNumeric(this char target) => target >= '0' && target <= '9';

        /// <summary>
        /// Creates a string of repeated characters.
        /// </summary>
        /// <param name="target">The target <see cref="Char"/>.</param>
        /// <param name="count">The number of times to repeat the target.</param>
        /// <returns>A <see cref="String"/> containing the target repeated the specified number of times.</returns>
        public static string Repeat(this char target, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
            if (count == 0)
                return "";
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++)
                sb.Append(target);
            return sb.ToString();
        }
    }
}
