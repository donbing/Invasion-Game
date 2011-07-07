using Invasion.SpriteManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Invasion.Screens
{
    public class GameScreen
    {
        public static MainMenuScreen MainMenuScreen = new MainMenuScreen();
        public static OptionsScreen OptionsScreen = new OptionsScreen();
        public static LevelScreen LevelScreen = new LevelScreen();
        public static GameplayScreen GameplayScreen = new GameplayScreen();
        public static GameOverScreen GameOverScreen = new GameOverScreen();
        public static CreditsScreen CreditsScreen = new CreditsScreen();
        public static HelpScreen HelpScreen = new HelpScreen();
        public static PauseScreen PauseScreen = new PauseScreen();

        public static void LoadContent(ContentManager content)
        {
            var starfield = content.Load<Texture2D>("Backgrounds/starfield_bg");
            var titleBackground = content.Load<Texture2D>("Backgrounds/title_bg");
            var selectedMenuItemSs = content.Load<Texture2D>("Sprites/select");

            PauseScreen.SetTexture(starfield);
            GameOverScreen.SetTexture(starfield);
            LevelScreen.SetTexture(starfield);
            GameplayScreen.SetTexture(starfield);
            MainMenuScreen.Load(content);
            MainMenuScreen.SetTexture(titleBackground, selectedMenuItemSs);
        }

        public static Vector2 GetCenterPos()
        {
            var viewport = InvasionGame.graphics.GraphicsDevice.Viewport;
            return new Vector2(viewport.Width, viewport.Height)/2;
        }
        
        public virtual void HandleBackButton(Game game)
        {
            Invasion.currentScreen = MainMenuScreen;
            UFOsManager.Instance.Reset();
            ExtrasManager.Instance.Reset();
            Invasion.StarShip.Reset();
        }
    }
}