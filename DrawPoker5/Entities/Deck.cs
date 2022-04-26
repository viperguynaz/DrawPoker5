using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPoker5.Entities
{
    public class Deck
    {
        /**** Card Names, Ranks and Suits by index in a non-shuffled deck ******
         *      Name    Rank    Clubs   Hearts  Diamonds    Spades
         *      2	    2	    0	    13	    26	        39
         *      3	    3	    1	    14	    27	        40
         *      4	    4	    2	    15	    28	        41
         *      5	    5	    3	    16	    29	        42
         *      6	    6	    4	    17	    30	        43
         *      7	    7	    5	    18	    31	        44
         *      8	    8	    6	    19	    32	        45
         *      9	    9	    7	    20	    33	        46
         *      10	    10	    8	    21	    34	        47
         *      Jack	11	    9	    22	    35	        48
         *      Queen	12	    10	    23	    36	        49
         *      King	13	    11	    24	    37	        50
         *      Ace	    14	    12	    25	    38	        51
        */

        public List<Card> Cards { get; set; }
        public int NextCard { get; set; }

        public Deck(bool shuffle = true, int ranks = 13)
        {
            Cards = new List<Card>();
            NextCard = 0;
            for (int suit = 0; suit < Enum.GetValues(typeof(Card.Suits)).Length; suit++) 
            {
                for (int rank = 2; rank < ranks + 2; rank++)
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
