using UnityEngine;
using UnityEngine.UI;

namespace Warlander.Minesweeper.Utils
{
    [RequireComponent(typeof(Button))]
    public class GameQuitButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void Start()
        {
            _button.onClick.AddListener(OnQuitClick);
        }

        private void OnQuitClick()
        {
            Application.Quit();
        }
    }
}