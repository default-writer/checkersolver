using System;
using System.Collections.Generic;
using System.Linq;

namespace checkersolver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int seed = 1;
            if (args.Length != 1 || !Int32.TryParse(args[0], out seed))
            {
                Random rnd = new Random((int)DateTime.Now.Ticks);
                seed = rnd.Next();
            }
            Console.WriteLine("RNG {0}", seed);
            Board board = new Board(seed);
            int depth = 0;
            do
            {
                foreach (Move move in board.Moves)
                {
                    move.DoMove();
                    depth++;
                }
                if (board.UsedCells == 1) break;
                if (depth > 2)
                {
                    Move last = board.Move;
                    if (last != null)
                    {
                        last.UndoMove();
                    }
                    depth--;
                }
                else
                {
                    break;
                }
            } while (true);
            Stack<Move> history = new Stack<Move>();
            while (depth-- > 0)
            {
                Move last = board.Move;
                if (last.UndoMove())
                {
                    history.Push(last);
                }

            }
            foreach (Move move in history)
            {
                move.DoMove();
                Console.WriteLine(string.Format(BinaryFormatter.Instance, "{0, 8} : - {1} {2:B}", ++depth, move, board.Hash));
            }
        }
    }
}