using MG_SplaTank.states;
using Microsoft.Xna.Framework;
using System;

namespace MG_SplaTank.view.Sprites
{
    public abstract class MovingSprite : Sprite
    {
		public bool IsMovinForward { get; set; } = true;
		public float Rotation { get; set; }
        public float Velocity { get; set; }
        public float Acceleration { get; set; }
        public float MaxVel { get; set; }
        public MovingSprite() { }
        public MovingSprite(Vector2 position) : base(position)
        {
            Acceleration = 0.5f;
            MaxVel = 5;
        }

        public Vector2 GetFrontPosition(bool invert)
        {
            float rotationRadians = MathHelper.ToRadians(Rotation);

			if (!IsMovinForward && invert)
			{
				rotationRadians = MathHelper.ToRadians(Rotation + 180);
			}

            float traslatedX = (float)(Math.Cos(rotationRadians) * (Width / 2) + Position.X - 5);
            float traslatedY = (float)(Math.Sin(rotationRadians) * (Height / 2) + Position.Y);

            Vector2 frontPosition = new Vector2(traslatedX, traslatedY);

            return frontPosition;
        }
        public Vector2 Move()
        {
			float rotation = MathHelper.ToRadians(Rotation);


			float direction = this.IsMovinForward ? 1 : -1; // Ajusta la dirección basada en el valor de isMovingForward
			float x = (float)(Position.X + direction * Math.Cos(rotation) * Velocity);
			float y = (float)(Position.Y + direction * Math.Sin(rotation) * Velocity);
			return new Vector2(x, y);
		}
        public void Accelerate()
        {
            if (Velocity < MaxVel)
            {
                Velocity += Acceleration * 0.35f;
            }
            if (Velocity > MaxVel)
            {
                Velocity = MaxVel;
            }
        }
		public void Decelerate(float multiplier = 0.5f)
		{
			if (Velocity > 0)
			{
				Velocity -= Acceleration * multiplier;
				if (Velocity < 0)
					Velocity = 0;
			}
			else if (Velocity < 0)
			{
				Velocity += Acceleration * multiplier;
				if (Velocity > 0)
					Velocity = 0;
			}
		}
		public virtual bool IsColliding()
		{
			Vector2 front = GetFrontPosition(true);
			Rectangle tankFrontRect = new Rectangle(
				(int)front.X,
				(int)front.Y,
                Width,
                Height
			);
			foreach (Sprite s in Match.Colliders)
			{
				if (tankFrontRect.Intersects(s.HitBox) && this != s ||
					!tankFrontRect.Intersects(Match.PlayingArea.HitBox))
				{
					IsMovinForward = !IsMovinForward;
					return true;
				}
			}
			return false;
		}
	}
}
