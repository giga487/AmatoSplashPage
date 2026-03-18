using SkiaSharp;
using Foundamentals.Skia;

namespace Components.Skia
{
    public class SkiaFooterDrawable : SkiaBarDrawable
    {
        public SkiaFooterDrawable(SkiaScene scene, float barHeight, SKColor color)
            : base(scene, barY: 0f, barHeight, color) { }

        public override void Draw(SKCanvas canvas)
        {
            BarY = _scene.CanvasHeight - BarHeight;
            base.Draw(canvas);
        }

        public override bool Contains(float px, float py)
        {
            float currentY = _scene.CanvasHeight - BarHeight;
            return px >= 0f && px <= _scene.CanvasWidth
                && py >= currentY && py <= currentY + BarHeight;
        }
    }
}
