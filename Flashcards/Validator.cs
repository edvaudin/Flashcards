using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    internal class Validator
    {
        public static bool IsValidOption(string input)
        {
            string[] validOptions = { "s", "v", "a", "d", "u", "0" };
            foreach (string validOption in validOptions)
            {
                if (input == validOption)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsValidStackId(int input)
        {
            DAL dal = new DAL();
            List<int> validIds = dal.GetStacks().Select(s => s.id).ToList();
            return validIds.Contains(input);
        }

        public static bool IsValidFlashcardId(int input, int stackId)
        {
            DAL dal = new DAL();
            List<int> validIds = dal.GetFlashcardsInStack(stackId).Select(s => s.id).ToList();
            return validIds.Contains(input);
        }

        public static bool IsStackOrFlashcard(string input)
        {
            string[] validOptions = { "s", "f" };
            foreach (string validOption in validOptions)
            {
                if (input == validOption)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
