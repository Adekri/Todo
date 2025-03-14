using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using Microsoft.Data.Sqlite;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Reflection;
using System.IO;
using System.Windows;

namespace todo
{
    public class DataAccess
    {
        private static readonly string connectionString = $"Data Source={GetDatabasePath()}";

        private static string GetDatabasePath()
        {
            // Získání cesty k adresáři projektu
            var projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
            var databasePath = Path.Combine(projectDirectory, "todo.db");


            return databasePath;
        }

        public static List<Task> GetTasks()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var query = "SELECT * FROM Tasks";
            return connection.Query<Task>(query).ToList();
        }


        public static void AddTask(string content, DateTime? dueDate)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var query = "INSERT INTO Tasks (Content, DueDate) VALUES (@Content, @DueDate)";
            connection.Execute(query, new { Content = content, DueDate = dueDate });
        }

        public static void DeleteTask(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var query = "DELETE FROM Tasks WHERE Id = @Id";
            connection.Execute(query, new { Id = id });
        }

        public static void UpdateTaskState(int id, string state)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var query = "UPDATE Tasks SET State = @State WHERE Id = @Id";
            connection.Execute(query, new { State = state, Id = id });
        }

        public static void DeleteAllDoneTasks()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var query = "DELETE FROM Tasks WHERE State = @State";
            connection.Execute(query, new { State = "Done" });
        }
    }
}
