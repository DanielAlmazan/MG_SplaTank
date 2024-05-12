using MG_SplaTank.business;
using MG_SplaTank.view.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MG_SplaTank.states
{
    internal class Match : IGameState
    {
        public const int SCREEN_WIDTH = 1920;
        public const int SCREEN_HEIGHT = 1080;
		public static Rectangle VSBarRectangle;
        public static Block PlayingArea;
		public static Cell[,] CellsGrid;
		public static List<Sprite> Colliders;
        private static Block[] blocks;
        private Player p1;
        private Player p2;
        private Game1 game;
        private SpriteFont font;
        private SpriteBatch spriteBatch;
        private TimeSpan timeRemaining;

        public Match(Game1 game)
        {
            this.game = game;
            game.Content.RootDirectory = "Content";
            game.IsMouseVisible = true;
            game.graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            game.graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            game.graphics.ApplyChanges();
            Initialize();
            LoadContent();
        }

        public void Initialize()
        {
            Assets.Init(game.Content);
            Colliders = new List<Sprite>();

            Player[] players = game.playersManagement.InitializePlayers(game.player1Name, game.player2Name);
            p1 = players[0];
            p2 = players[1];
            p1.Enemy = p2;
            p2.Enemy = p1;
            blocks = new Block[]
            {
                new Block(new Vector2(473, 460)),
                new Block(new Vector2(986, 242)),
                new Block(new Vector2(932, 919)),
                new Block(new Vector2(1445, 810))
            };
            PlayingArea = new Block(new Vector2(0, 0));
            PlayingArea.HitBox = new Rectangle
                (275, 260, SCREEN_WIDTH - 465, SCREEN_HEIGHT - 380);
            CellsGrid = new Cell[57, 29];
            InitCells();
            InitColliders();
            timeRemaining = TimeSpan.FromMinutes(1.5);
            VSBarRectangle = Assets.VSBarTexture.Bounds;
            VSBarRectangle.Offset(-Assets.VSBarTexture.Width / 4, 0);
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            font = game.Content.Load<SpriteFont>("font");
        }

        public void Update(GameTime gameTime)
        {
            if (p1.Tank.Health <= 0)
            {
                p2.Points += p1.Points / 2;
                p1.Points /= 2;
                p1.Tank.Restart();
            }
            if (p2.Tank.Health <= 0)
            {
                p1.Points += p2.Points / 2;
                p2.Points /= 2;
                p2.Tank.Restart();
            }
            timeRemaining -= gameTime.ElapsedGameTime;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) || timeRemaining < TimeSpan.Zero)
            {
                UpdatePlayersJson();
                game.statusEnum = Game1.Status.MENU;
            }
            p1.MoveTank();
            p2.MoveTank();
            p1.MoveBullets();
            p2.MoveBullets();
            p1.Tank.Invincible = p1.Tank.InvincibilityOn();
			p2.Tank.Invincible = p2.Tank.InvincibilityOn();
        }
        private void UpdatePlayersJson()
        {
            if (p1.Points > p1.Record)
            {
                game.playersManagement.playersDic[p1.Name].Record = p1.Points;
            }
            if (p2.Points > p2.Record)
            {
                game.playersManagement.playersDic[p2.Name].Record = p2.Points;
            }

            if (p1.Points > p2.Points)
            {
                game.playersManagement.playersDic[p1.Name].MatchesWon++;
                game.playersManagement.playersDic[p2.Name].MatchesLost++;
            }
            else if (p1.Points < p2.Points)
            {
                game.playersManagement.playersDic[p2.Name].MatchesWon++;
                game.playersManagement.playersDic[p1.Name].MatchesLost++;
            }
            game.playersManagement.SavePlayersToJson();
        }

        public void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            DrawBackground();
            DrawHUB();
            DrawCellsGrid();
            DrawBlocks();
            DrawTank(p1);
            DrawTank(p2);
            DrawBullets(p1, Assets.Bullet1Texture);
            DrawBullets(p2, Assets.Bullet2Texture);            

            spriteBatch.End();
        }
        #region MyMethods
        public void DrawHUB()
        {
            spriteBatch.Draw(
                Assets.VSBarTexture,
                VSBarRectangle,
                Color.White
            );

            // Player 1 Info
            string p1Info = $"{p1.Name} | Points {p1.Points} | Record {p1.Record}";
            Vector2 position1 = new Vector2(10, 10);

            spriteBatch.DrawString(
                font,
                p1Info,
                position1,
                Color.White,
                0,
                new Vector2(0, 0),
                1.0f,
                SpriteEffects.None,
                0.5f
            );

            // Player 2 Info
            string p2Info = $"{p2.Name} | Points {p2.Points} | Record {p2.Record}";
            Vector2 p2InfoMeasure = new Vector2(0, 0);
            Vector2 position2 = new Vector2(SCREEN_WIDTH - 10 - font.MeasureString(p2Info).X, 10);

            spriteBatch.DrawString(
                font,
                p2Info,
                position2,
                Color.White,
                0,
                p2InfoMeasure,
                1.0f,
                SpriteEffects.None,
                0.5f
            );
            string timerString = string.Format("{0:D1}:{1:D2}", timeRemaining.Minutes, timeRemaining.Seconds);
            spriteBatch.DrawString(font, timerString, new Vector2(SCREEN_WIDTH / 2, 10), Color.White);
        }
        public void InitCells()
        {
            for (int i = 0; i < CellsGrid.GetLength(0); i++)
            {
                for (int j = 0; j < CellsGrid.GetLength(1); j++)
                {
                    CellsGrid[i, j] = new Cell(new Vector2(207 + i * 27, 203 + j * 27));
                }
            }
        }
        public void DrawBackground()
        {
            Vector2 origin = new Vector2(0, 0);
            spriteBatch.Draw
            (
                Assets.BackgroundTexture,
                new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT),
                null,
                Color.White,
                0,
                origin,
                SpriteEffects.None,
                0f
            );

        }
        public void DrawTank(Player p)
        {
            Color color = p.Tank.Invincible ? Color.BurlyWood : Color.White;
            Debug.WriteLine(p.Tank.Invincible);
            Texture2D texture = p.Tank.Texture;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            spriteBatch.Draw
            (
                texture,
                p.Tank.UpdateRectangle(),
                null,
				color,
                MathHelper.ToRadians(p.Tank.Rotation),
                origin,
                SpriteEffects.None,
                0f
            );
        }

        public void DrawCellsGrid()
        {
            foreach (Cell cell in CellsGrid)
            {
                Vector2 origin = new Vector2(cell.Texture.Width / 2, cell.Texture.Height / 2);
                spriteBatch.Draw
                (
                    cell.Texture,
                    cell.Rectangle,
                    null,
                    Color.White,
                    0,
                    origin,
                    SpriteEffects.None,
                    0.5f
                );
            }
        }

        public void DrawBlocks()
        {
            Texture2D texture = Assets.BlockTexture;
            foreach (Block b in blocks)
            {
                Vector2 origin = new Vector2(b.Texture.Width / 2, b.Texture.Height / 2);
                spriteBatch.Draw
                (
                    texture,
                    b.Rectangle,
                    null,
                    Color.White,
                    0,
                    origin,
                    SpriteEffects.None,
                    0.5f
                );
            }
        }

        public void DrawBullets(Player p, Texture2D texture)
        {
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            foreach (Bullet b in p.Bullets)
            {
                spriteBatch.Draw
                (
                    texture,
                    b.UpdateRectangle(),
                    null,
                    Color.White,
                    MathHelper.ToRadians(b.Rotation),
                    origin,
                    SpriteEffects.None,
                    0f
                );
            }
        }
        public void InitColliders()
        {
            foreach (Block b in blocks)
            {
                Colliders.Add(b);
            }
            Colliders.Add(p1.Tank);
            Colliders.Add(p2.Tank);
        }
        #endregion
    }
}
