using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using SkiaSharp.Views.Blazor;

namespace AmatoFluent.ViewModels
{
    public class PongViewModel : IDisposable
    {
        public List<Ball> Balls { get; private set; } = new List<Ball>();
        public double CurrentFps { get; private set; } = 0;
        
        public int CanvasWidth { get; private set; } = 0;
        public int CanvasHeight { get; private set; } = 0;

        private bool _isInitialized = false;

        private System.Threading.Timer? timer;
        private DateTime lastFrameTime = DateTime.Now;
        private Queue<double> frameTimes = new Queue<double>(100);

        public event Action? OnStateChanged;
        private SKCanvasView? _canvas;
		public PongViewModel()
        {
            //for (int i = 0; i < 100; i++)
            //{
            //    frameTimes.Enqueue(1000.0 / 60.0);
            //}
        }

        public void Configure(SKCanvasView canvas)
        {
            _canvas = canvas;
            _canvas.OnPaintSurface += OnPaintSurface;



		}

        public void Start()
        {
            if (timer == null)
            {
                timer = new System.Threading.Timer((object? state) =>
                {
                    UpdatePhysics();
                    _canvas?.Invalidate();
                }, null, 0, 16);
            }
        }

        public void CalculateFPS()
        {
            DateTime now = DateTime.Now;
            double frameTime = (now - lastFrameTime).TotalMilliseconds;
            lastFrameTime = now;

            if (frameTimes.Count >= 100)
            {
                frameTimes.Dequeue();
            }
            frameTimes.Enqueue(frameTime);

            double averageFrameTime = frameTimes.Average();
            CurrentFps = 1000.0 / averageFrameTime;
        }

        private void UpdatePhysics()
        {
            if (CanvasWidth > 0 && CanvasHeight > 0)
            {
                foreach (Ball ball in Balls)
                {
                    ball.UpdatePhysics(CanvasWidth, CanvasHeight);
                }

                // Check for collisions between balls
                for (int i = 0; i < Balls.Count; i++)
                {
                    for (int j = i + 1; j < Balls.Count; j++)
                    {
                        Ball ballA = Balls[i];
                        Ball ballB = Balls[j];

                        float dx = ballB.X - ballA.X;
                        float dy = ballB.Y - ballA.Y;
                        float distanceSquare = (dx * dx) + (dy * dy);
                        float minDistance = ballA.Radius + ballB.Radius;

                        if (distanceSquare < (minDistance * minDistance))
                        {
                            float distance = (float)Math.Sqrt(distanceSquare);

                            if (distance > 0)
                            {
                                // Avoid overlap by pushing them apart
                                float overlap = minDistance - distance;
                                float nxPush = (dx / distance) * (overlap / 2f);
                                float nyPush = (dy / distance) * (overlap / 2f);
                                
                                ballA.X -= nxPush;
                                ballA.Y -= nyPush;
                                ballB.X += nxPush;
                                ballB.Y += nyPush;

                                // Normal vector
                                float nx = dx / distance;
                                float ny = dy / distance;

                                // Relative velocity
                                float vx = ballB.VX - ballA.VX;
                                float vy = ballB.VY - ballA.VY;

                                // Velocity along the normal
                                float dotProduct = (vx * nx) + (vy * ny);

                                // Do not resolve if velocities are separating
                                if (dotProduct <= 0)
                                {
                                    // Calculate restitution (elasticity)
                                    float restitution = 1.0f;

                                    // Calculate impulse scalar
                                    float impulse = -(1f + restitution) * dotProduct / ((1f / ballA.Mass) + (1f / ballB.Mass));

                                    // Apply impulse
                                    ballA.VX -= (impulse * nx) / ballA.Mass;
                                    ballA.VY -= (impulse * ny) / ballA.Mass;

                                    ballB.VX += (impulse * nx) / ballB.Mass;
                                    ballB.VY += (impulse * ny) / ballB.Mass;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

		private void OnPaintSurface(SKPaintSurfaceEventArgs e)
		{
			CalculateFPS();
			bool isResize = (_isInitialized && (CanvasWidth != e.Info.Width || CanvasHeight != e.Info.Height));

			CanvasWidth = e.Info.Width;
			CanvasHeight = e.Info.Height;

			if (!_isInitialized && CanvasWidth > 0)
			{
				Balls.Add(new Ball(CanvasWidth / 2f, CanvasHeight / 2f, 20, 5, 5, SKColors.OrangeRed));
				Balls.Add(new Ball((CanvasWidth / 2f) + 50f, (CanvasHeight / 2f) + 50f, 15, -7, 6, SKColors.LightSeaGreen));
				Balls.Add(new Ball((CanvasWidth / 2f) - 50f, (CanvasHeight / 2f) - 50f, 25, 4, -8, SKColors.Gold));
				_isInitialized = true;
			}
			else if (isResize)
			{
				foreach (Ball ball in Balls)
				{
					ball.ResetToCenter(CanvasWidth, CanvasHeight);
				}
			}

			SKCanvas canvas = e.Surface.Canvas;
			canvas.Clear(SKColor.Parse("#003366"));

			using SKPaint paintBall = new SKPaint
			{
			    IsAntialias = true,
			    Style = SKPaintStyle.Fill
			};

			foreach (Ball ball in Balls)
			{
			    paintBall.Color = ball.Color;
			    canvas.DrawCircle(ball.X, ball.Y, ball.Radius, paintBall);
			}

			string titleText = "Pong in Blazor Wasm";
			float titleSize = Math.Max(20f, CanvasWidth * 0.05f);
			SKTypeface typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright) ?? SKTypeface.Default;
			
			using SKFont fontTitle = new SKFont(typeface, titleSize);
			using SKPaint paintText = new SKPaint
			{
				Color = SKColors.White,
				IsAntialias = true,
			};

			while (fontTitle.MeasureText(titleText) > CanvasWidth - 40 && fontTitle.Size > 10)
			{
			    fontTitle.Size -= 1;
			}

			float titleY = fontTitle.Size + 20;
			canvas.DrawText(titleText, 20, titleY, fontTitle, paintText);

			string subText = "Canvas test";
			float subSize = Math.Max(12f, CanvasWidth * 0.025f);
			
			using SKFont fontSub = new SKFont();
			fontSub.Size = subSize;

			using SKPaint paintSubText = new SKPaint
			{
				Color = SKColors.LightGray,
				IsAntialias = true,
			};

			while (fontSub.MeasureText(subText) > CanvasWidth - 40 && fontSub.Size > 8)
			{
			    fontSub.Size -= 1;
			}

			float subY = titleY + fontSub.Size + 10;
			canvas.DrawText(subText, 20, subY, fontSub, paintSubText);

			string fpsText = $"FPS: {Math.Round(CurrentFps, 1)}";
			
			using SKFont fontFps = new SKFont();
			fontFps.Size = 16f;

			using SKPaint paintFps = new SKPaint
			{
				Color = SKColors.Yellow,
				IsAntialias = true,
			};

			float fpsX = CanvasWidth - fontFps.MeasureText(fpsText) - 20;
			float fpsY = CanvasHeight - 20;
			canvas.DrawText(fpsText, fpsX, fpsY, fontFps, paintFps);
		}
    }
}
