using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Models
{
    internal class Flashcard
    {
        public int id = 0;
        public int stackId = 0;
        public string prompt = string.Empty;
        public string answer = string.Empty;

        public Flashcard() { }
        public Flashcard(SqlDataReader reader)
        {
            id = reader.GetInt32(reader.GetOrdinal("id"));
            stackId = reader.GetInt32(reader.GetOrdinal("stack_id"));
            prompt = reader.GetString(reader.GetOrdinal("prompt"));
            answer = reader.GetString(reader.GetOrdinal("answer"));
        }
    }
}
