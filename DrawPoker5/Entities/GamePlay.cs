using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DrawPoker5.Entities
{
    public class GamePlay
    {
        public Guid Id { get; set; }
        public int Pot { get; set; }
        public int Wager { get; set; }
        public int PlayerCount { get; set; }
        public int RaiseCount { get; set; }
        public int Round { get; set; }

        public GameConfig Config { get; }
        public Deck Deck { get; set; }
        public List<Player> Players { get; set; }
        private Random random = new Random();

        public void AnteUp()
        {
            Players.ForEach(p => Pot += p.Ante(Config.Ante));
        }

        public void DealCards()
        {
            var cards = Deck.Deal(Config.NumPlayers, Config.HandSize);
            for (int i = 0; i < Config.NumPlayers; i++)
            {
                // create player
                Players[i].Hand.Cards = cards[i];
            }
        }


        // button should = last player to bet, controls the bet
        public int PlaceBets(int last2Bet)
        {
            var bettingOpen = true;
            Wager = 0;
            var player = Round == 1 ? 0 : last2Bet;
            var openingPlayer = player;
            PlayerAction play;

            while (bettingOpen)
            {
                var nextPlayer = NextActivePlayer(player);
                play = Players[player].EvalBet(Id, Round, last2Bet - player, Wager, Pot, RaiseCount, PlayerCount);
                Console.WriteLine($"\t{player}\t{Wager}\t{Pot}\t{RaiseCount}\t{PlayerCount}\t{play.Action}\t{play.Bet}\t{Players[player].Stake}\t{Players[player].Bank}\t{last2Bet}");
                switch (play.Action)
                {
                    case Player.Action.Bet:
                        last2Bet = player;
                        Wager += play.Bet;
                        Pot += play.Bet;
                        break;
                    case Player.Action.Call:
                        Pot += play.Bet;
                        if (nextPlayer == last2Bet) bettingOpen = false;
                        break;
                    case Player.Action.Fold:
                        //TODO - move this to scoring functions
                        //var bets = Players[ndx].Actions.Where(b => b.GameId == Id).ToList();
                        //bets.ForEach(bet => bet.Score = -Players[ndx].Stake);
                        PlayerCount--;
                        if (nextPlayer == last2Bet || PlayerCount == 1) bettingOpen = false;
                        break;
                    case Player.Action.Raise:
                        Pot += play.Bet + Wager;
                        Wager += play.Bet;
                        RaiseCount++;
                        last2Bet = player;
                        break;
                    default:  // Player.Action.Check
                        if (nextPlayer == openingPlayer && Wager == 0) bettingOpen = false;
                        break;
                }

                if (PlayerCount > 1)
                {
                    player = nextPlayer;
                }
                else
                {
                    bettingOpen = false;
                }
            }
            return last2Bet;
        }

        public int NextPlayer(int player) => player + 1 == Config.NumPlayers ? 0 : player + 1;
        public int NextActivePlayer(int player)
        {
            var next = NextPlayer(player);
            while (!Players[next].IsActive)
            {
                next = NextPlayer(next);
            }
            return next;
        }

        public void Draw()
        {
            Players.Where(p => p.IsActive).ToList().ForEach(p => p.Draw(Deck));
        }

        public Player Winner()
        {
            var orderedRanks = Players.Where(p => p.IsActive).OrderByDescending(p => p.Hand.Rank).ToList();
            if (orderedRanks.Count == 1) return orderedRanks[0];
            var p1 = orderedRanks[0];
            var p2 = orderedRanks[1];
            if (p1.Hand.Rank > p2.Hand.Rank) return p1;

            var playerCards = orderedRanks.Select(p => p.Hand.Cards.GroupBy(c => c.Rank).OrderByDescending(g => g.Count()).ThenByDescending(g => g.Key).ToList()).ToList();
            var c1 = playerCards[0];
            var c2 = playerCards[1];

            Player? winner = null;
            int i = 0;
            while (i < c1.Count && winner == null)
            {
                if (c1[i].Key > c2[i].Key) winner = p1;
                if (c1[i].Key < c2[i].Key) winner = p2;
                i++;
            }

            return winner != null ? winner : random.Next(2) == 0 ? p1 : p2;
        }

        public GamePlay()
        {
            Id = Guid.NewGuid();
            Config = new GameConfig();
            Wager = 0;
            Pot = 0;
            RaiseCount = 0;
            Round = 1;

            PlayerCount = Config.NumPlayers;
            Deck = new Deck(true);  // build and shuffle card deck
            Players = new List<Player>(Config.NumPlayers);

        }

        public GamePlay(bool readPlayers) : this()
        {
            if (readPlayers)
            {
                List<string> fileEntries = Directory.GetFiles("data").ToList();
                int i = 0;
                fileEntries.ForEach(file =>
                {
                    var player = JsonSerializer.Deserialize<Player>(File.ReadAllText(file))!;
                    player.Name = i.ToString();
                    player.IsActive = true;
                    Players.Add(player);
                    i++;
                });
            }
            else
            {
                for (int i = 0; i < Config.NumPlayers; i++)
                {
                    // create player
                    Players.Add(new Player(i.ToString()));
                }
            }            
        }
    }
}
