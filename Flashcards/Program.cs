using Flashcards.Models;
using System.Configuration;
using ConsoleTableExt;
using System.ComponentModel;

namespace Flashcards
{
    class Program
    {
        private static readonly DAL dal = new DAL();
        public static void Main(string[] args)
        {
            Viewer.DisplayTitle();
            while (true)
            {
                Viewer.DisplayOptionsMenu();
                string userInput = UserInput.GetUserOption();
                ProcessInput(userInput);
            }
        }

        private static void ProcessInput(string userInput)
        {
            switch (userInput)
            {
                case "r":
                    Revise();
                    break;
                case "v":
                    ViewStudies();
                    break;
                case "a":
                    Add();
                    break;
                case "d":
                    Delete();
                    break;
                case "u":
                    Update();
                    break;
                case "0":
                    Exit();
                    break;
                default:
                    break;
            }
        }

        private static void Exit()
        {
            Environment.Exit(0);
        }

        private static void Update()
        {
            throw new NotImplementedException();
        }

        private static void Delete()
        {
            throw new NotImplementedException();
        }

        private static void Add()
        {
            throw new NotImplementedException();
        }

        private static void ViewStudies()
        {
            throw new NotImplementedException();
        }

        private static void Revise()
        {
            ViewStacks();
            Console.WriteLine("Which stack would you like to revise?");
            int id = UserInput.GetStackId();
            if (id == -1) { return; }
            List<Flashcard> flashcards = dal.GetFlashcardsInStack(id);
            if (flashcards.Count == 0)
            {
                Console.WriteLine("This stack does not contain any flashcards yet! Try adding some from the menu.");
                return;
            }
            int score = 0;
            foreach (Flashcard flashcard in flashcards)
            {
                Console.WriteLine(flashcard.prompt);
                Console.Write("Your answer?: ");
                string answer = Console.ReadLine();
                if (answer == flashcard.answer)
                {
                    Console.WriteLine("Correct!\n");
                    score++;
                }
                else
                {
                    Console.WriteLine($"Sorry, the correct answer was: {flashcard.answer}\n");
                }
            }
            Console.WriteLine($"You have completed all the flashcards! You got {GetScorePercentage(score, flashcards.Count)}%");
        }

        private static float GetScorePercentage(int score, int maximum) => (score / maximum) * 100;

        private static void ViewStacks()
        {
            List<Stack> stacks = dal.GetStacks();
            var tableData = new List<List<object>>();
            foreach (Stack stack in stacks)
            {
                tableData.Add(new List<object> { stack.id, stack.name });
            }
            ConsoleTableBuilder.From(tableData).WithTitle("Your Stacks").WithColumn("Id", "Name").ExportAndWriteLine();
        }
    }
}
