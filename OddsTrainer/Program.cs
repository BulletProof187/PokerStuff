using System;
using System.Collections.Generic;
using static OddsTrainer.Trainer;


namespace OddsTrainer
{

    class Program
    {

        public static void Main(string[] args) //test (not final)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Mode = TrainingMode.None;
            while (true)
            {
                switch (Mode)
                {
                    case TrainingMode.OutsCounter:
                        OutsCounter();
                        break;
                    case TrainingMode.CallFold:
                        CallFold();
                        break;
                    case TrainingMode.PotOdds:
                        PotOdds();
                        break;
                    default:
                        Console.WriteLine("Please, enter the game mode to start training. Enter Modes to see the list of modes and what they are.");
                        Console.WriteLine();
                        ConsoleUI.UserInput(Console.ReadLine(), out string _);
                        break;
                }
            }
        }
    }
}
