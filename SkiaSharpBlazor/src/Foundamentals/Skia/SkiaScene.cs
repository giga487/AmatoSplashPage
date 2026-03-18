using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using Foundamentals.Skia.Interfaces;

namespace Foundamentals.Skia
{
    public class SkiaScene
    {
        private List<ISkiaObject> _objects = new List<ISkiaObject>();

        public IReadOnlyList<ISkiaObject> Objects => _objects.AsReadOnly();

        public float CanvasWidth { get; private set; }
        public float CanvasHeight { get; private set; }

        public void UpdateCanvasSize(float width, float height)
        {
            CanvasWidth = width;
            CanvasHeight = height;
        }

        public void AddObject(ISkiaObject obj)
        {
            _objects.Add(obj);
            _objects = _objects.OrderBy(o => o.Z).ToList();
        }

        public void RemoveObject(ISkiaObject obj)
        {
            _objects.Remove(obj);
        }

        public void Clear()
        {
            _objects.Clear();
        }

        public void HandleClick(float x, float y)
        {
            List<ISkiaObject> reversed = _objects.AsEnumerable().Reverse().ToList();
            foreach (ISkiaObject obj in reversed)
            {
                if (obj.IsVisible && obj.Contains(x, y) && obj is IClickable clickable)
                {
                    clickable.OnClick();
                    break;
                }
            }
        }

        public void Render(SKCanvas canvas)
        {
            foreach (ISkiaObject obj in _objects)
            {
                if (obj.IsVisible)
                {
                    obj.Draw(canvas);
                }
            }
        }
    }
}
