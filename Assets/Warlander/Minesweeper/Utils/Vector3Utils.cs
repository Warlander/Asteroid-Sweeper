using UnityEngine;

namespace Warlander.Minesweeper.Utils
{
    /// <summary>
    /// Some vector extensions for math readability improvement.
    /// </summary>
    public static class Vector3Utils
    {
        public static Vector2 ToXY(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.y);
        }
    }
}