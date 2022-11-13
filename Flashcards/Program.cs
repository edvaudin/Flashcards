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
                case "s":
                    Study();
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
            Console.WriteLine("Woud you like to delete a stack or a flashcard? (Type s or f)");
            string option = UserInput.ChooseStackOrFlashcard();
            if (option == "s")
            {
                ViewStacks();
                Console.WriteLine("Which stack do you want to delete? (Type its ID)");
                int id = UserInput.GetStackId();
                if (id == -1) { return; }
                dal.DeleteStack(id);
                Console.WriteLine($"\nSuccessfully deleted stack #{id}.");
            }
            else if (option == "f")
            {
                ViewStacks();
                Console.WriteLine("Which stack do you want to delete a flashcard from?");
                int stackId = UserInput.GetStackId();
                if (stackId == -1) { return; }
                Console.WriteLine("Which flashcard do you want to delete? (Type its ID)");
                ViewFlashcards(stackId);
                int flashcardId = UserInput.GetFlashcardId(stackId);
                if (flashcardId == -1) { return; }
                dal.DeleteFlashcard(flashcardId, stackId);
                Console.WriteLine($"\nSuccessfully deleted a flashcard from the {dal.GetStackById(stackId).name} stack.");
            }
        }

        private static void Add()
        {
            Console.WriteLine("Woud you like to add a new stack or a new flashcard? (Type s or f)");
            string option = UserInput.ChooseStackOrFlashcard();
            if (option == "s")
            {
                Console.WriteLine("What is the name of your new stack?");
                string name = UserInput.GetDbFriendlyString();
                dal.AddStack(name);
                Console.WriteLine($"\nSuccessfully created a new stack called {name}.");
            }
            else if (option == "f")
            {
                ViewStacks();
                Console.WriteLine("Which stack do you want to add a flashcard to?");
                int id = UserInput.GetStackId();
                if (id == -1) { return; }
                Console.WriteLine("What will be the prompt/question on the flashcard?");
                string prompt = UserInput.GetDbFriendlyString();
                Console.WriteLine("What will be the answer on the flashcard?");
                string answer = UserInput.GetDbFriendlyString();
                dal.AddFlashcard(id, prompt, answer);
                Console.WriteLine($"\nSuccessfully created a new flashcard in the {dal.GetStackById(id).name} stack.");
            }
        }

        private static void ViewStudies()
        {
            throw new NotImplementedException();
        }

        private static void Study()
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
            StartStudySession(flashcards);
        }

        private static void StartStudySession(List<Flashcard> flashcards)
        {
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

        private static float GetScorePercentage(int score, int maximum) => ((float)score / (float)maximum) * 100;

        private static void ViewStacks()
        {
            List<FlashcardStack> stacks = dal.GetStacks();
            var tableData = new List<List<object>>();
            foreach (FlashcardStack stack in stacks)
            {
                tableData.Add(new List<object> { stack.id, stack.name });
            }
            ConsoleTableBuilder.From(tableData).WithTitle("Your Stacks").WithColumn("Id", "Name").ExportAndWriteLine();
        }

        private static void ViewFlashcards(int stackId)
        {
            List<Flashcard> flashcards = dal.GetFlashcardsInStack(stackId);
            var tableData = new List<List<object>>();
            foreach (Flashcard flashcard in flashcards)
            {
                tableData.Add(new List<object> { flashcard.id, flashcard.prompt, flashcard.answer });
            }
            ConsoleTableBuilder.From(tableData).WithTitle($"Flashcards in {dal.GetStackById(flashcards[0].stackId).name}").WithColumn("Id", "Prompt", "Answer").ExportAndWriteLine();
        }
    }
}
