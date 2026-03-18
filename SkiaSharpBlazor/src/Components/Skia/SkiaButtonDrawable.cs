using System;
using SkiaSharp;
using Foundamentals.Skia;
using Foundamentals.Skia.Configs;
using Foundamentals.Skia.Interfaces;

namespace Components.Skia
{
    public class SkiaButtonDrawable : SkiaObjectBase, IClickable, IFontConfigurable, IBackgroundConfigurable
    {
        public string Text { get; set; }
        public SkiaFontConfig Font { get; set; }
        public SkiaBackgroundConfig Background { get; set; }

        public Func<float>? ComputeX { get; set; }
        public Func<float>? ComputeY { get; set; }

        private readonly Action _onClick;

        public SkiaButtonDrawable(
            string text,
            float x,
            float y,
            float width,
            float height,
            SkiaFontConfig font,
            SkiaBackgroundConfig background,
            Action onClick)
        {
            Text = text;
            X = x;
            Y = y;
            Z = 5f;
            Width = width;
            Height = height;
            Font = font;
            Background = background;
            _onClick = onClick;
        }

        public void OnClick()
        {
            _onClick.Invoke();
        }

        public override bool Contains(float px, float py)
        {
            float drawX = ComputeX?.Invoke() ?? X;
            float drawY = ComputeY?.Invoke() ?? Y;
            return px >= drawX && px <= drawX + Width && py >= drawY && py <= drawY + Height;
        }

        public override void Draw(SKCanvas canvas)
        {
            float drawX = ComputeX?.Invoke() ?? X;
            float drawY = ComputeY?.Invoke() ?? Y;

            SKRect rect = new SKRect(drawX, drawY, drawX + Width, drawY + Height);

            using SKPaint bgPaint = new SKPaint
            {
                Color = Background.Color,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };
            canvas.DrawRoundRect(rect, Background.CornerRadius, Background.CornerRadius, bgPaint);

            if (Background.BorderWidth > 0)
            {
                using SKPaint borderPaint = new SKPaint
                {
                    Color = Background.BorderColor,
                    IsAntialias = true,
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = Background.BorderWidth
                };
                canvas.DrawRoundRect(rect, Background.CornerRadius, Background.CornerRadius, borderPaint);
            }

            SKTypeface typeface = SKTypeface.FromFamilyName(
                Font.Family,
                Font.Weight,
                SKFontStyleWidth.Normal,
                Font.Slant
            );

            using SKFont skFont = new SKFont(typeface, Font.Size);
            using SKPaint textPaint = new SKPaint
            {
                Color = Font.Color,
                IsAntialias = true
            };

            float textWidth = skFont.MeasureText(Text);
            float textX = drawX + (Width - textWidth) / 2f;
            float textY = drawY + (Height + Font.Size) / 2f - 2f;

            canvas.DrawText(Text, textX, textY, skFont, textPaint);
        }
    }
}
