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
            InitCards();
        }

        private static void InitCards()
        {
            game.Players[0].Hand.Cards = new List<Card>() { Cards.CK,  Cards.HJ,  Cards.D9, Cards.S7, Cards.S4 };
            game.Players[1].Hand.Cards = new List<Card>() { Cards.CQ,  Cards.H10, Cards.H9, Cards.S6, Cards.C4 };
            game.Players[2].Hand.Cards = new List<Card>() { Cards.CJ,  Cards.S9,  Cards.H8, Cards.D7, Cards.C3 };
            game.Players[3].Hand.Cards = new List<Card>() { Cards.C10, Cards.S8,  Cards.H7, Cards.D5, Cards.C2 };
            game.Players[4].Hand.Cards = new List<Card>() { Cards.C9,  Cards.C6,  Cards.S5, Cards.H4, Cards.D3 };
            game.Players[5].Hand.Cards = new List<Card>() { Cards.C8,  Cards.C7,  Cards.D6, Cards.S3, Cards.H2 };
        }

        [TestMethod]
        public void WinnerTest()
        {
            // HighCard x 2
            Assert.AreEqual(game.Players[0], game.Winner());

            // OnePair (kings) x HighCard
            game.Players[0].Hand.Cards[1] = Cards.HK;
            Assert.AreEqual(game.Players[0], game.Winner());

            // OnePair (kings) x 2 
            game.Players[1].Hand.Cards[0] = Cards.DK;
            game.Players[1].Hand.Cards[1] = Cards.SK;
            Assert.AreEqual(game.Players[0], game.Winner());

            // OnePair x 2 (equal hands) & HighCard - #3 should NOT win
            game.Players[0].Hand.Cards[3] = Cards.H6;
            Assert.AreNotEqual(game.Players[2], game.Winner());
        }
    }
}