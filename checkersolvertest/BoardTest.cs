using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using checkersolver;

namespace checkersolvertest
{
    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void TestBoardMoves()
        {
            int seed = 2141152995;
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
