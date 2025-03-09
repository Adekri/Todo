using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using todo;

namespace todo
{

    public class DataAccess
    {
        //public static List<Task> GetTasks()
        //{
        //    //using var connection = new SqlConnection(Helper.GetConnectionString("TodoListDB"));
        //    //connection.Open();

        //    //var query = "SELECT * FROM Tasks";
        //    //return connection.Query<Task>(query).ToList();
        //}

        public static void AddTask(string content)
        {
            //using var connection = new SqlConnection(Helper.GetConnectionString("TodoListDB"));
            //connection.Open();

            //var query = "INSERT INTO Tasks (Content, Priority) VALUES (@Content, @Priority)";
            //connection.Execute(query, new { Content = content });
        }

        public static void DeleteTask(int id)
        {
            //using var connection = new SqlConnection(Helper.GetConnectionString("TodoListDB"));
            //connection.Open();

            //var query = "DELETE FROM TASKS WHERE Id = @Id";
            //connection.Execute(query, new { Id = id });
        }
    }
    

}
