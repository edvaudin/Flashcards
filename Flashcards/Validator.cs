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
            string[] validOptions = { "r", "v", "a", "d", "u", "0" };
            foreach (string validOption in validOptions)
            {
                if (input == validOption)
                {
                    return true;
                }
            }
            return false;
        }

        internal static bool IsValidStackId(int input)
        {
            DAL dal = new DAL();
            List<int> validIds = dal.GetStacks().Select(s => s.id).ToList();
            return validIds.Contains(input);
        }
    }
}
