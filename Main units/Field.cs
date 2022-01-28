
namespace ChessLibrary.Main_units
{
    /// <summary>
    /// Class that represents a singe field in board[Y,X]
    /// That may containts figure 
    /// </summary>
    public class Field
    {
        public Field(int y, int x)
        {
            X = x;
            Y = y;
        }

        public Figure? Figure { get; set; }
        public bool IsEmpty => Figure == null;



        public int X { get; private set; }
        public int Y { get; private set; }

        public override string ToString() => $"Field at y = {Y}, x = {X}, contains: {Figure?.ToString() ?? "nothing"}";

        public static Field FindFieldByFigure(Field[,] fields, Figure figure)
        {
            foreach (var field in fields)
                if (field.Figure == figure) return field;
            throw new Exception("");
        }
    }
}
