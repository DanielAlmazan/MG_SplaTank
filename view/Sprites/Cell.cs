using MG_SplaTank.business;
using MG_SplaTank.states;
using Microsoft.Xna.Framework;

namespace MG_SplaTank.view.Sprites
{
    internal class Cell : Sprite
    {
        public Cell(Vector2 position) : base(position)
        {
            Texture = Assets.Cell0Texture;
            Width = Texture.Width;
            Height = Texture.Height;
            HitBox = UpdateRectangle();
        }
        public void SetColor(Player player)
        {
            if (player.IsPlayer1)
            {
                if (Texture != Assets.Cell1Texture)
                {
                    if (Player.OffsetCounter < 0)
                    {
                        Match.VSBarRectangle.Offset(1, 0);
                        Player.OffsetCounter = 4;
                    }
                    if (Texture == Assets.Cell2Texture)
                    {
                        if (player.Enemy.Points > 0) player.Enemy.Points--;
                    }
                    player.Points++;
                }
                Texture = Assets.Cell1Texture;
            }
            else
            {
                if (Texture != Assets.Cell2Texture)
                {
                    if (Player.OffsetCounter < 0)
                    {
                        Match.VSBarRectangle.Offset(-1, 0);
                        Player.OffsetCounter = 4;
                    }
                    if (Texture == Assets.Cell1Texture)
                    {
                        if (player.Enemy.Points > 0) player.Enemy.Points--;
                    }
                    player.Points++;
                }
                Texture = Assets.Cell2Texture;
            }
            Player.OffsetCounter--;
        }
    }
}

