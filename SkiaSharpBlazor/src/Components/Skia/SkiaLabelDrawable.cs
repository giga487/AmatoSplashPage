using System;
using SkiaSharp;
using Foundamentals.Skia;
using Foundamentals.Skia.Configs;

namespace Components.Skia
{
    public class SkiaLabelDrawable : SkiaObjectBase
    {
        public string Text { get; set; }
        public Func<string>? GetText { get; set; }
        public SkiaFontConfig Font { get; set; }
        public ButtonAlignment Alignment { get; set; }
        public float Padding { get; set; }

        private readonly SkiaBarDrawable? _bar;

        public SkiaLabelDrawable(string text, SkiaFontConfig font, ButtonAlignment alignment, float padding, SkiaBarDrawable? bar)
        {
            Text = text;
            Font = font;
            Alignment = alignment;
            Padding = padding;
            _bar = bar;
            Z = 5f;
        }

        public override void Draw(SKCanvas canvas)
        {
            string currentText = GetText?.Invoke() ?? Text;

            SKTypeface typeface = SKTypeface.FromFamilyName(
                Font.Family,
                Font.Weight,
                SKFontStyleWidth.Normal,
                Font.Slant
            );

            using SKFont skFont = new SKFont(typeface, Font.Size);

            float textWidth = skFont.MeasureText(currentText);

            float drawX;
            if (_bar != null)
            {
                drawX = Alignment switch
                {
                    ButtonAlignment.Left   => _bar.GetChildXLeft(Padding),
                    ButtonAlignment.Right  => _bar.GetChildXRight(textWidth, Padding),
                    ButtonAlignment.Center => _bar.GetChildXCenter(textWidth),
                    _                      => X
                };
            }
            else
            {
                drawX = X;
            }

            float drawY;
            if (_bar != null)
            {
                skFont.GetFontMetrics(out SKFontMetrics metrics);
                float textHeight = metrics.Descent - metrics.Ascent;
                float topY = _bar.GetChildY(textHeight);
                drawY = topY - metrics.Ascent;
            }
            else
            {
                drawY = Y;
            }

            using SKPaint textPaint = new SKPaint
            {
                Color = Font.Color,
                IsAntialias = true
            };

            canvas.DrawText(currentText, drawX, drawY, skFont, textPaint);
        }
    }
}
