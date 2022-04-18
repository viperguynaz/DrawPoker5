﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPoker5.Entities
{
    public class GamePlay
    {
        public int Pot { get; set; }
        public int Wager { get; set; }
        public int PlayerCount { get; set; }
        public int RaiseCount { get; set; }
        public int Round { get; set; }

        public GameConfig Config { get; }
        public Deck Deck { get; set; }
        public List<Player> Players { get; set; }

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

        public int PlaceBets(int round, int button)
        {
            //TODO fix button logic
            var roundId = Guid.NewGuid();
            var bettingOpen = true;
            Wager = 0;
            var ndx = round == 1 ? 0 : button;
            PlayerAction play;
            while (bettingOpen)
            {
                if (Players[ndx].IsActive)
                {
                    play = Players[ndx].EvalBet(roundId, round, ndx, Wager, Pot, RaiseCount, PlayerCount);
                    Console.WriteLine($"\t{ndx}\t{Wager}\t{Pot}\t{RaiseCount}\t{PlayerCount}\t{play.Action}\t{play.Bet}\t{Players[ndx].Stake}\t{Players[ndx].Bank}\t{button}");
                    switch (play.Action)
                    {
                        case Player.Actions.Bet:
                            button = ndx;
                            Wager += play.Bet;
                            Pot += play.Bet;
                            break;
                        case Player.Actions.Call:
                            Pot += play.Bet;
                            if (ndx+1 == button) bettingOpen = false;
                            break;
                        case Player.Actions.Fold:
                            var bets = Players[ndx].Bets.Where(b => b.RoundId == roundId).ToList();
                            bets.ForEach(bet => bet.Score = -Players[ndx].Stake);
                            PlayerCount--;
                            if (ndx+1 == button) bettingOpen = false;
                            break;
                        case Player.Actions.Raise:
                            Pot += play.Bet + Wager;
                            Wager += play.Bet;
                            RaiseCount++;
                            button = ndx;
                            break;
                        default:
                            break;
                    }
                }

                ndx++;
                if (ndx == Config.NumPlayers)
                {
                    // loop back to start
                    ndx = 0;
                    // no bets - close round
                    if (Wager == 0) bettingOpen = false;
                }
                else if (ndx == button)
                {
                    // if we've reached the button we are done
                    bettingOpen = false;
                }
            }
            return button;
        }

        public void Draw()
        {
            Players.Where(p => p.IsActive).ToList().ForEach(p => p.Draw(Deck));
        }

        public GamePlay()
        {
            Config = new GameConfig();
            Wager = 0;
            Pot = 0;
            RaiseCount = 0;

            PlayerCount = Config.NumPlayers;
            Deck = new Deck(true);  // build and shuffle card deck
            Players = new List<Player>(Config.NumPlayers);
            for (int i = 0; i < Config.NumPlayers; i++)
            {
                // create player
                Players.Add(new Player(i.ToString()));
            }
        }
    }
}