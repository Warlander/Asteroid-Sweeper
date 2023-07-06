using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Warlander.Minesweeper.AutoPlay;
using Warlander.Minesweeper.GameCore;
using Warlander.Minesweeper.Levels;
using Warlander.Minesweeper.Utils;
using Zenject;

namespace Warlander.Minesweeper.GameUI
{
    public class GameUIController : WarlanderBehaviour
    {
        [Inject] private AutoPlayer _autoPlayer;
        [Inject] private Board _board;
        [Inject] private ZenjectSceneLoader _zenjectSceneLoader;
        [Inject] private GameLauncher _gameLauncher;
        [Inject] private Level _currentLevel;
        [Inject] private GameConfig _gameConfig;

        [SerializeField] private Transform _savingGraceRoot;

        [SerializeField] private UIPanel _gameLostPanel;
        [SerializeField] private UIPanel _gameWonPanel;
        
        [SerializeField] private Button _backToMenuButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private TMP_Text _minesRemainingText;

        [SerializeField] private Button _autoMoveButton;
        [SerializeField] private TMP_Text _autoMoveButtonText;
        [SerializeField] private Toggle _autoPlayToggle;
        [SerializeField] private TMP_Text _autoPlayButtonText;
        [SerializeField] private Color _errorButtonTextColor = new Color(1, 0.2f, 0.2f);
        [SerializeField] private float _errorButtonTime = 0.2f;

        private Coroutine _autoPlayCoroutine;
        
        private void Start()
        {
            _minesRemainingText.text = _board.RemainingMines.ToString();
            
            _savingGraceRoot.gameObject.SetActive(false);
            
            _board.SavingGraceTriggered += BoardOnSavingGraceTriggered;
            _board.BoardStateChanged += BoardOnBoardStateChanged;
            _board.TileFlagged += BoardOnTileFlagged;
            _board.TileUnflagged += BoardOnTileUnflagged;
            
            _backToMenuButton.onClick.AddListener(OnBackToMenuClicked);
            _restartButton.onClick.AddListener(OnRestartClicked);
            _autoMoveButton.onClick.AddListener(OnAutoMoveClicked);
            _autoPlayToggle.onValueChanged.AddListener(OnAutoPlayClicked);
        }

        private void OnAutoMoveClicked()
        {
            bool madeMove = _autoPlayer.MakeMove();
            if (madeMove == false)
            {
                _autoMoveButtonText.DOComplete();
                _autoMoveButtonText.DOColor(_errorButtonTextColor, _errorButtonTime)
                    .SetLoops(2, LoopType.Yoyo);
            }
        }

        private void OnAutoPlayClicked(bool toggled)
        {
            if (toggled)
            {
                _autoPlayCoroutine = StartCoroutine(AutoPlayCoroutine());
            }
            else if (_autoPlayCoroutine != null)
            {
                StopCoroutine(_autoPlayCoroutine);
            }
        }

        private IEnumerator AutoPlayCoroutine()
        {
            int movesMade = 0;
            while (_autoPlayer.MakeMove())
            {
                movesMade++;
                yield return new WaitForSeconds(_gameConfig.AutoPlayDelayBetweenMoves);
            }

            if (movesMade == 0)
            {
                _autoPlayButtonText.DOComplete();
                _autoPlayButtonText.DOColor(_errorButtonTextColor, _errorButtonTime)
                    .SetLoops(2, LoopType.Yoyo);
            }
            _autoPlayToggle.isOn = false;
        }
        
        private void BoardOnTileUnflagged(int x, int y)
        {
            _minesRemainingText.text = _board.RemainingMines.ToString();
        }

        private void BoardOnTileFlagged(int x, int y)
        {
            _minesRemainingText.text = _board.RemainingMines.ToString();
        }

        private void OnBackToMenuClicked()
        {
            _zenjectSceneLoader.LoadScene(SceneNames.MainMenuScene);
        }

        private void OnRestartClicked()
        {
            _gameLauncher.LaunchLevel(_currentLevel);
        }

        private void BoardOnBoardStateChanged(BoardState state)
        {
            if (state == BoardState.Won)
            {
                RunAfter(1f, () => _gameWonPanel.Show());
            }
            else if (state == BoardState.Lost)
            {
                RunAfter(1f, () => _gameLostPanel.Show());
            }
        }

        private void BoardOnSavingGraceTriggered()
        {
            _savingGraceRoot.DOKill();
            
            _savingGraceRoot.localScale = Vector3.zero;
            _savingGraceRoot.gameObject.SetActive(true);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_savingGraceRoot.DOScale(1, 0.5f).SetEase(Ease.OutBack));
            sequence.AppendInterval(1f);
            sequence.Append(_savingGraceRoot.DOScale(0, 0.5f).SetEase(Ease.InBack));
            sequence.AppendCallback(() => _savingGraceRoot.gameObject.SetActive(false));
        }

        private void OnDestroy()
        {
            _board.SavingGraceTriggered -= BoardOnSavingGraceTriggered;
            _board.BoardStateChanged -= BoardOnBoardStateChanged;

            _autoMoveButtonText.DOKill();
            _autoPlayButtonText.DOKill();
        }
    }
}