using ChessLibrary.Main_units;

namespace ChessLibrary.Factories
{
    /// <summary>
    /// Creating all types of figures.
    /// </summary>
    public abstract class FiguresFactory
    {
        protected SideEnum factorySide;
        protected DirectionOfMovementEnum pawnDirection;
        public abstract Figure CreateKing();
        public abstract Figure CreateQueen();
        public abstract Figure CreateRook();
        public abstract Figure CreateBishop();
        public abstract Figure CreateKnight();
        public abstract Figure CreatePawn();

        public void SetFactorySide(SideEnum side) => factorySide = side;
        /// <summary>
        /// Pawn movement direction. It depends by side.
        /// </summary>
        public void SetPawnMovementDirection(DirectionOfMovementEnum direction) => pawnDirection = direction;
    }
}
