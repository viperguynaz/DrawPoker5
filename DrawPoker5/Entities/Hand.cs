using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPoker5.Entities
{
    public class Hand
    {
        // re: https://en.wikipedia.org/wiki/Poker_probability
        public enum Ranks {
            RoyalFlush      = 2598956,
            StraightFlush   = 2598924,
            FourOfAKind     = 2598336,
            FullHouse       = 2595216,
            Flush           = 2593852,
            Straight        = 2588760,
            ThreeOfAKind    = 2544048,
            TwoPair         = 2475408,
            OnePair         = 1500720,
            HighCard        = 1296420
        }
        public List<Card> Cards { get; set; }

        public Ranks Rank
        {
            get
            {
                var cards = Cards.OrderByDescending(c => c.Rank).ToList();
                bool straight = ((cards.First().Rank - cards.Last().Rank) == 4) || (cards.First().Rank == 14 && cards[1].Rank == 5);
                bool flush = cards.GroupBy(c => c.Suit).Count() == 1;

                if (straight && flush) return cards.Last().Rank == 10 ? Ranks.RoyalFlush : Ranks.StraightFlush;
                if (straight) return Ranks.Straight;
                if (flush) return Ranks.Flush;

                Ranks rank;
                var groups = cards.GroupBy(c => c.Rank).OrderByDescending(g => g.Count()).ToList();
                switch (groups.Count())
                {
                    case 2:
                        rank = groups.First().Count() == 4 ? Ranks.FourOfAKind : Ranks.FullHouse;
                        break;
                    case 3:
                        rank = groups.First().Count() == 3 ? Ranks.ThreeOfAKind : Ranks.TwoPair;
                        break;
                    case 4:
                        rank = Ranks.OnePair;
                        break;
                    default:
                        rank = Ranks.HighCard;
                        break;
                }
                return rank;
            }
        }
        public Hand()
        {
            Cards = new List<Card>();
        }
        public Hand(List<Card> cards)
        {
            Cards = cards;
        }

    }
}
