using System;
using System.Diagnostics;
using ImproviSoft.System;
using Invasion.Sprites;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Invasion.Screens
{
    public class OptionsScreen : GameScreen, IGameScreen
    {
        private static int selectedEntry = -1;
        private const int OptionsTitleY = 40;

        private static float _timeSinceButtonPressSec = 60.0f;
        public const float OptionsButtonDebounceSec = 0.5f;
        private const int Options2NdSubmenuY = Menus.FirstOptionY + 9 * OptionsMenuItemDeltaY / 2;
        public const int OptionsMenuItemDeltaY = 45;

        public void SetEnabledGestures()
        {
            TouchPanel.EnabledGestures = GestureType.Tap;
        }

        public void Draw(GameTime gameTime)
        {
            var pos3 = new Vector2(GetCenterPos().X, OptionsTitleY);

            string scoreboardPos = InvasionGame.Scoreboard.DisplaySide.ToString().ToUpper();

            string gameMusicEnabled = (InvasionGame.musicManager.Enabled) ? "ON" : "OFF";

            string difficultyText = InvasionGame.Scoreboard.DifficultyLevel.ToString().ToUpper();

            string autoSelectWeaponText = (StarShip.AutoSelectWeapon) ? "YES" : "NO";

            InvasionGame.graphics.GraphicsDevice.Clear(Color.Black);

            TextHandler.DrawTextCentered("OPTIONS", pos3, 2.0f, Color.White);

            Color dimRowColor = Color.Gray;
            Color rowColor;
            pos3.Y = Menus.FirstOptionY;

            int iEntry = -1;

            if (!Invasion.GameInProgress)
            {
                rowColor = (selectedEntry == ++iEntry) ? Color.White : dimRowColor;
                TextHandler.DrawTextCentered("SCOREBOARD POSITION: " + scoreboardPos, pos3, 1.5f, rowColor);
                pos3.Y += OptionsMenuItemDeltaY;
            }

            rowColor = (selectedEntry == ++iEntry) ? Color.White : dimRowColor;
            TextHandler.DrawTextCentered("GAME MUSIC: " + gameMusicEnabled, pos3, 1.5f, rowColor);
            pos3.Y += OptionsMenuItemDeltaY;

            rowColor = (selectedEntry == ++iEntry) ? Color.White : dimRowColor;
            TextHandler.DrawTextCentered("DIFFICULTY LEVEL: " + difficultyText, pos3, 1.5f, rowColor);
            pos3.Y += OptionsMenuItemDeltaY;

            rowColor = (selectedEntry == ++iEntry) ? Color.White : dimRowColor;
            TextHandler.DrawTextCentered("AUTO-SELECT WEAPON: " + autoSelectWeaponText, pos3, 1.5f, rowColor);
            pos3.Y = Options2NdSubmenuY;

            rowColor = (selectedEntry == ++iEntry) ? Color.White : dimRowColor;
            TextHandler.DrawTextCentered("RATE AND REVIEW INVASION", pos3, 1.5f, rowColor);
            pos3.Y += OptionsMenuItemDeltaY;

            rowColor = (selectedEntry == ++iEntry) ? Color.White : dimRowColor;
            TextHandler.DrawTextCentered("FIND OUR OTHER GAMES!", pos3, 1.5f, rowColor);
            pos3.Y += OptionsMenuItemDeltaY;

            pos3.Y = 420;
            TextHandler.DrawTextCentered("PRESS THE BACK BUTTON", pos3, 1.0f, Color.White);
            pos3.Y += 30;
            
            TextHandler.DrawTextCentered(Invasion.GameInProgress ? "TO RESUME YOUR GAME" : "TO RETURN TO THE MAIN MENU", pos3);
        }

        public override void HandleBackButton(Game game)
        {
            if (Invasion.GameInProgress)
            {
                Invasion.currentScreen = GameplayScreen;
            }
            else
            {
                Invasion.currentScreen = MainMenuScreen;
            }
        }

        public void HandleInput(GameTime gameTime, InputState intouches, Game game)
        {
            Vector2 releasePt;
            Vector2 touchPt = intouches.TouchState.GetTouchPt(out releasePt);
            _timeSinceButtonPressSec += (float)gameTime.ElapsedGameTime.TotalSeconds;

            selectedEntry = -1;
            int iEntry = -1;

            if (touchPt != Menus.notTouched) {

                if (!Invasion.GameInProgress) {
                    // Scoreboard Entry
                    Menus.MenuItemBoundingBox.Y = Menus.FirstOptionY + OptionsMenuItemDeltaY*(++iEntry) - 10;

                    if (RectangleExtensions.Intersects((Rectangle) Menus.MenuItemBoundingBox, touchPt))
                        selectedEntry = iEntry;
                }

                // Game Music
                Menus.MenuItemBoundingBox.Y = Menus.FirstOptionY + OptionsMenuItemDeltaY*(++iEntry) - 10;

                if (RectangleExtensions.Intersects((Rectangle) Menus.MenuItemBoundingBox, touchPt))
                    selectedEntry = iEntry;

                // Difficulty Level
                Menus.MenuItemBoundingBox.Y = Menus.FirstOptionY + OptionsMenuItemDeltaY*(++iEntry) - 10;

                if (RectangleExtensions.Intersects((Rectangle) Menus.MenuItemBoundingBox, touchPt))
                    selectedEntry = iEntry;

                // Auto-Select Weapon
                Menus.MenuItemBoundingBox.Y = Menus.FirstOptionY + OptionsMenuItemDeltaY*(++iEntry) - 10;

                if (RectangleExtensions.Intersects((Rectangle) Menus.MenuItemBoundingBox, touchPt))
                    selectedEntry = iEntry;


                // Rate & Review
                Menus.MenuItemBoundingBox.Y = Options2NdSubmenuY - 10;
                ++iEntry;

                if (RectangleExtensions.Intersects((Rectangle) Menus.MenuItemBoundingBox, touchPt))
                    selectedEntry = iEntry;

                // Find our other Games
                Menus.MenuItemBoundingBox.Y = Options2NdSubmenuY + OptionsMenuItemDeltaY - 10;
                ++iEntry;

                if (RectangleExtensions.Intersects((Rectangle) Menus.MenuItemBoundingBox, touchPt))
                    selectedEntry = iEntry;

                return;
            }

            if (releasePt == Menus.notTouched || _timeSinceButtonPressSec <= OptionsButtonDebounceSec)
                return;

            _timeSinceButtonPressSec = 0.0f;

            iEntry = -1;

            // Scoreboard Position
            if (!Invasion.GameInProgress) {
                Menus.MenuItemBoundingBox.Y = Menus.FirstOptionY + OptionsMenuItemDeltaY*(++iEntry) - 10;

                if (RectangleExtensions.Intersects((Rectangle) Menus.MenuItemBoundingBox, touchPt))
                {
                    InvasionGame.Scoreboard.DisplaySide = (InvasionGame.Scoreboard.DisplaySide == DisplaySide.Left)?
                                                                                                                       DisplaySide.Right : DisplaySide.Left;
                    return;
                }
            }

            // Game Music
            Menus.MenuItemBoundingBox.Y = Menus.FirstOptionY + OptionsMenuItemDeltaY*(++iEntry) - 10;

            if (RectangleExtensions.Intersects((Rectangle) Menus.MenuItemBoundingBox, touchPt))
            {
                InvasionGame.musicManager.Enabled = !InvasionGame.musicManager.Enabled;

                var musicManager = (MusicManager)game.Services.GetService(typeof(MusicManager));

                if (InvasionGame.musicManager.Enabled)
                    musicManager.Play(InvasionGame.gameMusic, true);

                else if (musicManager.IsGameMusicPlaying)
                    musicManager.Stop();

                return;
            }

            // Difficulty Level
            Menus.MenuItemBoundingBox.Y = Menus.FirstOptionY + OptionsMenuItemDeltaY*(++iEntry) - 10;

            if (RectangleExtensions.Intersects((Rectangle) Menus.MenuItemBoundingBox, touchPt))
            {
                InvasionGame.Scoreboard.DifficultyLevel = (DifficultyLevel)(((int)InvasionGame.Scoreboard.DifficultyLevel+1)%3);
                return;
            }

            // Auto-Select Weapon
            Menus.MenuItemBoundingBox.Y = Menus.FirstOptionY + OptionsMenuItemDeltaY*(++iEntry) - 10;

            if (Menus.MenuItemBoundingBox.Intersects(touchPt))
            {
                StarShip.AutoSelectWeapon = !StarShip.AutoSelectWeapon;
                return;
            }

            // Rate & Review
            Menus.MenuItemBoundingBox.Y = Options2NdSubmenuY - 10;

            if (Menus.MenuItemBoundingBox.Intersects(touchPt))
            {
                try {
                    var mr = new MarketplaceReviewTask();
                    mr.Show();
                }
                catch (Exception ex) {
                    Debug.WriteLine("Exception while attempting to Show MarketplaceReviewTask:\n" + ex.Message);
                }

                return;
            }

            // Find our other Games
            Menus.MenuItemBoundingBox.Y = Options2NdSubmenuY + OptionsMenuItemDeltaY - 10;

            if (Menus.MenuItemBoundingBox.Intersects(touchPt))
            {
                try {
                    var task = new MarketplaceSearchTask
                                   {
                                       ContentType = MarketplaceContentType.Applications,
                                       SearchTerms = "ImproviSoft"
                                   };
                    task.Show();
                }
                catch (Exception ex) {
                    Debug.WriteLine("Exception while attempting to Show MarketplaceSearchTask:\n" + ex.Message);
                }

                return;
            }
        }
    }
}