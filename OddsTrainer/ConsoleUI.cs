using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OddsTrainer.Trainer;


namespace OddsTrainer
{
    static class ConsoleUI
    {
        public static void ShowModesList()
        {
            Console.WriteLine("The program has 3 training modes. Enter one of the commands listed below to switch to the corresponding mode:");
            Console.WriteLine();
            Console.WriteLine("Command: OutsCounter");
            Console.WriteLine("The basic outs counting trainer where the user is presented a hand and a board");
            Console.WriteLine("and is tasked to count and enter the number of outs to improve the existing hand.");
            Console.WriteLine();
            Console.WriteLine("Command: PotOdds");
            Console.WriteLine("The user is presented a hand and a board and is tasked to enter pot odds of the spot");
            Console.WriteLine("in either a percentage, decimal, basic fraction or relation format (e.g. \"33%\", \"0.33\", \"1/3\" or \"1:2\" are all accepted).");
            Console.WriteLine($"The margin of {errorMargin * 100}% or less is not considered an error.");
            Console.WriteLine("It can be changed by entering \"Margin\" followed by space and a number (e.g. \"Margin 15\" will change the margin to 15%)");
            Console.WriteLine();
            Console.WriteLine("Command: CallFold");
            Console.WriteLine("The user is presented a hand and a board with a pot and is facing an all-in bet.");
            Console.WriteLine("The task is to enter \"call\" or \"fold\" accodring to its profitability in a basic chip EV situation");
            Console.WriteLine("assuming that any outs to improve the hand guarantee victory.");

        }
        static public void ShowCommandList()
        {
            Console.WriteLine("The available commands are:");
            Console.WriteLine(""); 
            Console.WriteLine("Command: Modes");
            Console.WriteLine("Shows the list of training modes and commands to activate them.");
            Console.WriteLine();
            Console.WriteLine("Command: Margin __");
            Console.WriteLine("Changes the error margin for the PotOdds mode, where __ is a desired margin percentage.");
            Console.WriteLine();
            Console.WriteLine("Command: End");
            Console.WriteLine("Ends the active training session and shows the results.");
            Console.WriteLine();
            Console.WriteLine("Command: Results");
            Console.WriteLine("Shows the results of the active session so far without ending it.");
            Console.WriteLine();
            if (Mode == TrainingMode.CallFold)
            {
                Console.WriteLine("Commands for the current training mode:");
                Console.WriteLine();
                Console.WriteLine("Command: Call");
                Console.WriteLine("You identify the given spot as profitable to call");
                Console.WriteLine("Command: Fold");
                Console.WriteLine("You identify the given spot as NOT profitable to call");
                Console.WriteLine();
            }
            if (Mode == TrainingMode.OutsCounter)
            {
                Console.WriteLine("In your current training mode you must enter a number of outs.");
                Console.WriteLine();
            }
            if (Mode == TrainingMode.PotOdds)
            {
                Console.WriteLine("In your current training mode you must enter the pot odds of the spot");
                Console.WriteLine("in either a percentage, decimal, basic fraction or relation format");
                Console.WriteLine("(e.g. \"33%\", \"0.33\", \"1/3\" or \"1:2\" are all accepted).");
            }
        }
        static public void ShowResults(int roundingAccuracy = 0)
        {
            float results = MathF.Round((numberOfCorrectAnswers / numberOfTasks), roundingAccuracy);
            Console.WriteLine($"Number of tasks: {numberOfTasks}");
            Console.WriteLine($"Number of correct answers: {numberOfCorrectAnswers}");
            Console.WriteLine($"Correct answer percentage: {results}%");
        }
        static public void EndSession()
        {
            if (numberOfTasks == 0)
            {
                Console.WriteLine("The training session has not been started.");
                Console.WriteLine("Please enter \"Modes\" command to see the training modes.");
            }
            else
            {
                Mode = TrainingMode.None;
                Console.WriteLine("The session has been ended.");
                ShowResults();
                numberOfTasks = 0;
                numberOfCorrectAnswers = 0;
            }
        }
        static public void UserInput(string input, out string answer)
        {
            answer = null;
            input = input.ToLower();
            string firstPart = input.Split(' ').First();
            string secondPart = input.Split(' ').Last();
            if (!int.TryParse(secondPart, out int intInput))
            {
                Console.WriteLine("Invalid input");
                Console.WriteLine("Enter Help for command list");
            }
            bool hasInt = int.TryParse(firstPart, out int soleIntInput); //detecting sole int input
            
            if (hasInt == false) //setting up data for PotOdds training
            {
                if (firstPart.Contains('%'))
                {
                    firstPart.Where(x => x != '%');
                    if (double.TryParse(firstPart, out double filteredInput))
                    {
                        answer = (filteredInput / 100).ToString();
                    }
                }
                else if (firstPart.Contains('/'))
                {
                    string firstFilteredPart = firstPart.Split('/').First();
                    string secondFilteredPart = firstPart.Split('/').Last();
                    if (double.TryParse(firstFilteredPart, out double firstParsedPart) &&
                        double.TryParse(secondFilteredPart, out double secondParsedPart))
                    {
                        answer = (firstParsedPart / secondParsedPart).ToString();
                    }
                }
                else if (firstPart.Contains(':'))
                {
                    string firstFilteredPart = firstPart.Split(':').First();
                    string secondFilteredPart = firstPart.Split(':').Last();
                    if (double.TryParse(firstFilteredPart, out double firstParsedPart) &&
                        double.TryParse(secondFilteredPart, out double secondParsedPart))
                    {
                        answer = (firstParsedPart / (firstParsedPart + secondParsedPart)).ToString();
                    }
                }
                else
                {
                    double.TryParse(firstPart, out double checkedInput); //checking if the double is valid
                    answer = checkedInput.ToString();
                }

            }

            switch (firstPart)
            {
                case "modes":
                    ShowModesList();
                    break;
                case "margin":
                    SetErrorMargin(intInput);
                    break;
                case "end":
                    EndSession();
                    break;
                case "results":
                    ShowResults();
                    break;
                case "outscounter":
                    Mode = TrainingMode.OutsCounter;
                    Console.WriteLine("OutsCounter training session has started.");
                    Console.WriteLine("Enter the number of outs to improve the existing hand. Good luck!");
                    break;
                case "potodds":
                    Mode = TrainingMode.PotOdds;
                    Console.WriteLine("PotOdds training session has started.");
                    Console.WriteLine("Enter the pot odds of the spot in either a percentage, decimal, basic fraction or relation format");
                    Console.WriteLine("(e.g. \"33%\", \"0.33\", \"1/3\" or \"1:2\" are all accepted).");
                    Console.WriteLine($"The current error margin is set to {errorMargin * 100}%");
                    Console.WriteLine("Enter \"Margin\" followed by space and a number to change it (e.g. \"Margin 15\" will change the margin to 15%)");
                    break;
                case "callfold":
                    Mode = TrainingMode.CallFold;
                    Console.WriteLine("CallFold training session has started.");
                    Console.WriteLine("Enter \"call\" or \"fold\" accodring to the profitability in a basic chip EV situation.");
                    break;
                case "help":
                    ShowCommandList();
                    break;
                case "call":
                    if (Mode == TrainingMode.CallFold)
                        answer = firstPart;
                    else
                    {
                        Console.WriteLine("This command is only available in the CallFold mode.");
                    }
                    break;
                case "fold":
                    if (Mode == TrainingMode.CallFold)
                        answer = firstPart;
                    else
                    {
                        Console.WriteLine("This command is only available in the CallFold mode.");
                    }
                    break;
                case "":
                    if (Mode == TrainingMode.CallFold && hasInt)
                        answer = intInput.ToString();
                    else
                    {
                        Console.WriteLine("No valid command has been entered.");
                        ShowCommandList();
                    }
                    break;
                default:
                    Console.WriteLine("No valid command has been entered.");
                    ShowCommandList();
                    break;
            }
        }
    }
}
