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
            game.Players[0].Hand.Cards = new List<Card>() { Cards.CK, Cards.HJ, Cards.D9, Cards.S7, Cards.S4 };
            game.Players[1].Hand.Cards = new List<Card>() { Cards.CQ, Cards.H10, Cards.H9, Cards.S6, Cards.C4 };
            game.Players[2].Hand.Cards = new List<Card>() { Cards.CJ, Cards.S9, Cards.H8, Cards.D7, Cards.C3 };
            game.Players[3].Hand.Cards = new List<Card>() { Cards.C10, Cards.S8, Cards.H7, Cards.D5, Cards.C2 };
            game.Players[4].Hand.Cards = new List<Card>() { Cards.C9, Cards.C6, Cards.S5, Cards.H4, Cards.D3 };
            game.Players[5].Hand.Cards = new List<Card>() { Cards.C8, Cards.C7, Cards.D6, Cards.S3, Cards.H2 };
        }

        [TestMethod]
        public void HighCard_Test()
        {
            // HighCard x 2
            Assert.AreEqual(game.Players[0], game.Winner());
        }

        [TestMethod]
        public void OnePair_Test()
        {
            // OnePair (kings) x HighCard
            game.Players[0].Hand.Cards[1] = Cards.HK;

            Assert.AreEqual(game.Players[0], game.Winner());
        }

        [TestMethod]
        public void OnePair_x2_Test()
        {
            // OnePair (#1 kings, #2 queens)
            game.Players[0].Hand.Cards = new List<Card>() { Cards.CK, Cards.HK, Cards.S9, Cards.S8, Cards.S7 };
            game.Players[1].Hand.Cards = new List<Card>() { Cards.DQ, Cards.SQ, Cards.H9, Cards.H8, Cards.H7 };

            Assert.AreEqual(game.Players[0], game.Winner());
        }

        [TestMethod]
        public void OnePair_x2_HighCard_Test()
        {
            // OnePair (kings) x 2 
            game.Players[0].Hand.Cards = new List<Card>() { Cards.CK, Cards.HK, Cards.S9, Cards.S8, Cards.S7 };
            game.Players[1].Hand.Cards = new List<Card>() { Cards.DK, Cards.SK, Cards.H9, Cards.H8, Cards.H6 };

            Assert.AreEqual(game.Players[0], game.Winner());
        }

        [TestMethod]
        public void OnePair_EqualHands_Test()
        {
            // OnePair x 2 & HighCard (equal hands) - #1 or #2 should win (random)
            game.Players[0].Hand.Cards = new List<Card>() { Cards.CK, Cards.HK, Cards.S9, Cards.S8, Cards.S7 };
            game.Players[1].Hand.Cards = new List<Card>() { Cards.DK, Cards.SK, Cards.H9, Cards.H8, Cards.H7 };

            Assert.AreNotEqual(game.Players[2], game.Winner());
            Assert.AreNotEqual(game.Players[3], game.Winner());
            Assert.AreNotEqual(game.Players[4], game.Winner());
            Assert.AreNotEqual(game.Players[5], game.Winner());
        }

        [TestMethod]
        public void ThreeOfAKind_vs_2Pair_Test()
        {
            // (#1 3 x kings, #2 2 x queens, 2 x jacks)
            game.Players[0].Hand.Cards = new List<Card>() { Cards.CK, Cards.HK, Cards.DK, Cards.S10, Cards.S9 };
            game.Players[1].Hand.Cards = new List<Card>() { Cards.CQ, Cards.DQ, Cards.SJ, Cards.SJ, Cards.C4 };

            Assert.AreEqual(game.Players[0], game.Winner());
        }

        [TestMethod]
        public void Straight_vs_ThreeOfAKind()
        {
            // (#1 straight, #2 3 x queens)
            game.Players[0].Hand.Cards = new List<Card>() { Cards.CK, Cards.HQ, Cards.DJ, Cards.S10, Cards.S9 };
            game.Players[1].Hand.Cards = new List<Card>() { Cards.CQ, Cards.DQ, Cards.SQ, Cards.S6, Cards.C4 };

            Assert.AreEqual(game.Players[0], game.Winner());
        }

        [TestMethod]
        public void Flush_vs_Straight()
        {
            // (#1 flush, #2 straight)
            game.Players[0].Hand.Cards = new List<Card>() { Cards.CK, Cards.CJ, Cards.C9, Cards.C7, Cards.C4 };
            game.Players[1].Hand.Cards = new List<Card>() { Cards.CQ, Cards.HJ, Cards.H10, Cards.S9, Cards.C8 };

            Assert.AreEqual(game.Players[0], game.Winner());
        }

        [TestMethod]
        public void FullHouse_vs_Flush()
        {
            // (#1 3 x kings, #2 2 x queens, 2 x jacks)
            game.Players[0].Hand.Cards = new List<Card>() { Cards.CK, Cards.HK, Cards.DK, Cards.SQ, Cards.DQ };
            game.Players[1].Hand.Cards = new List<Card>() { Cards.SJ, Cards.S10, Cards.S8, Cards.S7, Cards.S4 };

            Assert.AreEqual(game.Players[0], game.Winner());
        }

        [TestMethod]
        public void FourOfAKind_vs_FullHouse()
        {
            // (#1 3 x kings, #2 2 x queens, 2 x jacks)
            game.Players[0].Hand.Cards = new List<Card>() { Cards.CK, Cards.HK, Cards.DK, Cards.SK, Cards.D3 };
            game.Players[1].Hand.Cards = new List<Card>() { Cards.SJ, Cards.CJ, Cards.DJ, Cards.S7, Cards.H7 };

            Assert.AreEqual(game.Players[0], game.Winner());
        }

        [TestMethod]
        public void StraightFlush_vs_FourOfAKind()
        {
            // (#1 straight flush, #2 4 x jacks)
            game.Players[0].Hand.Cards = new List<Card>() { Cards.CK, Cards.CQ, Cards.CJ, Cards.C10, Cards.C9 };
            game.Players[1].Hand.Cards = new List<Card>() { Cards.SJ, Cards.CJ, Cards.DJ, Cards.HJ, Cards.H7 };

            Assert.AreEqual(game.Players[0], game.Winner());
        }

        [TestMethod]
        public void RoyalFlush_vs_StraightFlush()
        {
            // (#1 royal flush, #2 straight flush)
            game.Players[0].Hand.Cards = new List<Card>() { Cards.CA, Cards.CK, Cards.CQ, Cards.CJ, Cards.C10 };
            game.Players[1].Hand.Cards = new List<Card>() { Cards.SJ, Cards.S10, Cards.S9, Cards.S8, Cards.S7 };

            Assert.AreEqual(game.Players[0], game.Winner());
        }
    }
}