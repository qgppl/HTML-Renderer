// "Therefore those skilled at the unorthodox
// are infinite as heaven and earth,
// inexhaustible as the great rivers.
// When they come to an end,
// they begin again,
// like the days and months;
// they die and are reborn,
// like the four seasons."
// 
// - Sun Tsu,
// "The Art of War"

using System;
using System.Globalization;
using TheArtOfDev.HtmlRenderer.Adapters;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;
using TheArtOfDev.HtmlRenderer.Core.Dom;
using TheArtOfDev.HtmlRenderer.Core.Utils;

namespace TheArtOfDev.HtmlRenderer.Core.Parse
{
    /// <summary>
    /// Parse CSS properties values like numbers, Urls, etc.
    /// </summary>
    internal class CssValueParser
    {
        #region Fields and Consts

        /// <summary>
        /// 
        /// </summary>
        protected readonly RAdapter _adapter;

        #endregion


        /// <summary>
        /// Init.
        /// </summary>
        public CssValueParser(RAdapter adapter)
        {
            ArgChecker.AssertArgNotNull(adapter, "global");

            _adapter = adapter;
        }

        /// <summary>
        /// Check if the given substring is a valid double number.
        /// Assume given substring is not empty and all indexes are valid!<br/>
        /// </summary>
        /// <returns>true - valid double number, false - otherwise</returns>
        public static bool IsFloat(string str, int idx, int length)
        {
            if (length < 1)
                return false;

            bool sawDot = false;
            for (int i = 0; i < length; i++)
            {
                if (str[idx + i] == '.')
                {
                    if (sawDot)
                        return false;
                    sawDot = true;
                }
                else if (!char.IsDigit(str[idx + i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check if the given substring is a valid double number.
        /// Assume given substring is not empty and all indexes are valid!<br/>
        /// </summary>
        /// <returns>true - valid int number, false - otherwise</returns>
        public static bool IsInt(string str, int idx, int length)
        {
            if (length < 1)
                return false;

            for (int i = 0; i < length; i++)
            {
                if (!char.IsDigit(str[idx + i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check if the given string is a valid length value.
        /// </summary>
        /// <param name="value">the string value to check</param>
        /// <returns>true - valid, false - invalid</returns>
        public static bool IsValidLength(string value)
        {
            if (value.Length > 1)
            {
                string number = string.Empty;
                if (value.EndsWith("%"))
                {
                    number = value.Substring(0, value.Length - 1);
                }
                else if (value.Length > 2)
                {
                    number = value.Substring(0, value.Length - 2);
                }
                double stub;
                return double.TryParse(number, out stub);
            }
            return false;
        }

        /// <summary>
        /// Evals a number and returns it. If number is a percentage, it will be multiplied by <see cref="hundredPercent"/>
        /// </summary>
        /// <param name="number">Number to be parsed</param>
        /// <param name="hundredPercent">Number that represents the 100% if parsed number is a percentage</param>
        /// <returns>Parsed number. Zero if error while parsing.</returns>
        public static double ParseNumber(string number, double hundredPercent)
        {
            if (string.IsNullOrEmpty(number))
            {
                return 0f;
            }

            string toParse = number;
            bool isPercent = number.EndsWith("%");
            double result;

            if (isPercent)
                toParse = number.Substring(0, number.Length - 1);

            if (!double.TryParse(toParse, NumberStyles.Number, NumberFormatInfo.InvariantInfo, out result))
            {
                return 0f;
            }

            if (isPercent)
            {
                result = (result / 100f) * hundredPercent;
            }

            return result;
        }

        /// <summary>
        /// Get the unit to use for the length, use default if no unit found in length string.
        /// </summary>
        protected static string GetUnit(string length, string defaultUnit, out bool hasUnit)
        {
            var unit = length.Length >= 3 ? length.Substring(length.Length - 2, 2) : string.Empty;
            switch (unit)
            {
                case CssConstants.Em:
                case CssConstants.Ex:
                case CssConstants.Px:
                case CssConstants.Mm:
                case CssConstants.Cm:
                case CssConstants.In:
                case CssConstants.Pt:
                case CssConstants.Pc:
                    hasUnit = true;
                    break;
                default:
                    hasUnit = false;
                    unit = defaultUnit ?? String.Empty;
                    break;
            }
            return unit;
        }

        /// <summary>
        /// Check if the given color string value is valid.
        /// </summary>
        /// <param name="colorValue">color string value to parse</param>
        /// <returns>true - valid, false - invalid</returns>
        public bool IsColorValid(string colorValue)
        {
            RColor color;
            return TryGetColor(colorValue, 0, colorValue.Length, out color);
        }

        /// <summary>
        /// Parses a color value in CSS style; e.g. #ff0000, red, rgb(255,0,0), rgb(100%, 0, 0)
        /// </summary>
        /// <param name="colorValue">color string value to parse</param>
        /// <returns>Color value</returns>
        public RColor GetActualColor(string colorValue)
        {
            RColor color;
            TryGetColor(colorValue, 0, colorValue.Length, out color);
            return color;
        }

        /// <summary>
        /// Parses a color value in CSS style; e.g. #ff0000, RED, RGB(255,0,0), RGB(100%, 0, 0)
        /// </summary>
        /// <param name="str">color substring value to parse</param>
        /// <param name="idx">substring start idx </param>
        /// <param name="length">substring length</param>
        /// <param name="color">return the parsed color</param>
        /// <returns>true - valid color, false - otherwise</returns>
        public bool TryGetColor(string str, int idx, int length, out RColor color)
        {
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    if (length > 1 && str[idx] == '#')
                    {
                        return GetColorByHex(str, idx, length, out color);
                    }
                    else if (length > 10 && CommonUtils.SubStringEquals(str, idx, 4, "rgb(") && str[length - 1] == ')')
                    {
                        return GetColorByRgb(str, idx, length, out color);
                    }
                    else if (length > 13 && CommonUtils.SubStringEquals(str, idx, 5, "rgba(") && str[length - 1] == ')')
                    {
                        return GetColorByRgba(str, idx, length, out color);
                    }
                    else
                    {
                        return GetColorByName(str, idx, length, out color);
                    }
                }
            }
            catch
            { }
            color = RColor.Black;
            return false;
        }




        #region Private methods

        /// <summary>
        /// Get color by parsing given hex value color string (#A28B34).
        /// </summary>
        /// <returns>true - valid color, false - otherwise</returns>
        private static bool GetColorByHex(string str, int idx, int length, out RColor color)
        {
            int r = -1;
            int g = -1;
            int b = -1;
            if (length == 7)
            {
                r = ParseHexInt(str, idx + 1, 2);
                g = ParseHexInt(str, idx + 3, 2);
                b = ParseHexInt(str, idx + 5, 2);
            }
            else if (length == 4)
            {
                r = ParseHexInt(str, idx + 1, 1);
                r = r * 16 + r;
                g = ParseHexInt(str, idx + 2, 1);
                g = g * 16 + g;
                b = ParseHexInt(str, idx + 3, 1);
                b = b * 16 + b;
            }
            if (r > -1 && g > -1 && b > -1)
            {
                color = RColor.FromArgb(r, g, b);
                return true;
            }
            color = RColor.Empty;
            return false;
        }

        /// <summary>
        /// Get color by parsing given RGB value color string (RGB(255,180,90))
        /// </summary>
        /// <returns>true - valid color, false - otherwise</returns>
        private static bool GetColorByRgb(string str, int idx, int length, out RColor color)
        {
            int r = -1;
            int g = -1;
            int b = -1;

            if (length > 10)
            {
                int s = idx + 4;
                r = ParseIntAtIndex(str, ref s);
                if (s < idx + length)
                {
                    g = ParseIntAtIndex(str, ref s);
                }
                if (s < idx + length)
                {
                    b = ParseIntAtIndex(str, ref s);
                }
            }

            if (r > -1 && g > -1 && b > -1)
            {
                color = RColor.FromArgb(r, g, b);
                return true;
            }
            color = RColor.Empty;
            return false;
        }

        /// <summary>
        /// Get color by parsing given RGBA value color string (RGBA(255,180,90,180))
        /// </summary>
        /// <returns>true - valid color, false - otherwise</returns>
        private static bool GetColorByRgba(string str, int idx, int length, out RColor color)
        {
            int r = -1;
            int g = -1;
            int b = -1;
            int a = -1;

            if (length > 13)
            {
                int s = idx + 5;
                r = ParseIntAtIndex(str, ref s);

                if (s < idx + length)
                {
                    g = ParseIntAtIndex(str, ref s);
                }
                if (s < idx + length)
                {
                    b = ParseIntAtIndex(str, ref s);
                }
                if (s < idx + length)
                {
                    a = ParseIntAtIndex(str, ref s);
                }
            }

            if (r > -1 && g > -1 && b > -1 && a > -1)
            {
                color = RColor.FromArgb(a, r, g, b);
                return true;
            }
            color = RColor.Empty;
            return false;
        }

        /// <summary>
        /// Get color by given name, including .NET name.
        /// </summary>
        /// <returns>true - valid color, false - otherwise</returns>
        private bool GetColorByName(string str, int idx, int length, out RColor color)
        {
            color = _adapter.GetColor(str.Substring(idx, length));
            return color.A > 0;
        }

        /// <summary>
        /// Parse the given decimal number string to positive int value.<br/>
        /// Start at given <paramref name="startIdx"/>, ignore whitespaces and take
        /// as many digits as possible to parse to int.
        /// </summary>
        /// <param name="str">the string to parse</param>
        /// <param name="startIdx">the index to start parsing at</param>
        /// <returns>parsed int or 0</returns>
        private static int ParseIntAtIndex(string str, ref int startIdx)
        {
            int len = 0;
            while (char.IsWhiteSpace(str, startIdx))
                startIdx++;
            while (char.IsDigit(str, startIdx + len))
                len++;
            var val = ParseInt(str, startIdx, len);
            startIdx = startIdx + len + 1;
            return val;
        }

        /// <summary>
        /// Parse the given decimal number string to positive int value.
        /// Assume given substring is not empty and all indexes are valid!<br/>
        /// </summary>
        /// <returns>int value, -1 if not valid</returns>
        private static int ParseInt(string str, int idx, int length)
        {
            if (length < 1)
                return -1;

            int num = 0;
            for (int i = 0; i < length; i++)
            {
                int c = str[idx + i];
                if (!(c >= 48 && c <= 57))
                    return -1;

                num = num * 10 + c - 48;
            }
            return num;
        }

        /// <summary>
        /// Parse the given hex number string to positive int value.
        /// Assume given substring is not empty and all indexes are valid!<br/>
        /// </summary>
        /// <returns>int value, -1 if not valid</returns>
        private static int ParseHexInt(string str, int idx, int length)
        {
            if (length < 1)
                return -1;

            int num = 0;
            for (int i = 0; i < length; i++)
            {
                int c = str[idx + i];
                if (!(c >= 48 && c <= 57) && !(c >= 65 && c <= 70) && !(c >= 97 && c <= 102))
                    return -1;

                num = num * 16 + (c <= 57 ? c - 48 : (10 + c - (c <= 70 ? 65 : 97)));
            }
            return num;
        }

        #endregion
    }
}