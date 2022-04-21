using DrawPoker5.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public static class Cards
    {
        public static List<Card> Clubs => Enumerable.Range(2, 14).Select(i => new Card(rank: i, suit: Card.Suits.Clubs)).ToList();
        public static List<Card> Hearts => Enumerable.Range(2, 14).Select(i => new Card(rank: i, suit: Card.Suits.Hearts)).ToList();
        public static List<Card> Diamonds => Enumerable.Range(2, 14).Select(i => new Card(rank: i, suit: Card.Suits.Diamonds)).ToList();
        public static List<Card> Spades => Enumerable.Range(2, 14).Select(i => new Card(rank: i, suit: Card.Suits.Spades)).ToList();
    }
}
