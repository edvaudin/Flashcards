using Flashcards.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Flashcards
{
    internal class DAL
    {
        private string connectionString;

        public DAL()
        {
            SetConectionString("flashcardapp");
        }

        private void SetConectionString(string name)
        {
            connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public List<Stack> GetStacks()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM stacks;";
                SqlCommand cmd = new SqlCommand(sql, conn);
                return GetQueriedList(cmd, reader => new Stack(reader));
            }
        }

        public List<Flashcard> GetFlashcardsInStack(int stackId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM flashcards WHERE stack_id = @stackId;";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@stackId", stackId);
                return GetQueriedList(cmd, reader => new Flashcard(reader));
            }
        }

        protected static List<T> GetQueriedList<T>(SqlCommand cmd, Func<SqlDataReader, T> creator)
        {
            List<T> results = new();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    results.Add(creator(reader));
                }
            }
            return results;
        }
    }
}
