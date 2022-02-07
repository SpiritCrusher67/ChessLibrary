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

        public override IEnumerable<Field>? GetAvaliableFields(Field field)
        {
            var figure = field.Figure;
            if (figure?.Side != CurrentTurnSide.CurrentSide) return null;

            return CurrentTurnSide.GetAllAvaliableFields(_fields)[field.Figure!];
        }

        public override Side CurrentTurnSide
        {
            get => currentTurnSide;
            set
            {
                if (currentTurnSide != null)
                    currentTurnSide.IsMyTurn = false;
                currentTurnSide = value;
                currentTurnSide.IsMyTurn = true;
            }
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

        protected override void InitializeFieldsArray()
        {
            _fields = new Field[8, 8];
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    _fields[y, x] = new Field(y, x);
                }
            }
        }

        protected override IList<Figure> WhiteFiguresPlacement(FiguresFactory factory)
        {
            factory.SetFactorySide(SideEnum.White);
            factory.SetPawnMovementDirection(DirectionOfMovementEnum.UP);
            return PlaceFiguresAtSide(factory, 0, 1);
        }
        protected override IList<Figure> BlackFiguresPlacement(FiguresFactory factory)
        {
            factory.SetFactorySide(SideEnum.Black);
            factory.SetPawnMovementDirection(DirectionOfMovementEnum.DOWN);
            return PlaceFiguresAtSide(factory, 7, 6);
        }

        protected override void SidesInitialization(IList<Figure> whiteFigures, IList<Figure> blackFigures)
        {
            var whiteKing = whiteFigures.First(f => f.Name == "King");
            var blackKing = blackFigures.First(f => f.Name == "King");

            WhiteSide = new Side(SideEnum.White, whiteFigures, whiteKing);
            WhiteSide.IsMyTurn = true;
            BlackSide = new Side(SideEnum.Black, blackFigures, blackKing);
            BlackSide.IsMyTurn = false;

            WhiteSide.SetEnemySide(BlackSide);
            BlackSide.SetEnemySide(WhiteSide);

            CurrentTurnSide = WhiteSide;
        }

        protected override void EventsInitialization()
        {
            MoveHasMakedEvent += (Field from, Field to) => CurrentTurnSide = (CurrentTurnSide == WhiteSide) ? BlackSide : WhiteSide;

            MoveHasMakedEvent += (Field from, Field to) =>
            {
                if (CurrentTurnSide == WhiteSide)
                {
                    if (BlackSide.GetAllAvaliableFields(_fields).SelectMany(pair => pair.Value.Select(f => f.Figure)).ToList().Contains(WhiteSide.King))
                        OnCheckHasSeted(BlackSide, WhiteSide);
                }
                else if (WhiteSide.GetAllAvaliableFields(_fields).SelectMany(pair => pair.Value.Select(f => f.Figure)).ToList().Contains(BlackSide.King))
                    OnCheckHasSeted(WhiteSide, BlackSide);
            };

            MoveHasMakedEvent += (Field from, Field to) =>
            {
                if (CurrentTurnSide.GetAllAvaliableFields(_fields).All(pair => pair.Value.Count == 0))
                    OnCheckMateHasSeted(CurrentTurnSide);
            };

            takeingBehavior.FigureHasTakedEvent += (f) => { if (f.Side == SideEnum.White) WhiteSide.RemoveFigure(f); else BlackSide.RemoveFigure(f); };
        }

        private IList<Figure> PlaceFiguresAtSide(FiguresFactory figuresFactory, int figuresY, int pawnsY)
        {
            var placedFigures = new List<Figure>();

            SetSingleFigure(figuresY, 0, figuresFactory.CreateRook(), placedFigures);
            SetSingleFigure(figuresY, 7, figuresFactory.CreateRook(), placedFigures);

            SetSingleFigure(figuresY, 1, figuresFactory.CreateKnight(), placedFigures);
            SetSingleFigure(figuresY, 6, figuresFactory.CreateKnight(), placedFigures);

            SetSingleFigure(figuresY, 2, figuresFactory.CreateBishop(), placedFigures);
            SetSingleFigure(figuresY, 5, figuresFactory.CreateBishop(), placedFigures);

            SetSingleFigure(figuresY, 3, figuresFactory.CreateKing(), placedFigures);
            SetSingleFigure(figuresY, 4, figuresFactory.CreateQueen(), placedFigures);

            SetLineOfFigures(pawnsY, figuresFactory, placedFigures);

            return placedFigures;

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
    }
}
