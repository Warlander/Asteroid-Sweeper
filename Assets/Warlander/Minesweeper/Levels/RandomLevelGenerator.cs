using UnityEngine;

namespace Warlander.Minesweeper.Levels
{
    public class RandomLevelGenerator
    {
        /// <summary>
        /// Simple level generation algorithm, we don't need anything more sophisticated as in every reasonably
        /// fun level there won't ever be enough mines to cause level to generate slowly.
        /// </summary>
        public Level GenerateLevel(int width, int height, int minesCount)
        {
            int totalSize = width * height;
            if (minesCount >= totalSize)
            {
                Debug.LogError("Can't have more mines on level than total possible space.");
                return null;
            }
            else if (minesCount >= totalSize / 2)
            {
                Debug.LogWarning("More than 50% mines in level - this can lead to slow level generation and frustrating levels.");
            }
            
            bool[,] mines = new bool[width, height];
            int remainingMines = minesCount;
            while (remainingMines > 0)
            {
                int randomX = Random.Range(0, width);
                int randomY = Random.Range(0, height);
                if (mines[randomX, randomY] == false)
                {
                    mines[randomX, randomY] = true;
                    remainingMines--;
                }
            }

            return new Level(mines);
        }
    }
}