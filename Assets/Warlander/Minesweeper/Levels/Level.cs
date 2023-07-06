namespace Warlander.Minesweeper.Levels
{
    public class Level
    {
        public bool[,] Mines => _mines;
        public int Width => _mines.GetLength(0);
        public int Height => _mines.GetLength(1);
        
        private bool[,] _mines;

        public Level(bool[,] mines)
        {
            _mines = mines;
        }
    }
}