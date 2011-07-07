using System;
using ImproviSoft.System;
using Invasion.SpriteManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Invasion.Screens
{
    public class HelpScreen : GameScreen, IGameScreen
    {
        private const int helpDelay = 30000;

        public const int HelpScreenExtrasFirstRowY = 110;
        public const int HelpScreenExtrasLineSpacing = 30;

        public void SetEnabledGestures()
        {
            TouchPanel.EnabledGestures = GestureType.Tap;
        }

        public void Draw(GameTime gameTime)
        {
            Vector2 pos;
            int yPos;
            int lineSpacing;
            InvasionGame.graphics.GraphicsDevice.Clear(Color.Black);

            pos = new Vector2(GetCenterPos().X, 40);
            TextHandler.DrawTextCentered("HELP", pos, 2.0f, Color.White); pos.Y += 36;
            TextHandler.DrawTextCentered("VERSION 1.0", pos, 1.0f, Menus.versionDimColor); 
                    
            yPos = HelpScreenExtrasFirstRowY;

            pos = new Vector2(280, yPos);
            lineSpacing = HelpScreenExtrasLineSpacing;

            TextHandler.DrawText("PHOTON AMMO BONUS", pos); pos.Y += lineSpacing;
            TextHandler.DrawText("WEAPON UPGRADE BONUS",pos); pos.Y += lineSpacing;
            TextHandler.DrawText("100 POINTS BONUS",pos); pos.Y += lineSpacing;
            TextHandler.DrawText("LASER AMMO BONUS",pos);	 pos.Y += lineSpacing;
            TextHandler.DrawText("SHIELD CHARGE BONUS",pos);
                    
            pos.X = GetCenterPos().X; pos.Y += 3 * lineSpacing / 2;

            TextHandler.DrawTextCentered("TILT SCREEN OR FLICK SHIP - MOVE SHIP", pos);	 pos.Y += lineSpacing;
            TextHandler.DrawTextCentered("DRAG SHIP - MOVE AND THEN STOP SHIP", pos);  pos.Y += lineSpacing;
            TextHandler.DrawTextCentered("TAP SCREEN - FIRE WEAPON", pos);  pos.Y += lineSpacing;
            TextHandler.DrawTextCentered("TAP WEAPON - CHANGE WEAPON IF AVAILABLE", pos);  pos.Y += lineSpacing;
            TextHandler.DrawTextCentered("HOLD SCOREBOARD - GO TO OPTIONS SCREEN", pos);
					
            ExtrasManager.Instance.Draw(gameTime);

            if (((Invasion.TickLast - Invasion.DelayStart) / Menus.delayBlink) % 2 == 1) {
                pos.X = GetCenterPos().X;
                pos.Y = 450;
                TextHandler.DrawTextCentered("TAP SCREEN TO CONTINUE", pos);
            }

            if (Invasion.DelayStart + helpDelay < Invasion.TickLast)
                Invasion.currentScreen = MainMenuScreen;
        }


        public void HandleInput(GameTime gameTime, InputState input, Game game)
        {
            Vector2 releasePt;
            Vector2 touchPt = input.TouchState.GetTouchPt(out releasePt);
            // go back to Main Menu when screen is tapped
            if (releasePt != Menus.notTouched)
            {
                Invasion.currentScreen = MainMenuScreen;
                Invasion.DelayStart = Environment.TickCount;
            }
        }
    }
}