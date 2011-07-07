using System;
using System.Diagnostics;
using ImproviSoft.System;
using Invasion.SpriteManagers;
using Invasion.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Invasion.Screens
{
    public class MainMenuScreen : GameScreen, IGameScreen
    {
        int iSelectFrame = 0;
        private Texture2D back;
        private Texture2D selectedMenuItemSs;
        private SoundEffect menuSoundEffect;

        public void SetEnabledGestures()
        {
            TouchPanel.EnabledGestures = GestureType.Tap;
        }
        public void Draw(GameTime gameTime)
        {
            Vector2 bgpos = Vector2.Zero;
            const int itemHeight = 40;

            InvasionGame.spriteBatch.Draw(back, bgpos, Color.White);
            var menupos = new Vector2(130,260);

            TextHandler.DrawText("START GAME", menupos, 2.0f, Color.White); menupos.Y += itemHeight;
            TextHandler.DrawText("OPTIONS", menupos, 2.0f, Color.White); menupos.Y += itemHeight;
            TextHandler.DrawText("CREDITS", menupos, 2.0f, Color.White); menupos.Y += itemHeight;
            TextHandler.DrawText("HELP", menupos, 2.0f, Color.White); menupos.Y += itemHeight;
            TextHandler.DrawText("QUIT", menupos, 2.0f, Color.White);

            menupos.X = 700;//centerPos.X;
            menupos.Y = 460;
            TextHandler.DrawTextCentered("VERSION 1.0", menupos, 1.0f, Menus.versionDimColor); 

            var srcRect = new Rectangle(iSelectFrame * 32,0,32,20);
            menupos = new Vector2(75, 260 + (itemHeight * Invasion.Option));
            InvasionGame.spriteBatch.Draw(selectedMenuItemSs, menupos, srcRect, Color.White, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.0f);

            iSelectFrame = ++iSelectFrame % 20;
        }

        public void Load(ContentManager c)
        {
            menuSoundEffect = c.Load<SoundEffect>("Sounds/tap");
        }


        public void SetTexture(Texture2D load, Texture2D selected)
        {
            back = load;
            selectedMenuItemSs = selected;
        }

        public override void HandleBackButton(Game game)
        {
            game.Exit();
        }

        public void HandleInput(GameTime gameTime, InputState intouches, Game game)
        {
            Vector2 releasePt;
            Vector2 touchPt = intouches.TouchState.GetTouchPt(out releasePt);
            const int itemHeight  = 40;
            var itemRect = new Rectangle(0, 260, 800, itemHeight);

            int touchedEntry = -1;

            const int numMenuItems = 5;

            for (int menuItem = 0; menuItem < numMenuItems; menuItem++) {
                if (releasePt.X >= itemRect.Left && releasePt.X <= itemRect.Right &&
                    releasePt.Y >= (itemRect.Top + menuItem*itemHeight) && releasePt.Y <= (itemRect.Bottom + menuItem*itemHeight))
                {
                    touchedEntry = menuItem;
                    break;
                }
            }

            if (touchedEntry != -1) {
                Invasion.Option = touchedEntry;

                switch (Invasion.Option) {
                    case 0: // Play Game Menu Item
                        Invasion.currentScreen = GameplayScreen;
                        Invasion.SetupNewGame();

                        var menuSoundEffect = this.menuSoundEffect;
                        SoundManager.Instance.Play(menuSoundEffect);
                        break;

                    case 1: // Options Menu Item
                        Invasion.currentScreen = OptionsScreen;
                        Invasion.DelayStart = Environment.TickCount;
                        break;

                    case 2: // Credits Menu Item
                        Invasion.currentScreen = CreditsScreen;
                        Invasion.DelayStart = Environment.TickCount;
                        break;

                    case 3: // Help Menu Item
                        Invasion.currentScreen = HelpScreen;
                        ExtrasManager.Instance.Reset();

                        for (int iExtra = 0; iExtra < Extra.NumExtraTypes; iExtra++) {
                            Debug.WriteLine("Setting up iExtra = " + iExtra + ", Extra = " + (Extra.ExtraType)(iExtra));
                            Vector2 extraPos = new Vector2(240, HelpScreen.HelpScreenExtrasFirstRowY+HelpScreen.HelpScreenExtrasLineSpacing*iExtra);
                            Extra extra = new Extra(game, extraPos, (Extra.ExtraType)(iExtra));
                            ExtrasManager.Instance.Add(extra);
                        }

                        Invasion.DelayStart = Environment.TickCount;
                        //Debug.WriteLine("delayOffset = " + delayOffset + ", delayStart = " + delayStart);
                        break;

                    case 4: // Quit Menu Item Exits the Game
                        game.Exit();
                        break;
                }
            }
            else {
                for (int menuItem = 0; menuItem < numMenuItems; menuItem++) {

                    if (touchPt.X >= itemRect.Left && touchPt.X <= itemRect.Right &&
                        touchPt.Y >= (itemRect.Top + menuItem*itemHeight) && touchPt.Y <= (itemRect.Bottom + menuItem*itemHeight))
                    {
                        touchedEntry = menuItem;
                        break;
                    }
                }

                if (touchedEntry != -1) {
                    Invasion.Option = touchedEntry;
                }
            }
        }
    }
}