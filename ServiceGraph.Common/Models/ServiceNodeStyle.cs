using System;

namespace ServiceGraph.Common
{
    public class ServiceNodeStyle
    {
        #region Default Values

        private const string _default_Width = "50px";
        private const string _default_Height = "50px";
        private const string _default_Shape = "rounded-rectangle";
        private const string _default_CornerRadius = "5px";
        private const string _default_BackgroundColor = "white";
        private const string _default_BackgroundOpacity = "1";
        private const string _default_BackgroundFill = "solid";
        private const string _default_BorderColor = "rgba(0, 122, 255, 1.0)";
        private const string _default_BorderWidth = "2px";
        private const string _default_BorderStyle = "solid";
        private const string _default_BorderOpacity = "1";
        private const string _default_Padding = "10px";
        private const string _default_Label = "";
        private const string _default_Color = "white";
        private const string _default_FontSize = "14px";
        private const string _default_FontFamily = "Arial, sans-serif";
        private const string _default_TextHAlign = "center";
        private const string _default_TextVAlign = "center";
        private const string _default_ZIndex = "1";

        #endregion

        #region Selected Default Values

        private const string _selected_default_Width = _default_Width;
        private const string _selected_default_Height = _default_Height;
        private const string _selected_default_Shape = _default_Shape;
        private const string _selected_default_CornerRadius = _default_CornerRadius;
        private const string _selected_default_BackgroundColor = _default_BackgroundColor;
        private const string _selected_default_BackgroundOpacity = _default_BackgroundOpacity;
        private const string _selected_default_BackgroundFill = _default_BackgroundFill;
        private const string _selected_default_BorderColor = _default_BorderColor;
        private const string _selected_default_BorderWidth = _default_BorderWidth;
        private const string _selected_default_BorderStyle = _default_BorderStyle;
        private const string _selected_default_BorderOpacity = _default_BorderOpacity;
        private const string _selected_default_Padding = _default_Padding;
        private const string _selected_default_Label = _default_Label;
        private const string _selected_default_Color = _default_Color;
        private const string _selected_default_FontSize = _default_FontSize;
        private const string _selected_default_FontFamily = _default_FontFamily;
        private const string _selected_default_TextHAlign = _default_TextHAlign;
        private const string _selected_default_TextVAlign = _default_TextVAlign;
        private const string _selected_default_ZIndex = _default_ZIndex;

        #endregion

        // Instance fields for normal state
        private string _width;
        private string _height;
        private string _shape;
        private string _cornerRadius;
        private string _backgroundColor;
        private string _backgroundOpacity;
        private string _backgroundFill;
        private string _borderColor;
        private string _borderWidth;
        private string _borderStyle;
        private string _borderOpacity;
        private string _padding;
        private string _label;
        private string _color;
        private string _fontSize;
        private string _fontFamily;
        private string _textHAlign;
        private string _textVAlign;
        private string _zIndex;

        // Instance fields for selected state
        private string _selectedWidth;
        private string _selectedHeight;
        private string _selectedShape;
        private string _selectedCornerRadius;
        private string _selectedBackgroundColor;
        private string _selectedBackgroundOpacity;
        private string _selectedBackgroundFill;
        private string _selectedBorderColor;
        private string _selectedBorderWidth;
        private string _selectedBorderStyle;
        private string _selectedBorderOpacity;
        private string _selectedPadding;
        private string _selectedLabel;
        private string _selectedColor;
        private string _selectedFontSize;
        private string _selectedFontFamily;
        private string _selectedTextHAlign;
        private string _selectedTextVAlign;
        private string _selectedZIndex;

        #region Properties

        // Normal state properties
        public string Width { get => string.IsNullOrWhiteSpace(_width) ? _default_Width : _width; set => _width = value; }
        public string Height { get => string.IsNullOrWhiteSpace(_height) ? _default_Height : _height; set => _height = value; }
        public string Shape { get => string.IsNullOrWhiteSpace(_shape) ? _default_Shape : _shape; set => _shape = value; }
        public string CornerRadius { get => string.IsNullOrWhiteSpace(_cornerRadius) ? _default_CornerRadius : _cornerRadius; set => _cornerRadius = value; }
        public string BackgroundColor { get => string.IsNullOrWhiteSpace(_backgroundColor) ? _default_BackgroundColor : _backgroundColor; set => _backgroundColor = value; }
        public string BackgroundOpacity { get => string.IsNullOrWhiteSpace(_backgroundOpacity) ? _default_BackgroundOpacity : _backgroundOpacity; set => _backgroundOpacity = value; }
        public string BackgroundFill { get => string.IsNullOrWhiteSpace(_backgroundFill) ? _default_BackgroundFill : _backgroundFill; set => _backgroundFill = value; }
        public string BorderColor { get => string.IsNullOrWhiteSpace(_borderColor) ? _default_BorderColor : _borderColor; set => _borderColor = value; }
        public string BorderWidth { get => string.IsNullOrWhiteSpace(_borderWidth) ? _default_BorderWidth : _borderWidth; set => _borderWidth = value; }
        public string BorderStyle { get => string.IsNullOrWhiteSpace(_borderStyle) ? _default_BorderStyle : _borderStyle; set => _borderStyle = value; }
        public string BorderOpacity { get => string.IsNullOrWhiteSpace(_borderOpacity) ? _default_BorderOpacity : _borderOpacity; set => _borderOpacity = value; }
        public string Padding { get => string.IsNullOrWhiteSpace(_padding) ? _default_Padding : _padding; set => _padding = value; }
        public string Label { get => string.IsNullOrWhiteSpace(_label) ? _default_Label : _label; set => _label = value; }
        public string Color { get => string.IsNullOrWhiteSpace(_color) ? _default_Color : _color; set => _color = value; }
        public string FontSize { get => string.IsNullOrWhiteSpace(_fontSize) ? _default_FontSize : _fontSize; set => _fontSize = value; }
        public string FontFamily { get => string.IsNullOrWhiteSpace(_fontFamily) ? _default_FontFamily : _fontFamily; set => _fontFamily = value; }
        public string TextHAlign { get => string.IsNullOrWhiteSpace(_textHAlign) ? _default_TextHAlign : _textHAlign; set => _textHAlign = value; }
        public string TextVAlign { get => string.IsNullOrWhiteSpace(_textVAlign) ? _default_TextVAlign : _textVAlign; set => _textVAlign = value; }
        public string ZIndex { get => string.IsNullOrWhiteSpace(_zIndex) ? _default_ZIndex : _zIndex; set => _zIndex = value; }

