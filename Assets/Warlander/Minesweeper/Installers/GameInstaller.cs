using UnityEngine;
using Warlander.Minesweeper.AutoPlay;
using Warlander.Minesweeper.GameCore;
using Warlander.Minesweeper.Levels;
using Zenject;

namespace Warlander.Minesweeper.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Inject] private RandomLevelGenerator _randomLevelGenerator;

        [SerializeField] private Camera _gameCamera;
        [SerializeField] private BoardView _boardView;
        
        public override void InstallBindings()
        {
            Container.Bind<Camera>().WithId(InjectIds.GameCamera).FromInstance(_gameCamera);
            
            Container.Bind<BoardView>().FromInstance(_boardView);
            Container.Bind<Board>().AsSingle();

            Container.Bind<GameController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

            Container.Bind<AutoPlayer>().AsSingle();

            if (Container.HasBinding<Level>() == false)
            {
                Level testingLevel = _randomLevelGenerator.GenerateLevel(16, 12, 5);
                Container.Bind<Level>().FromInstance(testingLevel).AsSingle();
            }
        }
    }
}