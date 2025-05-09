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

        #region Selected Default Values

        private const string _selected_default_Width = _default_Width;
        private const string _selected_default_CurveStyle = _default_CurveStyle;
        private const string _selected_default_LineColor = _default_LineColor;
        private const string _selected_default_LineStyle = _default_LineStyle;
        private const string _selected_default_LineCap = _default_LineCap;
        private const string _selected_default_LineDashPattern = _default_LineDashPattern;
        private const string _selected_default_SourceArrowShape = _default_SourceArrowShape;
        private const string _selected_default_TargetArrowShape = _default_TargetArrowShape;
        private const string _selected_default_SourceArrowColor = _default_SourceArrowColor;
        private const string _selected_default_TargetArrowColor = _default_TargetArrowColor;
        private const string _selected_default_SourceArrowWidth = _default_SourceArrowWidth;
        private const string _selected_default_TargetArrowWidth = _default_TargetArrowWidth;
        private const string _selected_default_ArrowScale = _default_ArrowScale;
        private const string _selected_default_LineOpacity = _default_LineOpacity;
        private const string _selected_default_ZIndex = _default_ZIndex;

        #endregion

        // Instance fields for normal state
        private string _width;
        private string _curveStyle;
        private string _lineColor;
        private string _lineStyle;
        private string _lineCap;
        private string _lineDashPattern;
        private string _sourceArrowShape;
        private string _targetArrowShape;
        private string _sourceArrowColor;
        private string _targetArrowColor;
        private string _sourceArrowWidth;
        private string _targetArrowWidth;
        private string _arrowScale;
        private string _lineOpacity;
        private string _zIndex;

        // Instance fields for selected state
        private string _selectedWidth;
        private string _selectedCurveStyle;
        private string _selectedLineColor;
        private string _selectedLineStyle;
        private string _selectedLineCap;
        private string _selectedLineDashPattern;
        private string _selectedSourceArrowShape;
        private string _selectedTargetArrowShape;
        private string _selectedSourceArrowColor;
        private string _selectedTargetArrowColor;
        private string _selectedSourceArrowWidth;
        private string _selectedTargetArrowWidth;
        private string _selectedArrowScale;
        private string _selectedLineOpacity;
        private string _selectedZIndex;

        #region Properties

        // Normal state properties
        public string Width { get => string.IsNullOrWhiteSpace(_width) ? _default_Width : _width; set => _width = value; }
        public string CurveStyle { get => string.IsNullOrWhiteSpace(_curveStyle) ? _default_CurveStyle : _curveStyle; set => _curveStyle = value; }
        public string LineColor { get => string.IsNullOrWhiteSpace(_lineColor) ? _default_LineColor : _lineColor; set => _lineColor = value; }
        public string LineStyle { get => string.IsNullOrWhiteSpace(_lineStyle) ? _default_LineStyle : _lineStyle; set => _lineStyle = value; }
        public string LineCap { get => string.IsNullOrWhiteSpace(_lineCap) ? _default_LineCap : _lineCap; set => _lineCap = value; }
        public string LineDashPattern { get => string.IsNullOrWhiteSpace(_lineDashPattern) ? _default_LineDashPattern : _lineDashPattern; set => _lineDashPattern = value; }
        public string SourceArrowShape { get => string.IsNullOrWhiteSpace(_sourceArrowShape) ? _default_SourceArrowShape : _sourceArrowShape; set => _sourceArrowShape = value; }
        public string TargetArrowShape { get => string.IsNullOrWhiteSpace(_targetArrowShape) ? _default_TargetArrowShape : _targetArrowShape; set => _targetArrowShape = value; }
        public string SourceArrowColor { get => string.IsNullOrWhiteSpace(_sourceArrowColor) ? _default_SourceArrowColor : _sourceArrowColor; set => _sourceArrowColor = value; }
        public string TargetArrowColor { get => string.IsNullOrWhiteSpace(_targetArrowColor) ? _default_TargetArrowColor : _targetArrowColor; set => _targetArrowColor = value; }
        public string SourceArrowWidth { get => string.IsNullOrWhiteSpace(_sourceArrowWidth) ? _default_SourceArrowWidth : _sourceArrowWidth; set => _sourceArrowWidth = value; }
        public string TargetArrowWidth { get => string.IsNullOrWhiteSpace(_targetArrowWidth) ? _default_TargetArrowWidth : _targetArrowWidth; set => _targetArrowWidth = value; }
        public string ArrowScale { get => string.IsNullOrWhiteSpace(_arrowScale) ? _default_ArrowScale : _arrowScale; set => _arrowScale = value; }
        public string LineOpacity { get => string.IsNullOrWhiteSpace(_lineOpacity) ? _default_LineOpacity : _lineOpacity; set => _lineOpacity = value; }
        public string ZIndex { get => string.IsNullOrWhiteSpace(_zIndex) ? _default_ZIndex : _zIndex; set => _zIndex = value; }

        // Selected state properties
        public string SelectedWidth { get => string.IsNullOrWhiteSpace(_selectedWidth) ? _selected_default_Width : _selectedWidth; set => _selectedWidth = value; }
        public string SelectedCurveStyle { get => string.IsNullOrWhiteSpace(_selectedCurveStyle) ? _selected_default_CurveStyle : _selectedCurveStyle; set => _selectedCurveStyle = value; }
        public string SelectedLineColor { get => string.IsNullOrWhiteSpace(_selectedLineColor) ? _selected_default_LineColor : _selectedLineColor; set => _selectedLineColor = value; }
        public string SelectedLineStyle { get => string.IsNullOrWhiteSpace(_selectedLineStyle) ? _selected_default_LineStyle : _selectedLineStyle; set => _selectedLineStyle = value; }
        public string SelectedLineCap { get => string.IsNullOrWhiteSpace(_selectedLineCap) ? _selected_default_LineCap : _selectedLineCap; set => _selectedLineCap = value; }
        public string SelectedLineDashPattern { get => string.IsNullOrWhiteSpace(_selectedLineDashPattern) ? _selected_default_LineDashPattern : _selectedLineDashPattern; set => _selectedLineDashPattern = value; }
        public string SelectedSourceArrowShape { get => string.IsNullOrWhiteSpace(_selectedSourceArrowShape) ? _selected_default_SourceArrowShape : _selectedSourceArrowShape; set => _selectedSourceArrowShape = value; }
        public string SelectedTargetArrowShape { get => string.IsNullOrWhiteSpace(_selectedTargetArrowShape) ? _selected_default_TargetArrowShape : _selectedTargetArrowShape; set => _selectedTargetArrowShape = value; }
        public string SelectedSourceArrowColor { get => string.IsNullOrWhiteSpace(_selectedSourceArrowColor) ? _selected_default_SourceArrowColor : _selectedSourceArrowColor; set => _selectedSourceArrowColor = value; }
        public string SelectedTargetArrowColor { get => string.IsNullOrWhiteSpace(_selectedTargetArrowColor) ? _selected_default_TargetArrowColor : _selectedTargetArrowColor; set => _selectedTargetArrowColor = value; }
        public string SelectedSourceArrowWidth { get => string.IsNullOrWhiteSpace(_selectedSourceArrowWidth) ? _selected_default_SourceArrowWidth : _selectedSourceArrowWidth; set => _selectedSourceArrowWidth = value; }
        public string SelectedTargetArrowWidth { get => string.IsNullOrWhiteSpace(_selectedTargetArrowWidth) ? _selected_default_TargetArrowWidth : _selectedTargetArrowWidth; set => _selectedTargetArrowWidth = value; }
        public string SelectedArrowScale { get => string.IsNullOrWhiteSpace(_selectedArrowScale) ? _selected_default_ArrowScale : _selectedArrowScale; set => _selectedArrowScale = value; }
        public string SelectedLineOpacity { get => string.IsNullOrWhiteSpace(_selectedLineOpacity) ? _selected_default_LineOpacity : _selectedLineOpacity; set => _selectedLineOpacity = value; }
        public string SelectedZIndex { get => string.IsNullOrWhiteSpace(_selectedZIndex) ? _selected_default_ZIndex : _selectedZIndex; set => _selectedZIndex = value; }

        #endregion
    }
}
