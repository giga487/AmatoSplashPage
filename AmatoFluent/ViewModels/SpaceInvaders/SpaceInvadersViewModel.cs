using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using Foundamentals.Skia;
using Foundamentals.Skia.Configs;
using Components.Skia;

namespace AmatoFluent.ViewModels.SpaceInvaders
{
    public class SpaceInvadersViewModel : IDisposable
    {
        private const int InvaderCols = 15;
        private const int InvaderRows = 8;
        private const float InvaderSpacingX = 45f;
        private const float InvaderSpacingY = 40f;
        private const float InvaderStartX = 30f;
        private const float InvaderStartY = 100f;
        private const float InvaderMoveStep = 12f;
        private const float InvaderDropStep = 20f;
        private const float PlayerBulletCooldownMs = 400f;

        public SkiaScene Scene { get; private set; } = new SkiaScene();
        public int Score { get; private set; } = 0;
        public int Lives { get; private set; } = 3;
        public bool IsGameOver { get; private set; } = false;
        public bool IsWon { get; private set; } = false;
        public bool IsPaused { get; private set; } = false;

        private PlayerShip? _player;
        private List<Invader> _invaders = new List<Invader>();
        private List<Bullet> _bullets = new List<Bullet>();

        private float _invaderDirectionX = 1f;
        private int _invaderMoveCounter = 0;
        private int _invaderMoveInterval = 20;

        private bool _moveLeft = false;
        private bool _moveRight = false;
        private bool _shootPressed = false;
        private float _lastPlayerShootMs = 0f;
        private float _elapsedMs = 0f;

        private bool _isInitialized = false;
        private SkiaBoxDrawable? _overlayBox;

        private DateTime _lastFrameTime = DateTime.Now;
        private Queue<double> _frameTimes = new Queue<double>(100);
        private double _currentFps = 0;

        public event Action? OnStateChanged;

        public void SetMoveLeft(bool active) { _moveLeft = active; }
        public void SetMoveRight(bool active) { _moveRight = active; }
        public void SetShoot(bool active) { _shootPressed = active; }
        public void TogglePause() { IsPaused = !IsPaused; }

        public void Restart()
        {
            if (Scene.CanvasWidth > 0 && Scene.CanvasHeight > 0)
            {
                InitializeGame((int)Scene.CanvasWidth, (int)Scene.CanvasHeight);
            }
        }

        public void UpdateFrame()
        {
            int canvasWidth = (int)Scene.CanvasWidth;
            int canvasHeight = (int)Scene.CanvasHeight;

            if (!_isInitialized && canvasWidth > 0 && canvasHeight > 0)
            {
                InitializeGame(canvasWidth, canvasHeight);
            }

            if (!_isInitialized || IsGameOver || IsWon || IsPaused) return;

            DateTime now = DateTime.Now;
            double frameDelta = (now - _lastFrameTime).TotalMilliseconds;
            _lastFrameTime = now;
            _elapsedMs += (float)frameDelta;

            CalculateFPS(frameDelta);
            MovePlayer(canvasWidth);
            HandleShooting();
            UpdateBullets(canvasHeight);
            MoveInvaders(canvasWidth);
            CheckCollisions();
            CheckGameConditions();
        }

