using ConsoleTableExt;
using Npgsql;
using System.Configuration;

namespace Flashcards
{
    internal class DatabaseManager
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["Flashcards"].ConnectionString;
        public DatabaseManager()
        {
            this.CreateTableIfNotExists();
        }

        private void CreateTableIfNotExists()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    Console.Out.WriteLine("Opening connection");
                    conn.Open();

                    var stackTableCmd = conn.CreateCommand();
                    stackTableCmd.CommandText =
                        @"CREATE TABLE IF NOT EXISTS stack(
                            id SERIAL PRIMARY KEY,
                            name VARCHAR (1024) UNIQUE NOT NULL
                            )";
                    stackTableCmd.ExecuteNonQuery();

                    var flashcardTableCmd = conn.CreateCommand();
                    flashcardTableCmd.CommandText =
                        @"flashcards=> CREATE TABLE IF NOT EXISTS flashcard(
                            id SERIAL PRIMARY KEY,
                            question TEXT,
                            answer TEXT,
                            stack_id INTEGER REFERENCES stack(id) ON DELETE CASCADE
                            )";
                    flashcardTableCmd.ExecuteNonQuery();

                    var sessionTableCmd = conn.CreateCommand();
                    sessionTableCmd.CommandText =
                        @"CREATE TABLE IF NOT EXISTS session(
                            id SERIAL PRIMARY KEY,
                            created_date DATE NOT NULL DEFAULT NOW(),
                            score INTEGER,
                            stack_id INTEGER REFERENCES stack(id) ON DELETE CASCADE
                            )";
                    sessionTableCmd.ExecuteNonQuery();

                    var indexTableCmd = conn.CreateCommand();
                    indexTableCmd.CommandText =
                        @"CREATE INDEX IF NOT EXISTS date_b ON session(date)";
                    indexTableCmd.ExecuteNonQuery();

                    conn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        internal void ViewStudySessions()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var tableData = new List<List<object>> { new List<object> { "Date", "Score", "Stack" } };

                var viewSessionCmd = conn.CreateCommand();
                viewSessionCmd.CommandText = $"SELECT session.created_at as created_at, session.score as score, stack.name FROM session JOIN stack ON session.stack_id = stack.id";

                var reader = viewSessionCmd.ExecuteReader();

                while (reader.Read())
                {
                    var date = reader.GetDateTime(0);
                    var score = reader.GetInt32(1);
                    var stack = reader.GetString(2);

                    tableData.Add(new List<object> { date, score, stack });
                }

                ConsoleTableBuilder
                    .From(tableData)
                    .WithFormat(ConsoleTableBuilderFormat.Alternative)
                    .ExportAndWriteLine(TableAligntment.Left);

            }
        }

        internal void ViewStacks()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var tableData = new List<object> { "Name" };

                var viewStacksCmd = conn.CreateCommand();
                viewStacksCmd.CommandText = "SELECT * FROM stack";
                var reader = viewStacksCmd.ExecuteReader();

                while (reader.Read())
                {
                    var stackName = reader.GetString(1);
                    tableData.Add(stackName);
                }

                ConsoleTableBuilder
                    .From(tableData)
                    .WithFormat(ConsoleTableBuilderFormat.Alternative)
                    .ExportAndWriteLine(TableAligntment.Left);


            }
        }

        internal bool CheckStackExists(string stackName)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var checkCmd = conn.CreateCommand();
                checkCmd.CommandText = $"SELECT COUNT(*) FROM stack WHERE name='{stackName}'";

                int result = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (result > 0)
                    return true;

                return false;


            }
        }

        internal int GetStackId(string parsedStackName)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var getStackIdCmd = conn.CreateCommand();
                getStackIdCmd.CommandText = $"SELECT id From stack WHERE name='{parsedStackName}'";
                var reader = Convert.ToInt32(getStackIdCmd.ExecuteScalar());
                return reader;

            }
        }

        internal List<Flashcard> GetListOfFlashcards(int stackId)
        {
            List<Flashcard> flashcards = new List<Flashcard> { };

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var getFlashcardsCmd = conn.CreateCommand();
                getFlashcardsCmd.CommandText = $"SELECT * FROM flashcard WHERE stack_id={stackId}";
                var reader = getFlashcardsCmd.ExecuteReader();

                while (reader.Read())
                {
                    int Id = reader.GetInt32(0);
                    string question = reader.GetString(1);
                    string answer = reader.GetString(2);
                    flashcards.Add(new Flashcard(Id, question, answer, stackId));
                }
            }

            return flashcards;
        }

        internal void CreateFlashcard(Flashcard fc)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var createFlashcardCmd = conn.CreateCommand();
                createFlashcardCmd.CommandText = $"INSERT INTO flashcard(question,answer,stack_id) VALUES ('{fc.question}','{fc.answer}','{fc.stackId}')";
                createFlashcardCmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        internal void DeleteFlashcard(Flashcard deletedFC)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var deleteFlashcardCmd = conn.CreateCommand();
                deleteFlashcardCmd.CommandText = $"DELETE FROM flashcard WHERE id={deletedFC.Id}";
                deleteFlashcardCmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        internal void EditFlashcard(Flashcard editedFc)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var editFlashcardCmd = conn.CreateCommand();
                editFlashcardCmd.CommandText = $"UPDATE flashcard SET question='{editedFc.question}', answer='{editedFc.answer}' WHERE id={editedFc.Id}";
                editFlashcardCmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        internal void CreateStack(string stackName)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var createStackCmd = conn.CreateCommand();
                createStackCmd.CommandText = $"INSERT INTO stack(name) VALUES ('{stackName}')";
                createStackCmd.ExecuteNonQuery();

                conn.Close();

            }
        }

        internal void DeleteStack(string stackName)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var deleteStackCmd = conn.CreateCommand();
                deleteStackCmd.CommandText = $"DELETE FROM stack WHERE name='{stackName}'";
                deleteStackCmd.ExecuteNonQuery();

                conn.Close();
            }

        }

        internal void EditStack(string newStackName, string stackName)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var editStackCmd = conn.CreateCommand();
                editStackCmd.CommandText = $"UPDATE stack SET name='{newStackName}' WHERE name='{stackName}'";
                editStackCmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        internal void InsertNewSession(StudySession newSession)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var insertSessionCmd = conn.CreateCommand();
                insertSessionCmd.CommandText = $"INSERT INTO session(score,stack_id) VALUES ({newSession.score},{newSession.stack.stackId})";
                insertSessionCmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
