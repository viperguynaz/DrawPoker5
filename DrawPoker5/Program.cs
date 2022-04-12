// See https://aka.ms/new-console-template for more information
using DrawPoker5.Entities;

static void Print(Card card) => Console.WriteLine($"{card.Rank,2} | {card.Suit}");

int numPlayers = 6;
int handSize = 5;
int ante = 1;
int maxBet = 5;
int maxRaise = 5;
int maxNumRaises = 4;
int wager = 0;
int pot = 0;
int raiseCount = 0;
int playerCount = numPlayers;

List<Player> players = new List<Player>();

Console.WriteLine("Card Deck ----- Build & Shuffle");
var deck = new Deck(true);

Console.WriteLine("----- Deal -----");
var cards = deck.Deal(numPlayers, handSize);

// setup the game
Console.WriteLine("-------- Hands ---------");
for(int i = 0; i < numPlayers; i++)
{
    // create player
    players.Add(new Player(i.ToString(), cards[i]));
    Console.Write($"  {i} ");
    cards[i].OrderByDescending(card => card.Rank).ToList().ForEach(card => Console.Write($"{card.Rank,2}.{card.Suit}\t"));
    Console.Write($"{players[i].Hand.Rank}\r\n");
    // ante
    players[i].Bank -= ante;
    pot += ante;
}

// 1st bet round
Console.WriteLine("----------- Bet 1st Round -----------");
var roundId = Guid.NewGuid();
var bettingOpen = true;
var round = 1;
var ndx = 0;
//TODO FIX button logic
var button = 0;  // track first person to bet or last to raise
PlayerAction play;
Console.WriteLine("----------- Action -----------");
Console.WriteLine($"\tindex\twager\tpot\traises\tplayers\taction\tbet\tstake\tbank\tbtn");

while (bettingOpen)
{
    if (players[ndx].BetHistory.Count == 0 || players[ndx].BetHistory.Last().Play.Action != Player.Actions.Fold)
    {
        play = players[ndx].EvalBet(roundId, round, ndx, wager, pot, raiseCount, playerCount);
        Console.WriteLine($"\t{ndx}\t{wager}\t{pot}\t{raiseCount}\t{playerCount}\t{play.Action}\t{play.Bet}\t{players[ndx].Stake}\t{players[ndx].Bank}\t{button}");
        switch (play.Action)
        {
            case Player.Actions.Bet:
                button = ndx;
                wager += play.Bet;
                pot += play.Bet;
                break;
            case Player.Actions.Call:
                pot += play.Bet;
                if (ndx == button) bettingOpen = false;
                break;
            case Player.Actions.Fold:
                var bets = players[ndx].BetHistory.Where(b => b.RoundId == roundId).ToList();
                bets.ForEach(bet => bet.Score = -players[ndx].Stake);
                playerCount--;
                if (ndx == button) bettingOpen = false;
                break;
            case Player.Actions.Raise:
                pot += play.Bet + wager;
                wager += play.Bet;
                raiseCount++;
                button = ndx;
                break;
            default:
                break;
        }
    }

    ndx++;
    if (ndx == numPlayers)
    {
        // loop back to start
        ndx = 0;
        // no bets - close round
        if (wager == 0) bettingOpen = false;
    }
    else if (ndx == button)
    { 
        // if we've reached the button we are done
        bettingOpen = false; 
    }
}

// get remaining players
var players2ndRound = players.Where(p => p.BetHistory.Last().Play.Action != Player.Actions.Fold).ToList();

//TODO Draw
Console.WriteLine("----------- Draw -----------");
players2ndRound.ForEach(p => 
{
    p.Hand.Cards.AddRange(deck.Draw(p.Draw()));
    Console.Write($"  {p.Name} ");
    p.Hand.Cards.OrderByDescending(card => card.Rank).ToList().ForEach(card => Console.Write($"{card.Rank,2}.{card.Suit}\t"));
    Console.Write($"{p.Hand.Rank}\r\n");
});

//TODO Bet round 2

//TODO record scores

Console.WriteLine("Press enter to quit...");
Console.ReadLine();





