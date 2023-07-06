using UnityEngine;
using Warlander.Minesweeper;
using Warlander.Minesweeper.Levels;
using Zenject;

namespace Warlander.Minesweeper.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _gameConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<LevelsManager>().AsSingle();
            
            Container.Bind<GameLauncher>().AsSingle();

            Container.Bind<RandomLevelGenerator>().AsSingle();

            Container.Bind<GameConfig>().FromInstance(_gameConfig);
            Container.BindInterfacesAndSelfTo<ProjectSettingsApplier>().AsSingle().NonLazy();
        }
    }
}