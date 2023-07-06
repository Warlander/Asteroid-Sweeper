using System.ComponentModel;
using UnityEngine.SceneManagement;
using Warlander.Minesweeper.Levels;
using Zenject;

namespace Warlander.Minesweeper
{
    public class GameLauncher
    {
        [Inject] private ZenjectSceneLoader _zenjectSceneLoader;
        
        public void LaunchLevel(Level level)
        {
            _zenjectSceneLoader.LoadScene(SceneNames.GameScene, LoadSceneMode.Single, container =>
            {
                container.Bind<Level>().FromInstance(level).AsSingle();
            });
        }
    }
}