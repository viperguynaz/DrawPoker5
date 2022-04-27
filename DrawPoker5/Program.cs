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
var button = game.PlaceBets(1, game.Config.NumPlayers - 1);


// Draw
Console.WriteLine("----------- Draw -----------");
game.Draw();
game.Players.Where(p => p.IsActive).ToList().ForEach(p => 
{
    p.PrintHand();
});

// 2nd betting round
Console.WriteLine("----------- Bet 2nd Round -----------");
PrintBetHeader();
game.PlaceBets(2, button);

// Establish winner and print final hands
Console.WriteLine("----------- Final Hands -----------");
game.Players.Where(p => p.IsActive).OrderByDescending(p => p.Hand.Rank).ToList().ForEach(p =>
{
    p.PrintHand();
});

Console.WriteLine($"---- WINNER is # {game.Winner().Name} ----");

//TODO record scores

Console.WriteLine("Press enter to quit...");
Console.ReadLine();





