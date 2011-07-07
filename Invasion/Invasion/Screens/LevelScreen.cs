using System;
using ImproviSoft.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Invasion.Screens
{
    public class LevelScreen : GameScreen, IGameScreen
    {
        private Texture2D back;
        private const int delayLevel = 3000;

        public void SetEnabledGestures()
        {
            TouchPanel.EnabledGestures = GestureType.None;
        }

        public void Draw(GameTime gameTime)
        {
            InvasionGame.spriteBatch.Draw(back, Vector2.Zero, Color.White);

            var screenWidth = InvasionGame.graphics.GraphicsDevice.Viewport.Width;

            var level = Invasion.Level.ToString().PadLeft(3, '0');

            TextHandler.DrawTextCentered("LEVEL " + level, new Vector2(screenWidth/2, 200.0f), 2.0f, Color.White);

            if (Invasion.DelayStart + delayLevel < Invasion.TickLast)
                Invasion.currentScreen = GameplayScreen;
        }

        public void HandleInput(GameTime gameTime, InputState input, Game game)
        {

        }

        public void SetTexture(Texture2D starfield)
        {
            back = starfield;
        }
    }
}