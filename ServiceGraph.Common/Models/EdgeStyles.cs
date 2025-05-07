namespace ServiceGraph.Common
{
    public class EdgeStyle
    {
        #region Default Values

        private const string _default_Width = "2px";
        private const string _default_CurveStyle = "bezier";
        private const string _default_LineColor = "black";
        private const string _default_LineStyle = "solid";
        private const string _default_LineCap = "butt";
        private const string _default_LineDashPattern = "[]";
        private const string _default_SourceArrowShape = "none";
        private const string _default_TargetArrowShape = "triangle";
        private const string _default_SourceArrowColor = "#000000";
        private const string _default_TargetArrowColor = "#000000";
        private const string _default_SourceArrowWidth = "match-line";
        private const string _default_TargetArrowWidth = "match-line";
        private const string _default_ArrowScale = "1.0";
        private const string _default_LineOpacity = "1";
        private const string _default_ZIndex = "0";

        #endregion

        private string _width;
        public string Width
        {
            get => string.IsNullOrWhiteSpace(_width) ? _default_Width : _width;
            set => _width = value;
        }

        private string _curveStyle;
        public string CurveStyle
        {
            get => string.IsNullOrWhiteSpace(_curveStyle) ? _default_CurveStyle : _curveStyle;
            set => _curveStyle = value;
        }

        private string _lineColor;
        public string LineColor
        {
            get => string.IsNullOrWhiteSpace(_lineColor) ? _default_LineColor : _lineColor;
            set => _lineColor = value;
        }

        private string _lineStyle;
        public string LineStyle
        {
            get => string.IsNullOrWhiteSpace(_lineStyle) ? _default_LineStyle : _lineStyle;
            set => _lineStyle = value;
        }

        private string _lineCap;
        public string LineCap
        {
            get => string.IsNullOrWhiteSpace(_lineCap) ? _default_LineCap : _lineCap;
            set => _lineCap = value;
        }

        private string _lineDashPattern;
        public string LineDashPattern
        {
            get => string.IsNullOrWhiteSpace(_lineDashPattern) ? _default_LineDashPattern : _lineDashPattern;
            set => _lineDashPattern = value;
        }

        private string _sourceArrowShape;
        public string SourceArrowShape
        {
            get => string.IsNullOrWhiteSpace(_sourceArrowShape) ? _default_SourceArrowShape : _sourceArrowShape;
            set => _sourceArrowShape = value;
        }

        private string _targetArrowShape;
        public string TargetArrowShape
        {
            get => string.IsNullOrWhiteSpace(_targetArrowShape) ? _default_TargetArrowShape : _targetArrowShape;
            set => _targetArrowShape = value;
        }

        private string _sourceArrowColor;
        public string SourceArrowColor
        {
            get => string.IsNullOrWhiteSpace(_sourceArrowColor) ? _default_SourceArrowColor : _sourceArrowColor;
            set => _sourceArrowColor = value;
        }

        private string _targetArrowColor;
        public string TargetArrowColor
        {
            get => string.IsNullOrWhiteSpace(_targetArrowColor) ? _default_TargetArrowColor : _targetArrowColor;
            set => _targetArrowColor = value;
        }

        private string _sourceArrowWidth;
        public string SourceArrowWidth
        {
            get => string.IsNullOrWhiteSpace(_sourceArrowWidth) ? _default_SourceArrowWidth : _sourceArrowWidth;
            set => _sourceArrowWidth = value;
        }

        private string _targetArrowWidth;
        public string TargetArrowWidth
        {
            get => string.IsNullOrWhiteSpace(_targetArrowWidth) ? _default_TargetArrowWidth : _targetArrowWidth;
            set => _targetArrowWidth = value;
        }

        private string _arrowScale;
        public string ArrowScale
        {
            get => string.IsNullOrWhiteSpace(_arrowScale) ? _default_ArrowScale : _arrowScale;
            set => _arrowScale = value;
        }

        private string _lineOpacity;
        public string LineOpacity
        {
            get => string.IsNullOrWhiteSpace(_lineOpacity) ? _default_LineOpacity : _lineOpacity;
            set => _lineOpacity = value;
        }

        private string _zIndex;
        public string ZIndex
        {
            get => string.IsNullOrWhiteSpace(_zIndex) ? _default_ZIndex : _zIndex;
            set => _zIndex = value;
        }
    }
}
