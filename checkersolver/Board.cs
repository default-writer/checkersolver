using System;
using System.Collections.Generic;

namespace checkersolver
{
    public class Board
    {
        private readonly Random rnd;
        private readonly Field[] nodes;
        private readonly List<Field> empty;
        private readonly History history;
        private readonly BoardEnumerator enumerator;
        private readonly Func<Field>[] f;
        private readonly HashSet<ulong> data;

        private ulong hash;

        public Board()
            : this((int)DateTime.Now.Ticks)
        {
        }

        public Board(int seed)
        {
            data = new HashSet<ulong>();
            enumerator = new BoardEnumerator(this);
            rnd = new Random(seed);
            history = new History();
            f = new Func<Field>[33]
            {
                () => nodes[0],
                () => nodes[1],
                () => nodes[2],
                () => nodes[3],
                () => nodes[4],
                () => nodes[5],
                () => nodes[6],
                () => nodes[7],
                () => nodes[8],
                () => nodes[9],
                () => nodes[10],
                () => nodes[11],
                () => nodes[12],
                () => nodes[13],
                () => nodes[14],
                () => nodes[15],
                () => nodes[16],
                () => nodes[17],
                () => nodes[18],
                () => nodes[19],
                () => nodes[20],
                () => nodes[21],
                () => nodes[22],
                () => nodes[23],
                () => nodes[24],
                () => nodes[25],
                () => nodes[26],
                () => nodes[27],
                () => nodes[28],
                () => nodes[29],
                () => nodes[30],
                () => nodes[31],
                () => nodes[32]
            };
            //  .  .  0  1  2  .  .
            //  .  .  3  4  5  .  .
            //  6  7  8  9 10 11 12
            // 13 14 15 .. 17 18 19
            // 20 21 22 23 24 25 26
            //  .  . 27 28 29  .  .
            //  .  . 30 31 32  .  .
            nodes = new Field[33]
            {
                new Field(this, 0, null, null, f[1], f[3]),
                new Field(this, 1, f[0], null, f[2], f[4]),
                new Field(this, 2, f[1], null, null, f[5]),
                new Field(this, 3, null, f[0], f[4], f[8]),
                new Field(this, 4, f[3], f[1], f[5], f[9]),
                new Field(this, 5, f[4], f[2], null, f[10]),
                new Field(this, 6, null, null, f[7], f[13]),
                new Field(this, 7, f[6], null, f[8], f[14]),
                new Field(this, 8, f[7], f[3], f[9], f[15]),
                new Field(this, 9, f[8], f[4], f[10], f[16]),
                new Field(this, 10, f[9], f[5], f[11], f[17]),
                new Field(this, 11, f[10], null, f[12], f[18]),
                new Field(this, 12, f[11], null, null, f[19]),
                new Field(this, 13, null, f[6], f[14], f[20]),
                new Field(this, 14, f[13], f[7], f[15], f[21]),
                new Field(this, 15, f[14], f[8], f[16], f[22]),
                new Field(this, 16, f[15], f[9], f[17], f[23]),
                new Field(this, 17, f[16], f[10], f[18], f[24]),
                new Field(this, 18, f[17], f[11], f[19], f[25]),
                new Field(this, 19, f[18], f[12], null, f[26]),
                new Field(this, 20, null, f[13], f[21], null),
                new Field(this, 21, f[20], f[14], f[22], null),
                new Field(this, 22, f[21], f[15], f[23], f[27]),
                new Field(this, 23, f[22], f[16], f[24], f[28]),
                new Field(this, 24, f[23], f[17], f[25], f[29]),
                new Field(this, 25, f[24], f[18], f[26], null),
                new Field(this, 26, f[25], f[19], null, null),
                new Field(this, 27, null, f[22], f[28], f[30]),
                new Field(this, 28, f[27], f[23], f[29], f[31]),
                new Field(this, 29, f[28], f[24], null, f[32]),
                new Field(this, 30, null, f[27], f[31], null),
                new Field(this, 31, f[30], f[28], f[32], null),
                new Field(this, 32, f[31], f[29], null, null)
            };
            empty = new List<Field>() { nodes[16] };
            Use(0);
            Use(1);
            Use(2);
            Use(3);
            Use(4);
            Use(5);
            Use(6);
            Use(7);
            Use(8);
            Use(9);
            Use(10);
            Use(11);
            Use(12);
            Use(13);
            Use(14);
            Use(15);
            Free(16);
            Use(17);
            Use(18);
            Use(19);
            Use(20);
            Use(21);
            Use(22);
            Use(23);
            Use(24);
            Use(25);
            Use(26);
            Use(27);
            Use(28);
            Use(29);
            Use(30);
            Use(31);
            Use(32);
            data.Add(hash);
        }

        //internal Field this[int index] { get { return nodes[index]; } }

        public int UsedCells { get { return nodes.Length - empty.Count; } }

        internal Move MoveNext()
        {
            List<Move> moves = new List<Move>();
            List<Move> query = new List<Move>();
            empty.ForEach((n) => n.Available(moves, history.Move));
            foreach (Move move in moves)
            {
                if (!move.Exists())
                {
                    query.Add(move);
                }
            }
            int count = query.Count;
            if (count > 0)
            {
                return query[rnd.Next(count)]; ;
            }
            return null;
        }

        internal void DoMove(Field node, Field near, Field far, Move move)
        {
            data.Add(hash);
            empty.Remove(node);
            empty.Add(near);
            empty.Add(far);
            history.DoMove(move);
        }

        internal void UndoMove(Field node, Field near, Field far, Move move)
        {
            //data.Remove(hash);
            empty.Add(node);
            empty.Remove(near);
            empty.Remove(far);
            history.UndoMove(move);
        }

        public IEnumerable<Move> Moves
        {
            get
            {
                return enumerator;
            }
        }

        internal bool IsUsed(int index)
        {
            return (hash & (1ul << index)) == (1ul << index);
        }

        internal bool IsFree(int index)
        {
            return (hash | ~(1ul << index)) == ~(1ul << index);
        }

        internal bool Use(int index)
        {
            if (IsFree(index))
            {
                hash = hash | (1ul << index);
                return true;
            }
            return false;
        }

        internal bool Free(int index)
        {
            if (IsUsed(index))
            {
                hash = hash & ~(1ul << index);
                return true;
            }
            return false;
        }

        public ulong Hash { get { return hash; } }

        internal bool Exists(ulong hash)
        {
            return data.Contains(hash);
        }

        public Move Move { get { if (history.Moves.Count > 0) { return history.Moves.Peek(); } return null; } }
    }
}