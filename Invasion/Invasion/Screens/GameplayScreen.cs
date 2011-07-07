using System;
using ImproviSoft.System;
using Invasion.SpriteManagers;
using Invasion.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Invasion.Screens
{
    public class GameplayScreen : GameScreen, IGameScreen
    {
        public static float timeSinceSpeedChangeSec = 60.0f;
        public void SetEnabledGestures()
        {
            TouchPanel.EnabledGestures =
                GestureType.Flick |
                GestureType.Hold |
                GestureType.HorizontalDrag |
                GestureType.Tap;
        }

        private Texture2D back;
        public void SetTexture(Texture2D starfield)
        {
            back = starfield;
        }

        public void Draw(GameTime gameTime)
        {
            InvasionGame.spriteBatch.Draw(back, Vector2.Zero, Color.White);

            InvasionGame.Scoreboard.Draw(gameTime);

            if (UFOsManager.Instance.Count == 0 && ExtrasManager.Instance.Count == 0)
            {
                SoundManager.Instance.Play(InvasionGame.startGameSoundEffect);
                Invasion.DelayStart = Environment.TickCount;
                Invasion.currentScreen = Invasion.InitNextLevel(Invasion.Level++, Invasion.StarShip);
                return;
            }

            ExtrasManager.Instance.Draw(gameTime);
            BulletsManager.Instance.Draw(gameTime);
            UFOsManager.Instance.Draw(gameTime);
            Invasion.StarShip.Draw(gameTime);
        }

        public override void HandleBackButton(Game game)
        {
            Invasion.currentScreen = PauseScreen;
        }

        public void HandleInput(GameTime gameTime, InputState input, Game game)
        {
            Vector2 releasePt;
            Vector2 touchPt = input.TouchState.GetTouchPt(out releasePt);
            timeSinceSpeedChangeSec += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (releasePt != Menus.notTouched)
            {
                if (InvasionGame.Scoreboard.WeaponTapRect.Intersects(releasePt))
                {
                    Invasion.StarShip.SelectNextWeapon();
                    InvasionGame.Scoreboard.Weapon = Invasion.StarShip.Weapon;
                    input.Gestures.Clear();
                    //while (TouchPanel.IsGestureAvailable)
                    //    TouchPanel.ReadGesture();
                    return;
                }
                else if (InvasionGame.Scoreboard.AutoSelectTapRect.Intersects(releasePt))
                {
                    StarShip.AutoSelectWeapon = !StarShip.AutoSelectWeapon;
                    input.Gestures.Clear();
                    return;
                }
            }

            bool firePressed = false;
            int moveShipLeft = 0;
            int moveShipRight = 0;
            bool skidToAStop = false;
            bool skid = false;
            bool stopMovingShip = false;

            if (touchPt != Menus.notTouched && Invasion.StarShip.Intersects(touchPt)) {
                starshipSelected = true;
            }

            foreach (GestureSample gesture in input.Gestures)
            {

                // we can use the type of gesture to determine our behavior
                switch (gesture.GestureType) {

                        // on taps, we change the color of the selected sprite
                    case GestureType.Tap:
                        //case GestureType.DoubleTap:
                        if (starshipSelected) {
                            if (Math.Abs(Invasion.StarShip.SpeedX) > 2)
                                skidToAStop = true;
                            else
                                stopMovingShip = true;
                        }

                        firePressed = true;
                        break;

                        // on drags, we just want to move the selected sprite with the drag
                    case GestureType.HorizontalDrag:
                        if (starshipSelected) {
                            stopMovingShip = true;
                            Invasion.StarShip.Position += gesture.Delta;

                            // skid if switching directions
                            if (gesture.Delta.X > 0.0f) { // dragging right
                                if (Invasion.StarShip.SpeedX < 0 || lastDragDelta < 0.0f)
                                    skid = true;
                            }
                            else if (gesture.Delta.X < 0.0f) { //dragging left
                                if (Invasion.StarShip.SpeedX > 0 || lastDragDelta > 0.0f)
                                    skid = true;
                            }
                            lastDragDelta = gesture.Delta.X;
                        }
                        break;

                        // When the scoreboard is held, go to the Options menu
                    case GestureType.Hold:
                        if ((InvasionGame.Scoreboard.DisplaySide == DisplaySide.Left && touchPt.X < Scoreboard.Width) ||
                            (InvasionGame.Scoreboard.DisplaySide == DisplaySide.Right && touchPt.X >= Scoreboard.RightPos.X)) 
                        {
                            Invasion.currentScreen = OptionsScreen;
                        }
                        break;

                        // on flicks, we want to update the selected sprite's velocity with
                        // the flick velocity, which is in pixels per second.
                    case GestureType.Flick:
                        if (starshipSelected) {
                            if (gesture.Delta.X > 0.0f) {
                                if (Invasion.StarShip.SpeedX < 0)
                                    skid = true;
                                moveShipRight = 4;
                            }
                            else if (gesture.Delta.X < 0.0f) {
                                if (Invasion.StarShip.SpeedX > 0)
                                    skid = true;
                                moveShipLeft = 4;
                            }
                        }
                        break;
                }
            }

            // give priority to touch gesture input
            if (moveShipLeft == 0 && moveShipRight == 0 && !stopMovingShip && !skidToAStop) {

                // Handle Accelerometer Input
                var accelerometerState = Accelerometer.GetState();

                float accelX = accelerometerState.Acceleration.X;
                float accelY = accelerometerState.Acceleration.Y;

                if (accelY < -0.1f) {
                    moveShipRight = (accelY < -0.2f)? 4 : 2;

                    if (Invasion.StarShip.SpeedX < -2)
                        skid = true;
                }

                if (accelY > 0.1f) {
                    moveShipLeft = (accelY > 0.2f)? 4 : 2;

                    if (Invasion.StarShip.SpeedX > 2)
                        skid = true;
                }

                // Make very slow moving starship slow to a stop
                if (accelY >= -0.1f && accelY <= 0.1f) {
                    if (timeSinceSpeedChangeSec > minTimeBetweenSpeedChangesSec) {
                        timeSinceSpeedChangeSec = 0.0f;

                        int speed = Math.Abs(Invasion.StarShip.SpeedX);

                        if (Invasion.StarShip.SpeedX < 0) {
                            Invasion.StarShip.MoveLeft(speed-1);
                        }
                        else if (Invasion.StarShip.SpeedX > 0) {
                            Invasion.StarShip.MoveRight(speed-1);
                        }
                    }
                }
            }

            if (firePressed) {
                if (Invasion.StarShip.ShipState == ShipState.OK) {
                    Invasion.StarShip.FireWeapon();
                }
            }

            if (skidToAStop) {
                Invasion.StarShip.SkidToAStop();
            }
            else {
                if (skid)
                    Invasion.StarShip.Skid();

                if (stopMovingShip)
                    Invasion.StarShip.StopMoving();
            }

            // Move after skid/stop in case change in direction : skid, then move opposition direction
            if (moveShipLeft != 0) {
                Invasion.StarShip.MoveLeft(moveShipLeft);
            }
            else if (moveShipRight != 0) {
                Invasion.StarShip.MoveRight(moveShipRight);
            }

            if (touchPt == Menus.notTouched) {
                starshipSelected = false;
                lastDragDelta = 0.0f;
            }
        }

        private static bool starshipSelected = false;
        public const float minTimeBetweenSpeedChangesSec = 1.0f;
        public static float lastDragDelta = 0.0f;
    }
}