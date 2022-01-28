using ChessLibrary.Main_units;

namespace ChessLibrary.Movement_Behaviors
{
    /// <summary>
    /// Represents strategy for knight movements
    /// </summary>
    public class KnightMovementsBehavior : IMovementBehavior
    {
        bool _isSingleMoving;
        int _boardDimension;
        public bool IsSingleMoving => _isSingleMoving;

        int IMovementBehavior.BoardDimension => _boardDimension;

        public KnightMovementsBehavior(bool isSingleMoving = false, int customBoardDimension = 8)
        {
            _isSingleMoving = isSingleMoving;
            _boardDimension = customBoardDimension;
        }

        public IEnumerable<Field> GetAvaliableMoves(Field[,] fields, (int y, int x) currentPosition, SideEnum currentSide)
        {
            var result = new List<Field>();
            List<Action<int, int, Action<int, int>>> exps = new List<Action<int, int, Action<int, int>>>()
            {
                (y, x, a) => { if (y + 2 < _boardDimension && x + 1 < _boardDimension) a(y + 2, x + 1); },
                (y, x, a) => { if (y + 2 < _boardDimension && x - 1 >= 0) a(y + 2, x - 1); },
                (y, x, a) => { if (y - 2 >= 0 && x + 1 < _boardDimension) a(y - 2, x + 1); },
                (y, x, a) => { if (y - 2 >= 0 && x - 1 >= 0) a(y - 2, x - 1); },

                (y, x, a) => { if (y + 1 < _boardDimension && x + 2 < _boardDimension) a(y + 1, x + 2); },
                (y, x, a) => { if (y + 1 < _boardDimension && x - 2 >= 0) a(y + 1, x - 2); },
                (y, x, a) => { if (y - 1 >= 0 && x + 2 < _boardDimension) a(y - 1, x + 2); },
                (y, x, a) => { if (y - 1 >= 0 && x - 2 >= 0) a(y - 1, x - 2); }
            };

            foreach (var e in exps)
                e(currentPosition.y, currentPosition.x, (y, x) => {
                    if (fields[y, x].IsEmpty || fields[y, x].Figure!.Side != currentSide)
                        result.Add(fields[y, x]);
                });

            return result;
        }
    }
}
