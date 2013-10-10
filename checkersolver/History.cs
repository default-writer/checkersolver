using System.Collections.Generic;

namespace checkersolver
{
    internal class History
    {
        private Stack<Move> moves = new Stack<Move>();
        private Move move;

        public History()
        {
            move = new Move();
        }

        internal Move Move { get { return move; } }

        internal void DoMove(Move move)
        {
            this.move = move;
            moves.Push(move);
        }

        internal void UndoMove(Move move)
        {
            this.move = move.Prev;
            this.moves.Pop();
        }

        internal Stack<Move> Moves { get { return moves; } }
    }
}