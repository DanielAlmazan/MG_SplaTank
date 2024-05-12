using Microsoft.Xna.Framework;

namespace MG_SplaTank.view.Sprites
{
    public class Block : Sprite
    {
        public Block(Vector2 position) : base(position)
        {
            Position = position;
            Texture = Assets.BlockTexture;
            Width = Texture.Width;
            Height = Texture.Height;
            Rectangle = UpdateRectangle();
        }
    }
}
