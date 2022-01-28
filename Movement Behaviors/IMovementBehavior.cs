using ChessLibrary.Main_units;
using ChessLibrary.Takeing_Behaviors;

namespace ChessLibrary.Movement_Behaviors
{
    /// <summary>
    /// Represents different strategies for figure movements
    /// </summary>
    public interface IMovementBehavior
    {
        public IEnumerable<Field> GetAvaliableMoves(Field[,] fields, (int y, int x) currentPosition, SideEnum currentSide);
        public void MakeAMove(Field from, Field to, ITakeingBehavior takeingBehavior, Field[,] fields);
        /// <summary>
        /// Represents can move on one field at the time
        /// </summary>
        public bool IsSingleMoving { get; }
        /// <summary>
        /// It needs for iterations on each line of board
        /// </summary>
        protected int BoardDimension { get; }
    }
}
