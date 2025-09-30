namespace TicTacToe
{
    public class Computer(Cell marker)
    {
        public readonly Cell Marker = marker;

        // Returns a coordinate pair indicating where the computer would like to play.
        public (int, int) GetMove(Board board)
        {
            foreach (var (i, j, k) in Board.Triplets)
            {
                var indices = new[] { i, j , k};
                var cells = indices.Select(i => board.GetCell(i)).ToArray();

                // One move from victory.
                if (cells.Count(s => s == Marker) == 2         // True when 2 of the triplet is the computer's marker
                    && cells.Count(s => s == Cell.Empty) == 1) // and 1 is empty space
                {
                    // Index of the empty cell.
                    int empty_idx = indices[cells.ToList().FindIndex(s => s == Cell.Empty)];
                    return Board.XToXY(empty_idx);
                }

                // Block player win victory.
                if (cells.Count(s => s == Marker.Inverse()) == 2 // True when 2 of the triplet is the player's marker
                    && cells.Count(s => s == Cell.Empty) == 1)   // and 1 is empty space
                {
                    int empty_idx = indices[cells.ToList().FindIndex(s => s == Cell.Empty)];
                    return Board.XToXY(empty_idx);
                }
            }

            // If the middle is empty take it.
            if (board.GetCell(1, 1) == Cell.Empty) return (1, 1);
            // Take the first empty cell as a fallback.
            return Board.XToXY(Enumerable.Range(0, 9).First(idx => board.GetCell(idx) == Cell.Empty));
        }
    }
}
