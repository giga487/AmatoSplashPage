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

        private SKTypeface? _typeface;
        private SKFont? _fontTitle;
        private SKFont? _fontSub;
        private SKFont? _fontFps;

        private SKPaint? _paintText;
        private SKPaint? _paintSubText;
        private SKPaint? _paintFps;
        private SKPaint? _paintBall;

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
            _typeface?.Dispose();
            _fontTitle?.Dispose();
            _fontSub?.Dispose();
            _fontFps?.Dispose();
            _paintText?.Dispose();
            _paintSubText?.Dispose();
            _paintFps?.Dispose();
            _paintBall?.Dispose();
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
				
                _typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright) ?? SKTypeface.Default;
                _fontTitle = new SKFont(_typeface);
                _fontSub = new SKFont();
                _fontFps = new SKFont();

                _paintText = new SKPaint { Color = SKColors.White, IsAntialias = true };
                _paintSubText = new SKPaint { Color = SKColors.LightGray, IsAntialias = true };
                _paintFps = new SKPaint { Color = SKColors.Yellow, IsAntialias = true };
                _paintBall = new SKPaint { IsAntialias = true, Style = SKPaintStyle.Fill };

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

            if (_paintBall != null)
            {
			    foreach (Ball ball in Balls)
			    {
			        _paintBall.Color = ball.Color;
			        canvas.DrawCircle(ball.X, ball.Y, ball.Radius, _paintBall);
			    }
            }

            if (_fontTitle != null && _paintText != null)
            {
			    string titleText = "Pong in Blazor Wasm";
			    float titleSize = Math.Max(20f, CanvasWidth * 0.05f);
			    _fontTitle.Size = titleSize;
			    
			    while (_fontTitle.MeasureText(titleText) > CanvasWidth - 40 && _fontTitle.Size > 10)
			    {
			        _fontTitle.Size -= 1;
			    }

			    float titleY = _fontTitle.Size + 20;
			    canvas.DrawText(titleText, 20, titleY, _fontTitle, _paintText);

                if (_fontSub != null && _paintSubText != null)
                {
			        string subText = "Canvas test";
			        float subSize = Math.Max(12f, CanvasWidth * 0.025f);
			        _fontSub.Size = subSize;

			        while (_fontSub.MeasureText(subText) > CanvasWidth - 40 && _fontSub.Size > 8)
			        {
			            _fontSub.Size -= 1;
			        }

			        float subY = titleY + _fontSub.Size + 10;
			        canvas.DrawText(subText, 20, subY, _fontSub, _paintSubText);
                }
            }

            if (_fontFps != null && _paintFps != null)
            {
			    string fpsText = $"FPS: {Math.Round(CurrentFps, 1)}";
			    _fontFps.Size = 16f;

			    float fpsX = CanvasWidth - _fontFps.MeasureText(fpsText) - 20;
			    float fpsY = CanvasHeight - 20;
			    canvas.DrawText(fpsText, fpsX, fpsY, _fontFps, _paintFps);
            }
		}
    }
}
