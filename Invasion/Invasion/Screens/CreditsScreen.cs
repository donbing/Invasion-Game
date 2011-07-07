using System;
using ImproviSoft.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Invasion.Screens
{
    public class CreditsScreen : GameScreen, IGameScreen
    {
        private const int DelayCredits = 30000;

        public void SetEnabledGestures()
        {
            TouchPanel.EnabledGestures = GestureType.Tap;
        }

        public void Draw (GameTime gameTime)
        {
            Vector2 pos2 = Vector2.Zero;
            InvasionGame.graphics.GraphicsDevice.Clear(Color.Black);

            int yPos = 35;
            int lineSpacing = 30;

            TextHandler.DrawTextCentered("THIS GAME IS OPEN-SOURCE FREEWARE!", new Vector2(GetCenterPos().X, yPos));
            yPos += lineSpacing;
            TextHandler.DrawTextCentered("FIND THE SOURCE CODE AT CODEPROJECT.COM", new Vector2(GetCenterPos().X, yPos));
            yPos += 3*lineSpacing/2;

            TextHandler.DrawTextCentered("FEEL FREE TO EMAIL US WITH QUESTIONS OR", new Vector2(GetCenterPos().X, yPos));
            yPos += lineSpacing;
            TextHandler.DrawTextCentered("ABOUT OUR SOFTWARE DEVELOPMENT SERVICES:",
                                         new Vector2(GetCenterPos().X, yPos));
            yPos += 3*lineSpacing/2;

            TextHandler.DrawTextCentered("WINDOWS - ORIGINAL DIRECTX VERSION, 2002",
                                         new Vector2(GetCenterPos().X, yPos), 1.0f, Color.Gray);
            yPos += lineSpacing;
            TextHandler.DrawTextCentered("MAURICIORITTER@HOTMAIL.COM", new Vector2(GetCenterPos().X, yPos), 1.0f,
                                         Color.Gray);
            yPos += 4*lineSpacing/3;

            TextHandler.DrawTextCentered("WINDOWS - MANAGED DIRECTX VERSION, 2003", new Vector2(GetCenterPos().X, yPos),
                                         1.0f, Color.Gray);
            yPos += lineSpacing;
            TextHandler.DrawTextCentered("STEVE@YSGARD.COM", new Vector2(GetCenterPos().X, yPos), 1.0f, Color.Gray);
            yPos += 4*lineSpacing/3;

            TextHandler.DrawTextCentered("WINDOWS PHONE - XNA VERSION, 2011", new Vector2(GetCenterPos().X, yPos));
            yPos += lineSpacing;
            TextHandler.DrawTextCentered("SUPPORT@IMPROVISOFT.COM", new Vector2(GetCenterPos().X, yPos));
            yPos += lineSpacing;
            TextHandler.DrawTextCentered("WWW.IMPROVISOFT.COM", new Vector2(GetCenterPos().X, yPos));
            yPos += lineSpacing;

            if (((Invasion.TickLast - Invasion.DelayStart)/Menus.delayBlink)%2 == 1)
            {
                pos2.X = GetCenterPos().X;
                pos2.Y = 450;
                TextHandler.DrawTextCentered("TAP SCREEN TO CONTINUE", pos2);
            }

            if (Invasion.DelayStart + DelayCredits < Invasion.TickLast)
                Invasion.currentScreen = MainMenuScreen;
        }

        public void HandleInput(GameTime gameTime, InputState input, Game game)
        {
            Vector2 releasePt;
            Vector2 touchPt = input.TouchState.GetTouchPt(out releasePt);
            // go back to Main Menu when screen is tapped
            if (releasePt != Menus.notTouched)
            {
                Invasion.currentScreen = GameScreen.MainMenuScreen;
                Invasion.DelayStart = Environment.TickCount;
            }
        }
    }
}