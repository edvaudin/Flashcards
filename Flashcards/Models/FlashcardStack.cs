using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Models
{
    internal class FlashcardStack
    {
        public int id = 0;
        public string name = string.Empty;

        public FlashcardStack() { }
        public FlashcardStack(SqlDataReader reader)
        {
            id = reader.GetInt32(reader.GetOrdinal("id"));
            name = reader.GetString(reader.GetOrdinal("name"));
        }

        public override string ToString()
        {
            return $"[#{id}] {name}";
        }
    }
}
