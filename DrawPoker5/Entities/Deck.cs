using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPoker5.Entities
{
    public class Deck
    {
        public List<Card> Cards { get; set; }

        public int NextCard { get; set; }

        public Deck(bool shuffle = true, int ranks = 14)
        {
            Cards = new List<Card>();
            NextCard = 0;
            for(int rank = 2; rank <= ranks; rank++)
            {
                for(int suit = 0; suit < Enum.GetValues(typeof(Card.Suits)).Length; suit++)
                {
                    Cards.Add(new Card(rank, (Card.Suits) suit));
                }
            }

            if (shuffle) Shuffle();
        }

        public void Shuffle()
        {
            var random = new Random();
            Cards = Cards.OrderBy(card => random.Next()).ToList();
        }

        public List<List<Card>> Deal(int numPlayers, int handSize)
        {
            var hands = new List<List<Card>>();
            if (numPlayers * handSize > Cards.Count) throw new Exception($"# hands ({numPlayers}) * hand size ({handSize}) > # cards ({Cards.Count}) in deck");
            for(int i = 0; i < numPlayers; i++)
            {
                hands.Add(new List<Card>());
            }
            int index = 0;
            for(int x = 0; x < handSize; x++)
            {
                foreach(var hand in hands)
                {
                    hand.Add(Cards[index]);
                    index++;
                }
            }
            NextCard = index;
            return hands;
        }

        public List<Card> Draw(int count)
        {
            var cards = new List<Card>();
            for(int i = NextCard; i < NextCard + count; i++)
            {
                cards.Add(Cards[i]);
            }
            NextCard += count;
            return cards;
        }

    }
}
