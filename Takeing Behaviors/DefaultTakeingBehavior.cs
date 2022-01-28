using ChessLibrary.Main_units;

namespace ChessLibrary.Takeing_Behaviors
{
    public class DefaultTakeingBehavior : ITakeingBehavior
    {
        private List<Figure> takedFigures;

        public DefaultTakeingBehavior()
        {
            takedFigures = new List<Figure>();
        }
        public IEnumerable<Figure> GetTakedFigures => takedFigures;

        public event Action<Figure> FigureHasTakedEvent;

        public void TakeFigureFromField(Field field)
        {
            var figure = field.Figure;
            takedFigures.Add(figure);
            field.Figure = null;

            FigureHasTakedEvent?.Invoke(figure);
        }
    }
}
