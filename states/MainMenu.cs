using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;
using MG_SplaTank.business;

namespace MG_SplaTank.states
{
    public class MainMenu : IGameState
    {
        #region variables
        private const byte COOLDOWN = 7;
        private const int SCREEN_WIDTH = 1920;
        private const int SCREEN_HEIGHT = 1080;
        private int cooldown = COOLDOWN;
        private SpriteBatch spriteBatch;
        private SpriteFont font1;
        private SpriteFont font2;
        private Game1 game;
        private bool player1Ready;
        private bool player2Ready;
        private bool newGame;
        private bool showLeaderBoard;
        private Keys lastKey;
        private byte selectedOption = 0;
        private Dictionary<Keys, char> keyMap = new Dictionary<Keys, char>();
        #endregion

        public MainMenu(Game1 game)
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
            for (int i = 0; i < 10; i++)
            {
                keyMap.Add(
                    (Keys)Enum.Parse(typeof(Keys), "D" + i)
                    , char.Parse(i.ToString())
                );
            }
            for (char c = 'A'; c <= 'Z'; c++)
            {
                keyMap.Add(
                    (Keys)Enum.Parse(typeof(Keys),
                    c.ToString()), c
                );
            }
            keyMap.Add(Keys.Space, ' ');
            keyMap.Add(Keys.OemMinus, '_');
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            font1 = Assets.Font1Texture;
            font2 = Assets.Font2Texture;
        }

        public void Update(GameTime gameTime)
        {
            cooldown--;

            KeyboardState keyboardState = Keyboard.GetState();
            Keys[] pressedKeys = keyboardState.GetPressedKeys();

            if (pressedKeys.Length > 0 && cooldown < 0)
            {
                Keys currentKey = pressedKeys[0];
                if (currentKey != lastKey) { ProcessKey(currentKey); }

                cooldown = COOLDOWN;
            }
            else if (keyboardState.IsKeyUp(lastKey)) { lastKey = Keys.None; }
        }

        private void ProcessKey(Keys currentKey)
        {
            if (keyMap.ContainsKey(currentKey) && newGame)
            {
                NameBuilder(currentKey);
            }
            else
            {
                switch (currentKey)
                {
                    case Keys.Escape:
                        if (newGame || showLeaderBoard) GoBack();
                        else game.Exit();
                        break;
                    case Keys.Down:
                    case Keys.Up:
                        if (!newGame && !showLeaderBoard)
                        {
                            ChangeSelectedOption(currentKey == Keys.Down);
                        }
                        break;
                    case Keys.Enter: ProcessEnterKey(); break;
                    case Keys.Back:
                        GoBack();
                        if (newGame)
                        {
                            if (!player1Ready) { game.player1Name = ""; }
                            else if (!player2Ready) { game.player2Name = ""; }
                        }
                        break;
                }
            }
            lastKey = currentKey;
        }

        private void ChangeSelectedOption(bool isDownKey)
        {
            selectedOption = (byte)(isDownKey
                ? selectedOption < 2 ? ++selectedOption : 0
                : selectedOption > 0 ? --selectedOption : 2);
        }

        private void ProcessEnterKey()
        {
            if (selectedOption == 0)
            {
                newGame = true;
                NamesSetter();
            }
            else if (selectedOption == 1) { showLeaderBoard = true; }
            else { game.Exit(); }
        }

        public void GoBack()
        {
            if (game.player1Name == "" && (newGame || showLeaderBoard))
            {
                newGame = false;
                showLeaderBoard = false;
            }
            else if (game.player2Name == "")
            {
                game.player2Name = "";
                player1Ready = false;
            }
        }

        public void NameBuilder(Keys currentKey)
        {
            const int MAX_LENGTH = 10;
            char letter = keyMap[currentKey];
            if (!player1Ready && game.player1Name.Length < MAX_LENGTH)
            {
                game.player1Name += letter;
            }
            else if (player1Ready && !player2Ready && game.player2Name.Length < MAX_LENGTH)
            {
                game.player2Name += letter;
            }
            lastKey = currentKey;
        }
        public void NamesSetter()
        {
            if (!player1Ready)
            {
                player1Ready = game.player1Name.Length > 0;
            }
            else if (!player2Ready)
            {
                player2Ready = game.player2Name.Length > 0 &&
                    game.player2Name != game.player1Name;
            }
            else game.statusEnum = Game1.Status.MATCH;
        }

        public void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            DrawBackground(Assets.MainMenuTexture);
            DrawHUB();

            spriteBatch.End();

