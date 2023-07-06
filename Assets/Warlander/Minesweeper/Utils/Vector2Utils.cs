using UnityEngine;

namespace Warlander.Minesweeper.Utils
{
    /// <summary>
    /// Some vector extensions for math readability improvement.
    /// </summary>
    public static class Vector2Utils
    {
        public static Vector2 SetX(this Vector2 vec, float newX)
        {
            return new Vector2(newX, vec.y);
        }
        
        public static Vector2 SetY(this Vector2 vec, float newY)
        {
            return new Vector2(vec.x, newY);
        }

        public static Vector2 AddX(this Vector2 vec, float addX)
        {
            return new Vector2(vec.x + addX, vec.y);
        }
        
        public static Vector2 AddY(this Vector2 vec, float addY)
        {
            return new Vector2(vec.x, vec.y + addY);
        }

        public static Vector2Int RoundToInt(this Vector2 vec)
        {
            return new Vector2Int(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y));
        }
        
        public static Vector2Int FloorToInt(this Vector2 vec)
        {
            return new Vector2Int(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.y));
        }
    }
}