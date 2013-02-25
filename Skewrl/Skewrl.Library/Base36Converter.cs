using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Skewrl.Library
{
    public static class Base36Converter
    {
        private const String Base36Chart_ = "0123456789abcdefghijklmnopqrstuvwxyz";

        //Code borrowed from https://github.com/mstum/mstum.utils
        public static String Encode(ulong numberToEncode)
        {
            if (numberToEncode < 0) return null;

            Char[] cArray = Base36Chart_.ToArray();

            var result = new Stack<char>();
            while (numberToEncode != 0)
            {
                result.Push(cArray[numberToEncode % 36]);
                numberToEncode /= 36;
            }
            return new string(result.ToArray());

        }

        public static ulong Decode(String input)
        {
            ulong number = 0;
            foreach (char c in input)
                number = number * 36 + (ulong)Base36Chart_.IndexOf(c);

            return number;
        }

    }
}
