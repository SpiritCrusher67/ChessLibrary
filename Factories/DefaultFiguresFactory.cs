using ChessLibrary.Main_units;
using ChessLibrary.Movement_Behaviors;

namespace ChessLibrary.Factories
{
    public class DefaultFiguresFactory : FiguresFactory
    {
        public override Figure CreateBishop() => new Figure("Bishop", factorySide, new DiagonalMovementsBehavior());
        public override Figure CreateRook() => new Figure("Rook", factorySide, new HorizontalAndVerticalMovementsBehavior(), FigureStateEnum.DONTMOVED);
        public override Figure CreateKnight() => new Figure("Knight", factorySide, new KnightMovementsBehavior());
        public override Figure CreatePawn() => new Figure("Pawn", factorySide, new PawnMovementsBehavior(pawnDirection), FigureStateEnum.DONTMOVED);

        public override Figure CreateQueen()
        {
            var hAndV = new HorizontalAndVerticalMovementsBehavior();
            var diagonal = new DiagonalMovementsBehavior();
            return new Figure("Queen", factorySide, new CombinedMovingBehavior(hAndV, diagonal));
        }
        public override Figure CreateKing()
        {
            var hAndV = new HorizontalAndVerticalMovementsBehavior(true);
            var diagonal = new DiagonalMovementsBehavior(true);
            return new Figure("King", factorySide, new CombinedMovingBehavior(hAndV, diagonal), FigureStateEnum.DONTMOVED);
        }
    }
}
