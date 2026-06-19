using SkiaSharp;
using Foundamentals.Skia;
using Foundamentals.Skia.Configs;

namespace Components.Skia
{
    public class SkiaBoxDrawable : SkiaObjectBase
    {
        public string Text { get; set; }
        public SkiaFontConfig Font { get; set; }
        public string? SubText { get; set; }
        public SkiaFontConfig? SubFont { get; set; }
        public SkiaBackgroundConfig Background { get; set; }
        public float Padding { get; set; }
        public ButtonAlignment TextAlignment { get; set; }

        public SkiaBoxDrawable(
            float x, float y, float z,
            float width, float height,
            string text,
            SkiaFontConfig font,
            SkiaBackgroundConfig background,
            float padding,
            ButtonAlignment textAlignment)
        {
            X = x;
            Y = y;
            Z = z;
            Width = width;
            Height = height;
            Text = text;
            Font = font;
            Background = background;
            Padding = padding;
            TextAlignment = textAlignment;
        }

        public override void Draw(SKCanvas canvas)
        {
            SKRect rect = new SKRect(X, Y, X + Width, Y + Height);

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

            if (string.IsNullOrEmpty(Text))
                return;

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

            skFont.GetFontMetrics(out SKFontMetrics metrics);
            float lineHeight = metrics.Descent - metrics.Ascent;

            string[] lines = Text.Split('\n');
            float totalTextHeight = lines.Length * lineHeight + (lines.Length - 1) * 2f;

            bool hasSubText = !string.IsNullOrEmpty(SubText);
            SkiaFontConfig subFontConfig = SubFont ?? new SkiaFontConfig { Size = Font.Size * 0.55f, Color = new SKColor(50, 50, 50) };

            SKTypeface subTypeface = SKTypeface.FromFamilyName(
                subFontConfig.Family,
                subFontConfig.Weight,
                SKFontStyleWidth.Normal,
                subFontConfig.Slant
            );
            using SKFont skSubFont = new SKFont(subTypeface, subFontConfig.Size);
            skSubFont.GetFontMetrics(out SKFontMetrics subMetrics);
            float subLineHeight = subMetrics.Descent - subMetrics.Ascent;

            float subGap = hasSubText ? 12f : 0f;
            float allContentHeight = totalTextHeight + (hasSubText ? subGap + subLineHeight : 0f);
            float startY = Y + (Height - allContentHeight) / 2f - metrics.Ascent;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                float lineWidth = skFont.MeasureText(line);

                float drawX = TextAlignment switch
                {
                    ButtonAlignment.Right  => X + Width - Padding - lineWidth,
                    ButtonAlignment.Center => X + (Width - lineWidth) / 2f,
                    _                      => X + Padding
                };

                float drawY = startY + i * (lineHeight + 2f);
                canvas.DrawText(line, drawX, drawY, skFont, textPaint);
            }

            if (hasSubText)
            {
                using SKPaint subPaint = new SKPaint { Color = subFontConfig.Color, IsAntialias = true };
                float subWidth = skSubFont.MeasureText(SubText!);
                float subX = TextAlignment switch
                {
                    ButtonAlignment.Right  => X + Width - Padding - subWidth,
                    ButtonAlignment.Center => X + (Width - subWidth) / 2f,
                    _                      => X + Padding
                };
                float lastTitleBaseline = startY + (lines.Length - 1) * (lineHeight + 2f);
                float subY = lastTitleBaseline + metrics.Descent + subGap - subMetrics.Ascent;
                canvas.DrawText(SubText!, subX, subY, skSubFont, subPaint);
            }
        }
    }
}
