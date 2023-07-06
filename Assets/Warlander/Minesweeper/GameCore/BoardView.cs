using System;
using UnityEngine;
using Warlander.Minesweeper.Utils;
using Zenject;

namespace Warlander.Minesweeper.GameCore
{
    public class BoardView : WarlanderBehaviour
    {
        [Inject] private IInstantiator _instantiator;
        [Inject] private Board _board;

        [SerializeField] private BoxCollider2D _gameZoneCollider;
        [SerializeField] private BoardTileView _tileViewPrefab;

        [SerializeField] private float _cascadePropagationTimePerTile = 0.1f;

        private readonly Vector2 _tileSize = Vector2.one;

        private Transform _selfTransform;
        private BoardTileView[,] _boardTiles;

        private void Awake()
        {
            _selfTransform = transform;
            
            _board.SafeTileRevealed += BoardOnSafeTileRevealed;
            _board.CascadeTileRevealed += BoardOnCascadeTileRevealed;
            _board.TileFlagged += BoardOnTileFlagged;
            _board.TileUnflagged += BoardOnTileUnflagged;
            _board.MineHit += BoardOnMineHit;
        }

        private void BoardOnMineHit(int x, int y)
        {
            _boardTiles[x, y].SetMine();
        }

        private void BoardOnTileUnflagged(int x, int y)
        {
            _boardTiles[x, y].SetHidden();
        }

        private void BoardOnTileFlagged(int x, int y)
        {
            _boardTiles[x, y].SetFlagged();
        }

        private void BoardOnCascadeTileRevealed(int x, int y, int originX, int originY)
        {
            float distance = Vector2.Distance(new Vector2(x, y), new Vector2(originX, originY));
            float animationDelay = distance * _cascadePropagationTimePerTile;
            RunAfter(animationDelay, () => _boardTiles[x, y].SetNumber(_board.GetNearbyMines(x, y)));
        }

        private void BoardOnSafeTileRevealed(int x, int y)
        {
            _boardTiles[x, y].SetNumber(_board.GetNearbyMines(x, y));
        }

        public void CreateBoard()
        {
            _boardTiles = new BoardTileView[_board.Width, _board.Height];
            for (int x = 0; x < _board.Width; x++)
            {
                for (int y = 0; y < _board.Height; y++)
                {
                    BoardTileView tileView =
                        _instantiator.InstantiatePrefabForComponent<BoardTileView>(_tileViewPrefab, _selfTransform);

                    tileView.transform.localPosition = CalculateTilePosition(x, y);
                    tileView.transform.localScale = Vector3.one * CalculateTileScale();
                    
                    tileView.SetHidden(instant: true);
                    _boardTiles[x, y] = tileView;
                }
            }
        }

        public Vector2Int? GetTileCoordsAtPositionOrNull(Vector2 worldPosition)
        {
            Vector2 localPosition = worldPosition - _selfTransform.position.ToXY();
            
            Vector2 playAreaSize = CalculatePlayAreaSize();

            Bounds playBounds = new Bounds(Vector3.zero, playAreaSize);
            if (playBounds.Contains(localPosition) == false)
            {
                return null;
            }

            float tileScale = CalculateTileScale();
            Vector2 scaledPosition = (playAreaSize / 2 + localPosition) / tileScale;

            return scaledPosition.FloorToInt();
        }
        
        private Vector2 CalculateTilePosition(int x, int y)
        {
            float tileScale = CalculateTileScale();
            Vector2 playAreaSize = CalculatePlayAreaSize();
            
            return -playAreaSize / 2 + (new Vector2(x, y) + _tileSize / 2) * tileScale;
        }

        private Vector2 CalculatePlayAreaSize()
        {
            Vector2 gameZoneSize = _gameZoneCollider.size;
            float scale = Mathf.Min(gameZoneSize.x / _board.Width, gameZoneSize.y / _board.Height);

            Vector2 playArea = new Vector2(_board.Width, _board.Height) * scale;
            return playArea;
        }

        private float CalculateTileScale()
        {
            int boardWidth = _board.Width;
            int boardHeight = _board.Height;

            Vector2 gameZoneSize = _gameZoneCollider.size;
            float gameZoneWidth = gameZoneSize.x;
            float gameZoneHeight = gameZoneSize.y;

            float maxPossibleTileWidth = gameZoneWidth / boardWidth;
            float maxPossibleTileHeight = gameZoneHeight / boardHeight;

            float targetWidthAndHeight = Mathf.Min(maxPossibleTileWidth, maxPossibleTileHeight);
            return targetWidthAndHeight;
        }

        private void OnDestroy()
        {
            _board.SafeTileRevealed -= BoardOnSafeTileRevealed;
            _board.CascadeTileRevealed -= BoardOnCascadeTileRevealed;
            _board.TileFlagged -= BoardOnTileFlagged;
            _board.TileUnflagged -= BoardOnTileUnflagged;
            _board.MineHit -= BoardOnMineHit;
        }
    }
}