using System;
using ImproviSoft.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Invasion.Screens
{
    public class GameOverScreen : GameScreen, IGameScreen
    {
        private Texture2D backgroundTex;

        public void SetEnabledGestures()
        {
            TouchPanel.EnabledGestures = GestureType.None;
        }

        public void Draw(GameTime gameTime)
        {
            Vector2 pos1 = Vector2.Zero;
            InvasionGame.spriteBatch.Draw(
                backgroundTex,
                new Rectangle(backgroundTex.Bounds.X, backgroundTex.Bounds.Y, backgroundTex.Bounds.Width,
                              backgroundTex.Bounds.Height),
                Color.White
                );

            float sectorCenter = InvasionGame.SectorPos.X + InvasionGame.SectorWidth / 2;

            pos1.X = sectorCenter;
            pos1.Y = 200;

            TextHandler.DrawTextCentered("GAME OVER", pos1, 2.0f, Color.White);

            InvasionGame.Scoreboard.Draw(gameTime);

            if (((Invasion.TickLast - Invasion.DelayStart) / Menus.delayBlink) % 3 != 1)
            {
                pos1.Y = 420;
                TextHandler.DrawTextCentered("PRESS THE BACK BUTTON", pos1);
                pos1.Y += 30;
                TextHandler.DrawTextCentered("TO RETURN TO THE MAIN MENU", pos1);
            }
        }

        public void HandleInput(GameTime gameTime, InputState input, Game game)
        {

        }

        public void SetTexture(Texture2D texture2D)
        {
            backgroundTex = texture2D;
        }
    }
}