        // Selected state properties
        public string SelectedWidth { get => string.IsNullOrWhiteSpace(_selectedWidth) ? _selected_default_Width : _selectedWidth; set => _selectedWidth = value; }
        public string SelectedHeight { get => string.IsNullOrWhiteSpace(_selectedHeight) ? _selected_default_Height : _selectedHeight; set => _selectedHeight = value; }
        public string SelectedShape { get => string.IsNullOrWhiteSpace(_selectedShape) ? _selected_default_Shape : _selectedShape; set => _selectedShape = value; }
        public string SelectedCornerRadius { get => string.IsNullOrWhiteSpace(_selectedCornerRadius) ? _selected_default_CornerRadius : _selectedCornerRadius; set => _selectedCornerRadius = value; }
        public string SelectedBackgroundColor { get => string.IsNullOrWhiteSpace(_selectedBackgroundColor) ? _selected_default_BackgroundColor : _selectedBackgroundColor; set => _selectedBackgroundColor = value; }
        public string SelectedBackgroundOpacity { get => string.IsNullOrWhiteSpace(_selectedBackgroundOpacity) ? _selected_default_BackgroundOpacity : _selectedBackgroundOpacity; set => _selectedBackgroundOpacity = value; }
        public string SelectedBackgroundFill { get => string.IsNullOrWhiteSpace(_selectedBackgroundFill) ? _selected_default_BackgroundFill : _selectedBackgroundFill; set => _selectedBackgroundFill = value; }
        public string SelectedBorderColor { get => string.IsNullOrWhiteSpace(_selectedBorderColor) ? _selected_default_BorderColor : _selectedBorderColor; set => _selectedBorderColor = value; }
        public string SelectedBorderWidth { get => string.IsNullOrWhiteSpace(_selectedBorderWidth) ? _selected_default_BorderWidth : _selectedBorderWidth; set => _selectedBorderWidth = value; }
        public string SelectedBorderStyle { get => string.IsNullOrWhiteSpace(_selectedBorderStyle) ? _selected_default_BorderStyle : _selectedBorderStyle; set => _selectedBorderStyle = value; }
        public string SelectedBorderOpacity { get => string.IsNullOrWhiteSpace(_selectedBorderOpacity) ? _selected_default_BorderOpacity : _selectedBorderOpacity; set => _selectedBorderOpacity = value; }
        public string SelectedPadding { get => string.IsNullOrWhiteSpace(_selectedPadding) ? _selected_default_Padding : _selectedPadding; set => _selectedPadding = value; }
        public string SelectedLabel { get => string.IsNullOrWhiteSpace(_selectedLabel) ? _selected_default_Label : _selectedLabel; set => _selectedLabel = value; }
        public string SelectedColor { get => string.IsNullOrWhiteSpace(_selectedColor) ? _selected_default_Color : _selectedColor; set => _selectedColor = value; }
        public string SelectedFontSize { get => string.IsNullOrWhiteSpace(_selectedFontSize) ? _selected_default_FontSize : _selectedFontSize; set => _selectedFontSize = value; }
        public string SelectedFontFamily { get => string.IsNullOrWhiteSpace(_selectedFontFamily) ? _selected_default_FontFamily : _selectedFontFamily; set => _selectedFontFamily = value; }
        public string SelectedTextHAlign { get => string.IsNullOrWhiteSpace(_selectedTextHAlign) ? _selected_default_TextHAlign : _selectedTextHAlign; set => _selectedTextHAlign = value; }
        public string SelectedTextVAlign { get => string.IsNullOrWhiteSpace(_selectedTextVAlign) ? _selected_default_TextVAlign : _selectedTextVAlign; set => _selectedTextVAlign = value; }
        public string SelectedZIndex { get => string.IsNullOrWhiteSpace(_selectedZIndex) ? _selected_default_ZIndex : _selectedZIndex; set => _selectedZIndex = value; }

        #endregion
    }
}
