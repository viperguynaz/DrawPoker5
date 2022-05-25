using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DrawPoker5.Entities
{
    public class Player
    {
        public enum Action { Check, Bet, Fold, Call, Raise }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Bank { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public int Stake { get; set; }  // contribution to pot for each hand, i.e. risk
        public List<ActionHistory> Actions { get; set; }
        public List<DrawHistory> Draws { get; set; }

        [JsonIgnore]
        public bool IsActive { get; set; }
        [JsonIgnore] 
        public Hand Hand { get; set; }
        [JsonIgnore] 
        public string FileName => $"data/{Id}.json";

        private Random random = new Random();

        public int Ante(int ante)
        {
            Stake += ante;
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
            var handBefore = new Hand(Hand.Cards.ToList());

            // randomly remove draw cards
            for (int i = 0; i < numToDraw; i++)
            {
                Hand.Cards.RemoveAt(random.Next(0, Hand.Cards.Count));
            }

            // get new cards
            var newCards = deck.Draw(numToDraw);
            Hand.Cards.AddRange(newCards);
            Draws.Add(new DrawHistory(handBefore, numToDraw, Hand));

            return numToDraw;
        }

        public Player()
        {
            Id = Guid.NewGuid();
            Hand = new Hand();
            Actions = new List<ActionHistory>();
            Draws = new List<DrawHistory>();
            Name = String.Empty;
        }

        //public Player(Guid id)
        //{
        //    Id = id;
        //    Hand = new Hand();
        //    Actions = ReadActions();
        //    Draws = ReadDraws();
        //    Name = String.Empty;
        //    IsActive = true;
        //}

        public Player(string name, int stake = 500) : this()
        {
            Name = name;
            Bank = stake;
            IsActive = true;
        }

        //public Player(Guid id, string name, int stake = 500) : this(id)
        //{
        //    Name = name;
        //    Bank = stake;
        //}

        public PlayerAction EvalBet(Guid gameId, int round, int position, int wager, int pot, int raiseCount, int playerCount)
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
                GameId = gameId,
                Round = round,
                Position = position,
                Wager = wager,
                Pot = pot,
                RaiseCount = raiseCount,
                PlayerCount = playerCount,
                Hand = Hand,
            };

            //TODO fix: check that bet actions are valid
            List<ActionHistory> similarBets = wager > 0 ? 
                Actions.Where(b => b.Hand.Rank == Hand.Rank && b.Score > 0 && b.Play.Action != Action.Check)
                    .OrderByDescending(b => b.Score)
                    .ToList() :
                Actions.Where(b => b.Hand.Rank == Hand.Rank && b.Score > 0)
                    .OrderByDescending(b => b.Score)
                    .ToList();

            // choose an action
            //TODO need a more robust evaluation function to select action
            var bet = 0;
            if (similarBets.Count == 0)
            {
                actionHistory.Play.Action = wager == 0 ? (Action)random.Next(0, 3) : (Action)random.Next(2, 5);
                bet = actionHistory.Play.Action == Action.Bet || actionHistory.Play.Action == Action.Raise ? random.Next(1, 6) : 0;
            }
            else
            {
                actionHistory.Play.Action = similarBets.First().Play.Action;
                bet = similarBets.First().Play.Bet;
            }

            // choose a bet if appropriate
            switch (actionHistory.Play.Action)  
            {
                case Action.Bet:
                    actionHistory.Play.Bet = bet;
                    Stake += bet;
                    Bank -= bet;
                    break;
                case Action.Call:
                    actionHistory.Play.Bet = wager;
                    Stake += wager;
                    Bank -= wager;
                    break;
                case Action.Raise:
                    actionHistory.Play.Bet = bet;
                    Stake += wager + bet;
                    Bank -= wager + bet;
                    break;
                case Action.Fold:
                    actionHistory.Play.Bet = 0;
                    IsActive = false;
                    break;
                default:  // Action.Chack
                    actionHistory.Play.Bet = 0;
                    break;
            }

            actionHistory.Stake = Stake;
            actionHistory.Bank = Bank;
            Actions.Add(actionHistory);
            return actionHistory.Play;
        }

        //public void WriteAll()
        //{
        //    WriteActions();
        //    WriteDraws();
        //}

        //public void WriteActions()
        //{
        //    string jsonString = JsonSerializer.Serialize(Actions);
            
        //    File.WriteAllText(fileName("actions"), jsonString);
        //}

        //public List<ActionHistory> ReadActions()
        //{
        //    List<ActionHistory> actionHistory = new();
        //    if (File.Exists(fileName("actions")))
        //    {
        //        string jsonString = File.ReadAllText(fileName("actions"));
        //        actionHistory = JsonSerializer.Deserialize<List<ActionHistory>>(jsonString)!;
        //    }
        //    return actionHistory;
        //}

        //public void WriteDraws()
        //{
        //    string jsonString = JsonSerializer.Serialize(Draws);
        //    File.WriteAllText(fileName("draws"), jsonString);
        //}

        //public List<DrawHistory> ReadDraws()
        //{
        //    List<DrawHistory> drawHistory = new();
        //    if (File.Exists(fileName("draws")))
        //    {
        //        string jsonString = File.ReadAllText(fileName("draws"));
        //        drawHistory = JsonSerializer.Deserialize<List<DrawHistory>>(jsonString)!;
        //    }
        //    return drawHistory;
        //}
    }
}
