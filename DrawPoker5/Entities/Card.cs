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

        public string LongName => Rank switch
        {
            14 => $"Ace of {Suit}",
            13 => $"King of {Suit}",
            12 => $"Queen of {Suit}",
            11 => $"Jack of {Suit}",
             _ => $"{Rank} of {Suit}",
        };

        public string ShortName => $"{Rank,2}:{Suit.ToString().Substring(0, 1)}";

        public Card(int rank, Suits suit)
        {
            Rank = rank;
            Suit = suit;
        }
    }
}
