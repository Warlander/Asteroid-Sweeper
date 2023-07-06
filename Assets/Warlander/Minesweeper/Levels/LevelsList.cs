using UnityEngine;
using Zenject;

namespace Warlander.Minesweeper.Levels
{
    public class LevelsList : MonoBehaviour
    {
        [Inject] private IInstantiator _instantiator;
        [Inject] private LevelsManager _levelsManager;
        [Inject] private GameLauncher _gameLauncher;
        
        [SerializeField] private LevelListItem _itemPrefab;
        [SerializeField] private Transform _content;

        public void Start()
        {
            foreach (LevelJson level in _levelsManager.AllLevels)
            {
                LevelListItem levelItem = _instantiator.InstantiatePrefabForComponent<LevelListItem>(_itemPrefab, _content);
                levelItem.Set(level);
                levelItem.gameObject.SetActive(true);
            }
        }
    }
}