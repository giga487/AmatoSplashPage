using SkiaSharp;
using Foundamentals.Skia;

namespace AmatoFluent.ViewModels.SpaceInvaders
{
    public class PlayerShip : SkiaObjectBase
    {
        public float Speed { get; set; } = 5f;

        public PlayerShip(float x, float y)
        {
            X = x;
            Y = y;
            Z = 4f;
            Width = 40f;
            Height = 30f;
        }

        public override void Draw(SKCanvas canvas)
        {
            using SKPaint paint = new SKPaint
            {
                Color = SKColors.LimeGreen,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            SKPath path = new SKPath();
            path.MoveTo(X + Width / 2f, Y);
            path.LineTo(X, Y + Height);
            path.LineTo(X + Width, Y + Height);
            path.Close();

            canvas.DrawPath(path, paint);
        }
    }
}
