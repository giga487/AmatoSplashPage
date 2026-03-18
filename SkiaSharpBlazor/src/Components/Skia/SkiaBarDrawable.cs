using SkiaSharp;
using Foundamentals.Skia;

namespace Components.Skia
{
    public class SkiaBarDrawable : SkiaObjectBase
    {
        protected readonly SkiaScene _scene;

        public float BarY { get; set; }
        public float BarHeight { get; set; }
        public SKColor Color { get; set; }

        public SkiaBarDrawable(SkiaScene scene, float barY, float barHeight, SKColor color)
        {
            _scene = scene;
            BarY = barY;
            BarHeight = barHeight;
            Color = color;
            Z = 4.5f;
        }

        public float GetChildY(float childHeight) => BarY + (BarHeight - childHeight) / 2f;
        public float GetChildXLeft(float padding) => padding;
        public float GetChildXRight(float childWidth, float padding) => _scene.CanvasWidth - childWidth - padding;
        public float GetChildXCenter(float childWidth) => (_scene.CanvasWidth - childWidth) / 2f;

        public override void Draw(SKCanvas canvas)
        {
            X = 0f;
            Y = BarY;
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
                && py >= BarY && py <= BarY + BarHeight;
        }
    }
}
