using SkiaSharp;

namespace Foundamentals.Skia.Configs
{
    public class SkiaFontConfig
    {
        public string Family { get; set; } = "Arial";
        public float Size { get; set; } = 16f;
        public SKColor Color { get; set; } = SKColors.White;
        public SKFontStyleWeight Weight { get; set; } = SKFontStyleWeight.Normal;
        public SKFontStyleSlant Slant { get; set; } = SKFontStyleSlant.Upright;
    }
}
