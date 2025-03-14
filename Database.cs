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

                string createTableSql = @"
                    CREATE TABLE IF NOT EXISTS Tasks (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Content TEXT NOT NULL,
                        State TEXT DEFAULT 'Todo',
                        Date DATETIME DEFAULT CURRENT_TIMESTAMP
                    )";

                using (var command = new SqliteCommand(createTableSql, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Add the State column if it does not exist
                string alterTableSql = @"
                    ALTER TABLE Tasks
                    ADD COLUMN State TEXT DEFAULT 'Todo'";

                try
                {
                    using (var command = new SqliteCommand(alterTableSql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqliteException ex) when (ex.SqliteErrorCode == 1)
                {
                    // Ignore the error if the column already exists
                }
            }
        }
    }
}

