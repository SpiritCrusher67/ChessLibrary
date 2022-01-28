using ChessLibrary.Main_units;

namespace ChessLibrary.Movement_Behaviors
{
    /// <summary>
    /// It combine two behaviors. Properties throws NotImplementedException.
    /// </summary>
    public class CombinedMovingBehavior : IMovementBehavior
    {
        IMovementBehavior firstMovingBehavior;
        IMovementBehavior secondMovingBehavior;

        public CombinedMovingBehavior(IMovementBehavior firstBehavior, IMovementBehavior secondBehavior)
        {
            firstMovingBehavior = firstBehavior;
            secondMovingBehavior = secondBehavior;
        }

        bool IMovementBehavior.IsSingleMoving => throw new NotImplementedException();

        int IMovementBehavior.BoardDimension => throw new NotImplementedException();

        public IEnumerable<Field> GetAvaliableMoves(Field[,] fields, (int y, int x) currentPosition, SideEnum currentSide) =>
            firstMovingBehavior.GetAvaliableMoves(fields, currentPosition, currentSide)
            .Concat(secondMovingBehavior.GetAvaliableMoves(fields, currentPosition, currentSide));
    }
}
