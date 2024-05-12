using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MG_SplaTank
{
	public class Assets
	{
		public static Texture2D BackgroundTexture;
		public static Texture2D MainMenuTexture;
		public static Texture2D SelectionArrow0Texture;
		public static Texture2D SelectionArrow1Texture;
		public static Texture2D SelectionArrow2Texture;
		public static Texture2D MenuOptionsTexture;
		public static Texture2D TutorialTexture;
		public static Texture2D BackEraseTexture;
		public static Texture2D LeaderBoardTexture;
		public static Texture2D Tank1Texture;
		public static Texture2D Tank2Texture;
		public static Texture2D BlockTexture;
		public static Texture2D Bullet1Texture;
		public static Texture2D Bullet2Texture;
		public static Texture2D Cell0Texture;
		public static Texture2D Cell1Texture;
		public static Texture2D Cell2Texture;
		public static Texture2D VSBarTexture;
		public static SpriteFont Font1Texture;
		public static SpriteFont Font2Texture;
		public static void Init(ContentManager Content)
		{
			BackgroundTexture = Content.Load<Texture2D>("background");
			MainMenuTexture = Content.Load<Texture2D>("MainScreen");
			MenuOptionsTexture = Content.Load<Texture2D>("menuOptions");
			BackEraseTexture = Content.Load<Texture2D>("backErase");
			SelectionArrow0Texture = Content.Load<Texture2D>("selectionArrow0");
			SelectionArrow1Texture = Content.Load<Texture2D>("selectionArrow1");
			SelectionArrow2Texture = Content.Load<Texture2D>("selectionArrow2");
			LeaderBoardTexture = Content.Load<Texture2D>("leaderBoard");
			TutorialTexture = Content.Load<Texture2D>("tutorial");
			VSBarTexture = Content.Load<Texture2D>("VSBar");
			Tank1Texture = Content.Load<Texture2D>("tank1");
			Tank2Texture = Content.Load<Texture2D>("tank2");
			BlockTexture = Content.Load<Texture2D>("block");
			Bullet1Texture = Content.Load<Texture2D>("bullet1");
			Bullet2Texture = Content.Load<Texture2D>("bullet2");
			Cell0Texture = Content.Load<Texture2D>("cell0");
			Cell1Texture = Content.Load<Texture2D>("cell1");
			Cell2Texture = Content.Load<Texture2D>("cell2");
			Font1Texture = Content.Load<SpriteFont>("font");
			Font2Texture = Content.Load<SpriteFont>("font2");
		}
	}
}
