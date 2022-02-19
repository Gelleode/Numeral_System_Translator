using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Numeral_System_Translator
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            string answer, Number = "251,513";
            int toBase = 16;
            int fromBase = 8;
            int accuracy = 6;
            stopwatch.Start();
            answer = AnyToAny(Number, toBase, fromBase, accuracy);
            stopwatch.Stop();
            Console.WriteLine(answer);
            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
        }

        public static string ConvertToDecimal(string number, int fromBase)
        {
            string[] num = number.Split(',');
            double numberBeforeFloat = 0;
            for (int i = 0; i<num[0].Length; i++)
                numberBeforeFloat = numberBeforeFloat + Convert.ToInt32(num[0][i].ToString(), fromBase) * Math.Pow(fromBase, num[0].Length-i-1);
            if (num.Length == 1)
                return numberBeforeFloat.ToString();
            else
            {
                double numberAfterFloat = 0;
                for (int i = 0; i < num[1].Length; i++)
                    numberAfterFloat = numberAfterFloat + Convert.ToInt32(num[1][i].ToString(), fromBase) * Math.Pow(fromBase, -1*i-1);
                return (numberBeforeFloat + numberAfterFloat).ToString();
            }
        }
        public static string ConvertFromDecimal(string number, int toBase, int accuracy)
        {
            string[] num = number.Split(',');
            long temp_i = Convert.ToInt64(num[0]);
            string numBeforeFloat = null;
            while (temp_i > 0)
            {
                numBeforeFloat = (temp_i % toBase).ToString("X") + numBeforeFloat;
                temp_i = temp_i / toBase;
            }
            if (num.Length == 1)
                return numBeforeFloat;
            else
            {
                string numAfterFloat = "";
                double floatPart = Convert.ToDouble($"0,{num[1]}");
                while (floatPart != 0)
                {
                    floatPart = floatPart * toBase;
                    numAfterFloat += Convert.ToInt64(Math.Truncate(floatPart)).ToString("X");
                    floatPart = floatPart - Math.Truncate(floatPart);
                    if (numAfterFloat.Length == accuracy)
                        break;
                }
                return numBeforeFloat + "," + numAfterFloat;
            }
        }
        public static string AnyToAny(string Number, int toBase, int fromBase, int accuracy)
        {
            if (toBase == fromBase)
                return Number;
            if (toBase < 2 | fromBase < 2 | toBase > 16 | fromBase > 16)
                throw new Exception("Please use base 2-16");
            var number = Number.Split(",");
            char[] tmpChar;
            if (number.Length == 1)
                tmpChar = number[0].ToCharArray();
            else
                tmpChar = (number[0]+number[1]).ToCharArray();

            foreach (char ch in tmpChar)
                if (int.Parse(ch.ToString(), System.Globalization.NumberStyles.HexNumber) > fromBase-1)
                    throw new Exception($"This number is not {fromBase} based");

            return ConvertFromDecimal(ConvertToDecimal(Number, fromBase), toBase, accuracy);
        }
    }
}