using SkiaSharp;

namespace Foundamentals.Skia
{
    public interface ISkiaObject
    {
        float X { get; set; }
        float Y { get; set; }
        float Z { get; set; }
        float Width { get; set; }
        float Height { get; set; }
        bool IsVisible { get; set; }

        void Draw(SKCanvas canvas);
        bool Contains(float px, float py);
    }
}
