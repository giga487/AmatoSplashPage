using SkiaSharp;

namespace Foundamentals.Skia
{
    public abstract class SkiaObjectBase : ISkiaObject
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public bool IsVisible { get; set; } = true;

        public abstract void Draw(SKCanvas canvas);

        public virtual bool Contains(float px, float py)
        {
            return px >= X && px <= X + Width && py >= Y && py <= Y + Height;
        }
    }
}
