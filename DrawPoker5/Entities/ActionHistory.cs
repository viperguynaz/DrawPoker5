using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPoker5.Entities
{
    public class ActionHistory
    {
        // INPUTS
        public Guid RoundId { get; set; }
        public int Round { get; set; }
        public int Position { get; set; }
        public int Wager { get; set; }
        public int Pot { get; set; }
        public int RaiseCount { get; set; }
        public int PlayerCount { get; set; }
        public Hand Hand { get; set; }
        public int Bank { get; set; }
        public int Stake { get; set; }

        // OBSERVATIONS
        public PlayerAction Play { get; set; }
        public int Score { get; set; }  
        public ActionHistory()
        {
            Play = new PlayerAction();
            Hand = new Hand();
        }
    }
}
