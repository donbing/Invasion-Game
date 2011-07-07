using System;
using ImproviSoft.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Invasion.Screens
{
    public class Menus{
        public const int ItemDeltaY = 50;
        public const int FirstOptionY = 110;
        public const int delayBlink = 800;
        public static Color versionDimColor = new Color(80, 80, 80);
        public static readonly Vector2 notTouched = new Vector2(-1, -1);
        public static Rectangle MenuItemBoundingBox = new Rectangle(0, 0, 800, 10 + 15 + 15);
    }

    public class PauseScreen : GameScreen, IGameScreen
    {
        public static int selectedEntry = -1;
        private Texture2D BGTexture;

        public void SetEnabledGestures()
        {
            TouchPanel.EnabledGestures = GestureType.Tap;
        }

        public void Draw(GameTime gameTime)
        {
            var menuoption = selectedEntry;
            var centerX = GetCenterPos().X;
            Vector2 pos = Vector2.Zero;
            pos = new Vector2(centerX, 45);
            InvasionGame.spriteBatch.Draw(BGTexture, Vector2.Zero, Color.White);

            TextHandler.DrawTextCentered("GAME IS PAUSED", pos, 2.0f, Color.White);
            pos.Y = Menus.FirstOptionY;

            Color rowColor1 = (menuoption == 0) ? Color.White : Color.DarkGray;
            TextHandler.DrawTextCentered("RESUME GAME", pos, 1.5f, rowColor1);
            pos.Y += OptionsScreen.OptionsMenuItemDeltaY;
            
            rowColor1 = (menuoption == 1) ? Color.White : Color.DarkGray;
            TextHandler.DrawTextCentered("MAIN MENU", pos, 1.5f, rowColor1);
            pos.Y += OptionsScreen.OptionsMenuItemDeltaY;
            
            rowColor1 = (menuoption == 2) ? Color.White : Color.DarkGray;
            TextHandler.DrawTextCentered("QUIT", pos, 1.5f, rowColor1);
            pos.Y += OptionsScreen.OptionsMenuItemDeltaY;
        }

        public override void HandleBackButton(Game game)
        {
            Invasion.currentScreen = GameplayScreen;
        }

        public void SetTexture(Texture2D texture2D)
        {
            BGTexture = texture2D;
        }

        public void HandleInput(GameTime gameTime, InputState input, Game game)
        {
            Vector2 releasePt;
            Vector2 touchPt = input.TouchState.GetTouchPt(out releasePt);

            selectedEntry = -1;

            if (touchPt != Menus.notTouched) {

                // Resume Game
                Menus.MenuItemBoundingBox.Y = Menus.FirstOptionY - 10;

                if (Menus.MenuItemBoundingBox.Intersects(touchPt))
                    selectedEntry = 0;

                // Main Menu
                Menus.MenuItemBoundingBox.Y = Menus.FirstOptionY + Menus.ItemDeltaY - 10;

                if (Menus.MenuItemBoundingBox.Intersects(touchPt))
                    selectedEntry = 1;

                // Quit
                Menus.MenuItemBoundingBox.Y = Menus.FirstOptionY + Menus.ItemDeltaY*2 - 10;
                if (Menus.MenuItemBoundingBox.Intersects(touchPt))
                    selectedEntry = 2;

                return;
            }

            if (releasePt == Menus.notTouched)
                return;

            // Resume Game
            Menus.MenuItemBoundingBox.Y = Menus.FirstOptionY - 10;
            if (Menus.MenuItemBoundingBox.Intersects(touchPt))
            {
                Invasion.currentScreen = GameplayScreen;
                return;
            }

            // Main Menu
            Menus.MenuItemBoundingBox.Y = Menus.FirstOptionY + Menus.ItemDeltaY - 10;
            if (Menus.MenuItemBoundingBox.Intersects(touchPt))
            {
                Invasion.currentScreen = MainMenuScreen;
                return;
            }

            // Quit
            Menus.MenuItemBoundingBox.Y = Menus.FirstOptionY + Menus.ItemDeltaY*2 - 10;
            if (Menus.MenuItemBoundingBox.Intersects(touchPt))
            {
                game.Exit();
                return;
            }
        }
    }
}