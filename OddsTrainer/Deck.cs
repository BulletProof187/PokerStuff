using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddsTrainer
{
    internal class DeckDb
    {
        public static List<Card> Deck = new(52);
        static public void DeckInitialize()
        {
            Deck.Add(new Card(2, Card.SuitType.Clubs));
            Deck.Add(new Card(3, Card.SuitType.Clubs));
            Deck.Add(new Card(4, Card.SuitType.Clubs));
            Deck.Add(new Card(5, Card.SuitType.Clubs));
            Deck.Add(new Card(6, Card.SuitType.Clubs));
            Deck.Add(new Card(7, Card.SuitType.Clubs));
            Deck.Add(new Card(8, Card.SuitType.Clubs));
            Deck.Add(new Card(9, Card.SuitType.Clubs));
            Deck.Add(new Card(10, Card.SuitType.Clubs));
            Deck.Add(new Card(11, Card.SuitType.Clubs));
            Deck.Add(new Card(12, Card.SuitType.Clubs));
            Deck.Add(new Card(13, Card.SuitType.Clubs));
            Deck.Add(new Card(14, Card.SuitType.Clubs));

            Deck.Add(new Card(2, Card.SuitType.Diamonds));
            Deck.Add(new Card(3, Card.SuitType.Diamonds));
            Deck.Add(new Card(4, Card.SuitType.Diamonds));
            Deck.Add(new Card(5, Card.SuitType.Diamonds));
            Deck.Add(new Card(6, Card.SuitType.Diamonds));
            Deck.Add(new Card(7, Card.SuitType.Diamonds));
            Deck.Add(new Card(8, Card.SuitType.Diamonds));
            Deck.Add(new Card(9, Card.SuitType.Diamonds));
            Deck.Add(new Card(10, Card.SuitType.Diamonds));
            Deck.Add(new Card(11, Card.SuitType.Diamonds));
            Deck.Add(new Card(12, Card.SuitType.Diamonds));
            Deck.Add(new Card(13, Card.SuitType.Diamonds));
            Deck.Add(new Card(14, Card.SuitType.Diamonds));

            Deck.Add(new Card(2, Card.SuitType.Hearts));
            Deck.Add(new Card(3, Card.SuitType.Hearts));
            Deck.Add(new Card(4, Card.SuitType.Hearts));
            Deck.Add(new Card(5, Card.SuitType.Hearts));
            Deck.Add(new Card(6, Card.SuitType.Hearts));
            Deck.Add(new Card(7, Card.SuitType.Hearts));
            Deck.Add(new Card(8, Card.SuitType.Hearts));
            Deck.Add(new Card(9, Card.SuitType.Hearts));
            Deck.Add(new Card(10, Card.SuitType.Hearts));
            Deck.Add(new Card(11, Card.SuitType.Hearts));
            Deck.Add(new Card(12, Card.SuitType.Hearts));
            Deck.Add(new Card(13, Card.SuitType.Hearts));
            Deck.Add(new Card(14, Card.SuitType.Hearts));

            Deck.Add(new Card(2, Card.SuitType.Spades));
            Deck.Add(new Card(3, Card.SuitType.Spades));
            Deck.Add(new Card(4, Card.SuitType.Spades));
            Deck.Add(new Card(5, Card.SuitType.Spades));
            Deck.Add(new Card(6, Card.SuitType.Spades));
            Deck.Add(new Card(7, Card.SuitType.Spades));
            Deck.Add(new Card(8, Card.SuitType.Spades));
            Deck.Add(new Card(9, Card.SuitType.Spades));
            Deck.Add(new Card(10, Card.SuitType.Spades));
            Deck.Add(new Card(11, Card.SuitType.Spades));
            Deck.Add(new Card(12, Card.SuitType.Spades));
            Deck.Add(new Card(13, Card.SuitType.Spades));
            Deck.Add(new Card(14, Card.SuitType.Spades));
        }

                
    }

}