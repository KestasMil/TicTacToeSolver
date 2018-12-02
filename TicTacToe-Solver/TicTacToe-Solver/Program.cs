using System;
using System.Linq;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            int[][] board = new int[][]
            {
                /*new int[] { 0, 0, 0 },
                new int[] { 0, 0, 0 },
                new int[] { 0, 0, 0 }*/
                new int[] { 0, 2, 0 },
                new int[] { 0, 1, 0 },
                new int[] { 0, 0, 2 }
            };
            var result = TurnMethod(board, 1);
            Console.WriteLine();
            Console.WriteLine("###Resulting move: " + result[0] + ":" + result[1]);
            Console.ReadLine();
        }

        public static int[] TurnMethod(int[][] board, int player)
        {
            //fix axis to work on codewars board
            var b1 = board.Select(s => s.ToArray()).ToArray();

            //Invert board if player 2
            if (player == 2) board = InvertBoard(board);

            //Temp boards
            int[][] newBoard = board.Select(s => s.ToArray()).ToArray();
            int[][] tempBoard = board.Select(s => s.ToArray()).ToArray();

            for (int i = 0; i < 4; i++)
            {
                //get hash code of each line
                int[] lines = new int[]
                {
                    newBoard[0][0] | newBoard[0][1] * 4 | newBoard[0][2] * 16,
                    newBoard[1][0] | newBoard[1][1] * 4 | newBoard[1][2] * 16,
                    newBoard[2][0] | newBoard[2][1] * 4 | newBoard[2][2] * 16,
                    newBoard[0][0] | newBoard[1][1] * 4 | newBoard[2][2] * 16
                };

                //RETURN MOVE IF AVAILABLE
                //MOVE TO WIN
                //Move to win at index (X 1 1)?
                int st = Array.IndexOf(lines, 20);
                if (st > -1) return RotateAntiClockwise(CreateCoords(st, 0), i);
                //Move to win at index (1 X 1)?
                st = Array.IndexOf(lines, 17);
                if (st > -1) return RotateAntiClockwise(CreateCoords(st, 1), i);
                //Move to win at index (1 1 X)?
                st = Array.IndexOf(lines, 5);
                if (st > -1) return RotateAntiClockwise(CreateCoords(st, 2), i);
                //MOVE TO DEFEND
                //Move to defend at index (X 2 2)?
                st = Array.IndexOf(lines, 40);
                if (st > -1) return RotateAntiClockwise(CreateCoords(st, 0), i);
                //Move to defend at index (2 X 2)?
                st = Array.IndexOf(lines, 34);
                if (st > -1) return RotateAntiClockwise(CreateCoords(st, 1), i);
                //Move to defend at index (2 2 X)?
                st = Array.IndexOf(lines, 10);
                if (st > -1) return RotateAntiClockwise(CreateCoords(st, 2), i);
                //------------------------
                //Rotate board 1 time clockwise
                newBoard[0] = new int[] { tempBoard[2][0], tempBoard[1][0], tempBoard[0][0] };
                newBoard[1] = new int[] { tempBoard[2][1], tempBoard[1][1], tempBoard[0][1] };
                newBoard[2] = new int[] { tempBoard[2][2], tempBoard[1][2], tempBoard[0][2] };
                tempBoard = newBoard.Select(s => s.ToArray()).ToArray();
            }

            //Temp boards
            newBoard = board.Select(s => s.ToArray()).ToArray();
            tempBoard = board.Select(s => s.ToArray()).ToArray();
            for (int i = 0; i < 4; i++)
            {
                //get hash code of each line
                int[] lines = new int[]
                {
                    newBoard[0][0] | newBoard[0][1] * 4 | newBoard[0][2] * 16,
                    newBoard[1][0] | newBoard[1][1] * 4 | newBoard[1][2] * 16,
                    newBoard[2][0] | newBoard[2][1] * 4 | newBoard[2][2] * 16,
                    newBoard[0][0] | newBoard[1][1] * 4 | newBoard[2][2] * 16
                };

                //RETURN MOVE IF AVAILABLE
                //STARTING MOVES DEPENDING ON SITUATION
                //take center if op taken corner on first move
                if (lines[0] == 2 && lines[1] + lines[2] == 0) return new int[] { 1, 1 };
                //take oposite corner if op has center and you got one corner
                if (lines[0] == 1 && lines[1] == 8 && lines[2] == 0) return RotateAntiClockwise(new int[] { 2, 2 }, i);
                //take oposite corner if op has corner and you got center
                if (lines[0] == 2 && lines[1] == 4 && lines[2] == 0) return RotateAntiClockwise(new int[] { 2, 2 }, i);
                //take middle side square if op has two oposite corners and you have center
                if (lines[3] == 38 && lines[0] == 2 && lines[2] == 32) return RotateAntiClockwise(new int[] { 0, 1 }, i);
                //if op starter on the middle side take middle
                if (lines[0] == 8 && lines[1] == 0 && lines[2] == 0) return new int[] { 1, 1 };
                //some other defend tactics
                if ((lines[0] == 8 || lines[2] == 8) && (lines[0] == 32 || lines[2] == 32) && (lines[1] == 4)) return RotateAntiClockwise(new int[] { 1, 0 }, i);
                if (lines[0] == 8 && lines[1] == 6) return RotateAntiClockwise(new int[] { 0, 0 }, i);

                //MOVE TO FORM TRAP
                //if having the center
                if (lines[1] == 4)
                {
                    if (board[0][0] == 0 && board[1][0] == 0 && board[2][0] == 0) return RotateAntiClockwise(new int[] { 0, 0 }, i);
                    if (board[0][0] == 1 && board[1][0] == 0 && board[2][0] == 0) return RotateAntiClockwise(new int[] { 2, 0 }, i);
                }
                //if not having the center
                if (lines[1] != 4)
                {
                    if (lines[0] == 1 && board[1][0] == 0 && board[2][0] == 0) return RotateAntiClockwise(new int[] { 0, 2 }, i);
                    if (board[0][0] == 1 && board[1][0] == 0 && board[2][0] == 0) return RotateAntiClockwise(new int[] { 2, 0 }, i);
                    if (lines[0] == 1 && board[1][1] == 0 && board[2][0] == 0) return RotateAntiClockwise(new int[] { 0, 2 }, i);
                    if (lines[3] == 1 && lines[3] == 1) return RotateAntiClockwise(new int[] { 2, 2 }, i);
                }
                //------------------------
                //Rotate board 1 time clockwise
                newBoard[0] = new int[] { tempBoard[2][0], tempBoard[1][0], tempBoard[0][0] };
                newBoard[1] = new int[] { tempBoard[2][1], tempBoard[1][1], tempBoard[0][1] };
                newBoard[2] = new int[] { tempBoard[2][2], tempBoard[1][2], tempBoard[0][2] };
                tempBoard = newBoard.Select(s => s.ToArray()).ToArray();
            }

            //No vialable move found, take first available move
            return FirstAvailable(board);
        }

        //Rotate coordinates anticlockwise
        public static int[] RotateAntiClockwise(int[] coords, int times)
        {
            int[] temp = new int[] { 0, 0 };
            for (int i = 0; i < times; i++)
            {
                if (coords[0] == 0 && coords[1] == 0) temp = new int[] { 2, 0 };
                if (coords[0] == 0 && coords[1] == 1) temp = new int[] { 1, 0 };
                if (coords[0] == 0 && coords[1] == 2) temp = new int[] { 0, 0 };

                if (coords[0] == 1 && coords[1] == 0) temp = new int[] { 2, 1 };
                if (coords[0] == 1 && coords[1] == 1) temp = new int[] { 1, 1 };
                if (coords[0] == 1 && coords[1] == 2) temp = new int[] { 0, 1 };

                if (coords[0] == 2 && coords[1] == 0) temp = new int[] { 2, 2 };
                if (coords[0] == 2 && coords[1] == 1) temp = new int[] { 1, 2 };
                if (coords[0] == 2 && coords[1] == 2) temp = new int[] { 0, 2 };
                coords[0] = temp[0];
                coords[1] = temp[1];
            }
            return coords;
        }

        //invert board.
        public static int[][] InvertBoard(int[][] board)
        {
            for (int i = 0; i < board.Count(); i++)
            {
                for (int ii = 0; ii < board[i].Count(); ii++)
                {
                    if (board[i][ii] == 1)
                    {
                        board[i][ii] = 2;
                    }
                    else if (board[i][ii] == 2)
                    {
                        board[i][ii] = 1;
                    }
                }
            }
            return board;
        }

        //Get first available square coordinates
        public static int[] FirstAvailable(int[][] board)
        {
            int r = 0;
            int c = 0;
            foreach (var row in board)
            {
                c = 0;
                foreach (var square in row)
                {
                    if (square == 0) return new int[] { r, c };
                    c++;
                }
                r++;
            }
            return new int[] { -1, -1 };
        }

        //Coordinates builder
        public static int[] CreateCoords(int lineIndex, int index)
        {
            switch (lineIndex)
            {
                case 0:
                case 1:
                case 2:
                    return new int[] { lineIndex, index };
                case 3:
                    return new int[] { index, index };
                default:
                    return null;
            }
        }
    }
}
