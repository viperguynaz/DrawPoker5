using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrawPoker5.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests;

namespace DrawPoker5.Entities.Tests
{
    [TestClass]
    public class GamePlayTests
    {
        private static GamePlay game = new();

        [TestInitialize]
        public void InitializeTests()
        {
            // setup player 0 & 1 as the only active players to test Winner
            for (int i = 2; i < game.Config.NumPlayers; i++)
            {
                game.Players[i].IsActive = false;
            }
        }

        [TestMethod]
        public void WinnerTest()
        {
            // HighCard x 2
            game.Players[0].Hand.Cards = new List<Card>() { Cards.Hearts[0], Cards.Clubs[2], Cards.Hearts[4], Cards.Spades[6], Cards.Diamonds[8] }; //  2H  4C  6H  8S 10D - HighCard 10
            game.Players[1].Hand.Cards = new List<Card>() { Cards.Spades[0], Cards.Diamonds[1], Cards.Spades[3], Cards.Clubs[5], Cards.Hearts[7] }; //  2S  3D  5S  7C  9H - HighCard  9
            Assert.AreEqual(game.Players[0], game.Winner());

            // OnePair x HighCard
            game.Players[0].Hand.Cards = new List<Card>() { Cards.Hearts[0], Cards.Clubs[0], Cards.Hearts[4], Cards.Spades[6], Cards.Diamonds[8] }; //  2H  2C  6H  8S 10D - OnePair(2) 10-high
            game.Players[1].Hand.Cards = new List<Card>() { Cards.Spades[0], Cards.Diamonds[1], Cards.Spades[3], Cards.Clubs[5], Cards.Hearts[7] }; //  2S  3D  5S  7C  9H - HighCard 9
            Assert.AreEqual(game.Players[0], game.Winner());

            // OnePair x 2 - #1 should win
            game.Players[0].Hand.Cards = new List<Card>() { Cards.Hearts[0], Cards.Clubs[0], Cards.Hearts[4], Cards.Spades[6], Cards.Diamonds[8] }; //  2H  2C  6H  8S 10D - OnePair(2) 10-high
            game.Players[1].Hand.Cards = new List<Card>() { Cards.Spades[0], Cards.Diamonds[0], Cards.Spades[3], Cards.Clubs[5], Cards.Hearts[9] }; //  2S  2D  5S  7C  9H - OnePair(2) 11-high
            Assert.AreEqual(game.Players[1], game.Winner());
        }
    }
}