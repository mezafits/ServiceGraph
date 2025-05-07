using System;

namespace ServiceGraph.Common
{
    public class ServiceNodeStyle
    {
        private string _width;
        public string Width
        {
            get => string.IsNullOrWhiteSpace(_width) ? "100px" : _width;
            set => _width = value;
        }

        private string _height;
        public string Height
        {
            get => string.IsNullOrWhiteSpace(_height) ? "60px" : _height;
            set => _height = value;
        }

        private string _shape;
        public string Shape
        {
            get => string.IsNullOrWhiteSpace(_shape) ? "rectangle" : _shape;
            set => _shape = value;
        }

        private string _cornerRadius;
        public string CornerRadius
        {
            get => string.IsNullOrWhiteSpace(_cornerRadius) ? "5px" : _cornerRadius;
            set => _cornerRadius = value;
        }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get => string.IsNullOrWhiteSpace(_backgroundColor) ? "white" : _backgroundColor;
            set => _backgroundColor = value;
        }

        private string _backgroundOpacity;
        public string BackgroundOpacity
        {
            get => string.IsNullOrWhiteSpace(_backgroundOpacity) ? "1" : _backgroundOpacity;
            set => _backgroundOpacity = value;
        }

        private string _backgroundFill;
        public string BackgroundFill
        {
            get => string.IsNullOrWhiteSpace(_backgroundFill) ? "solid" : _backgroundFill;
            set => _backgroundFill = value;
        }

        private string _borderColor;
        public string BorderColor
        {
            get => string.IsNullOrWhiteSpace(_borderColor) ? "#000000" : _borderColor;
            set => _borderColor = value;
        }

        private string _borderWidth;
        public string BorderWidth
        {
            get => string.IsNullOrWhiteSpace(_borderWidth) ? "2px" : _borderWidth;
            set => _borderWidth = value;
        }

        private string _borderStyle;
        public string BorderStyle
        {
            get => string.IsNullOrWhiteSpace(_borderStyle) ? "solid" : _borderStyle;
            set => _borderStyle = value;
        }

        private string _borderOpacity;
        public string BorderOpacity
        {
            get => string.IsNullOrWhiteSpace(_borderOpacity) ? "1" : _borderOpacity;
            set => _borderOpacity = value;
        }

        private string _padding;
        public string Padding
        {
            get => string.IsNullOrWhiteSpace(_padding) ? "10px" : _padding;
            set => _padding = value;
        }

        private string _label;
        public string Label
        {
            get => string.IsNullOrWhiteSpace(_label) ? "" : _label;
            set => _label = value;
        }

        private string _color;
        public string Color
        {
            get => string.IsNullOrWhiteSpace(_color) ? "#000000" : _color;
            set => _color = value;
        }

        private string _fontSize;
        public string FontSize
        {
            get => string.IsNullOrWhiteSpace(_fontSize) ? "14px" : _fontSize;
            set => _fontSize = value;
        }

        private string _fontFamily;
        public string FontFamily
        {
            get => string.IsNullOrWhiteSpace(_fontFamily) ? "Arial, sans-serif" : _fontFamily;
            set => _fontFamily = value;
        }

        private string _textHAlign;
        public string TextHAlign
        {
            get => string.IsNullOrWhiteSpace(_textHAlign) ? "center" : _textHAlign;
            set => _textHAlign = value;
        }

        private string _textVAlign;
        public string TextVAlign
        {
            get => string.IsNullOrWhiteSpace(_textVAlign) ? "center" : _textVAlign;
            set => _textVAlign = value;
        }

        private string _zIndex;
        public string ZIndex
        {
            get => string.IsNullOrWhiteSpace(_zIndex) ? "1" : _zIndex;
            set => _zIndex = value;
        }
    }
}
