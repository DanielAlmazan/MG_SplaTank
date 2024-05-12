using Microsoft.Xna.Framework;

namespace MG_SplaTank.states
{
    internal interface IGameState
    {
        public void Initialize();
        public void LoadContent();
        public void Update(GameTime gameTime);
        public void Draw(GameTime gameTime);
    }
}
