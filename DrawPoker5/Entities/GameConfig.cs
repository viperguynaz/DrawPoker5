using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPoker5.Entities
{
    public class GameConfig
    {
        public Guid Id { get; }
        public int NumPlayers { get; }
        public int HandSize { get; }
        public int Ante { get; }
        public int MaxBet { get; }
        public int MaxRaise { get; }
        public int MaxNumRaises { get; }


        public GameConfig()
        {
            Id = Guid.NewGuid();
            NumPlayers = 6;
            HandSize = 5;
            Ante = 1;
            MaxBet = 5;
            MaxRaise = 5;
            MaxNumRaises = 4;
        }
    }
}
