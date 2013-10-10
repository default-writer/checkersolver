using System.Collections;
using System.Collections.Generic;

namespace checkersolver
{
    internal class BoardEnumerator : IEnumerator<Move>, IEnumerable<Move>
    {
        private readonly Board board;

        private Move query;

        public BoardEnumerator(Board board)
        {
            this.board = board;
        }

        public Move Current { get { return query; } }

        object IEnumerator.Current { get { return (object)Current; } }

        public bool MoveNext()
        {
            query = board.MoveNext();
            return query != null;
        }

        public IEnumerator<Move> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public void Reset()
        {
        }

        public void Dispose()
        {
        }
    }
}