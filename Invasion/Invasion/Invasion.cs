using System;
using Invasion.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using ImproviSoft.System;

using Invasion.Sprites;
using Invasion.SpriteManagers;


namespace Invasion
{
    public enum DifficultyLevel { Easy, Medium, Hard }

	public class Invasion : DrawableGameComponent
	{
        static IGameScreen _prevFrameScreen = GameScreen.GameOverScreen;
        public static IGameScreen currentScreen = GameScreen.MainMenuScreen;

        public static int Option = 0;
        public static int Level = 0;
        bool waitForRelease = false;

        internal static StarShip StarShip;

		// setup the delay variables and constants;
        public static int TickLast = 0; // Holds the value of the last call to GetTick.
        public static int DelayStart = 0;

	    readonly InputState input = new InputState();

        public static bool GameInProgress {
            get;
            private set;
        }
        
		public Invasion(Game game) : base(game) 
        {
            GameInProgress = false;

            // Initialize the static Instance 
            Game.Components.Add(new BulletsManager(Game));
            Game.Components.Add(new ExtrasManager(Game));
            Game.Components.Add(new UFOsManager(Game));

            StarShip = new StarShip(Game);
            Game.Components.Add(StarShip);

            ContentLoader.LoadContent(Game.Content);

            TouchPanel.EnabledGestures = GestureType.None;
		}

        protected override void LoadContent() {
            base.LoadContent();

            TextHandler.LoadContent(Game.Content);
            GameScreen.LoadContent(Game.Content);
        }

        public override void Update(GameTime gameTime) {

            input.Update();

            if (currentScreen != _prevFrameScreen) {
                DefineGesturesForCurrentScreen();

                if (input.TouchState.Count > 0)
                    waitForRelease = true;

                // Don't allow any gestures generated on previous screen to be handled by the new one
                while (input.Gestures.Count > 0)
                    input.Gestures.Clear();

                if (currentScreen == GameScreen.MainMenuScreen) {
                    GameInProgress = false;
                    Option = 0;
                    UFOsManager.Instance.Reset();
                }

                _prevFrameScreen = currentScreen;
            }

            HandleInput(gameTime);
            
            if (currentScreen == GameScreen.GameplayScreen) {
                if (StarShip.ShipState == ShipState.Destroyed) {
                    currentScreen = GameScreen.GameOverScreen;
                    GameInProgress = false;
                    DelayStart = Environment.TickCount;
                    SoundManager.Instance.Play(InvasionGame.gameOverSoundEffect);
                }
            }

            StarShip.Update(gameTime);

            TickLast = Environment.TickCount;
		}

		public override void Draw(GameTime gameTime)
	    {
            currentScreen.Draw(gameTime);
	    }

        public static IGameScreen CurrentScreen
        {
            get { return currentScreen; }
        }

        private static void DefineGesturesForCurrentScreen()
        {
            if (currentScreen != null)
                currentScreen.SetEnabledGestures();
            else
                TouchPanel.EnabledGestures = GestureType.None;
        }

        private static bool HandleBackButton(InputState inputState, Game game) {

            PlayerIndex player;
            bool backPressed = inputState.IsNewButtonPress(Buttons.Back, PlayerIndex.One, out player);

            if (backPressed) 
                currentScreen.HandleBackButton(game);
              
            return backPressed;
        }

	    private void HandleInput(GameTime gameTime)
        {
            if (HandleBackButton(input, Game))
                return;

            var touches = input.TouchState;

            if (waitForRelease) {
                if (touches.Count == 0)
                    waitForRelease = false;
                else
                    return;
            }

            currentScreen.HandleInput(gameTime, input, Game);
        }

        public static void SetupNewGame() {
            GameInProgress = true;

            Level = 0;

            StarShip.Reset();

            InvasionGame.Scoreboard.Score = 0;

			DelayStart = Environment.TickCount;
            currentScreen = InitNextLevel(Level++, StarShip);
        }

        public static IGameScreen InitNextLevel(int level, StarShip ship)
		{
            InvasionGame.Scoreboard.Level = level;

            BulletsManager.Instance.Reset();
            UFOsManager.Instance.Reset();
            ExtrasManager.Instance.Reset();

            ship.InitLevel();

            UFOsManager.Instance.SetLevel(level);
            UFOsManager.Instance.InitLevel(level);


            return GameScreen.LevelScreen;
		}
	}
}
