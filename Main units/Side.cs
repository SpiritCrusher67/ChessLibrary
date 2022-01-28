
namespace ChessLibrary.Main_units
{
    /// <summary>
    /// Represents white or black side. Contains all figures of this side. Can find all avaliable moves depending whose turn is it.
    /// </summary>
    public class Side
    {
        private Side? enemySide;
        private IList<Figure> figures;

        public Side(SideEnum side, IList<Figure> figures, Figure king)
        {
            CurrentSide = side;
            this.figures = figures;
            King = king;
        }

        public SideEnum CurrentSide { get; private set; }
        public Figure King { get; private set; }

        public IEnumerable<Figure> Figures { get => figures; }

        public void RemoveFigure(Figure figure) => figures.Remove(figure);

        public bool IsMyTurn { get; set; }

        public void SetEnemySide(Side enemy)
        {
            if (enemy.CurrentSide == CurrentSide)
                throw new ArgumentException();
            enemySide = enemy;
        }
        /// <summary>
        /// For enemy side returns all moves, but for current side return moves that dont break rules
        /// </summary>
        /// <param name="allFields">All fields on the board</param>
        /// <returns>Dictionary witch all avaliable moves for each figure</returns>
        public Dictionary<Figure, List<Field>> GetAllAvaliableFields(Field[,] allFields)
        {
            var result = new Dictionary<Figure, List<Field>>();
            var fields = new Dictionary<Figure, List<Field>>();

            var allAvaliableFields = Figures.Select(f => new { kye = f, value = f.GetAvaliableFields(allFields, Field.FindFieldByFigure(allFields, f)) });
            allAvaliableFields.ToList().ForEach(x => result.Add(x.kye, x.value.ToList()));

            if (!IsMyTurn) return result;


            foreach (var pair in result)
            {
                var currentField = Field.FindFieldByFigure(allFields, pair.Key);
                currentField.Figure = null;
                var tmpList = new List<Field>();
                foreach (var field in pair.Value)
                {
                    var prevVal = field.Figure;
                    field.Figure = pair.Key;
                    var kingsField = Field.FindFieldByFigure(allFields, King);
                    if (!enemySide!.GetAllAvaliableFields(allFields).Any(p => p.Value.Contains(kingsField)))
                        tmpList.Add(field);
                    field.Figure = prevVal;
                }
                result[pair.Key] = tmpList;
                currentField.Figure = pair.Key;
            }

            return result;
        }
    }
}
