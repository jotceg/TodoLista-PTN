using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoLista.Scripts.LoginScripts;
using TodoLista.Scripts.Tasks;

namespace TodoLista.Scripts
{
    public static class DatabaseManager
    {

        private const string userDataBase = "UserDataBase.accdb";
        private const string tasksListsDataBase = "TasksListsDataBase.accdb";
        private const string tasksDataBase = "TasksDataBase.accdb";
        private const string userDataBaseConnectionAndDataSetting = "Provider=Microsoft.ACE.OleDb.16.0; Data Source=" + userDataBase;
        private const string tasksListDataBaseConnectionAndDataSetting = "Provider=Microsoft.ACE.OleDb.16.0; Data Source=" + tasksListsDataBase;
        private const string tasksDataBaseConnectionAndDataSetting = "Provider=Microsoft.ACE.OleDb.16.0; Data Source=" + tasksDataBase;

        public static void AddTasksList(int UserId, string ListName)
        {
            OleDbConnection conn;
            OleDbCommand cmd;
            OleDbDataAdapter adapter;
            DataTable dt;

            using (conn = new OleDbConnection(tasksListDataBaseConnectionAndDataSetting))
            {
                string query = "INSERT INTO Lists (ListName, UserId) VALUES (@ListName, @UserId)";

                using (cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ListName", ListName);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            RetrieveUserData();
        }

        public static void RetrieveUserData()
        {
            int userId = 0;
            string userLogin = "", userPassword = "";

            OleDbConnection conn;
            //OleDbCommand cmd;
            OleDbDataAdapter adapter;
            DataTable dt;

            using (conn = new OleDbConnection(userDataBaseConnectionAndDataSetting))
            {
                string query = "SELECT Id, Name, UserPassword FROM Users WHERE Name = @Name AND UserPassword = @UserPassword";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", State.User.Login);
                    cmd.Parameters.AddWithValue("@UserPassword", State.User.Password);

                    conn.Open();

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = reader.GetInt32(reader.GetOrdinal("Id"));
                            userLogin = reader.GetString(reader.GetOrdinal("Name"));
                            userPassword = reader.GetString(reader.GetOrdinal("UserPassword"));
                        }
                    }
                }
            }

            List<TasksList> tasksLists = new List<TasksList>();

            using (conn = new OleDbConnection(tasksListDataBaseConnectionAndDataSetting))
            {
                string query = "SELECT * FROM Lists where UserId = @UserId";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Assuming your table has columns like "Id", "Task", "Completed"
                            string tasksListName = reader["ListName"].ToString();
                            int tasksListId = (int)reader["ListId"]; // Adjust data type based on actual column
                            List<Scripts.Tasks.Task> tasks = new List<Scripts.Tasks.Task>();

                            tasksLists.Add(new TasksList(userId, tasksListId, tasksListName, tasks));
                        }
                    }
                }
            }

            for (int i = 0; i < tasksLists.Count(); i++)
            {
                using (conn = new OleDbConnection(tasksDataBaseConnectionAndDataSetting))
                {
                    string query = "SELECT * FROM Tasks where ListId = @ListId";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ListId", tasksLists[i].Id);

                        conn.Open();

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Assuming your table has columns like "Id", "Task", "Completed"
                                string taskName = reader["Title"].ToString();
                                int taskId = (int)reader["TaskId"]; // Adjust data type based on actual column
                                string description = reader["Description"].ToString();
                                string priority = (string)reader["Priority"];
                                DateTime realizationDate = (DateTime)reader["RealizationDate"];

                                tasksLists[i].Tasks.Add(new Scripts.Tasks.Task(taskId, tasksLists[i].Id, taskName, description, priority, realizationDate));
                            }
                        }
                    }
                }
            }

            State.User = new User(userId, userLogin, userPassword, tasksLists);
        }
    }
}
