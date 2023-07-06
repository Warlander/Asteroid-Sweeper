using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Warlander.Minesweeper.GameUI
{
    public class GameWonPanel : MonoBehaviour
    {
        [Inject] private ZenjectSceneLoader _zenjectSceneLoader;
        
        [SerializeField] private Button _backToMenuButton;
        
        private void Start()
        {
            _backToMenuButton.onClick.AddListener(OnBackToMenuClicked);
        }

        private void OnBackToMenuClicked()
        {
            _zenjectSceneLoader.LoadScene(SceneNames.MainMenuScene);
        }
    }
}