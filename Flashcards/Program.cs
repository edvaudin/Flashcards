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
                List<FlashcardStack> stacks = ViewStacks();
                Console.WriteLine("Which stack do you want to delete?");
                int index = UserInput.GetIndex(stacks) - 1;
                if (index == -1) { return; }
                dal.DeleteStack(stacks[index].id);
                Console.WriteLine($"\nSuccessfully deleted stack #{index}.");
            }
            else if (option == "f")
            {
                List<FlashcardStack> stacks = ViewStacks();
                Console.WriteLine("Which stack do you want to delete a flashcard from?");
                int index = UserInput.GetIndex(stacks) - 1;
                if (index == -1) { return; }
                Console.WriteLine("Which flashcard do you want to delete?");
                List<Flashcard> flashcards = ViewFlashcards(stacks[index].id);
                int flashcardIndex = UserInput.GetIndex(flashcards) - 1;
                if (flashcardIndex == -1) { return; }
                dal.DeleteFlashcard(flashcards[flashcardIndex].id, stacks[index].id);
                Console.WriteLine($"\nSuccessfully deleted a flashcard from the {dal.GetStackById(stacks[index].id).name} stack.");
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
                List<FlashcardStack> stacks = ViewStacks();
                Console.WriteLine("Which stack do you want to add a flashcard to?");
                int index = UserInput.GetIndex(stacks) - 1;
                if (index == -1) { return; }
                Console.WriteLine("What will be the prompt/question on the flashcard?");
                string prompt = UserInput.GetDbFriendlyString();
                Console.WriteLine("What will be the answer on the flashcard?");
                string answer = UserInput.GetDbFriendlyString();
                dal.AddFlashcard(stacks[index].id, prompt, answer);
                Console.WriteLine($"\nSuccessfully created a new flashcard in the {dal.GetStackById(stacks[index].id).name} stack.");
            }
        }

        private static void ViewStudies()
        {
            throw new NotImplementedException();
        }

        private static void Study()
        {
            List<FlashcardStack> stacks = ViewStacks();
            Console.WriteLine("Which stack would you like to revise?");
            int index = UserInput.GetIndex(stacks) - 1;
            if (index == -1) { return; }
            List<Flashcard> flashcards = dal.GetFlashcardsInStack(stacks[index].id);
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

        private static List<FlashcardStack> ViewStacks()
        {
            List<FlashcardStack> stacks = dal.GetStacks();
            var tableData = new List<List<object>>();
            foreach (FlashcardStack stack in stacks)
            {
                tableData.Add(new List<object> { stacks.IndexOf(stack) + 1, stack.name });
            }
            ConsoleTableBuilder.From(tableData).WithTitle("Your Stacks").WithColumn("Id", "Name").ExportAndWriteLine();
            return stacks;
        }

        private static List<Flashcard> ViewFlashcards(int stackId)
        {
            List<Flashcard> flashcards = dal.GetFlashcardsInStack(stackId);
            var tableData = new List<List<object>>();
            foreach (Flashcard flashcard in flashcards)
            {
                tableData.Add(new List<object> { flashcards.IndexOf(flashcard) + 1, flashcard.prompt, flashcard.answer });
            }
            ConsoleTableBuilder.From(tableData).WithTitle($"Flashcards in {dal.GetStackById(flashcards[0].stackId).name}").WithColumn("Id", "Prompt", "Answer").ExportAndWriteLine();
            return flashcards;
        }
    }
}
