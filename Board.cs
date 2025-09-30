using System.Text;

namespace TicTacToe
{
    public class Board
    {
        // Internal array for the board.
        private Cell[] _cells = new Cell[9];
        // Constants that represent each winning cell combination
        public static readonly (int, int, int)[] Triplets = {
            (0, 1, 2),
            (3, 4, 5),
            (6, 7, 8),
            (0, 3, 6),
            (1, 4, 7),
            (2, 5, 8),
            (0, 4, 8),
            (2, 4, 6),
        };

        // Get the value of the cell at (x, y).
        public Cell GetCell(int x, int y)
        {
            return _cells[Board.XYToX(x, y)];
        }

        // Get the value of the ith index of the internal `Cell` array.
        public Cell GetCell(int i)
        {
            return _cells[i];
        }

        // Try to set the cell at (x, y) to `s`.
        //
        // Returns true if the value was changed successfully.
        public bool SetCell(int x, int y, Cell cell)
        {
            Cell original = GetCell(x, y);

            if (original.IsSet())
            {
                return false;
            }

            _cells[(3 * y) + x] = cell;
            return true;
        }

        // Sets the ith index of the internal `Cell` array.
        //
        // Returns true if the value was changed sucessfully.
        public bool SetCell(int i, Cell cell)
        {
            var (x, y) = Board.XToXY(i);
            return SetCell(x, y, cell);
        }

        // Checks to see if the given cell is a valid selection for the player to take.
        public bool ValidPlayerIndices(int x, int y) =>
            (0 <= x && x < 3) && (0 <= y && y < 3) && GetCell(x, y) == Cell.Empty;

        // Checks to see if the given cell is a valid selection for the player to take.
        public bool ValidPlayerIndices(int i)
        {
            var (x, y) = Board.XToXY(i);
            return ValidPlayerIndices(x, y) && GetCell(i) == Cell.Empty;
        }

        // X <=> XY conversions
        public static int XYToX(int x, int y) => (3 * y) + x;
        public static (int, int) XToXY(int x) => (x % 3, x / 3);

        // Returns `Space.X` or `Space.O` if either is the winner, otherwise returns `Space.Empty`
        public Cell GameOver() =>
            Triplets
                .Select(t =>
                {
                    var (i, j, k) = t;
                    // If all the cells on a given contiguous three cells are equal return what they are set as.
                    // Otherwise, just return empty.
                    return (_cells[i] == _cells[j] && _cells[j] == _cells[k])
                        ? _cells[i]
                        : Cell.Empty;
                })
                .FirstOrDefault(s => s.IsSet()); // Return the first non-empty value if it exists.

        // Determines if the cat has the game.
        public bool Cat() =>
            _cells.All(c => c != Cell.Empty) && GameOver() == Cell.Empty;

        public override string ToString()
        {
            var symbols = _cells.Select(t =>
                t switch
                {
                    Cell.Empty => " ",
                    Cell.X => "X",
                    Cell.O => "O",
                    _ => "?",
                }
            )
            .ToArray();

            //╔═══╦═══╦═══╗
            //║   ║   ║   ║
            //╠═══╬═══╬═══╣
            //║   ║   ║   ║
            //╠═══╬═══╬═══╣
            //║   ║   ║   ║
            //╚═══╩═══╩═══╝
            var sb = new StringBuilder();

            sb.AppendLine("╔═══╦═══╦═══╗");
            sb.Append("║ ");

            int i = 0;
            foreach (var sym in symbols)
            {
                sb.Append(sym);
                sb.Append(" ║ ");

                if (i % 3 == 2 && i != 8)
                {
                    sb.AppendLine("\n╠═══╬═══╬═══╣");
                    sb.Append("║ ");
                }
                i++;
            }
            sb.Append("\n╚═══╩═══╩═══╝");

            return sb.ToString();
        }
    }
}
