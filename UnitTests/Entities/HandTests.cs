using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DrawPoker5.Entities;

namespace DrawPoker5.Entities.Tests
{
    [TestClass]
    public class HandTests
    {
        [TestMethod]
        public void HandTest()
        {
            var cards = new List<Card>();
            cards.Add(new Card(rank: 2, suit: Card.Suits.Hearts));
            cards.Add(new Card(rank: 3, suit: Card.Suits.Hearts));
            cards.Add(new Card(rank: 4, suit: Card.Suits.Hearts));
            cards.Add(new Card(rank: 5, suit: Card.Suits.Hearts));
            cards.Add(new Card(rank: 7, suit: Card.Suits.Hearts));

            var hand = new Hand(cards);

            Assert.AreEqual(hand.Rank, Hand.Ranks.Flush);

            hand.Cards[4].Rank = 6;
            Assert.AreEqual(hand.Rank, Hand.Ranks.StraightFlush);

            hand.Cards[4].Suit = Card.Suits.Clubs;
            Assert.AreEqual(hand.Rank, Hand.Ranks.Straight);

            // test low straight with ace exception
            hand.Cards[4].Rank = 14;
            Assert.AreEqual(hand.Rank, Hand.Ranks.Straight);

            hand.Cards[4].Suit = Card.Suits.Hearts;
            Assert.AreEqual(hand.Rank, Hand.Ranks.StraightFlush);

            hand.Cards[0].Rank = 10;
            hand.Cards[1].Rank = 11;
            hand.Cards[2].Rank = 12;
            hand.Cards[3].Rank = 13;
            Assert.AreEqual(hand.Rank, Hand.Ranks.RoyalFlush);

            hand.Cards[0].Rank = 4;
            hand.Cards[1].Rank = 5;
            hand.Cards[2].Rank = 4;
            hand.Cards[3].Rank = 4;
            hand.Cards[4].Rank = 4;

            hand.Cards[2].Suit = Card.Suits.Clubs;
            hand.Cards[4].Suit = Card.Suits.Diamonds;
            hand.Cards[3].Suit = Card.Suits.Spades;
            Assert.AreEqual(hand.Rank, Hand.Ranks.FourOfAKind);

            hand.Cards[3].Rank = 5;  // 4 5 4 5 4
            Assert.AreEqual(hand.Rank, Hand.Ranks.FullHouse);

            hand.Cards[3].Rank = 6;  // 4 5 4 6 4
            Assert.AreEqual(hand.Rank, Hand.Ranks.ThreeOfAKind);

            hand.Cards[4].Rank= 5;  // 4 5 4 6 5
            Assert.AreEqual(hand.Rank, Hand.Ranks.TwoPair);

            hand.Cards[4].Rank = 7;  // 4 5 4 6 7
            Assert.AreEqual(hand.Rank, Hand.Ranks.OnePair);

            hand.Cards[2].Rank = 2;  // 4 5 2 6 7
            Assert.AreEqual(hand.Rank, Hand.Ranks.HighCard);
        }
    }
}