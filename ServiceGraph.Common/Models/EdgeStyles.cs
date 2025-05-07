namespace ServiceGraph.Common
{
    public class EdgeStyle
    {
        private string _width;
        public string Width
        {
            get => string.IsNullOrWhiteSpace(_width) ? "2px" : _width;
            set => _width = value;
        }

        private string _curveStyle;
        public string CurveStyle
        {
            get => string.IsNullOrWhiteSpace(_curveStyle) ? "bezier" : _curveStyle;
            set => _curveStyle = value;
        }

        private string _lineColor;
        public string LineColor
        {
            get => string.IsNullOrWhiteSpace(_lineColor) ? "black" : _lineColor;
            set => _lineColor = value;
        }

        private string _lineStyle;
        public string LineStyle
        {
            get => string.IsNullOrWhiteSpace(_lineStyle) ? "solid" : _lineStyle;
            set => _lineStyle = value;
        }

        private string _lineCap;
        public string LineCap
        {
            get => string.IsNullOrWhiteSpace(_lineCap) ? "butt" : _lineCap;
            set => _lineCap = value;
        }

        private string _lineDashPattern;
        public string LineDashPattern
        {
            get => string.IsNullOrWhiteSpace(_lineDashPattern) ? "[]" : _lineDashPattern;
            set => _lineDashPattern = value;
        }

        private string _sourceArrowShape;
        public string SourceArrowShape
        {
            get => string.IsNullOrWhiteSpace(_sourceArrowShape) ? "none" : _sourceArrowShape;
            set => _sourceArrowShape = value;
        }

        private string _targetArrowShape;
        public string TargetArrowShape
        {
            get => string.IsNullOrWhiteSpace(_targetArrowShape) ? "triangle" : _targetArrowShape;
            set => _targetArrowShape = value;
        }

        private string _sourceArrowColor;
        public string SourceArrowColor
        {
            get => string.IsNullOrWhiteSpace(_sourceArrowColor) ? "#000000" : _sourceArrowColor;
            set => _sourceArrowColor = value;
        }

        private string _targetArrowColor;
        public string TargetArrowColor
        {
            get => string.IsNullOrWhiteSpace(_targetArrowColor) ? "#000000" : _targetArrowColor;
            set => _targetArrowColor = value;
        }

        private string _sourceArrowWidth;
        public string SourceArrowWidth
        {
            get => string.IsNullOrWhiteSpace(_sourceArrowWidth) ? "match-line" : _sourceArrowWidth;
            set => _sourceArrowWidth = value;
        }

        private string _targetArrowWidth;
        public string TargetArrowWidth
        {
            get => string.IsNullOrWhiteSpace(_targetArrowWidth) ? "match-line" : _targetArrowWidth;
            set => _targetArrowWidth = value;
        }

        private string _arrowScale;
        public string ArrowScale
        {
            get => string.IsNullOrWhiteSpace(_arrowScale) ? "1.0" : _arrowScale;
            set => _arrowScale = value;
        }

        private string _lineOpacity;
        public string LineOpacity
        {
            get => string.IsNullOrWhiteSpace(_lineOpacity) ? "1" : _lineOpacity;
            set => _lineOpacity = value;
        }

        private string _zIndex;
        public string ZIndex
        {
            get => string.IsNullOrWhiteSpace(_zIndex) ? "0" : _zIndex;
            set => _zIndex = value;
        }
    }
}
