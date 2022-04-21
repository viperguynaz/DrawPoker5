using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrawPoker5.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPoker5.Entities.Tests
{
    [TestClass]
    public class GamePlayTests
    {
        private static GamePlay game = new();

        [TestInitialize]
        public static void InitializeTests()
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
            Assert.Fail();
        }
    }
}