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
            var left = Math.Ceiling(rect.Left);
            return new RRect(left, rect.Top + topBorderWidth / 2, rect.Right - 1 - left, 0);
        }

        public override RRect GetLeftBorderRect(RRect rect, double topBorderWidth, double leftBorderWidth, double rightBorderWidth, double bottomBorderWidth)
        {
            var top = Math.Ceiling(rect.Top);
            return new RRect(rect.Left + leftBorderWidth / 2, top, 0, Math.Floor(rect.Bottom) - top);
        }

        public override RRect GetRightBorderRect(RRect rect, double topBorderWidth, double leftBorderWidth, double rightBorderWidth, double bottomBorderWidth)
        {
            var top = Math.Ceiling(rect.Top);
            return new RRect(rect.Right - rightBorderWidth / 2, top, 0, Math.Floor(rect.Bottom) - top);
        }

        public override RRect GetBottomBorderRect(RRect rect, double topBorderWidth, double leftBorderWidth, double rightBorderWidth, double bottomBorderWidth)
        {
            var left = Math.Ceiling(rect.Left);
            return new RRect(left, rect.Bottom - bottomBorderWidth / 2, rect.Right - 1 - left, 0);
        }
    }
}
