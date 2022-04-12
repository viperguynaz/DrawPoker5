using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPoker5.Entities
{
    public class Card
    {
        public enum Suits { Clubs, Hearts, Diamonds, Spades }  // values 0..3

        public Suits Suit { get; set; }
        public int Rank { get; set; }  // values

        public int Value { get; set; }  // not used here

        public Card(int rank, Suits suit)
        {
            Rank = rank;
            Suit = suit;
        }
    }
}
