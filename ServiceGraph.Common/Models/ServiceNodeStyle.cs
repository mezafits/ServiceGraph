using System;

namespace ServiceGraph.Common
{
    public class ServiceNodeStyle
    {
        #region Default Values

        private const string _default_Width = "50px";
        private const string _default_Height = "50px";
        private const string _default_Shape = "rountd-rectangle";
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
        private const string _default_FontFamily = "Arial, sans‑serif";
        private const string _default_TextHAlign = "center";
        private const string _default_TextVAlign = "center";
        private const string _default_ZIndex = "1";

        #endregion

        private string _width;
        public string Width
        {
            get => string.IsNullOrWhiteSpace(_width) ? _default_Width : _width;
            set => _width = value;
        }

        private string _height;
        public string Height
        {
            get => string.IsNullOrWhiteSpace(_height) ? _default_Height : _height;
            set => _height = value;
        }

        private string _shape;
        public string Shape
        {
            get => string.IsNullOrWhiteSpace(_shape) ? _default_Shape : _shape;
            set => _shape = value;
        }

        private string _cornerRadius;
        public string CornerRadius
        {
            get => string.IsNullOrWhiteSpace(_cornerRadius) ? _default_CornerRadius : _cornerRadius;
            set => _cornerRadius = value;
        }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get => string.IsNullOrWhiteSpace(_backgroundColor) ? _default_BackgroundColor : _backgroundColor;
            set => _backgroundColor = value;
        }

        private string _backgroundOpacity;
        public string BackgroundOpacity
        {
            get => string.IsNullOrWhiteSpace(_backgroundOpacity) ? _default_BackgroundOpacity : _backgroundOpacity;
            set => _backgroundOpacity = value;
        }

        private string _backgroundFill;
        public string BackgroundFill
        {
            get => string.IsNullOrWhiteSpace(_backgroundFill) ? _default_BackgroundFill : _backgroundFill;
            set => _backgroundFill = value;
        }

        private string _borderColor;
        public string BorderColor
        {
            get => string.IsNullOrWhiteSpace(_borderColor) ? _default_BorderColor : _borderColor;
            set => _borderColor = value;
        }

        private string _borderWidth;
        public string BorderWidth
        {
            get => string.IsNullOrWhiteSpace(_borderWidth) ? _default_BorderWidth : _borderWidth;
            set => _borderWidth = value;
        }

        private string _borderStyle;
        public string BorderStyle
        {
            get => string.IsNullOrWhiteSpace(_borderStyle) ? _default_BorderStyle : _borderStyle;
            set => _borderStyle = value;
        }

        private string _borderOpacity;
        public string BorderOpacity
        {
            get => string.IsNullOrWhiteSpace(_borderOpacity) ? _default_BorderOpacity : _borderOpacity;
            set => _borderOpacity = value;
        }

        private string _padding;
        public string Padding
        {
            get => string.IsNullOrWhiteSpace(_padding) ? _default_Padding : _padding;
            set => _padding = value;
        }

        private string _label;
        public string Label
        {
            get => string.IsNullOrWhiteSpace(_label) ? _default_Label : _label;
            set => _label = value;
        }

        private string _color;
        public string Color
        {
            get => string.IsNullOrWhiteSpace(_color) ? _default_Color : _color;
            set => _color = value;
        }

        private string _fontSize;
        public string FontSize
        {
            get => string.IsNullOrWhiteSpace(_fontSize) ? _default_FontSize : _fontSize;
            set => _fontSize = value;
        }

        private string _fontFamily;
        public string FontFamily
        {
            get => string.IsNullOrWhiteSpace(_fontFamily) ? _default_FontFamily : _fontFamily;
            set => _fontFamily = value;
        }

        private string _textHAlign;
        public string TextHAlign
        {
            get => string.IsNullOrWhiteSpace(_textHAlign) ? _default_TextHAlign : _textHAlign;
            set => _textHAlign = value;
        }

        private string _textVAlign;
        public string TextVAlign
        {
            get => string.IsNullOrWhiteSpace(_textVAlign) ? _default_TextVAlign : _textVAlign;
            set => _textVAlign = value;
        }

        private string _zIndex;
        public string ZIndex
        {
            get => string.IsNullOrWhiteSpace(_zIndex) ? _default_ZIndex : _zIndex;
            set => _zIndex = value;
        }
    }
}
