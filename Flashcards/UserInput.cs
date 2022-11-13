using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    internal class UserInput
    {
        public static string GetUserOption()
        {
            string input = Console.ReadLine();
            while (!Validator.IsValidOption(input))
            {
                Console.Write("\nThis is not a valid input. Please enter one of the above options: ");
                input = Console.ReadLine();
            }
            return input;
        }

        internal static int GetStackId()
        {
            while (true)
            {
                if (Int32.TryParse(Console.ReadLine(), out int result))
                {
                    if (Validator.IsValidStackId(result) || result == -1)
                    {
                        return result;
                    }
                }
                Console.Write("\nThis is not a valid id, please enter a number or to return to main menu type '-1': ");
            }
        }
    }
}
