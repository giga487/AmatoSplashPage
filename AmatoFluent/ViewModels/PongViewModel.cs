using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using SkiaSharp.Views.Blazor;

namespace AmatoFluent.ViewModels
{
    public class PongViewModel : IDisposable
    {
        public float BallX { get; private set; } = 400;
        public float BallY { get; private set; } = 300;
        public float Radius { get; private set; } = 20;
        public double CurrentFps { get; private set; } = 0;
        
        public int CanvasWidth { get; private set; } = 800;
        public int CanvasHeight { get; private set; } = 600;

        private float ballVX = 5;
        private float ballVY = 5;

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
            BallX += ballVX;
            BallY += ballVY;

            if (BallX - Radius < 0 || BallX + Radius > CanvasWidth)
            {
                ballVX = -ballVX;
            }

            if (BallY - Radius < 0 || BallY + Radius > CanvasHeight)
            {
                ballVY = -ballVY;
            }
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

		private void OnPaintSurface(SKPaintSurfaceEventArgs e)
		{
			SKCanvas canvas = e.Surface.Canvas;
			canvas.Clear(SKColor.Parse("#003366"));

			using SKPaint paintBall = new SKPaint
			{
			    Color = SKColors.OrangeRed,
			    IsAntialias = true,
			    Style = SKPaintStyle.Fill
			};

			canvas.DrawCircle(BallX, BallY, Radius, paintBall);
		}
    }
}
