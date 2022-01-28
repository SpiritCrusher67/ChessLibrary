using ChessLibrary.Main_units;

namespace ChessLibrary.Movement_Behaviors
{
    /// <summary>
    /// Represents different strategy for figure that can move horizontally and vertically
    /// </summary>
    public class HorizontalAndVerticalMovementsBehavior : IMovementBehavior
    {
        bool _isSingleMoving;
        int _boardDimension;
        public bool IsSingleMoving => _isSingleMoving;

        int IMovementBehavior.BoardDimension => _boardDimension;

        public HorizontalAndVerticalMovementsBehavior(bool isSingleMoving = false, int customBoardDimension = 8)
        {
            _isSingleMoving = isSingleMoving;
            _boardDimension = customBoardDimension;
        }

        public IEnumerable<Field> GetAvaliableMoves(Field[,] fields, (int y, int x) currentPosition, SideEnum currentSide)
        {
            var result = new List<Field>();

            if (IsSingleMoving)
            {
                var lowY = currentPosition.y - 1 > 0 ? currentPosition.y - 1 : 0;
                var highY = currentPosition.y + 1 < _boardDimension ? currentPosition.y + 1 : _boardDimension - 1;
                var lowX = currentPosition.x - 1 > 0 ? currentPosition.x - 1 : 0;
                var highX = currentPosition.x + 1 < _boardDimension ? currentPosition.x + 1 : _boardDimension - 1;
                AddingYFields(lowY);
                AddingYFields(highY);
                AddingXFields(lowX);
                AddingXFields(highX);
            }
            else
            {
                for (int y = currentPosition.y - 1; y >= 0; y--)
                    if (AddingYFields(y)) break;
                for (int y = currentPosition.y + 1; y < _boardDimension; y++)
                    if (AddingYFields(y)) break;
                for (int x = currentPosition.x - 1; x >= 0; x--)
                    if (AddingXFields(x)) break;
                for (int x = currentPosition.x + 1; x < _boardDimension; x++)
                    if (AddingXFields(x)) break;
            }

            bool AddingYFields(int y)
            {
                if (!fields[y, currentPosition.x].IsEmpty)
                {
                    if (fields[y, currentPosition.x].FigureSide != currentSide)
                        result.Add(fields[y, currentPosition.x]);
                    return true;
                }
                else result.Add(fields[y, currentPosition.x]);
                return false;
            }
            bool AddingXFields(int x)
            {
                if (!fields[currentPosition.y, x].IsEmpty)
                {
                    if (fields[currentPosition.y, x].FigureSide != currentSide)
                        result.Add(fields[currentPosition.y, x]);
                    return true;
                }
                else result.Add(fields[currentPosition.y, x]);
                return false;
            }

            return result;
        }
    }
}
