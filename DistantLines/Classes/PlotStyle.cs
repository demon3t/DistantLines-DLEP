using ScottPlot.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WpfApp.Classes
{
    internal class PlotStyle : IStyle
    {
        public static PlotStyle Default =>
                new PlotStyle()
                {
                    _figureBackgroundColor = Color.FromArgb(0, 239, 239, 238),
                    _dataBackgroundColor = Color.FromArgb(255, 255, 254, 254),
                    _gridLineColor = Color.FromArgb(255, 238, 239, 238),
                    _tickLabelColor = Color.FromArgb(255, 0, 0, 0),
                    _axisLabelColor = Color.FromArgb(255, 0, 0, 0),
                    _titleFontColor = Color.FromArgb(255, 0, 0, 0),
                    _tickMajorColor = Color.Black,
                    _frameColor = Color.Black,
                    _tickMinorColor = Color.Black,
                };

        #region Свойства

        Color IStyle.FigureBackgroundColor => _figureBackgroundColor;
        private Color _figureBackgroundColor;
        Color IStyle.DataBackgroundColor => _dataBackgroundColor;
        private Color _dataBackgroundColor;
        Color IStyle.FrameColor => _frameColor;
        private Color _frameColor;
        Color IStyle.GridLineColor => _gridLineColor;
        private Color _gridLineColor;
        Color IStyle.AxisLabelColor => _axisLabelColor;
        private Color _axisLabelColor;
        Color IStyle.TitleFontColor => _titleFontColor;
        private Color _titleFontColor;
        Color IStyle.TickLabelColor => _tickLabelColor;
        private Color _tickLabelColor;
        Color IStyle.TickMajorColor => _tickMajorColor;
        private Color _tickMajorColor;
        Color IStyle.TickMinorColor => _tickMinorColor;
        private Color _tickMinorColor;
        string IStyle.AxisLabelFontName => _axisLabelFontName;
        private string _axisLabelFontName;
        string IStyle.TitleFontName => _titleFontName;
        private string _titleFontName;
        string IStyle.TickLabelFontName => _tickLabelFontName;
        private string _tickLabelFontName;

        #endregion
    }
}
