using System;
using System.Collections.Generic;
using System.Linq;

namespace OddsTrainer
{

    class Program
    {

        public static void Main(string[] args)
        {
            DeckDb.DeckInitialize();
            Card[] CardArray = new Card[2];
            Hand Hero = new(CardArray);

            Hero.Draw(2);

            Card[] CardArray2 = new Card[2];
            Hand Villain = new(CardArray2);

            int CardCounter = 0;
            foreach (Card card in Hero)
            {
                Console.WriteLine(card);
                CardCounter++;
            }
            Console.WriteLine();
            Console.WriteLine(CardCounter);
            Console.WriteLine();

            int DeckCounter = 0;
            foreach (Card card in DeckDb.Deck)
            {
                Console.WriteLine(card);
                DeckCounter++;
            }
            Console.WriteLine();
            Console.WriteLine(DeckCounter);
        }

        
        
    }

}
