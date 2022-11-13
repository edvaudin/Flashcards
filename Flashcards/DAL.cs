using Flashcards.Models;
using System;
using System.Collections;
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

        public void AddStack(string name)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO stacks (name) VALUES (@name);";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteStack(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM stacks WHERE id = @id;";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        // Technically, we do not need the stackId here yet. But we will when we want to change the IDs of the flashcards to be ordered.
        public void DeleteFlashcard(int flashcardId, int stackId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM flashcards WHERE id = @id AND stack_id = @stack_id;";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", flashcardId);
                cmd.Parameters.AddWithValue("@stack_id", stackId);
                cmd.ExecuteNonQuery();
            }
        }

        public List<FlashcardStack> GetStacks()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM stacks;";
                SqlCommand cmd = new SqlCommand(sql, conn);
                return GetQueriedList(cmd, reader => new FlashcardStack(reader));
            }
        }

        public FlashcardStack GetStackById(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT TOP 1 * FROM stacks WHERE id = @id;";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                return GetQueriedResult(cmd, reader => new FlashcardStack(reader));
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

        protected static T GetQueriedResult<T> (SqlCommand cmd, Func<SqlDataReader, T> creator) where T : new()
        {
            T result = new T();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = creator(reader);
                }
            }
            return result;
        }

        public void AddFlashcard(int id, string prompt, string answer)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO flashcards (stack_id, prompt, answer) VALUES (@stack_id, @prompt, @answer);";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@stack_id", id);
                cmd.Parameters.AddWithValue("@prompt", prompt);
                cmd.Parameters.AddWithValue("@answer", answer);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
