using MG_SplaTank.states;
using Microsoft.Xna.Framework;

namespace MG_SplaTank.business
{
    public class Game1 : Game
    {

        public enum Status { MENU, MATCH, EXIT };
        public Status statusEnum = Status.MENU;
        private IGameState currentStatus;
        public PlayersManagement playersManagement;
        public string player1Name = "";
        public string player2Name = "";
        public GraphicsDeviceManager graphics;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            currentStatus = new MainMenu(this);
            playersManagement = new PlayersManagement();
        }

        protected override void Update(GameTime gameTime)
        {
            if (statusEnum == Status.MENU && currentStatus is not MainMenu)
            {
                currentStatus = new MainMenu(this);
            }
            else if (statusEnum == Status.MATCH && currentStatus is not Match)
            {
                currentStatus = new Match(this);
                player1Name = "";
                player2Name = "";
            }
            currentStatus.Update(gameTime);
            base.Update(gameTime);
         }

        protected override void Draw(GameTime gameTime)
        {
            currentStatus.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
