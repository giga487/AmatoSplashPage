using SkiaSharp;
using Foundamentals.Skia;

namespace AmatoFluent.ViewModels.SpaceInvaders
{
    public class Invader : SkiaObjectBase
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public Invader(float x, float y, int row, int col)
        {
            X = x;
            Y = y;
            Z = 2f;
            Width = 30f;
            Height = 25f;
            Row = row;
            Col = col;
        }

        public override void Draw(SKCanvas canvas)
        {
            SKColor[] rowColors = new SKColor[]
            {
                SKColors.Red,
                SKColors.OrangeRed,
                SKColors.Orange,
                SKColors.Gold,
                SKColors.Yellow,
                SKColors.GreenYellow,
                SKColors.Cyan,
                SKColors.DeepSkyBlue
            };

            SKColor bodyColor = rowColors[Row % rowColors.Length];

            using SKPaint paint = new SKPaint
            {
                Color = bodyColor,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            // Body: cross shape to evoke alien silhouette
            canvas.DrawRect(X + 5, Y, Width - 10, Height - 5, paint);
            canvas.DrawRect(X, Y + 5, Width, Height - 10, paint);

            // Eyes
            using SKPaint eyePaint = new SKPaint
            {
                Color = SKColors.Black,
                Style = SKPaintStyle.Fill
            };

            canvas.DrawCircle(X + 8, Y + 8, 3, eyePaint);
            canvas.DrawCircle(X + Width - 8, Y + 8, 3, eyePaint);
        }
    }
}
