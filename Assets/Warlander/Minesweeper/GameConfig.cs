using UnityEngine;

namespace Warlander.Minesweeper
{
    [CreateAssetMenu(menuName = "Minesweeper/GameConfig", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Tooltip("Saving grace is a feature which reveals 3X3 square around the mine and marks it if we hit the mine during first move.")]
        [SerializeField] private bool _savingGraceEnabled = true;

        [Tooltip("Delay between movies in auto play mode with UI.")]
        [SerializeField] private float _autoPlayDelayBetweenMoves = 0.5f;

        [SerializeField] private int _tweensCapacity = 500;
        [SerializeField] private int _sequencesCapacity = 50;
        
        public bool SavingGraceEnabled => _savingGraceEnabled;
        public float AutoPlayDelayBetweenMoves => _autoPlayDelayBetweenMoves;
        public int TweensCapacity => _tweensCapacity;
        public int SequencesCapacity => _sequencesCapacity;
    }
}