using ImproviSoft.System;
using Microsoft.Xna.Framework;

namespace Invasion.Screens
{
    public interface IGameScreen
    {
        void SetEnabledGestures();
        void Draw(GameTime gameTime);
        void HandleBackButton(Game game);
        void HandleInput(GameTime gameTime, InputState input, Game game);
    }
}