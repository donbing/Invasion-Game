using Microsoft.Xna.Framework;

namespace Invasion
{
    public static class RectangleExtensions
    {
        // Helper function to determine if a point (Vector2) is contained within a Rectangle
        public static bool Intersects(this Rectangle rect, Vector2 pt)
        {
            return
                pt.X >= rect.X && pt.X <= rect.X + rect.Width &&
                pt.Y >= rect.Y && pt.Y <= rect.Y + rect.Height;
        }
    }
}