        public void DrawHUD(SKCanvas canvas)
        {
            int canvasWidth = (int)Scene.CanvasWidth;
            int canvasHeight = (int)Scene.CanvasHeight;

            using SKFont fontFps = new SKFont();
            fontFps.Size = 14f;

            using SKPaint paintFps = new SKPaint
            {
                Color = SKColors.Yellow,
                IsAntialias = true
            };

            canvas.DrawText($"FPS: {Math.Round(_currentFps, 1)}", canvasWidth - 90f, canvasHeight - 10f, fontFps, paintFps);

            if (_overlayBox != null)
            {
                if (IsPaused)
                {
                    _overlayBox.Text = "PAUSED";
                    _overlayBox.SubText = "Press Pauseè∏ to resume";
                    _overlayBox.Font = new SkiaFontConfig { Size = 48f, Color = new SKColor(30, 80, 160) };
                    _overlayBox.IsVisible = true;
                }
                else if (IsGameOver)
                {
                    _overlayBox.Text = "GAME OVER";
                    _overlayBox.SubText = "Press R to restart";
                    _overlayBox.Font = new SkiaFontConfig { Size = 48f, Color = new SKColor(180, 20, 20) };
                    _overlayBox.IsVisible = true;
                }
                else if (IsWon)
                {
                    _overlayBox.Text = "YOU WIN!";
                    _overlayBox.SubText = "Press R to restart";
                    _overlayBox.Font = new SkiaFontConfig { Size = 48f, Color = new SKColor(20, 130, 20) };
                    _overlayBox.IsVisible = true;
                }
                else
                {
                    _overlayBox.IsVisible = false;
                }
            }
        }

        private void InitializeGame(int canvasWidth, int canvasHeight)
        {
            if (_player != null) Scene.RemoveObject(_player);
            foreach (Invader invader in _invaders) Scene.RemoveObject(invader);
            foreach (Bullet bullet in _bullets) Scene.RemoveObject(bullet);
            _invaders.Clear();
            _bullets.Clear();

            if (_overlayBox != null) Scene.RemoveObject(_overlayBox);
            float boxWidth = 420f;
            float boxHeight = 160f;
            _overlayBox = new SkiaBoxDrawable(
                x: (canvasWidth - boxWidth) / 2f,
                y: (canvasHeight - boxHeight) / 2f,
                z: 10f,
                width: boxWidth,
                height: boxHeight,
                text: "",
                font: new SkiaFontConfig { Size = 48f, Color = SKColors.Black },
                background: new SkiaBackgroundConfig
                {
                    Color = new SKColor(160, 160, 160, 255),
                    CornerRadius = 12f,
                    BorderColor = new SKColor(100, 100, 100, 255),
                    BorderWidth = 1.5f
                },
                padding: 20f,
                textAlignment: ButtonAlignment.Center
            )
            { IsVisible = false };
            Scene.AddObject(_overlayBox);

            _player = new PlayerShip(canvasWidth / 2f - 20f, canvasHeight - 70f);
            Scene.AddObject(_player);

            for (int row = 0; row < InvaderRows; row++)
            {
                for (int col = 0; col < InvaderCols; col++)
                {
                    float invaderX = InvaderStartX + col * InvaderSpacingX;
                    float invaderY = InvaderStartY + row * InvaderSpacingY;
                    Invader invader = new Invader(invaderX, invaderY, row, col);
                    _invaders.Add(invader);
                    Scene.AddObject(invader);
                }
            }

            _isInitialized = true;
            Score = 0;
            Lives = 3;
            IsGameOver = false;
            IsWon = false;
            IsPaused = false;
            _invaderDirectionX = 1f;
            _invaderMoveCounter = 0;
            _invaderMoveInterval = 20;
            _elapsedMs = 0f;
            _lastPlayerShootMs = 0f;
        }

        private void MovePlayer(int canvasWidth)
        {
            if (_player == null) return;

            if (_moveLeft)
            {
                _player.X = Math.Max(0f, _player.X - _player.Speed);
            }
            if (_moveRight)
            {
                _player.X = Math.Min(canvasWidth - _player.Width, _player.X + _player.Speed);
            }
        }

        private void HandleShooting()
        {
            if (_player == null) return;
            if (!_shootPressed) return;
            if (_elapsedMs - _lastPlayerShootMs < PlayerBulletCooldownMs) return;

            Bullet bullet = new Bullet(
                _player.X + _player.Width / 2f - 2f,
                _player.Y - 12f,
                BulletOwner.Player
            );
            _bullets.Add(bullet);
            Scene.AddObject(bullet);
            _lastPlayerShootMs = _elapsedMs;
        }

