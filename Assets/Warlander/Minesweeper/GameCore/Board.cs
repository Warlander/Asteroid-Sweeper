using System;
using UnityEngine;
using Warlander.Minesweeper.Levels;
using Zenject;

namespace Warlander.Minesweeper.GameCore
{
    public class Board
    {
        [Inject] private GameConfig _gameConfig;
        
        /// <summary>
        /// Mine hit. This should end the game.
        /// </summary>
        public event TileCoordsDelegate MineHit;
        /// <summary>
        /// Happens once all tiles except mines are revealed.
        /// </summary>
        public event Action<BoardState> BoardStateChanged;
        /// <summary>
        /// Safe tile is directly revealed by a player. (not part of the cascade of tiles with 0 nearby mines)
        /// </summary>
        public event TileCoordsDelegate SafeTileRevealed;
        /// <summary>
        /// Automatically revealed tiles as part of the cascade.
        /// </summary>
        public event TileCoordsWithOriginDelegate CascadeTileRevealed;
        /// <summary>
        /// Tile got flagged by a player.
        /// </summary>
        public event TileCoordsDelegate TileFlagged;
        /// <summary>
        /// Tile got unflagged by a player.
        /// </summary>
        public event TileCoordsDelegate TileUnflagged;
        /// <summary>
        /// Saving grace feature is triggered, if it's enabled in config. Refer to config for more info.
        /// </summary>
        public event Action SavingGraceTriggered;

        private bool[,] _mines;
        private bool[,] _exploredTiles;
        private bool[,] _flaggedTiles;
        private int[,] _nearbyMines;

        /// <summary>
        /// This can go into negatives - we count incorrectly set flags too, otherwise game would be too easy.
        /// </summary>
        public int RemainingMines => _totalMines - _totalFlags;
        public int Width => IsInitialised() ? _mines.GetLength(0) : 0;
        public int Height => IsInitialised() ? _mines.GetLength(1) : 0;
        public BoardState BoardState => _boardState;
        
        private BoardState _boardState = BoardState.BeforeInit;
        private bool _anyMovePerformed;
        private int _totalMines;
        private int _totalFlags;
        
        public void InitialiseBoard(Level level)
        {
            bool[,] mines = level.Mines;
            
            int width = mines.GetLength(0);
            int height = mines.GetLength(1);
            
            _mines = new bool[width, height];
            _exploredTiles = new bool[width, height];
            _flaggedTiles = new bool[width, height];
            _nearbyMines = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (mines[x, y])
                    {
                        _totalMines++;
                    }
                    _mines[x, y] = mines[x, y];
                    _exploredTiles[x, y] = false;
                    _nearbyMines[x, y] = CountNearbyMines(mines, x, y);
                }
            }

