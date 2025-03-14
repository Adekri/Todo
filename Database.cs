using Microsoft.Data.Sqlite;
using System;


namespace todo
{
    class Database
    {
        public void CreateDatabase()
        {
            string connectionString = "Data Source=todo.db";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    CREATE TABLE IF NOT EXISTS Tasks (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Content TEXT NOT NULL,
                        State TEXT DEFAULT 'todo',
                        Date DATETIME DEFAULT CURRENT_TIMESTAMP
                    )";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
