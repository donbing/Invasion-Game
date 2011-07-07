using Invasion.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Invasion
{
    public static class InputExtensions
    {
        public static Vector2 GetTouchPt(this TouchCollection touches, out Vector2 releasePt)
        {
            Vector2 touchPt = Menus.notTouched;
            releasePt = Menus.notTouched;

            if (touches.Count > 0) {
                if (touches[0].State == TouchLocationState.Pressed || touches[0].State == TouchLocationState.Moved)
                    touchPt = touches[0].Position;

                else if (touches[0].State == TouchLocationState.Released)
                    releasePt = touches[0].Position;
            }
            return touchPt;
        }
    }
}