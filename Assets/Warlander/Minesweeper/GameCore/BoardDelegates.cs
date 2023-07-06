namespace Warlander.Minesweeper.GameCore
{
    public delegate void TileCoordsDelegate(int x, int y);
    
    public delegate void TileCoordsWithOriginDelegate(int x, int y, int originX, int originY);
}