using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    static class Viewer
    {
        public static void DisplayOptionsMenu()
        {
            Console.WriteLine("\nChoose an action from the following list:");
            Console.WriteLine("\tr - Revise");
            Console.WriteLine("\tv - View study sessions");
            Console.WriteLine("\ta - Add new stack or flashcard");
            Console.WriteLine("\td - Delete a stack or flashcard");
            Console.WriteLine("\tu - Update a stack or flashcard");
            Console.WriteLine("\t0 - Quit this application");
            Console.Write("Your option? ");
        }

        public static void DisplayTitle()
        {
            Console.WriteLine("Flashcards");
            Console.WriteLine("+-+-+-+-+-+-+-+-+");
        }
    }
}
