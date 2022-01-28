using ChessLibrary.Main_units;

namespace ChessLibrary.Takeing_Behaviors
{
    /// <summary>
    /// Needs for takeing figure by other figure. It can take figure and contain taked figures.
    /// </summary>
    public interface ITakeingBehavior
    {
        public void TakeFigureFromField(Field field);
        public IEnumerable<Figure> GetTakedFigures { get; }

        public event Action<Figure> FigureHasTakedEvent;
    }
}
