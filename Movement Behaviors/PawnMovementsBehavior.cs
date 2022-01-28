using ChessLibrary.Main_units;

namespace ChessLibrary.Movement_Behaviors
{
    /// <summary>
    /// Represents strategy for pawn movements
    /// </summary>
    public class PawnMovementsBehavior : IMovementBehavior
    {
        private int _dirMultiplier;
        int _boardDimension;
        public bool IsSingleMoving => true;

        int IMovementBehavior.BoardDimension => _boardDimension;

        public PawnMovementsBehavior(DirectionOfMovementEnum direction, int customBoardDimension = 8)
        {
            _dirMultiplier = (int)direction;
            _boardDimension = customBoardDimension;
        }

        public IEnumerable<Field> GetAvaliableMoves(Field[,] fields, (int y, int x) currentPosition, SideEnum currentSide)
        {
            var result = new List<Field>();
            var figure = fields[currentPosition.y, currentPosition.x].Figure;

            if (figure!.State == FigureStateEnum.DONTMOVED &&
                currentPosition.y + (2 * _dirMultiplier) > 0 &&
                currentPosition.y + (2 * _dirMultiplier) < _boardDimension &&
                fields[currentPosition.y + _dirMultiplier, currentPosition.x].IsEmpty)
                AddYFields(currentPosition.y + 2 * _dirMultiplier);

            if (currentPosition.y + _dirMultiplier > 0 && currentPosition.y + _dirMultiplier < _boardDimension)
                AddYFields(currentPosition.y + _dirMultiplier);

            if (currentPosition.y + _dirMultiplier > 0 && currentPosition.y + _dirMultiplier < _boardDimension)
            {
                if (currentPosition.x - 1 > 0)
                    AddEnemyesFields(currentPosition.y + _dirMultiplier, currentPosition.x - 1);
                if (currentPosition.x + 1 < _boardDimension)
                    AddEnemyesFields(currentPosition.y + _dirMultiplier, currentPosition.x + 1);
            }

            void AddYFields(int y)
            {
                if (fields[y, currentPosition.x].IsEmpty)
                    result.Add(fields[y, currentPosition.x]);
            }
            void AddEnemyesFields(int y, int x)
            {
                if (!fields[y, x].IsEmpty)
                {
                    if (fields[y, x].FigureSide != currentSide)
                        result.Add(fields[y, x]);
                }
                if (y + _dirMultiplier > 0 && y + _dirMultiplier < _boardDimension && fields[y + _dirMultiplier, x].IsEmpty && !fields[y + _dirMultiplier * _dirMultiplier, x].IsEmpty)
                    if (fields[y + _dirMultiplier * _dirMultiplier, x].FigureSide != currentSide && fields[y + _dirMultiplier * _dirMultiplier, x].Figure!.State == FigureStateEnum.MOVED)
                        result.Add(fields[y, x]);
            }

            return result;
        }
    }
}
