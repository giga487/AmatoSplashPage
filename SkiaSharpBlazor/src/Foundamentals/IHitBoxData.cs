namespace Foundamentals
{
    public interface IHitBoxData
    {
        float X { get; set; }
        float Y { get; set; }
        float Width { get; set; }
        float Height { get; set; }

        bool Contains(float x, float y);
    }
}
