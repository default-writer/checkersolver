using System;
using System.Collections.Generic;

namespace checkersolver
{
    internal class Field
    {
        private readonly Board board;
        private readonly int index;
        private readonly Func<Field>[] nodes;

        public Field(Board board, int index, Func<Field> left, Func<Field> top, Func<Field> right, Func<Field> bottom/*, bool empty = false*/)
        {
            this.board = board;
            this.index = index;
            nodes = new Func<Field>[4] { left, top, right, bottom };
        }

        internal Field this[Direction direction]
        {
            get
            {
                Func<Field> f = nodes[(int)direction];
                if (f == null)
                {
                    return null;
                }
                return f();
            }
        }

        private static IEnumerable<Move> Create(Field node, Move move)
        {
            List<Move> moves = new List<Move>();
            Create(moves, node, move, Direction.Left);
            Create(moves, node, move, Direction.Top);
            Create(moves, node, move, Direction.Right);
            Create(moves, node, move, Direction.Bottom);
            return moves;
        }

        internal void Available(List<Move> moves, Move move)
        {
            foreach (Move available in Create(this, move))
            {
                if (!move.Contains(available))
                {
                    move.Add(available);
                }
                Append(moves, move, available);
            }
        }

        private bool CanUse()
        {
            return board.IsFree(index);
        }

        private bool CanFree()
        {
            return board.IsUsed(index);
        }

        private bool Use()
        {
            return board.Use(index);
        }

        private bool Free()
        {
            return board.Free(index);
        }

        internal static void Create(List<Move> moves, Field node, Move move, Direction direction)
        {
            Field near = node[direction];
            if (near == null) return;
            Field far = near[direction];
            if (far == null) return;
            if (node.CanUse() && near.CanFree() && far.CanFree())
            {
                Append(moves, move, move.Create(node, near, far));
            }
        }

        private static void Append(List<Move> moves, Move move, Move available)
        {
            Move m = move.Find(available);
            if (m == null) m = available;
            if (!moves.Contains(m))
            {
                moves.Add(m);
            }
        }

        internal bool CanMove(Field near, Field far)
        {
            return CanUse() && near.CanFree() && far.CanFree();
        }

        internal bool DoMove(Move move, Field near, Field far)
        {
            if (!CanMove(near, far)) return false;
            if (Use() && near.Free() && far.Free())
            {
                board.DoMove(this, near, far, move);
                return true;
            }
            return false;
        }

        internal bool Exists(Move move, Field near, Field far)
        {
            bool result = false;
            if (move.Hash == 0)
            {
                if (!CanMove(near, far)) 
                    throw new InvalidOperationException();
                {
                    Use();
                    near.Free();
                    far.Free();
                }
                result = move.Update(board);
                {
                    Free();
                    near.Use();
                    far.Use();
                }
                return result;
            }
            return board.Exists(move.Hash);
        }

        internal bool CanUndoMove(Field near, Field far)
        {
            return CanFree() && near.CanUse() && far.CanUse();
        }

        internal bool UndoMove(Move move, Field near, Field far)
        {
            if (!CanUndoMove(near, far)) return false;
            if (Free() && near.Use() && far.Use())
            {
                board.UndoMove(this, near, far, move);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            if (index == -1)
            {
                return "";
            }
            return string.Format("{0,4} {1,1}", index, board.IsUsed(index) ? "o" : ".");
        }
    }
}