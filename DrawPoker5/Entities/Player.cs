using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPoker5.Entities
{
    public class Player
    {
        public enum Actions { Check, Bet, Fold, Call, Raise }
        public Guid Id { get; }

        public Guid RoundId { get; set; }
        public string Name { get; set; }

        public int Bank { get; set; }

        public int Wins { get; set; }
        public int Losses { get; set; }
        public bool IsActive { get; set; }

        public Hand Hand { get; set; }
        public int Stake { get; set; }  // contribution to pot for each hand, risk
        public List<ActionHistory> Bets { get; set; }
        public List<DrawHistory> Draws { get; set; }

        private Random random = new Random();

        public int Ante(int ante)
        {
            Stake -= ante;
            return ante;
        }

        public void PrintHand()
        {
            Console.Write($"  {Name} ");
            Hand.Cards.OrderByDescending(card => card.Rank).ToList().ForEach(card => Console.Write($"{card.Rank,2}.{card.Suit}\t"));
            Console.Write($"{Hand.Rank}\r\n");
        }

        public int Draw(Deck deck)
        {
            //TODO Update evaluation function to choose discards
            int numToDraw = random.Next(1, 4);   // draw 1-3 cards randomly for now
            var handBefore = Hand;

            // randomly remove draw cards
            for (int i = 0; i < numToDraw; i++)
            {
                Hand.Cards.RemoveAt(random.Next(0, Hand.Cards.Count));
            }

            // get new cards
            Hand.Cards.AddRange(deck.Draw(numToDraw));
            Draws.Add(new DrawHistory(handBefore, numToDraw, Hand));

            return numToDraw;
        }

        public Player()
        {
            Id = Guid.NewGuid();
            Hand = new Hand();
            Bets = new List<ActionHistory>();
            Draws = new List<DrawHistory>();
            Name = String.Empty;
            IsActive = true;
        }

        public Player(string name, int stake = 500) : this()
        {
            Name = name;
            Bank = stake;
        }

        public PlayerAction EvalBet(Guid roundId, int round, int position, int wager, int pot, int raiseCount, int playerCount)
        {
            /**
             * Inputs
             * - Position 0..5 +1 with each hand, rotating
             * - Round: int betting round
             * - Wager: int, sum due from each player to continue play
             * - Pot: int total in play
             * - RaiseCount
             * - PlayerCount - # players remaining in hand 
             * 
             * Observations
             * - Hand rank
             * - Bank
             * - Stake
             */

            var actionHistory = new ActionHistory()
            {
                RoundId = roundId,
                HandRank = Hand.Rank,
                Bank = Bank,
                Stake = Stake,
                Position = position,
                Round = round,
                Wager = wager,
                Pot = pot,
                RaiseCount = raiseCount,
                PlayerCount = playerCount
            };

            var similarBets = Bets.Where(b => b.HandRank == Hand.Rank && b.Score > 0)
                .OrderByDescending(b => b.Score)
                .ToList();

            // choose an action
            //TODO need a more robust evaluation function to select action
            var bet = 0;
            if (similarBets.Count() == 0)
            {
                actionHistory.Play.Action = wager == 0 ? (Actions)random.Next(0, 3) : (Actions)random.Next(2, 5);
                bet = actionHistory.Play.Action == Actions.Bet || actionHistory.Play.Action == Actions.Raise ? random.Next(1, 6) : 0;
            }
            else
            {
                actionHistory.Play.Action = similarBets.First().Play.Action;
                bet = similarBets.First().Play.Bet;
            }

            // choose a bet if appropriate
            switch (actionHistory.Play.Action)  
            {
                case Actions.Bet:
                    actionHistory.Play.Bet = bet;
                    Stake += bet;
                    Bank -= bet;
                    break;
                case Actions.Call:
                    actionHistory.Play.Bet = wager;
                    Stake += wager;
                    Bank -= wager;
                    break;
                case Actions.Raise:
                    actionHistory.Play.Bet = bet;
                    Stake += wager + bet;
                    Bank -= wager + bet;
                    break;
                case Actions.Fold:
                    actionHistory.Play.Bet = 0;
                    IsActive = false;
                    break;
                default:
                    actionHistory.Play.Bet = 0;
                    break;
            }

            Bets.Add(actionHistory);
            return actionHistory.Play;
        }
    }
}
