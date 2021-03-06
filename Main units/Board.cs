using ChessLibrary.Takeing_Behaviors;
using ChessLibrary.Factories;

namespace ChessLibrary.Main_units
{
	/// <summary>
	/// That thing will controll all the game mechanics
	/// </summary>
	public abstract class Board
	{
		protected Field[,] _fields;
		protected Side currentTurnSide;
		protected ITakeingBehavior takeingBehavior;

		/// <summary>
		/// Invokes after move. First parameter is FROM field, second - TO field.
		/// </summary>
		public event Action<Field, Field> MoveHasMakedEvent;
		/// <summary>
		/// Invokes after move where First side set Check to Second side.
		/// </summary>
		public event Action<Side, Side> CheckHasSetedEvent;
		/// <summary>
		/// Invokes after move where side wins.
		/// </summary>
		public event Action<Side> CheckMateHasSetedEvent;
		/// <summary>
		/// Invokes after takeing figure.
		/// </summary>
		public event Action<Figure> FigureHasTakedEvent
        {
			add { takeingBehavior.FigureHasTakedEvent += value; }
            remove { takeingBehavior.FigureHasTakedEvent -= value; }
		}


		public Board(ITakeingBehavior takeingBehavior, FiguresFactory factory)
		{
			this.takeingBehavior = takeingBehavior;
			InitializeBoard(takeingBehavior, factory);
		}

		public Side WhiteSide { get; protected set; }
		public Side BlackSide { get; protected set; }
		public virtual Side CurrentTurnSide
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
		public Field[,] GetFields() => _fields;
		public IEnumerable<Figure> TakedFigures => takeingBehavior.GetTakedFigures;
		public abstract IEnumerable<Field> GetAvaliableFields(Field field);
		public IEnumerable<Field> GetAvaliableFields((int y, int x) coords) => GetAvaliableFields(_fields[coords.y, coords.x]);

		public abstract bool MakeMove(Field from, Field to);
		public bool MakeMove((int y, int x) from, (int y, int x) to) => MakeMove(_fields[from.y, from.x], _fields[to.y, to.x]);

		protected void OnMoveHasMaked(Field from, Field to) => MoveHasMakedEvent?.Invoke(from, to);
		protected void OnCheckHasSeted(Side from, Side to) => CheckHasSetedEvent?.Invoke(from, to);
		protected void OnCheckMateHasSeted(Side winnerSide) => CheckMateHasSetedEvent?.Invoke(winnerSide);

		protected void InitializeBoard(ITakeingBehavior takeingBehavior, FiguresFactory figuresFactory)
		{
			InitializeFieldsArray();
			SidesInitialization(WhiteFiguresPlacement(figuresFactory), BlackFiguresPlacement(figuresFactory));
			EventsInitialization();
		}

		protected abstract void InitializeFieldsArray();
		protected abstract IList<Figure> WhiteFiguresPlacement(FiguresFactory factory);
		protected abstract IList<Figure> BlackFiguresPlacement(FiguresFactory factory);
		protected abstract void SidesInitialization(IList<Figure> whiteFigures, IList<Figure> blackFigures);
		protected abstract void EventsInitialization();

	}
}
