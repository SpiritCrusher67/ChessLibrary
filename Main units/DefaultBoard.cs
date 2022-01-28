using ChessLibrary.Movement_Behaviors;
using ChessLibrary.Takeing_Behaviors;
using ChessLibrary.Factories;

namespace ChessLibrary.Main_units
{
    /// <summary>
    /// Default board (8x8) for one player.
    /// </summary>
    public class DefaultBoard : Board
    {
        public DefaultBoard(ITakeingBehavior takeingBehavior, FiguresFactory figuresFactory) : base(takeingBehavior, figuresFactory) { }

        public override IEnumerable<Field> GetAvaliableFields(Field field)
        {
            throw new NotImplementedException();
        }

        protected override void InitializeBoard(ITakeingBehavior takeingBehavior, FiguresFactory figuresFactory)
        {
            #region Field list initialization
            _fields = new Field[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    _fields[i, j] = new Field(j, i);
                }
            }
            #endregion

            var whiteFigures = new List<Figure>();
            var blackFigures = new List<Figure>();

            figuresFactory.SetFactorySide(SideEnum.White);
            figuresFactory.SetPawnMovementDirection(DirectionOfMovementEnum.UP);
            var whiteKing = figuresFactory.CreateKing();

            #region White figures placement
            SetSingleFigure(0, 0, figuresFactory.CreateRook(), whiteFigures);
            SetSingleFigure(0, 7, figuresFactory.CreateRook(), whiteFigures);

            SetSingleFigure(0, 1, figuresFactory.CreateKnight(), whiteFigures);
            SetSingleFigure(0, 6, figuresFactory.CreateKnight(), whiteFigures);

            SetSingleFigure(0, 2, figuresFactory.CreateBishop(), whiteFigures);
            SetSingleFigure(0, 5, figuresFactory.CreateBishop(), whiteFigures);

            SetSingleFigure(0, 3, whiteKing, whiteFigures);
            SetSingleFigure(0, 4, figuresFactory.CreateQueen(), whiteFigures);

            SetLineOfFigures(1, figuresFactory, whiteFigures);
            #endregion

            figuresFactory.SetFactorySide(SideEnum.Black);
            figuresFactory.SetPawnMovementDirection(DirectionOfMovementEnum.DOWN);
            var blackKing = figuresFactory.CreateKing();

            #region Black figures placement
            SetSingleFigure(7, 0, figuresFactory.CreateRook(), blackFigures);
            SetSingleFigure(7, 7, figuresFactory.CreateRook(), blackFigures);

            SetSingleFigure(7, 1, figuresFactory.CreateKnight(), blackFigures);
            SetSingleFigure(7, 6, figuresFactory.CreateKnight(), blackFigures);

            SetSingleFigure(7, 2, figuresFactory.CreateBishop(), blackFigures);
            SetSingleFigure(7, 5, figuresFactory.CreateBishop(), blackFigures);

            SetSingleFigure(7, 3, blackKing, blackFigures);
            SetSingleFigure(7, 4, figuresFactory.CreateQueen(), blackFigures);

            SetLineOfFigures(6, figuresFactory, blackFigures);
            #endregion

            #region Initialization MoveHasMakedEvent listeners
            MoveHasMakedEvent += (Field from, Field to) => CurrentTurnSide = (CurrentTurnSide == WhiteSide) ? BlackSide : WhiteSide;
            MoveHasMakedEvent += (Field from, Field to) =>
            {
                if (CurrentTurnSide == WhiteSide)
                    if (BlackSide.GetAllAvaliableFields(_fields).SelectMany(pair => pair.Value.Select(f => f.Figure)).ToList().Contains(WhiteSide.King))
                        OnCheckHasSeted(BlackSide, WhiteSide);
                    else
                if (WhiteSide.GetAllAvaliableFields(_fields).SelectMany(pair => pair.Value.Select(f => f.Figure)).ToList().Contains(BlackSide.King))
                        OnCheckHasSeted(WhiteSide, BlackSide);
            };
            MoveHasMakedEvent += (Field from, Field to) =>
            {
                if (CurrentTurnSide.GetAllAvaliableFields(_fields).All(pair => pair.Value.Count == 0))
                    OnCheckMateHasSeted(CurrentTurnSide);
            };
            #endregion

            #region Initialization FigureHasTakedEvent listeners
            takeingBehavior.FigureHasTakedEvent += (f) => { if (f.Side == SideEnum.White) WhiteSide.RemoveFigure(f); else BlackSide.RemoveFigure(f); };
            #endregion

            #region Sides initialization
            WhiteSide = new Side(SideEnum.White, whiteFigures, whiteKing);
            WhiteSide.IsMyTurn = true;
            BlackSide = new Side(SideEnum.Black, blackFigures, blackKing);
            BlackSide.IsMyTurn = false;
            CurrentTurnSide = WhiteSide;

            WhiteSide.SetEnemySide(BlackSide);
            BlackSide.SetEnemySide(WhiteSide);
            #endregion


            #region Utility methods
            void SetSingleFigure(int y, int x, Figure figure, List<Figure> figuresList)
            {
                _fields[y, x].Figure = figure;
                figuresList.Add(figure);
            }

            void SetLineOfFigures(int y, FiguresFactory figuresFactory, List<Figure> figuresList)
            {
                for (int i = 0; i < 8; i++)
                {
                    var figure = figuresFactory.CreatePawn();
                    _fields[y, i].Figure = figure;
                    figuresList.Add(figure);
                }
            }
            #endregion
        }

        public override bool MakeMove(Field from, Field to)
        {
            if (from.IsEmpty || from.Figure!.Side != currentTurnSide.CurrentSide) return false;

            var avaliableFields = CurrentTurnSide.GetAllAvaliableFields(_fields)[from.Figure];

            if (avaliableFields.Contains(to))
            {
                from.Figure.MakeAMove(from, to, takeingBehavior, _fields);
                OnMoveHasMaked(from, to);
                return true;
            }
            return false;
        }
    }
}
