namespace ChessLibrary.Main_units
{
    /// <summary>
    /// Side enum represents white or black side
    /// </summary>
    public enum SideEnum
    {
        White,
        Black
    }
    /// <summary>
    /// Statement of figure. Needs for pawns (if theq DONTMOVED then they can move at 2 fields forvard), 
    /// for pawns takeing on the pass and for king castling.
    /// </summary>
    public enum FigureStateEnum
    {
        NONE,
        DONTMOVED,
        MOVED
    }
    /// <summary>
    /// Only for pawns. Needs for calculations Y-movements of pawns.
    /// </summary>
    public enum DirectionOfMovementEnum
    {
        UP = 1,
        DOWN = -1,
    }
}
