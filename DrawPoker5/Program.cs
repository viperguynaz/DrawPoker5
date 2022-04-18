// See https://aka.ms/new-console-template for more information
using DrawPoker5.Entities;

static void PrintBetHeader() => Console.WriteLine($"\tindex\twager\tpot\traises\tplayers\taction\tbet\tstake\tbank\tbtn");
var game = new GamePlay();

// Ante
game.AnteUp();

// Deal
game.DealCards();
Console.WriteLine("-------- Hands ---------");
game.Players.ForEach(p => p.PrintHand());

// 1st betting round
Console.WriteLine("----------- Bet 1st Round -----------");
PrintBetHeader();
var button = game.PlaceBets(1, game.Config.NumPlayers);


// Draw
Console.WriteLine("----------- Draw -----------");
game.Players.Where(p => p.IsActive).ToList().ForEach(p => 
{
    Console.Write($"  {p.Name} ");
    p.Hand.Cards.OrderByDescending(card => card.Rank).ToList().ForEach(card => Console.Write($"{card.Rank,2}.{card.Suit}\t"));
    Console.Write($"{p.Hand.Rank}\r\n");
});

// 2nd betting round
Console.WriteLine("----------- Bet 2nd Round -----------");
PrintBetHeader();
game.PlaceBets(2, button);

//TODO establish winner


//TODO record scores

Console.WriteLine("Press enter to quit...");
Console.ReadLine();





