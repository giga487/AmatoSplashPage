using SkiaSharp;
using Foundamentals.Skia;

namespace Components.Skia
{
    public class SkiaFooterDrawable : SkiaObjectBase
    {
        private readonly SkiaScene _scene;

        public float BarHeight { get; set; }
        public SKColor Color { get; set; }

        private float CurrentY => _scene.CanvasHeight - BarHeight;

        public SkiaFooterDrawable(SkiaScene scene, float barHeight, SKColor color)
        {
            _scene = scene;
            BarHeight = barHeight;
            Color = color;
            Z = 4.5f;
        }

        public float GetChildY(float childHeight) => CurrentY + (BarHeight - childHeight) / 2f;
        public float GetChildXLeft(float padding) => padding;
        public float GetChildXRight(float childWidth, float padding) => _scene.CanvasWidth - childWidth - padding;
        public float GetChildXCenter(float childWidth) => (_scene.CanvasWidth - childWidth) / 2f;

        public override void Draw(SKCanvas canvas)
        {
            X = 0f;
            Y = CurrentY;
            Width = _scene.CanvasWidth;
            Height = BarHeight;

            using SKPaint paint = new SKPaint
            {
                Color = Color,
                IsAntialias = false,
                Style = SKPaintStyle.Fill
            };

            canvas.DrawRect(X, Y, Width, Height, paint);
        }

        public override bool Contains(float px, float py)
        {
            return px >= 0f && px <= _scene.CanvasWidth
                && py >= CurrentY && py <= CurrentY + BarHeight;
        }
    }
}
