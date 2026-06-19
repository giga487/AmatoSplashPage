using SkiaSharp;

namespace Foundamentals.Skia.Configs
{
    public class SkiaBackgroundConfig
    {
        public SKColor Color { get; set; } = new SKColor(50, 80, 200);
        public float CornerRadius { get; set; } = 6f;
        public SKColor BorderColor { get; set; } = SKColors.White;
        public float BorderWidth { get; set; } = 1.5f;
    }
}
