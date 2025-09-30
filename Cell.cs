namespace TicTacToe
{
    public enum Cell
    {
        Empty = 0,
        X = 1,
        O = 2,
    }

    public static class SpaceExtensions
    {
        // Returns true if the cell is not empty.
        public static bool IsSet(this Cell s) => s != Cell.Empty;

        // Returns true if the cell is empty.
        public static bool IsEmpty(this Cell s) => s == Cell.Empty;

        // Gives the marker of the opposite player if a "set" cell is passed, else the empty value.
        public static Cell Inverse(this Cell s) =>
            s switch
            {
                Cell.X => Cell.O,
                Cell.O => Cell.X,
                _ => Cell.Empty,
            };
    }
}
