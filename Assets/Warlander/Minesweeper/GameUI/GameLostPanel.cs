using UnityEngine;
using UnityEngine.UI;
using Warlander.Minesweeper.Levels;
using Zenject;

namespace Warlander.Minesweeper.GameUI
{
    public class GameLostPanel : MonoBehaviour
    {
        [Inject] private ZenjectSceneLoader _zenjectSceneLoader;
        [Inject] private GameLauncher _gameLauncher;
        [Inject] private Level _currentLevel;
        
        [SerializeField] private Button _backToMenuButton;
        [SerializeField] private Button _restartButton;
        
        private void Start()
        {
            _backToMenuButton.onClick.AddListener(OnBackToMenuClicked);
            _restartButton.onClick.AddListener(OnRestartClicked);
        }
        
        private void OnBackToMenuClicked()
        {
            _zenjectSceneLoader.LoadScene(SceneNames.MainMenuScene);
        }

        private void OnRestartClicked()
        {
            _gameLauncher.LaunchLevel(_currentLevel);
        }
    }
}