            ChangeState(BoardState.Playing);
        }

        public void RevealTile(int x, int y)
        {
            if (CheckBoundsWithWarning(x, y) == false || _exploredTiles[x, y] || _flaggedTiles[x, y])
            {
                return;
            }

            bool hitMine = _mines[x, y];
            bool savingGrace = _gameConfig.SavingGraceEnabled && _anyMovePerformed == false && hitMine;
            
            _anyMovePerformed = true;
            if (hitMine && savingGrace == false)
            {
                ChangeState(BoardState.Lost);
                _exploredTiles[x, y] = true;
                MineHit?.Invoke(x, y);
            }
            else if (hitMine && savingGrace)
            {
                TriggerSavingGrace(x, y);
                SavingGraceTriggered?.Invoke();
            }
            else
            {
                _exploredTiles[x, y] = true;
                SafeTileRevealed?.Invoke(x, y);
            }

            if (_nearbyMines[x, y] == 0)
            {
                TriggerCascadeCheck(x, y, x, y);
            }

            if (WinCheck())
            {
                ChangeState(BoardState.Won);
            }
        }

        private void TriggerSavingGrace(int x, int y)
        {
            for (int xDelta = -1; xDelta <= 1; xDelta++)
            {
                for (int yDelta = -1; yDelta <= 1; yDelta++)
                {
                    int checkX = x + xDelta;
                    int checkY = y + yDelta;

                    SavingGraceCheckTile(checkX, checkY);
                }
            }
        }

        private void SavingGraceCheckTile(int x, int y)
        {
            if (CheckBounds(x, y) == false)
            {
                return;
            }

            if (_mines[x, y])
            {
                FlagTileInternal(x, y, true);
            }
            else
            {
                _exploredTiles[x, y] = true;
                SafeTileRevealed?.Invoke(x, y);
            }
        }

        private void TriggerCascadeCheck(int x, int y, int originalX, int originalY)
        {
            for (int xDelta = -1; xDelta <= 1; xDelta++)
            {
                for (int yDelta = -1; yDelta <= 1; yDelta++)
                {
                    CascadeCheckRecursive(x + xDelta, y + yDelta, originalX, originalY);
                }
            }
        }
        
        private void CascadeCheckRecursive(int x, int y, int originalX, int originalY)
        {
            bool originTile = x == originalX && y == originalY;
            
            if (CheckBounds(x, y) == false || originTile || _exploredTiles[x, y] || _flaggedTiles[x, y])
            {
                return;
            }

            _exploredTiles[x, y] = true;
            CascadeTileRevealed?.Invoke(x, y, originalX, originalY);

            if (_nearbyMines[x, y] == 0)
            {
                TriggerCascadeCheck(x, y, originalX, originalY);
            }
        }

        public void FlagTile(int x, int y)
        {
            if (CheckBoundsWithWarning(x, y) == false || _exploredTiles[x, y])
            {
                return;
            }

            if (_flaggedTiles[x, y] == false)
            {
                FlagTileInternal(x, y, true);
            }
            else if (_flaggedTiles[x, y])
            {
                FlagTileInternal(x, y, false);
            }
        }

        public bool IsTileMine(int x, int y)
        {
            if (CheckBoundsWithWarning(x, y) == false)
            {
                return false;
            }

            return _mines[x, y];
        }

        public bool IsTileFlagged(int x, int y)
        {
            if (CheckBoundsWithWarning(x, y) == false)
            {
                return false;
            }
            
            return _flaggedTiles[x, y];
        }
        
        public bool IsTileExplored(int x, int y)
        {
            if (CheckBoundsWithWarning(x, y) == false)
            {
                return false;
            }

            return _exploredTiles[x, y];
        }
        
        public int GetNearbyMines(int x, int y)
        {
            if (CheckBoundsWithWarning(x, y) == false)
            {
                return 0;
            }

            return _nearbyMines[x, y];
        }

        private bool WinCheck()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (_exploredTiles[x, y] == false && _mines[x, y] == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
        private bool CheckBoundsWithWarning(int x, int y)
        {
            bool withinBounds = CheckBounds(x, y);
            if (withinBounds == false)
            {
                Debug.LogWarning($"Tile outside bounds: {x} {y}");
            }
            return withinBounds;
        }

        public bool CheckBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        private int CountNearbyMines(bool[,] mines, int x, int y)
        {
            int mineCount = 0;
            for (int xDelta = -1; xDelta <= 1; xDelta++)
            {
                for (int yDelta = -1; yDelta <= 1; yDelta++)
                {
                    if (ContainsMineWithBoundsCheck(mines, x + xDelta, y + yDelta))
                    {
                        mineCount++;
                    }
                }
            }

            return mineCount;
        }
        
        private bool ContainsMineWithBoundsCheck(bool[,] mines, int x, int y)
        {
            if (x < 0 || x >= mines.GetLength(0)
                      || y < 0 || y >= mines.GetLength(1))
            {
                return false;
            }

            return mines[x, y];
        }

        private void ChangeState(BoardState state)
        {
            _boardState = state;
            BoardStateChanged?.Invoke(_boardState);
        }

        private void FlagTileInternal(int x, int y, bool flagged)
        {
            bool stateChanged = _flaggedTiles[x, y] != flagged;
            if (stateChanged == false)
            {
                return;
            }
            
            _flaggedTiles[x, y] = flagged;

            if (flagged)
            {
                _totalFlags++;
                TileFlagged?.Invoke(x, y);
            }
            else
            {
                _totalFlags--;
                TileUnflagged?.Invoke(x, y);
            }
        }
        
        private bool IsInitialised()
        {
            return _boardState != BoardState.BeforeInit;
        }
    }
}