using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Warlander.Minesweeper.Levels
{
    [RequireComponent(typeof(Button))]
    public class RandomLevelButton : MonoBehaviour
    {
        [Inject] private GameLauncher _gameLauncher;
        [Inject] private RandomLevelGenerator _randomLevelGenerator;
        
        [SerializeField] private Button _button;
        [SerializeField] private int _levelWidth;
        [SerializeField] private int _levelHeight;
        [SerializeField] private int _mines;

        private void Start()
        {
            _button.onClick.AddListener(OnRandomLevelButtonClick);
        }

        private void OnRandomLevelButtonClick()
        {
            Level generatedLevel = _randomLevelGenerator.GenerateLevel(_levelWidth, _levelHeight, _mines);
            _gameLauncher.LaunchLevel(generatedLevel);
        }
    }
}