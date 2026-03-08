using SkiaSharp;

namespace AmatoFluent.ViewModels
{
    public class Ball
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Radius { get; set; }
        public float VX { get; set; }
        public float VY { get; set; }
        public SKColor Color { get; set; }
        public float Mass { get; set; }

        public Ball(float startX, float startY, float radius, float vx, float vy, SKColor color)
        {
            X = startX;
            Y = startY;
            Radius = radius;
            VX = vx;
            VY = vy;
            Color = color;
            Mass = radius; // Assign mass proportional to radius
        }

        public void UpdatePhysics(int canvasWidth, int canvasHeight)
        {
            X += VX;
            Y += VY;

            // Bounce on X axis
            if (X - Radius < 0)
            {
                X = Radius;
                VX = -VX;
            }
            else if (X + Radius > canvasWidth)
            {
                X = canvasWidth - Radius;
                VX = -VX;
            }

            // Bounce on Y axis
            if (Y - Radius < 0)
            {
                Y = Radius;
                VY = -VY;
            }
            else if (Y + Radius > canvasHeight)
            {
                Y = canvasHeight - Radius;
                VY = -VY;
            }
        }
        
        public void ResetToCenter(int canvasWidth, int canvasHeight)
        {
            X = canvasWidth / 2f;
            Y = canvasHeight / 2f;
        }
    }
}
