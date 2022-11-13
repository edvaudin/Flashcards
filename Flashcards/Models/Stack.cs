using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Models
{
    internal class Stack
    {
        public int id = 0;
        public string name = string.Empty;

        public Stack() { }
        public Stack(SqlDataReader reader)
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
