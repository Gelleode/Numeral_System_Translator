using System;
using System.Collections.Generic;

namespace Numeral_System_Translator
{
    public static class Extensions
    {
        public static string Filter(this string str, List<char> charsToRemove)
        {
            foreach (char c in charsToRemove)
            {
                str = str.Replace(c.ToString(), String.Empty);
            }

            return str;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string Number = "8B,1D";
            int toBase = 8;
            int fromBase = 16;
            int accuracy = 9;
            string Answer = Any_To_Any(Number, toBase, fromBase, accuracy);
            Console.WriteLine(Answer);
        }

        private static string Any_To_Any(string Number, int toBase, int fromBase, int accuracy)
        {
            string value1;
            string value2 = null;

            var number = Number.Split(",");

            List<char> charsToRemove = new List<char>() { ',', '.' };
            string tmp_s = null;

            char[] numBeforeFloat = number[0].ToCharArray();

            for (int i = 0; i < numBeforeFloat.Length / 2; i++)
            {
                char tmp = numBeforeFloat[i];
                numBeforeFloat[i] = numBeforeFloat[numBeforeFloat.Length - i - 1];
                numBeforeFloat[numBeforeFloat.Length - i - 1] = tmp;
            }

            // Перевод в десятичную систему числа перед запятой
            if (fromBase != 10)
            {
                int index = 0;
                double numberBeforeFloat = 0;
                foreach (char ch in numBeforeFloat)
                {
                    string num = ch.ToString();
                    long tmp_i = long.Parse(num, System.Globalization.NumberStyles.HexNumber);

                    numberBeforeFloat = numberBeforeFloat + tmp_i * Math.Pow(fromBase, index);

                    index++;
                }
                tmp_s = numberBeforeFloat.ToString();
            }
            else
            {
                foreach (char ch in numBeforeFloat)
                {
                    tmp_s = tmp_s + numBeforeFloat;
                }
            }

            // Перевод из десятичной системы в другие
            if (toBase != 10)
            {
                long temp_i = Convert.ToInt64(tmp_s);
                tmp_s = null;
                while (temp_i != 0)
                {
                    tmp_s = Convert.ToString(temp_i % toBase) + tmp_s;
                    temp_i = temp_i / toBase;
                }
                value1 = tmp_s;
            }
            else
            {
                value1 = tmp_s;
            }
            // Проверка на наличие запятой
            if (number.Length != 2)
            {
                return value1;
            }
            else
            {
                char[] numAfterFloat = number[1].ToCharArray();
                for (int i = 0; i < numAfterFloat.Length / 2; i++)
                {
                    char tmp = numAfterFloat[i];
                    numAfterFloat[i] = numAfterFloat[numAfterFloat.Length - i - 1];
                    numAfterFloat[numAfterFloat.Length - i - 1] = tmp;
                }
                tmp_s = null;
                // Перевод в десятичную систему числа после запятой
                if (fromBase != 10)
                {
                    foreach (char ch in numAfterFloat)
                    {
                        string num = ch.ToString();
                        double tmp_d = int.Parse(num, System.Globalization.NumberStyles.HexNumber);

                        tmp_s = Convert.ToString(tmp_d) + tmp_s;
                        tmp_s = Convert.ToString(Convert.ToDouble(tmp_s) / fromBase);
                        tmp_s = tmp_s.Filter(charsToRemove);
                        if (tmp_s.StartsWith("0"))
                        {
                            tmp_s = tmp_s.Remove(0, 1);
                        }
                    }
                    value2 = tmp_s;
                }
                else
                {
                    foreach (char ch in numAfterFloat)
                    {
                        tmp_s = tmp_s + numAfterFloat;
                    }
                }

                // Перевод в нужную систему числа после запятой НЕ РАБТАЕТ С ДВОИЧНЫМ И ДЕСЯТЕРИЧНЫМ
                if (toBase != 10)
                {
                    long j_i = Convert.ToInt64(tmp_s);
                    value2 = null;
                    int arr_count = tmp_s.Length;
                    tmp_s = null;
                    while (j_i != 0)
                    {
                        if (tmp_s != null)
                        {
                            j_i = Convert.ToInt64(tmp_s);
                        }

                        j_i = j_i * toBase;
                        tmp_s = j_i.ToString();

                        if (value2 != null)
                        {
                            if (value2.Length == accuracy + 1)
                            {
                                break;
                            }
                        }

                        if (tmp_s.Length == arr_count + 2)
                        {
                            string tmp_s1 = Convert.ToInt32(tmp_s.Substring(0, 2)).ToString("X");
                            value2 = value2 + tmp_s1;
                            tmp_s = tmp_s.Remove(0, 2);
                        }
                        else if (tmp_s.Length == arr_count + 1)
                        {
                            value2 = value2 + tmp_s.Substring(0, 1);
                            tmp_s = tmp_s.Remove(0, 1);
                        }
                        else
                            value2 = value2 + "0";
                    }
                    value2 = value2.Remove(value2.Length - 1, 1);
                }
                return value1 + "," + value2;
            }
        }
    }
}
