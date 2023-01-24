using System.Collections.Generic;

namespace JsonParser
{
    /// <summary>
    /// Represents a series of Hash generation utility methods.
    /// </summary>
    //Base Hash function taken from: https://stackoverflow.com/questions/1660501/what-is-a-good-64bit-hash-function-in-java-for-textual-strings
    public static class HashUtils
    {
        /// <summary>
        /// The hashing basis value.
        /// </summary>
        private const long HashBasis = 1125899906842597L;

        /// <summary>
        /// Calculates a Long Hash using the given String.
        /// </summary>
        /// <param name="chars">The String used to calculate the Hash.</param>
        /// <returns>The calculated Hash.</returns>
        public static long GetHash(string chars)
        {
            var h = HashBasis;
            for (var i = 0; i < chars.Length; i++)
            {
                h = 31 * h + chars[i];
            }
            return h;
        }

        /// <summary>
        /// Calculates a Long Hash using the given Char Array.
        /// </summary>
        /// <param name="chars">The Char Array used to calculate the Hash.</param>
        /// <param name="count">The Char Array count.</param>
        /// <returns>The calculated Hash.</returns>
        public static long GetHash(IList<char> chars, int count)
        {
            var h = HashBasis;
            for (var i = 0; i < count; i++)
            {
                h = 31 * h + chars[i];
            }
            return h;
        }

        /// <summary>
        /// Calculates a Long Hash using the given Byte Array.
        /// </summary>
        /// <param name="bytes">The Byte Array used to calculate the Hash.</param>
        /// <param name="count">The Byte Array count.</param>
        /// <returns>The calculated Hash.</returns>
        public static long GetHash(IList<byte> bytes, int count)
        {
            var h = HashBasis;
            for (var i = 0; i < count; i++)
            {
                h = 31 * h + bytes[i];
            }
            return h;
        }

        /// <summary>
        /// Gets the hashing base value.
        /// </summary>
        /// <returns>The Hash base value.</returns>
        public static long GetHashInitialValue()
        {
            var h = HashBasis;
            return h;
        }

        /// <summary>
        /// Calculates a Long Hash from the given Value, using the given Hash as basis.
        /// </summary>
        /// <param name="hash">The Hash basis value.</param>
        /// <param name="value">The value to Hash.</param>
        /// <returns>The calculated Hash.</returns>
        public static long GetHash(long hash, int value)
        {
            return 31 * hash + value;
        }
    }
}
