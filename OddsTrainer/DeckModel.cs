using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OddsTrainer.Card;

namespace OddsTrainer
{
    internal static class DeckModel
    {
        public static List<Card> Deck { get; set; } 
        static DeckModel()
        {
            Deck = new List<Card>();
        }
        public static void SetUpDeck()
        {
            Deck.Clear();
            foreach (SuitType s in Enum.GetValues(typeof(SuitType)))
            {
                foreach (EnumValue v in Enum.GetValues(typeof(EnumValue)))
                {
                    Card c = new(v, s);
                    Deck.Add(c);
                }
            }
        }
    }
}