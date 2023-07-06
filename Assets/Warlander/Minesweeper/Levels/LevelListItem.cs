using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Warlander.Minesweeper.Levels
{
    public class LevelListItem : MonoBehaviour
    {
        [Inject] private GameLauncher _gameLauncher;
        
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private Button _button;

        public LevelJson Level => _level;
        
        private LevelJson _level;
        
        public void Set(LevelJson level)
        {
            _level = level;

            _levelText.text = level.GetTextDescription();
            
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _gameLauncher.LaunchLevel(_level.ToLevel());
        }
    }
}