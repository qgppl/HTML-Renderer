using System;
using System.Collections.Generic;
using System.Text;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;

namespace TheArtOfDev.HtmlRenderer.Adapters
{
    /// <summary>
    /// Default implementation for discrete precision drawing devices (e.g. screen)
    /// </summary>
    public class RBorderRectangleCalculatorRaster : RBorderRectangleCalculator
    {
        public override RRect GetTopBorderRect(RRect rect, double topBorderWidth, double leftBorderWidth, double rightBorderWidth, double bottomBorderWidth)
        {
            var left = Math.Floor(rect.Left);
            var right = Math.Floor(rect.Right) - 0.5;
            var top = Math.Floor(rect.Top) + topBorderWidth * 0.5;
            return new RRect(left, top, right - left, 0);
        }

        public override RRect GetBottomBorderRect(RRect rect, double topBorderWidth, double leftBorderWidth, double rightBorderWidth, double bottomBorderWidth)
        {
            var left = Math.Floor(rect.Left);
            var right = Math.Floor(rect.Right) - 0.5;
            var bottom = rect.Bottom - bottomBorderWidth * 0.5;
            return new RRect(left, bottom, right - left, 0);
        }

        public override RRect GetLeftBorderRect(RRect rect, double topBorderWidth, double leftBorderWidth, double rightBorderWidth, double bottomBorderWidth)
        {
            var top = Math.Floor(rect.Top) + topBorderWidth;
            var bottom = rect.Bottom - bottomBorderWidth;
            var left = Math.Floor(rect.Left + leftBorderWidth * 0.5);
            return new RRect(left, top, 0, bottom - top);
        }

        public override RRect GetRightBorderRect(RRect rect, double topBorderWidth, double leftBorderWidth, double rightBorderWidth, double bottomBorderWidth)
        {
            var top = Math.Floor(rect.Top) + topBorderWidth;
            var bottom = rect.Bottom - bottomBorderWidth;
            var right = Math.Floor(rect.Right - rightBorderWidth * 0.5);
            return new RRect(right, top, 0, bottom - top);
        }
    }
}
