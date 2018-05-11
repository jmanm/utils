using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Appends a delimiter and value to this instance.
        /// </summary>
        /// <param name="target">The target StringBuilder instance.</param>
        /// <param name="token">The value to append.</param>
        /// <param name="delimiter">The character which delimits tokens in this instance.</param>
        /// <returns>The target instance.</returns>
        public static StringBuilder AppendToken(this StringBuilder target, object token, char delimiter = ' ')
        {
            if (target == null)
                throw new NullReferenceException();
            if (token == null)
                throw new ArgumentNullException("token");
            if (target.Length > 0 && target[target.Length - 1] != delimiter)
                target.Append(delimiter);
            target.Append(token);
            return target;
        }

        /// <summary>
        /// Gets the token at the specified index.
        /// </summary>
        /// <param name="index">The index within this instance.</param>
        /// <param name="backTrack">If set to <c>true</c>, search backward instead of forward.</param>
        /// <returns>The token starting or ending at the specified index.</returns>
        public static string GetToken(this StringBuilder target, int index, bool backTrack = false)
        {
            if (index < 0 || index >= target.Length)
                throw new ArgumentOutOfRangeException("index");

            int p = index, start = 0;
            if (backTrack)
            {
                while (p > 0 && target[p] == ' ') p--;
                start = p;
                while (p > 0 && target[p] != ' ') p--;
                return target.ToString(p + 1, start - p);
            }
            else
            {
                while (p < target.Length && target[p] == ' ') p++;
                start = p;
                while (p < target.Length && target[p] != ' ') p++;
                return target.ToString(start, p - start);
            }
        }

        public static StringBuilder InsertToken(this StringBuilder target, int index, string token)
        {
            if (index < target.Length && target[index] != ' ')
                target.Insert(index, ' '); //space after
            if (index > 0 && target[index - 1] != ' ')
                target.Insert(index++, ' '); //space before
            target.Insert(index, token);
            return target;
        }

        /// <summary>
        /// Makes the target instance uppercase.
        /// </summary>
        /// <param name="target">The target <see cref="StringBuilder"/> instance.</param>
        /// <returns>The target instance with all alpha characters converted to uppercase.</returns>
        public static StringBuilder MakeUpperCase(this StringBuilder target)
        {
            for (int p = 0; p < target.Length; p++)
            {
                char c = target[p];
                if (c >= 'a' && c <= 'z')
                    target[p] = Char.ToUpper(c);
            }
            return target;
        }

        /// <summary>
        /// Removes unnecessary spaces from the target instance.
        /// </summary>
        /// <param name="target">The target <see cref="StringBuilder"/> instance.</param>
        /// <returns>The target instance with leading, trailing, and consecutive spaces removed.</returns>
        public static StringBuilder RemoveUnnecessarySpaces(this StringBuilder target)
        {
            int p = 0;
            while (p < target.Length)
            {
                char c = target[p];
                if (c == ' ' && (p == 0 || p == target.Length - 1 || (p + 1 < target.Length && target[p + 1] == ' ')))
                    target.Remove(p, 1);
                else
                    p++;
            }
            return target;
        }
    }
}
