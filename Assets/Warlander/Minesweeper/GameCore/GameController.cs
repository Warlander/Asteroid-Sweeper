using System;
using UnityEngine;
using Warlander.Minesweeper.Levels;
using Zenject;

namespace Warlander.Minesweeper.GameCore
{
    public class GameController : MonoBehaviour
    {
        [Inject(Id = InjectIds.GameCamera)] private Camera _gameCamera;

        [Inject] private Board _board;
        [Inject] private BoardView _boardView;

        [Inject] private Level _currentLevel;

        private void Start()
        {
            _board.InitialiseBoard(_currentLevel);
            _boardView.CreateBoard();
        }

        private void Update()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            if (_board.BoardState != BoardState.Playing)
            {
                return;
            }
            
            bool leftClick = Input.GetMouseButtonDown(0);
            bool rightClick = Input.GetMouseButtonDown(1);

            if (leftClick || rightClick)
            {
                Ray ray = _gameCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                if (hit.transform == _boardView.transform)
                {
                    Vector2Int? hitTile = _boardView.GetTileCoordsAtPositionOrNull(hit.point);
                    if (hitTile.HasValue)
                    {
                        if (leftClick)
                        {
                            ProcessRevealClick(hitTile.Value);
                        }
                        else if (rightClick)
                        {
                            ProcessFlagClick(hitTile.Value);
                        }
                    }
                }
            }
        }

        private void ProcessRevealClick(Vector2Int tile)
        {
            int x = tile.x;
            int y = tile.y;
            _board.RevealTile(x, y);
        }

        private void ProcessFlagClick(Vector2Int tile)
        {
            int x = tile.x;
            int y = tile.y;
            _board.FlagTile(x, y);
        }
    }
}