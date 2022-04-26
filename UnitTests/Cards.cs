using DrawPoker5.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public static class Cards
    {
        public static List<Card> Clubs => Enumerable.Range(2, 14).Select(i => new Card(rank: i, suit: Card.Suits.Clubs)).ToList();
        public static List<Card> Hearts => Enumerable.Range(2, 14).Select(i => new Card(rank: i, suit: Card.Suits.Hearts)).ToList();
        public static List<Card> Diamonds => Enumerable.Range(2, 14).Select(i => new Card(rank: i, suit: Card.Suits.Diamonds)).ToList();
        public static List<Card> Spades => Enumerable.Range(2, 14).Select(i => new Card(rank: i, suit: Card.Suits.Spades)).ToList();

        private static Deck deck = new Deck(false);

        private static List<Card> cards = deck.Cards;

        #region card mnemonics
        public static Card C2 => cards[0];
        public static Card C3 => cards[1];
        public static Card C4 => cards[2];
        public static Card C5 => cards[3];
        public static Card C6 => cards[4];
        public static Card C7 => cards[5];
        public static Card C8 => cards[6];
        public static Card C9 => cards[7];
        public static Card C10 => cards[8];
        public static Card CJ => cards[9];
        public static Card CQ => cards[10];
        public static Card CK => cards[11];
        public static Card CA => cards[12];
        public static Card H2 => cards[13];
        public static Card H3 => cards[14];
        public static Card H4 => cards[15];
        public static Card H5 => cards[16];
        public static Card H6 => cards[17];
        public static Card H7 => cards[18];
        public static Card H8 => cards[19];
        public static Card H9 => cards[20];
        public static Card H10 => cards[21];
        public static Card HJ => cards[22];
        public static Card HQ => cards[23];
        public static Card HK => cards[24];
        public static Card HA => cards[25];
        public static Card D2 => cards[26];
        public static Card D3 => cards[27];
        public static Card D4 => cards[28];
        public static Card D5 => cards[29];
        public static Card D6 => cards[30];
        public static Card D7 => cards[31];
        public static Card D8 => cards[32];
        public static Card D9 => cards[33];
        public static Card D10 => cards[34];
        public static Card DJ => cards[35];
        public static Card DQ => cards[36];
        public static Card DK => cards[37];
        public static Card DA => cards[38];
        public static Card S2 => cards[39];
        public static Card S3 => cards[40];
        public static Card S4 => cards[41];
        public static Card S5 => cards[42];
        public static Card S6 => cards[43];
        public static Card S7 => cards[44];
        public static Card S8 => cards[45];
        public static Card S9 => cards[46];
        public static Card S10 => cards[47];
        public static Card SJ => cards[48];
        public static Card SQ => cards[49];
        public static Card SK => cards[50];
        public static Card SA => cards[51];
        #endregion

    }
}
