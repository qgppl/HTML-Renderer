using System;
using System.Collections.Generic;
using System.Text;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;

namespace TheArtOfDev.HtmlRenderer.Adapters
{
    public abstract class RBorderRectangleCalculator
    {
        /// <summary>
        /// Returns drawing coordinates for simple top border around rect (starting point and width)
        /// <param name="rect">the rectangle the border is enclosing</param>
        /// <param name="topBorderWidth">actual width of top border</param>
        /// <param name="leftBorderWidth">actual width of left border</param>
        /// <param name="rightBorderWidth">actual width of right border</param>
        /// <param name="bottomBorderWidth">actual width of bottom border</param>
        /// <returns>Rectangle encoding the starting point (Top, Left) and width</returns>
        /// </summary>
        public abstract RRect GetTopBorderRect(RRect rect, double topBorderWidth, double leftBorderWidth, double rightBorderWidth, double bottomBorderWidth);
        /// <summary>
        /// Returns drawing coordinates for simple left border around rect (starting point and height)
        /// <param name="rect">the rectangle the border is enclosing</param>
        /// <param name="topBorderWidth">actual width of top border</param>
        /// <param name="leftBorderWidth">actual width of left border</param>
        /// <param name="rightBorderWidth">actual width of right border</param>
        /// <param name="bottomBorderWidth">actual width of bottom border</param>
        /// <returns>Rectangle encoding the starting point (Top, Left) and height</returns>
        /// </summary>
        public abstract RRect GetLeftBorderRect(RRect rect, double topBorderWidth, double leftBorderWidth, double rightBorderWidth, double bottomBorderWidth);
        /// <summary>
        /// Returns drawing coordinates for simple right border around rect (starting point and height)
        /// <param name="rect">the rectangle the border is enclosing</param>
        /// <param name="topBorderWidth">actual width of top border</param>
        /// <param name="leftBorderWidth">actual width of left border</param>
        /// <param name="rightBorderWidth">actual width of right border</param>
        /// <param name="bottomBorderWidth">actual width of bottom border</param>
        /// <returns>Rectangle encoding the starting point (Top, Left) and height</returns>
        /// </summary>
        public abstract RRect GetRightBorderRect(RRect rect, double topBorderWidth, double leftBorderWidth, double rightBorderWidth, double bottomBorderWidth);
        /// <summary>
        /// Returns drawing coordinates for simple top border around rect (starting point and width)
        /// <param name="rect">the rectangle the border is enclosing</param>
        /// <param name="topBorderWidth">actual width of top border</param>
        /// <param name="leftBorderWidth">actual width of left border</param>
        /// <param name="rightBorderWidth">actual width of right border</param>
        /// <param name="bottomBorderWidth">actual width of bottom border</param>
        /// <returns>Rectangle encoding the starting point (Top, Left) and width</returns>
        /// </summary>
        public abstract RRect GetBottomBorderRect(RRect rect, double topBorderWidth, double leftBorderWidth, double rightBorderWidth, double bottomBorderWidth);
    }
}
