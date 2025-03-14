using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using Microsoft.Data.Sqlite;
using DocumentFormat.OpenXml.Wordprocessing;

namespace todo
{
    public class DataAccess
    {
        private static readonly string connectionString = "Data Source=todo.db";

        public static List<Task> GetTasks()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var query = "SELECT * FROM Tasks";
            return connection.Query<Task>(query).ToList();
        }

        public static void AddTask(string content)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var query = "INSERT INTO Tasks (Content) VALUES (@Content)";
            connection.Execute(query, new { Content = content});
        }

        public static void DeleteTask(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var query = "DELETE FROM Tasks WHERE Id = @Id";
            connection.Execute(query, new { Id = id });
        }
    }
}
