using ChessLibrary.Movement_Behaviors;
using ChessLibrary.Takeing_Behaviors;

namespace ChessLibrary.Main_units
{
    /// <summary>
    /// Class that can represents any figure on the board
    /// </summary>
    public sealed class Figure
    {
        IMovementBehavior _movementBehavior;

        public string Name { get; private set; }
        public SideEnum Side { get; private set; }
        public FigureStateEnum State { get; set; }

        public Figure(string name, SideEnum side, IMovementBehavior behavior, FigureStateEnum figureState = FigureStateEnum.NONE)
        {
            Name = name;
            Side = side;
            _movementBehavior = behavior;
            State = figureState;
        }

        public void SetName(string name) => Name = name;

        public IEnumerable<Field> GetAvaliableFields(Field[,] fields, Field? currentField) => (currentField == null) ? new List<Field>() :
            _movementBehavior.GetAvaliableMoves(fields, new(currentField.Y, currentField.X), Side);

        public void MakeAMove(Field from, Field to, ITakeingBehavior takeingBehavior, Field[,] fields) => _movementBehavior.MakeAMove(from, to, takeingBehavior, fields);
    }
}
