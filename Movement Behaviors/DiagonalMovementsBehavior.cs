using ChessLibrary.Main_units;

namespace ChessLibrary.Movement_Behaviors
{
    /// <summary>
    /// Represents different strategy for figure that can move diagonally
    /// </summary>
    public class DiagonalMovementsBehavior : IMovementBehavior
    {
        bool _isSingleMoving;
        int _boardDimension;
        public bool IsSingleMoving => _isSingleMoving;

        int IMovementBehavior.BoardDimension => _boardDimension;

        public DiagonalMovementsBehavior(bool isSingleMoving = false, int customBoardDimension = 8)
        {
            _isSingleMoving = isSingleMoving;
            _boardDimension = customBoardDimension;
        }

        public IEnumerable<Field> GetAvaliableMoves(Field[,] fields, (int y, int x) currentPosition, SideEnum currentSide)
        {
            List<Func<int, int, bool>> exps = new List<Func<int, int, bool>>()
            {
                (x,y) => x + y == currentPosition.x + currentPosition.y && y > currentPosition.y,
                (x,y) => x + y == currentPosition.x + currentPosition.y && y < currentPosition.y,
                (x,y) => x - y == currentPosition.x - currentPosition.y && y > currentPosition.y,
                (x,y) => x - y == currentPosition.x - currentPosition.y && y < currentPosition.y
            };

            var result = new List<Field>();

            if (IsSingleMoving)
            {
                var low = currentPosition.y - 1 > 0 ? currentPosition.y - 1 : 0;
                var high = currentPosition.y + 1 < _boardDimension ? currentPosition.y + 1 : _boardDimension - 1;
                HorizontalCycle(low);
                HorizontalCycle(high);
            }
            else
            {
                for (int y = currentPosition.y; y >= 0; y--)
                    HorizontalCycle(y);
                for (int y = currentPosition.y; y < _boardDimension; y++)
                    HorizontalCycle(y);
            }

            void HorizontalCycle(int y)
            {
                for (int x = 0; x < _boardDimension; x++)
                    for (int e = 0; e < exps.Count(); e++)
                    {
                        if (exps.ElementAt(e).Invoke(x, y))
                        {
                            if (!fields[y, x].IsEmpty)
                            {
                                exps.RemoveAt(e);
                                if (fields[y, x].Figure!.Side == currentSide)
                                    break;
                            }
                            result.Add(fields[y, x]);
                            break;
                        }
                    }
            }
            return result;
        }
    }
}
