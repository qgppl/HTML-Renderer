using System;
using System.Collections.Generic;
using System.Text;
using TheArtOfDev.HtmlRenderer.Adapters;
using TheArtOfDev.HtmlRenderer.Core.Dom;
using TheArtOfDev.HtmlRenderer.Core.Utils;

namespace TheArtOfDev.HtmlRenderer.Core.Parse
{
    class CssLengthParser: CssValueParser
    {
        /// <summary>
        /// Init.
        /// </summary>
        public CssLengthParser(RAdapter adapter): base(adapter)
        {
        }

        /// <summary>
        /// Parses a length. Lengths are followed by an unit identifier (e.g. 10px, 3.1em)
        /// </summary>
        /// <param name="length">Specified length</param>
        /// <param name="hundredPercent">Equivalent to 100 percent when length is percentage</param>
        /// <param name="box"></param>
        /// <returns>the parsed length value with adjustments</returns>
        internal double ParseLength(string length, double hundredPercent, CssBoxProperties box)
        {
            return ParseLength(length, hundredPercent, box.GetEmHeight(), null, false);
        }

        /// <summary>
        /// Parses a length. Lengths are followed by an unit identifier (e.g. 10px, 3.1em)
        /// </summary>
        /// <param name="length">Specified length</param>
        /// <param name="hundredPercent">Equivalent to 100 percent when length is percentage</param>
        /// <param name="box"></param>
        /// <param name="defaultUnit"></param>
        /// <returns>the parsed length value with adjustments</returns>
        internal double ParseLength(string length, double hundredPercent, CssBoxProperties box, string defaultUnit)
        {
            return ParseLength(length, hundredPercent, box.GetEmHeight(), defaultUnit, false);
        }

        internal double ParseLengthToPoints(string length, double hundredPercent, CssBoxProperties box)
        {
            return ParseLength(length, hundredPercent, box.GetEmHeight(), null, true);
        }

        private const double MmPerInch = 25.4;
        private const double MmPerCm = 10;
        private const double PointsPerInch = 72;
        private const double PointsPerPica = 12;

        /// <summary>
        /// Parses a length. Lengths are followed by an unit identifier (e.g. 10px, 3.1em)
        /// </summary>
        /// <param name="length">Specified length</param>
        /// <param name="hundredPercent">Equivalent to 100 percent when length is percentage</param>
        /// <param name="emFactor">Font size in pixels</param>
        /// <param name="defaultUnit"></param>
        /// <param name="returnPoints">Allows the return double to be in points. If false, result will be device units (pixels or points)</param>
        /// <returns>the parsed length value with adjustments</returns>
        internal double ParseLength(string length, double hundredPercent, double emFactor, string defaultUnit, bool returnPoints)
        {
            //Return zero if no length specified, zero specified
            if (string.IsNullOrEmpty(length) || length == "0")
                return 0f;

            //If percentage, use ParseNumber
            if (length.EndsWith("%"))
                return ParseNumber(length, hundredPercent);

            //Get units of the length
            bool hasUnit;
            string unit = GetUnit(length, defaultUnit, out hasUnit);


            //Number of the length
            string number = hasUnit ? length.Substring(0, length.Length - 2) : length;

            double value = ParseNumber(number, hundredPercent);

            if (returnPoints)
            {
                switch (unit)
                {
                    case CssConstants.Em:
                        return _adapter.PixelToDeviceUnit(value * emFactor) / _adapter.PointToDeviceUnit(1);
                    case CssConstants.Ex:
                        return _adapter.PixelToDeviceUnit(value * emFactor / 2f) / _adapter.PointToDeviceUnit(1);
                    case CssConstants.Px:
                        return _adapter.PixelToDeviceUnit(value) / _adapter.PointToDeviceUnit(1);
                    case CssConstants.Mm:
                        return value / MmPerInch * PointsPerInch;
                    case CssConstants.Cm:
                        return value * MmPerCm / MmPerInch * PointsPerInch;
                    case CssConstants.In:
                        return value * PointsPerInch;
                    case CssConstants.Pt:
                        return value;
                    case CssConstants.Pc:
                        return value * PointsPerPica;
                    default:
                        return 0f;
                }
            }
            else
            {
                switch (unit)
                {
                    case CssConstants.Em:
                        return _adapter.PixelToDeviceUnit(value * emFactor);
                    case CssConstants.Ex:
                        return _adapter.PixelToDeviceUnit(value * emFactor / 2f);
                    case CssConstants.Px:
                        return _adapter.PixelToDeviceUnit(value);
                    case CssConstants.Mm:
                        return _adapter.PointToDeviceUnit(value / MmPerInch * PointsPerInch);
                    case CssConstants.Cm:
                        return _adapter.PointToDeviceUnit(value * MmPerCm / MmPerInch * PointsPerInch);
                    case CssConstants.In:
                        return _adapter.PointToDeviceUnit(value * PointsPerInch);
                    case CssConstants.Pt:
                        return _adapter.PointToDeviceUnit(value);
                    case CssConstants.Pc:
                        return _adapter.PointToDeviceUnit(value * PointsPerPica);
                    default:
                        return 0f;
                }
            }
        }

        /// <summary>
        /// Parses a border value in CSS style; e.g. 1px, 1, thin, thick, medium
        /// </summary>
        /// <param name="borderValue"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        internal double GetActualBorderWidth(string borderValue, CssBoxProperties b)
        {
            if (string.IsNullOrEmpty(borderValue))
            {
                return GetActualBorderWidth(CssConstants.Medium, b);
            }

            switch (borderValue)
            {
                case CssConstants.Thin:
                    return 1f;
                case CssConstants.Medium:
                    return 2f;
                case CssConstants.Thick:
                    return 4f;
                default:
                    return Math.Abs(ParseLength(borderValue, 1, b));
            }
        }

    }
}
