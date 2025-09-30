using System.Diagnostics;

namespace TicTacToe
{
    public class Program
    {
        public static void Main()
        {
            bool firstGame = true;
            while (true)
            {
                try
                {
                    // Set up a new game.
                    Cell playerMarker = GetPlayerMarker();
                    bool isXTurn = true; // Let x go first.
                    var comp = new Computer(playerMarker.Inverse());
                    var board = new Board();

                    // On the first iteration, display a help message.
                    if (firstGame)
                    {
                        HelpMessage();
                        firstGame = false;
                    }

                    // Until someone wins or a draw occurs.
                    while (board.GameOver() == Cell.Empty && !board.Cat())
                    {
                        bool isPlayerTurn = playerMarker switch
                        {
                            Cell.X => isXTurn,
                            Cell.O => !isXTurn,
                            _ => throw new UnreachableException(),
                        };

                        // The appropriate player takes its turn.
                        if (isPlayerTurn)
                        {
                            PlayerMove(board, playerMarker);
                        }
                        else
                        {
                            ComputerMove(comp, board);
                        }

                        // Change the turn and display the board.
                        Console.WriteLine(board);
                        isXTurn = !isXTurn;
                    }

                    // Announce the winner.
                    GameOverAnnouncement(board);
                }
                catch (ArgumentException) // Allows CTRL+D to exit from the marker selection prompt.
                {
                    Console.WriteLine("\nExiting.");
                    break;
                }
            }
        }

        // Prompt the player for a move, and update the board.
        public static void PlayerMove(Board board, Cell marker)
        {
            while (true)
            {
                try
                {
                    var (x, y) = GetPlayerMove(board);
                    board.SetCell(x, y, marker);
                    break;
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        // Have the computer calculate and make its move.
        public static void ComputerMove(Computer comp, Board board)
        {
            var (x, y) = comp.GetMove(board);
            Console.WriteLine($"Computer plays ({x}, {y}).");
            board.SetCell(x, y, comp.Marker);

        }

        // Print a help message explaining the expected input format.
        public static void HelpMessage()
        {
            Console.WriteLine("Moves should be entered as zero-indexed coordinate pairs.");
            Console.WriteLine("e.g., the center is expressed as `1,1`.");
            Console.WriteLine("Alternatively a flat zero-based index 0-8 is acceptable.");
        }

        // Print the game over message.
        public static void GameOverAnnouncement(Board board)
        {

                if (board.Cat())
                {
                    Console.WriteLine("The Cat has it.");
                    return;
                }

                string msg = board.GameOver() switch
                {
                    Cell.X => "X wins!",
                    Cell.O => "O wins!",
                    _ => ""
                };
                Console.WriteLine(msg);
        }

        // Prompt the user for their desired marker (X or O), signifying the beginning of a new game.
        public static Cell GetPlayerMarker()
        {
            Console.Write("Select your marker [X/o] (Ctrl+D to exit): ");
            Console.Out.Flush();

            string? input = Console.ReadLine();
            if (input == null)
            {
                throw new ArgumentException("No input received.");
            }

            return input.Trim().FirstOrDefault() switch
            {
                'x' or 'X' => Cell.X,
                'o' or 'O' => Cell.O,
                _ => Cell.X,
            };
        }

        // Promt the player for a move, parsing it into a coordinate pair.
        public static (int, int) GetPlayerMove(Board board)
        {
            Console.Write("Input index or coordinate pair: ");
            Console.Out.Flush();
            string? input = Console.ReadLine();
            if (input == null)
            {
                throw new ArgumentException("No input received.");
            }

            // Split on commas.
            var parts = input.Split(',').Select(p => p.Trim()).ToArray();

            switch (parts.Length)
            {
                // If only one number is provided, then parse it and check its validity.
                case 1:
                    // Ensure input is valid
                    if (!int.TryParse(parts[0], out int i))
                    {
                        throw new ArgumentException("The provided input is not a number.");
                    }
                    if (!board.ValidPlayerIndices(i))
                    {
                        throw new ArgumentException($"The index {i} is out of range or not empty. Valid values 0-8.");
                    }

                    // Return as (x, y).
                    return Board.XToXY(i);
                // If two numbers are supplied, parse and check them, and return as-is.
                case 2:
                    if (!int.TryParse(parts[0], out int x) || !int.TryParse(parts[1], out int y))
                    {
                        throw new ArgumentException("One or more invalid numbers provided.");
                    }
                    if (!board.ValidPlayerIndices(x, y))
                    {
                        throw new ArgumentException($"One or both indices ({x},{y}) is out of range or not empty. Valid values 0-2.");
                    }

                    // Return as (x, y)
                    return (x, y);
                default:
                    throw new ArgumentException("Too many arguments. Either a single index or a coordinate pair is expected.");
            }
        }

    }
}
