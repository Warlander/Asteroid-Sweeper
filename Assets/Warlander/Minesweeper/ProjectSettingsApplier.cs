using DG.Tweening;
using Zenject;

namespace Warlander.Minesweeper
{
    public class ProjectSettingsApplier : IInitializable
    {
        [Inject] private GameConfig _gameConfig;
        
        void IInitializable.Initialize()
        {
            DOTween.SetTweensCapacity(_gameConfig.TweensCapacity, _gameConfig.SequencesCapacity);
        }
    }
}