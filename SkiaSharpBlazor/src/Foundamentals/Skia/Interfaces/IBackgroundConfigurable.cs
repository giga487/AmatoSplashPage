using Foundamentals.Skia.Configs;

namespace Foundamentals.Skia.Interfaces
{
    public interface IBackgroundConfigurable
    {
        SkiaBackgroundConfig Background { get; set; }
    }
}
