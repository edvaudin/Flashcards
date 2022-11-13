using Flashcards.Models;
using System.Configuration;

namespace Flashcards
{
    class Program
    {
        private static DAL dal = new DAL();
        public static void Main(string[] args)
        {
            List<Flashcard> flashcards = dal.GetFlashcardsInStack(1);
            string output = string.Empty;
            foreach (Flashcard flashcard in flashcards)
            {
                output += $"Prompt: {flashcard.prompt}. Answer: {flashcard.answer}.\n";
            }
            Console.WriteLine("Test" + output);
        }
    }
}
