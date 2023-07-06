using System.Linq;
using UnityEngine;
using Zenject;

namespace Warlander.Minesweeper.Levels
{
    public class LevelsManager
    {
        private LevelJson[] _levels;
        
        public LevelJson[] AllLevels {
            get
            {
                if (_levels == null)
                {
                    _levels = LoadLevels();
                }

                return _levels;
            }
        }

        private LevelJson[] LoadLevels()
        {
            TextAsset[] levelsTextAssets = Resources.LoadAll<TextAsset>("Minesweeper/Levels");
            LevelJson[] validLevels = levelsTextAssets
                .Select(asset => JsonUtility.FromJson<LevelJson>(asset.text))
                .Where(level => level.Validate())
                .ToArray();

            return validLevels;
        }
    }
}