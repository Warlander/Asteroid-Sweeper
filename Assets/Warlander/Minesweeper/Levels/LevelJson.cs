using System;
using System.Linq;
using UnityEngine;

namespace Warlander.Minesweeper.Levels
{
    [Serializable]
    public class LevelJson
    {
        [SerializeField] private string levelName;
        [SerializeField] private string[] levelRows;

        public string LevelName => levelName;

        public int Width => levelRows[0].Length;
        public int Height => levelRows.Length;
        
        public Level ToLevel()
        {
            bool[,] mines = StringArrayTo2DBoolArray(levelRows, '1');
            return new Level(mines);
        }
        
        /// <summary>
        /// This method should ALWAYS be called before using levels anywhere outside code loading them or any properties called.
        /// Any levels which do not pass validation should be discarded and never shown to the player.
        /// </summary>
        /// <returns>Is the level valid.</returns>
        public bool Validate()
        {
            if (string.IsNullOrEmpty(levelName))
            {
                Debug.LogError("Level with empty name found!");
                return false;
            }

            if (levelRows == null || levelRows.Length == 0)
            {
                Debug.LogError($"Level rows must be defined for a level {levelName}.");
                return false;
            }
            
            int distictLevelRowSizes = levelRows.Select(row => row.Length).Distinct().Count();
            if (distictLevelRowSizes != 1)
            {
                Debug.LogError($"Row sizes of level {levelName} are uneven - they must be all the same size!");
                return false;
            }

            return true;
        }

        private bool[,] StringArrayTo2DBoolArray(string[] stringArray, char lookupChar)
        {
            if (stringArray.Length == 0)
            {
                return new bool[0, 0];
            }

            bool[,] boolArray = new bool[stringArray[0].Length, stringArray.Length];
            for (int y = 0; y < stringArray.Length; y++)
            {
                // Reverse rows to transform them into Unity's coordinate system.
                string rowString = stringArray[stringArray.Length - 1 - y];
                for (int x = 0; x < rowString.Length; x++)
                {
                    if (rowString[x] == lookupChar)
                    {
                        boolArray[x, y] = true;
                    }
                }
            }

            return boolArray;
        }

        public string GetTextDescription()
        {
            return $"{levelName} ({Width}X{Height})";
        }
    }
}