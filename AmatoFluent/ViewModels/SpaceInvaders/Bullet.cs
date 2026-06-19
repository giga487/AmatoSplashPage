using SkiaSharp;
using Foundamentals.Skia;

namespace AmatoFluent.ViewModels.SpaceInvaders
{
    public enum BulletOwner
    {
        Player,
        Invader
    }

    public class Bullet : SkiaObjectBase
    {
        public float VelocityY { get; set; }
        public BulletOwner Owner { get; set; }

        public Bullet(float x, float y, BulletOwner owner)
        {
            X = x;
            Y = y;
            Z = 3f;
            Width = 4f;
            Height = 12f;
            Owner = owner;
            VelocityY = owner == BulletOwner.Player ? -10f : 7f;
        }

        public override void Draw(SKCanvas canvas)
        {
            SKColor bulletColor = Owner == BulletOwner.Player ? SKColors.Cyan : SKColors.OrangeRed;

            using SKPaint paint = new SKPaint
            {
                Color = bulletColor,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            canvas.DrawRect(X, Y, Width, Height, paint);
        }
    }
}
