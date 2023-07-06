using UnityEngine;
using Warlander.Minesweeper.GameCore;
using Zenject;

namespace Warlander.Minesweeper.AutoPlay
{
    public class AutoPlayer
    {
        [Inject] private Board _board;
        
        /// <summary>
        /// Performs one move automatically playing the game. This does not equal to one tile at the time, as multiple
        /// tiles can be affected by features like 0 neighbours auto-tile reveal.
        /// It will never perform move which have any chance to lead to game over (like first move and once board
        /// is in state without any safe moves).
        /// It can accept board with any state by design and will fix the incorrect flags, while utilising exact same
        /// knowledge player has. (so no direct checking if tile is mine at all)
        /// </summary>
        /// <returns>Can any safe move be performed?</returns>
        public bool MakeMove()
        {
            // Flag any tiles that must contain mines first. This doesn't depend on any other check.
            for (int x = 0; x < _board.Width; x++)
            {
                for (int y = 0; y < _board.Height; y++)
                {
                    Vector2Int? foundMine = CheckIfCanFlagNearbyTile(x, y);
                    if (foundMine.HasValue)
                    {
                        _board.FlagTile(foundMine.Value.x, foundMine.Value.y);
                        return true;
                    }
                }
            }
            
            // Check for incorrectly placed flags.
            // This should be done after but doesn't strictly depend on marking all tiles with mines.
            for (int x = 0; x < _board.Width; x++)
            {
                for (int y = 0; y < _board.Height; y++)
                {
                    if (CheckIfFlagStatusIsCorrect(x, y) == false)
                    {
                        _board.FlagTile(x, y);
                        return true;
                    }
                }
            }
            
            // Check for any save tile reveals. We must make sure all flags are valid before.
            for (int x = 0; x < _board.Width; x++)
            {
                for (int y = 0; y < _board.Height; y++)
                {
                    Vector2Int? safeTile = CheckIfCanRevealForNearbyTile(x, y);
                    if (safeTile.HasValue)
                    {
                        _board.RevealTile(safeTile.Value.x, safeTile.Value.y);
                        return true;
                    }
                }
            }

            return false;
        }

        private Vector2Int? CheckIfCanFlagNearbyTile(int x, int y)
        {
            if (_board.IsTileExplored(x, y) == false)
            {
                return null;
            }
            
            int nearbyMines = _board.GetNearbyMines(x, y);
            if (nearbyMines == 0)
            {
                return null;
            }
            
            int unexploredTiles = CountUnexploredNeighbours(x, y);

            if (unexploredTiles <= nearbyMines)
            {
                // All nearby tiles are mines, put flag on any tile that's not yet flagged if one exists.
                return FindUnflaggedNeighbour(x, y);
            }

            return null;
        }

        private bool CheckIfFlagStatusIsCorrect(int x, int y)
        {
            if (_board.IsTileFlagged(x, y) == false)
            {
                return true;
            }
            
            for (int deltaX = -1; deltaX <= 1; deltaX++)
            {
                for (int deltaY = -1; deltaY <= 1; deltaY++)
                {
                    int checkX = x + deltaX;
                    int checkY = y + deltaY;

                    if (_board.CheckBounds(checkX, checkY) && AreAllNeighboursMines(checkX, checkY))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        /// <summary>
        /// This method assumes we checked validity of all flags and put all possible flags before.
        /// </summary>
        private Vector2Int? CheckIfCanRevealForNearbyTile(int x, int y)
        {
            if (AreAllNearbyMinesMarked(x, y) == false)
            {
                return null;
            }

            return FindUnflaggedNeighbour(x, y);
        }

        private int CountUnexploredNeighbours(int x, int y)
        {
            int unexploredTiles = 0;
            
            for (int deltaX = -1; deltaX <= 1; deltaX++)
            {
                for (int deltaY = -1; deltaY <= 1; deltaY++)
                {
                    int checkX = x + deltaX;
                    int checkY = y + deltaY;

                    if (_board.CheckBounds(checkX, checkY)
                        && _board.IsTileExplored(checkX, checkY) == false)
                    {
                        unexploredTiles++;
                    }
                }
            }

            return unexploredTiles;
        }

        /// <summary>
        /// Returns true only if all neighbouring tiles must be mines and we revealed this tile.
        /// </summary>
        private bool AreAllNeighboursMines(int x, int y)
        {
            if (_board.IsTileExplored(x, y) == false)
            {
                return false;
            }
            
            int nearbyMines = _board.GetNearbyMines(x, y);
            int unexploredNeighbours = CountUnexploredNeighbours(x, y);
            return nearbyMines == unexploredNeighbours;
        }

        private Vector2Int? FindUnflaggedNeighbour(int x, int y)
        {
            for (int deltaX = -1; deltaX <= 1; deltaX++)
            {
                for (int deltaY = -1; deltaY <= 1; deltaY++)
                {
                    int checkX = x + deltaX;
                    int checkY = y + deltaY;

                    if (_board.CheckBounds(checkX, checkY)
                        && _board.IsTileExplored(checkX, checkY) == false
                        && _board.IsTileFlagged(checkX, checkY) == false)
                    {
                        return new Vector2Int(checkX, checkY);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// This method assumes we checked validity of all flags and put all possible flags before.
        /// </summary>
        private bool AreAllNearbyMinesMarked(int x, int y)
        {
            if (_board.IsTileExplored(x, y) == false)
            {
                return false;
            }
            
            int nearbyMines = _board.GetNearbyMines(x, y);
            int flaggedNeighbours = CountFlaggedNeighbours(x, y);

            return nearbyMines == flaggedNeighbours;
        }
        
        private int CountFlaggedNeighbours(int x, int y)
        {
            int flaggedTiles = 0;
            
            for (int deltaX = -1; deltaX <= 1; deltaX++)
            {
                for (int deltaY = -1; deltaY <= 1; deltaY++)
                {
                    int checkX = x + deltaX;
                    int checkY = y + deltaY;

                    if (_board.CheckBounds(checkX, checkY)
                        && _board.IsTileFlagged(checkX, checkY))
                    {
                        flaggedTiles++;
                    }
                }
            }

            return flaggedTiles;
        }
    }
}