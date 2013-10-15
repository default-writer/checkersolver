using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace checkersolver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int seed = 1538872388;
            int value;
            if (args.Length == 1)
            {
                if (Int32.TryParse(args[0], out value))
                {
                    seed = value;
                }
            }
            else
            {
                Random rnd = new Random((int)DateTime.Now.Ticks);
                seed = rnd.Next();
            }
            Board board = new Board(seed);
            Stopwatch sw = new Stopwatch();
            int depth = 0;
            sw.Start();
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
            sw.Stop();
            Console.WriteLine("RNG {0} {1}", seed, sw.Elapsed);
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