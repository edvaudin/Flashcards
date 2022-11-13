using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Models
{
    internal class Study
    {
        public int id = 0;
        public int stackId = 0;
        public DateTime date = DateTime.MinValue;
        public float score = 0;

        public Study() { }

        public Study(SqlDataReader reader)
        {
            id = reader.GetInt16(reader.GetOrdinal("id"));
            stackId = reader.GetInt16(reader.GetOrdinal("stack_id"));
            date = reader.GetDateTime(reader.GetOrdinal("date"));
            score = reader.GetFloat(reader.GetOrdinal("score"));
        }
    }
}
