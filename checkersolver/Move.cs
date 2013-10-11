using System;
using System.Collections.Generic;

namespace checkersolver
{
    public class Move : IEquatable<Move>
    {
        private readonly Field node;
        private readonly Field near;
        private readonly Field far;

        private readonly Move prev;
        private readonly List<Move> moves;

        internal Move()
        {
            moves = new List<Move>();
        }

        private Move(Field node, Field near, Field far, Move prev)
            : this()
        {
            this.node = node;
            this.near = near;
            this.far = far;
            this.prev = prev;
        }

        internal Move Prev { get { return prev; } }

        public bool Contains(Move move)
        {
            return moves.Find((p) => p.Equals(move)) != null;
        }

        public Move Find(Move move)
        {
            return moves.Find((p) => p.Equals(move));
        }

        internal void Add(Move move)
        {
            moves.Add(move);
        }

        internal bool Exists()
        {
            return node.Exists(this, near, far);
        }

        public bool DoMove()
        {
            return node.DoMove(this, near, far);
        }

        public bool UndoMove()
        {
            return node.UndoMove(this, near, far);
        }

        public bool Equals(Move other)
        {
            return other != null && this.node == other.node && this.near == other.near && this.far == other.far;
        }

        public override string ToString()
        {
            if (prev == null)
            {
                return "";
            }
            return string.Format("{0,8} -> {1,8} -> {2,8} ({3,8})", node, near, far, NotVisited(prev));
        }

        internal Move Create(Field node, Field near, Field far)
        {
            return new Move(node, near, far, this);
        }

        internal static string NotVisited(Move move)
        {
            int count = 0;
            int total = 0;
            foreach (Move m in move.moves)
            {
                if (m.Exists())
                {
                    count++;
                }
                total++;
            }
            return string.Format("{0, 3}/{1, 3}", count, total);
        }

        private ulong hash;
        public ulong Hash { get { return hash; } }

        internal bool Update(Board board)
        {
            hash = board.Hash;
            return board.Exists(hash);
        }
    }
}