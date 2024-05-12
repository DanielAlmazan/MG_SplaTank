using Microsoft.Xna.Framework;
using MG_SplaTank.business;
using MG_SplaTank.states;

namespace MG_SplaTank.view.Sprites
{
    public class Bullet : MovingSprite
    {
        public const int MAX_DISTANCE = 200;
        public Vector2 OriginPosition;
        public Player player;
        public Bullet(Player player) : base(player.Tank.GetFrontPosition(false))
        {
            this.player = player;
			Velocity = this.player.Tank.Velocity + 4;

			this.Rotation = this.player.Tank.Rotation;
            this.OriginPosition = Position;
            this.Texture = this.player.IsPlayer1 ? Assets.Bullet1Texture : Assets.Bullet2Texture;
            this.Width = Texture.Width;
            this.Height = Texture.Height;
        }
        public override bool IsColliding()
        {
			foreach (Sprite s in Match.Colliders)
            {
                if (s is Block && (HitBox.Intersects(s.HitBox) ||
                    !HitBox.Intersects(Match.PlayingArea.HitBox)))
                {
					return true;
                }
                if (HitBox.Intersects(player.Enemy.Tank.Rectangle))
                {
                    if (!player.Enemy.Tank.Invincible) { player.Enemy.Tank.Health -= 10; }
                    return true;
				}
            }
            return false;
        }
    }
}
