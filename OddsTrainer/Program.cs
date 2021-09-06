using System;
using System.Collections.Generic;
using System.Linq;
using static OddsTrainer.BoardConstructor;
using static OddsTrainer.DeckModel;
using static OddsTrainer.Hand;


namespace OddsTrainer
{

    class Program
    {

        public static void Main(string[] args)
        {
            ConsoleUI.UserInput("Margin 15", out _);
            Console.ReadLine();
        }
    }
}