        private void UpdateBullets(int canvasHeight)
        {
            List<Bullet> toRemove = new List<Bullet>();

            foreach (Bullet bullet in _bullets)
            {
                bullet.Y += bullet.VelocityY;
                if (bullet.Y < -20f || bullet.Y > canvasHeight + 20f)
                {
                    toRemove.Add(bullet);
                }
            }

            foreach (Bullet bullet in toRemove)
            {
                _bullets.Remove(bullet);
                Scene.RemoveObject(bullet);
            }
        }

        private void MoveInvaders(int canvasWidth)
        {
            _invaderMoveCounter++;
            if (_invaderMoveCounter < _invaderMoveInterval) return;
            _invaderMoveCounter = 0;

            List<Invader> activeInvaders = _invaders.Where(i => i.IsVisible).ToList();
            if (activeInvaders.Count == 0) return;

            bool hitEdge = false;
            foreach (Invader invader in activeInvaders)
            {
                float nextX = invader.X + _invaderDirectionX * InvaderMoveStep;
                if (nextX < 0 || nextX + invader.Width > canvasWidth)
                {
                    hitEdge = true;
                    break;
                }
            }

            if (hitEdge)
            {
                _invaderDirectionX *= -1f;
                foreach (Invader invader in activeInvaders)
                {
                    invader.Y += InvaderDropStep;
                }

                int remaining = activeInvaders.Count;
                int total = InvaderCols * InvaderRows;
                _invaderMoveInterval = Math.Max(4, 20 - (total - remaining) / 2);
            }
            else
            {
                foreach (Invader invader in activeInvaders)
                {
                    invader.X += _invaderDirectionX * InvaderMoveStep;
                }
            }
        }

        private void CheckCollisions()
        {
            if (_player == null) return;

            List<Bullet> bulletsToRemove = new List<Bullet>();
            List<Invader> invadersToRemove = new List<Invader>();

            List<Invader> activeInvaders = _invaders.Where(i => i.IsVisible).ToList();
            List<Bullet> currentBullets = _bullets.ToList();

            foreach (Bullet bullet in currentBullets)
            {
                if (bullet.Owner == BulletOwner.Player)
                {
                    foreach (Invader invader in activeInvaders)
                    {
                        if (BoundsOverlap(bullet, invader))
                        {
                            invader.IsVisible = false;
                            invadersToRemove.Add(invader);
                            bulletsToRemove.Add(bullet);
                            Score += 10;
                            break;
                        }
                    }
                }
                else
                {
                    if (BoundsOverlap(bullet, _player))
                    {
                        bulletsToRemove.Add(bullet);
                        Lives--;
                        if (Lives <= 0)
                        {
                            IsGameOver = true;
                        }
                    }
                }
            }

            foreach (Bullet bullet in bulletsToRemove.Distinct().ToList())
            {
                _bullets.Remove(bullet);
                Scene.RemoveObject(bullet);
            }

            foreach (Invader invader in invadersToRemove)
            {
                _invaders.Remove(invader);
                Scene.RemoveObject(invader);
            }
        }

        private static bool BoundsOverlap(ISkiaObject a, ISkiaObject b)
        {
            return a.X < b.X + b.Width
                && a.X + a.Width > b.X
                && a.Y < b.Y + b.Height
                && a.Y + a.Height > b.Y;
        }

        private void CheckGameConditions()
        {
            if (_invaders.Count(i => i.IsVisible) == 0)
            {
                IsWon = true;
                return;
            }

            if (_player == null) return;

            foreach (Invader invader in _invaders.Where(i => i.IsVisible))
            {
                if (invader.Y + invader.Height >= _player.Y)
                {
                    IsGameOver = true;
                    return;
                }
            }
        }

        private void CalculateFPS(double frameDelta)
        {
            if (_frameTimes.Count >= 100)
            {
                _frameTimes.Dequeue();
            }
            _frameTimes.Enqueue(frameDelta);
            double averageFrameTime = _frameTimes.Average();
            _currentFps = 1000.0 / averageFrameTime;
        }

        public void Dispose()
        {
        }
    }
}
