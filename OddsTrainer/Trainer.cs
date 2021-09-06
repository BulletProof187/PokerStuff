using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OddsTrainer.BoardConstructor;
using static OddsTrainer.Hand;

namespace OddsTrainer
{
    static class Trainer
    {
        public enum TrainingMode
        {
            OutsCounter, PotOdds, CallFold, None
        }
        public static float errorMargin;
        public static TrainingMode Mode;
        public static int numberOfTasks;
        public static int numberOfCorrectAnswers;

        private static string ToPercent(float dec, int precision = 0)
        {
            return $"{Math.Round(dec*100, precision)}%";
        }
        public static void SetErrorMargin (float marginInPercents)
        {
            errorMargin = marginInPercents / 100;
            Console.WriteLine($"Error margin has been set to {MathF.Round(errorMargin * 100, 2)}%");
        }
        private static void SetUpNewTask()
        {
            numberOfTasks++;
            Board.Cards.Clear();
            Hero.Cards.Clear();
            BuildTaskBoard();
            Hero.Draw(2);
        }
        public static void OutsCounter(string input)
        {
            SetUpNewTask();

            int outs = Hero.CountOuts();
            int parsedOuts = int.Parse(input);
                if (parsedOuts == outs)
                {
                    numberOfCorrectAnswers++;
                    Console.WriteLine("Correct answer!");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Incorrect answer.");
                    Console.WriteLine($"The actual number of outs is {outs}");
                }
        }
        private static void ShowPotOddsBreakdown(int outs, float potOdds, float outOdds)
        {
            Console.WriteLine($"The pot odds required for profitable call are {ToPercent(potOdds, 1)}, or 1:{Math.Round(1 / potOdds, 1)}");
            Console.WriteLine($"There is {outs} outs which gives {ToPercent(outOdds, 1)} chance for improvement,");
            Console.WriteLine($"therefore, call is profitable.");
        }
        public static void CallFold (string input)
        {
            SetUpNewTask();

            Random rand0 = new();
            int pot = rand0.Next(40, 1000) * 10;

            Random rand1 = new();
            int bet = pot / 2 + rand1.Next(-pot / 3, pot / 3);

            float potOdds = bet / (bet + pot);
            float outOdds;
            int outs = Hero.CountOuts();

            if (Board.Count() == 4)
            {
                outOdds = outs / DeckModel.Deck.Count();
            }
            else if (Board.Count() == 3)
            {
                outOdds = outs / DeckModel.Deck.Count() * (Hero.CountOuts() - 1) / (DeckModel.Deck.Count() - 1);
            }
            else throw new InvalidOperationException("The board size is invalid.");

            if (outOdds >= potOdds)
            {
                if (input == "call")
                {
                    numberOfCorrectAnswers++;
                    Console.WriteLine("Correct answer!");
                    ShowPotOddsBreakdown(outs, potOdds, outOdds);
                }
                else if (input == "fold")
                {
                    Console.WriteLine("Incorrect answer.");
                    ShowPotOddsBreakdown(outs, potOdds, outOdds);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                if (input == "call")
                {
                    Console.WriteLine("Incorrect answer.");
                    ShowPotOddsBreakdown(outs, potOdds, outOdds);
                }
                else if (input == "fold")
                {
                    numberOfCorrectAnswers++;
                    Console.WriteLine("Correct answer!");
                    ShowPotOddsBreakdown(outs, potOdds, outOdds);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }
        //public static void PotOdds(string input)
        
        /*public static void Training(string input) //to Program.Main when finished
        {
            switch (Mode)
            {
                case TrainingMode.OutsCounter:
                    

                    break;
                default:
                    break;
            }
            
        }*/
    }
}
