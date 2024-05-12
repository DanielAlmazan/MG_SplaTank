using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG_SplaTank.view.Sprites
{
    public abstract class Sprite
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Rectangle Rectangle;
        public Rectangle HitBox;
        public Sprite() { }
        public Sprite(Vector2 position)
        {
            Position = position;
            HitBox = new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    Width,
                    Height
            );
        }
        
        public virtual Rectangle UpdateRectangle()
        {
            int newWidth = (int)(Width * 0.09);
            int newHeight = (int)(Height * 0.8);
            HitBox.Width = newWidth;
            HitBox.Height = newHeight;
            HitBox.X = (int)(Position.X + 36);
            HitBox.Y = (int)(Position.Y - newHeight / 5);

            Rectangle.Width = Width;
            Rectangle.Height = Height;
            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;

            return Rectangle;
        }
    }
}
