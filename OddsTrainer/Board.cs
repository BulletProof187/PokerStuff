using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddsTrainer
{
    internal static class BoardDb
    {
        private static Hand Board = null;

        public static void DrawBoard(int HowManyCards)
        {
            for (int i = 0; i < HowManyCards; i++)
            {
                var rand = new Random();
                Card r = DeckDb.Deck[rand.Next(DeckDb.Deck.Count)];
                DeckDb.Deck.Remove(r);
                Board.Append(r);
            }
        }
        public static void DrawBoard()
        {
            var rand = new Random();
            Card r = DeckDb.Deck[rand.Next(DeckDb.Deck.Count)];
            DeckDb.Deck.Remove(r);
            Board.Append(r);
        }
        public static void DrawSpecificBoardCard(Card card)
        {
            
        }
    }
}
