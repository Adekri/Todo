using Microsoft.Data.Sqlite;
using System;
using System.Reflection;
using System.IO;
using System.Windows;

namespace todo
{
    class Database
    {

        private static string GetDatabasePath()
        {
            // Získání cesty k adresáři projektu
            var projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
            var databasePath = Path.Combine(projectDirectory, "todo.db");


            return databasePath;
        }

        public void CreateDatabase()
        {    // Připojovací řetězec k SQLite databázi
            string connectionString = $"Data Source={GetDatabasePath()}";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                // SQL příkaz pro vytvoření tabulky Tasks, pokud ještě neexistuje
                string createTableSql = @"
                    CREATE TABLE IF NOT EXISTS Tasks (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Content TEXT NOT NULL,
                        State TEXT DEFAULT 'Todo',
                        Date DATETIME DEFAULT CURRENT_TIMESTAMP,
                        DueDate DATETIME 

                    )";
                // Spuštění příkazu pro vytvoření tabulky

                using (var command = new SqliteCommand(createTableSql, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Pokus o přidání sloupce "State", pokud už tabulka existuje ale sloupec chybí
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
                }
            }
        }
    }
}