            // base.Draw(gameTime);
        }
        public void DrawBackground(Texture2D texture)
        {
            Vector2 origin = new Vector2(0, 0);
            spriteBatch.Draw
            (
                texture,
                new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT),
                null,
                Color.White,
                0,
                origin,
                SpriteEffects.None,
                0f
            );
        }

        public void DrawHUB()
        {
            if (!newGame && !showLeaderBoard)
            {
                DrawMenu();
            }
            else if (newGame)
            {
                DrawBackErase();
                DrawNamesRequest();
            }
            else
            {
                DrawBackErase();
                DrawLeaderBoard();
            }
        }
        public void DrawMenu()
        {
            Texture2D menuTexture = Assets.MenuOptionsTexture;
            Vector2 menuOptPos = new Vector2(
                -SCREEN_WIDTH / 2 + menuTexture.Width / 2,
                -SCREEN_HEIGHT / 2 + 120);
            spriteBatch.Draw(
                menuTexture,
                menuTexture.Bounds,
                null,
                Color.White,
                0,
                menuOptPos,
                SpriteEffects.None,
                0
            );
            DrawArrow(menuOptPos);
        }

        public void DrawArrow(Vector2 menuBoard)
        {
            float x = menuBoard.X + 100;
            Texture2D[] textures =
            {
                Assets.SelectionArrow0Texture,
                Assets.SelectionArrow1Texture,
                Assets.SelectionArrow2Texture
            };

            // Calcula la posición del origen en función de la opción seleccionada
            Vector2 arrowOrigin = new Vector2(x, menuBoard.Y - 130 * selectedOption);

            // Dibuja la textura correspondiente
            spriteBatch.Draw(
                textures[selectedOption],
                textures[selectedOption].Bounds,
                null,
                Color.White,
                0,
                arrowOrigin,
                SpriteEffects.None,
                0
            );
        }

        public void DrawNamesRequest()
        {
            float halfScreenWidth = SCREEN_WIDTH / 2;
            float halfScreenHeight = SCREEN_HEIGHT / 2;
            string text2 = "";
            string text1;
            if (!player1Ready)
            {
                text1 = "PLAYER 1 NAME";
                text2 = game.player1Name;
            }
            else if (!player2Ready)
            {
                text1 = "PLAYER 2 NAME";
                text2 = game.player2Name;
            }
            else
            {
                text1 = "PRESS [ENTER] TO START";
                DrawBackground(Assets.TutorialTexture);
            }

            DrawStringCentered(font2, text1, halfScreenWidth, halfScreenHeight - 140);
            DrawStringCentered(font2, text2, halfScreenWidth, halfScreenHeight - 40);
        }

        public void DrawStringCentered(SpriteFont font, string text, float x, float y)
        {
            Vector2 position = new Vector2(x, y);
            Vector2 textMiddlePoint = font.MeasureString(text) / 2;
            spriteBatch.DrawString(
                font,
                text,
                position,
                Color.White,
                0,
                textMiddlePoint,
                1.0f,
                SpriteEffects.None,
                0.5f
            );
        }

        public void DrawLeaderBoard()
        {
            Texture2D boardTexture = Assets.LeaderBoardTexture;
            float halfScreenWidth = SCREEN_WIDTH / 2;
            float halfScreenHeight = SCREEN_HEIGHT / 2;

            Vector2 boardPosition = new Vector2(
                -halfScreenWidth + boardTexture.Width / 2,
                -halfScreenHeight + 190
            );

            spriteBatch.Draw(
                boardTexture,
                boardTexture.Bounds,
                null,
                Color.White,
                0,
                boardPosition,
                SpriteEffects.None,
                0
            );

            string[] players = game.playersManagement.GetLeaderBoard();
            float y = -boardPosition.Y + 70;
            Vector2 origin = new Vector2(0, 0);

            DrawText(font1, players[0], new Vector2(-boardPosition.X + 62, y), origin);
            DrawText(font1, players[1], new Vector2(-boardPosition.X + 110, y), origin);
            DrawText(font1, players[2], new Vector2(-boardPosition.X + 290, y), origin);
            DrawText(font1, players[3], new Vector2(-boardPosition.X + 425, y), origin);
        }

        private void DrawText(SpriteFont font, string text, Vector2 position, Vector2 origin)
        {
            spriteBatch.DrawString(font, text, position, Color.White, 0, origin, 1.0f, SpriteEffects.None, 0.5f);
        }
        public void DrawBackErase()
        {
            Texture2D texture = Assets.BackEraseTexture;
            spriteBatch.Draw(
                texture,
                texture.Bounds,
                null,
                Color.White,
                0,
                new Vector2(-SCREEN_WIDTH + texture.Width, -SCREEN_HEIGHT + texture.Height),
                SpriteEffects.None,
                0
            );
        }
    }
}
