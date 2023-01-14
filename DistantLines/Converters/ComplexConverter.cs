﻿using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Windows.Data;

namespace WpfApp.DataGridConverters
{
    [ValueConversion(typeof(Complex), typeof(string))]
    internal class ComplexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Complex cValue)
            {
                string sRe = DoubleFormatter(cValue.Real);
                string sIm = DoubleFormatter(Math.Abs(cValue.Imaginary));

                if (cValue.Imaginary != 0 && cValue.Real != 0)
                {
                    string pm = cValue.Imaginary < 0 ?
                        "-" : "+";

                    return $"{sRe} {pm} j{sIm}";
                }
                else if (cValue.Imaginary == 0)
                {
                    return sRe;
                }
                else if (cValue.Real == 0)
                {
                    return "j" + sIm;
                }
            }
            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //string str = (string)value;

            //str = str.Replace(" ", "").Replace("j", "");

            //string[] subs = str.Split('+', '-');

            //double re = double.Parse(subs[0]);
            //double im = subs.Count() > 1 ? double.Parse(subs[1]) : 0;

            //return new Complex(re, im);
            return null;
        }

        private string DoubleFormatter(double value)
        {
            string str = value.ToString("E3");

            string[] subs = str.Split('E');

            subs = CheckPlus(subs);

            if (subs.Length == 2)
            {
                subs[1] = CheckPrevZero(subs[1]);
                subs[1] = ToSmallUpperNumber(subs[1]);
                return $"{subs[0]} ∙ 10{subs[1]}";
            }
            else
            {
                return subs[0];
            }
        }

        private string CheckPrevZero(string str)
        {
            int index = 1;
            for (int i = index; i < str.Length; i++)
            {
                if (str[i] != '0')
                    break;
                index++;
            }

            return "-" + str.Substring(index, str.Length - index);
        }

        private string ToSmallUpperNumber(string str)
        {
            str = str.Replace('0', '⁰');
            str = str.Replace('1', '¹');
            str = str.Replace('2', '²');
            str = str.Replace('3', '³');
            str = str.Replace('4', '⁴');
            str = str.Replace('5', '⁵');
            str = str.Replace('6', '⁶');
            str = str.Replace('7', '⁷');
            str = str.Replace('8', '⁸');
            str = str.Replace('9', '⁹');
            str = str.Replace('-', '⁻');

            return str;
        }
        private string[] CheckPlus(string[] str)
        {
            if (str.Length == 1)
                return str;

            if (str[1].Contains('+'))
            {
                int degree = int.Parse(str[1]);
                for (int i = 0; i < degree; i++)
                {
                    int index = str[0].IndexOf(',');

                    if (index + 1 < str[0].Length && index != -1)
                    {
                        str[0] = str[0].Replace(",", "");
                        str[0] = str[0].Insert(index + 1, ",");
                    }
                    else
                    {
                        str[0] = str[0] + "0";
                    }

                }
                if (str[0].Last() == ',')
                    str[0] = str[0].Replace(",", "");

                return new string[] { str[0] };
            }

            return str;
        }
    }
}
