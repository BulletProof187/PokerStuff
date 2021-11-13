using System;
using static OddsTrainer.Trainer;


namespace OddsTrainer
{

    class Program
    {

        public static void Main(string[] args)
        {
            Mode = TrainingMode.None;
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
                    Console.WriteLine("Lets start sum shit");
                    ConsoleUI.UserInput(Console.ReadLine(), out string _);
                    break;
            }
        }
    }
}
