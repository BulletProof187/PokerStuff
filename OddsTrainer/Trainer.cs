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
        private static void ShowPotOddsBreakdown(int outs, float potOdds, float outOdds)
        {
            Console.WriteLine($"The pot odds required for profitable call are {ToPercent(potOdds, 1)}, or 1:{Math.Round(1 / potOdds, 1)}");
            Console.WriteLine($"There is {outs} outs which gives {ToPercent(outOdds, 1)} chance for improvement,");
            Console.WriteLine($"therefore, call is profitable.");
        }
        public static void SetUpPotAndBet (out int pot, out int bet, out float potOdds)
        {
            Random randMultiplier = new();
            int multiplier = randMultiplier.Next(1, 20);

            Random rand0 = new();
            pot = rand0.Next(40, 1000) * 10;
            pot *= multiplier;

            Random rand1 = new();
            bet = pot / 2 + rand1.Next(-pot / 3, pot / 3);
            bet *= multiplier;

            potOdds = bet / (bet + pot);
        }
        public static void OutsCounter()
        {
            SetUpNewTask();
            Console.WriteLine($"Your hand is {Hero}.");
            Console.WriteLine($"The board is {Board}.");
            Console.WriteLine($"Enter the number of cards suitable to improve the current combination.");
            int outs = Hero.CountOuts();

            ConsoleUI.UserInput(Console.ReadLine(), out string answer);

            int parsedOuts = int.Parse(answer);
                if (parsedOuts == outs)
                {
                    numberOfCorrectAnswers++;
                    Console.WriteLine("Correct answer!");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Incorrect answer. Keep practicing!");
                    Console.WriteLine($"The actual number of outs is {outs}");
                }
        }
        public static void CallFold()
        {
            SetUpNewTask();
            SetUpPotAndBet(out int pot, out int bet, out float potOdds);
            Console.WriteLine($"Your hand is {Hero}.");
            Console.WriteLine($"The board is {Board}.");
            Console.WriteLine($"The pot is {pot}.");
            Console.WriteLine($"You are facing a bet of {bet}.");
            Console.WriteLine($"Please enter Call if the call is profitable in this chip EV spot, or enter Fold if calling is not profitable.");

            float outOdds;
            int outs = Hero.CountOuts();

            if (Board.Count() == 4)
            {
                outOdds = outs / DeckModel.Deck.Count;
            }
            else if (Board.Count() == 3)
            {
                outOdds = outs / DeckModel.Deck.Count * (Hero.CountOuts() - 1) / (DeckModel.Deck.Count - 1);
            }
            else throw new InvalidOperationException("The board size is invalid.");

            ConsoleUI.UserInput(Console.ReadLine(), out string answer);
            
            if (outOdds >= potOdds)
            {
                if (answer == "call")
                {
                    numberOfCorrectAnswers++;
                    Console.WriteLine("Correct answer!");
                    ShowPotOddsBreakdown(outs, potOdds, outOdds);
                }
                else if (answer == "fold")
                {
                    Console.WriteLine("Incorrect answer. Keep practicing!");
                    ShowPotOddsBreakdown(outs, potOdds, outOdds);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                if (answer == "call")
                {
                    Console.WriteLine("Incorrect answer. Keep practicing!");
                    ShowPotOddsBreakdown(outs, potOdds, outOdds);
                }
                else if (answer == "fold")
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
        public static void PotOdds() //finish this you bitch!!!
        {
            SetUpNewTask();

            SetUpPotAndBet(out int pot, out int bet, out float potOdds);

            Console.WriteLine($"Your hand is {Hero}.");
            Console.WriteLine($"The board is {Board}.");
            Console.WriteLine($"The pot is {pot}.");
            Console.WriteLine($"You are facing a bet of {bet}.");
            Console.WriteLine($"Enter the pot odds of the given spot. The current acceptable error margin is {errorMargin * 100}%.");

            ConsoleUI.UserInput(Console.ReadLine(), out string answer);

            if (!float.TryParse(answer, out float floatAnswer))
                throw new ArgumentException("UserInput didn't parse the inpit properly.");

            //checking if the answer is within error margin
            if (floatAnswer < potOdds + potOdds * errorMargin && floatAnswer > potOdds - potOdds * errorMargin)
            {
                numberOfCorrectAnswers++;
                Console.WriteLine("Correct answer!");
                Console.WriteLine($"The actual pot odds are {potOdds}.");
            }
            else
            {
                Console.WriteLine("Incorrect answer. Keep practicing!");
                Console.WriteLine($"The actual pot odds are {potOdds}.");
            }
        }
        
    }
}
