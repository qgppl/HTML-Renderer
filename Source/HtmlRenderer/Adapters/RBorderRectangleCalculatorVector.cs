using System;
using System.Collections.Generic;
using System.Text;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;

namespace TheArtOfDev.HtmlRenderer.Adapters
{
    /// <summary>
    /// Default implementation for arbitrary precision drawing devices (e.g. PDF)
    /// </summary>
    public class RBorderRectangleCalculatorVector: RBorderRectangleCalculator
    {
        // tiny overlap between adjacent borders to help reduce spacing artifacts
        private double eps = 0.001;

        public override RRect GetTopBorderRect(RRect rect, double topBorderWidth, double leftBorderWidth, double rightBorderWidth, double bottomBorderWidth)
        {
            var left = rect.Left;
            var top = rect.Top + topBorderWidth * 0.5;
            var width = rect.Right - left + eps;
            return new RRect(left, top, width, 0);
        }

        public override RRect GetLeftBorderRect(RRect rect, double topBorderWidth, double leftBorderWidth, double rightBorderWidth, double bottomBorderWidth)
        {
            var left = rect.Left + leftBorderWidth * 0.5;
            var top = rect.Top + topBorderWidth;
            var height = rect.Bottom - bottomBorderWidth - top + eps;
            return new RRect(left, top, 0, height);
        }

        public override RRect GetRightBorderRect(RRect rect, double topBorderWidth, double leftBorderWidth, double rightBorderWidth, double bottomBorderWidth)
        {
            var right = rect.Right - rightBorderWidth * 0.5;
            var top = rect.Top + topBorderWidth;
            var height = rect.Bottom - bottomBorderWidth - top + eps;
            return new RRect(right, top, 0, height);
        }

        public override RRect GetBottomBorderRect(RRect rect, double topBorderWidth, double leftBorderWidth, double rightBorderWidth, double bottomBorderWidth)
        {
            var left = rect.Left;
            var bottom = rect.Bottom - bottomBorderWidth * 0.5;
            var width = rect.Right - left + eps;
            return new RRect(left, bottom, width, 0);
        }
    }
}
