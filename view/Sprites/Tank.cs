using MG_SplaTank.states;
using Microsoft.Xna.Framework;
using System;

namespace MG_SplaTank.view.Sprites
{
    public class Tank : MovingSprite
    {
        public bool IsPlayer1 { get; set; }
        public int Health { get; set; } = 100;
        public bool Invincible { get; set; }
        private const int SPAWN_X1 = 280;
        private const int SPAWN_X2 = Match.SCREEN_WIDTH - SPAWN_X1;
        private const int SPAWN_Y = Match.SCREEN_HEIGHT / 2 + 43;
        public const int DELTA_ROTATION = 2;
        public const int STD_VELOCITY = 2;
        public const int MAX_VELOCITY = 4;
        private DateTime invincibilityEnd;

        public Tank(bool player1)
        {
            MaxVel = STD_VELOCITY;
            Acceleration = 0.2f;
            IsPlayer1 = player1;
            Restart();
            Width = Texture.Width;
            Height = Texture.Height;
        }
        public void Restart()
        {
            if (IsPlayer1)
            {
                Position = new Vector2(SPAWN_X1, SPAWN_Y);
                Rotation = 0;
                Texture = Assets.Tank1Texture;
            }
            else
            {
                Position = new Vector2(SPAWN_X2, SPAWN_Y);
                Rotation = 180;
                Texture = Assets.Tank2Texture;
            }
            Health = 100;
            UpdateRectangle();
            Invincible = true;
            invincibilityEnd = DateTime.Now.AddSeconds(5);

        }
        public bool InvincibilityOn() { return DateTime.Now < invincibilityEnd; }
    }
}
