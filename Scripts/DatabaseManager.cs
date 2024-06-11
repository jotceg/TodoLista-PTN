using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoLista.Scripts.LoginScripts;
using TodoLista.Scripts.Tasks;
using System.Windows.Documents;
using System.Collections;
using System.Windows;
using System.IO;

namespace TodoLista.Scripts
{
    public static class DatabaseManager
    {
        private const string connectionAndDataSetting = "Provider=Microsoft.ACE.OleDb.16.0; Data Source=DataBase.accdb";

        public static bool DoesUserExist(string name)
        {
            //Connection References
            OleDbConnection conn;
            OleDbCommand cmd;
            OleDbDataAdapter adapter;
            DataTable dt;

            name = name.Trim(); // Do not take into account spaces in login 
            conn = new OleDbConnection(connectionAndDataSetting);
            string query = "SELECT COUNT(*) FROM Users WHERE  StrComp(Name ,@Name,0) = 0";

            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", name);

            conn.Open();
            int userCount = (int)cmd.ExecuteScalar(); // return the overall value of Users in DataBase  
            conn.Close();

            return userCount > 0;
        }
        public static void ImportDatabase(string filePath)
        {
            try
            {
                string destinationPath = Path.Combine(Environment.CurrentDirectory, "Database.accdb");

                ClearAllData();
                File.Copy(filePath, destinationPath, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas importu: {ex.Message}");
            }
        }

        private static void ClearAllData()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionAndDataSetting))
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    cmd.Connection = conn;
                    List<string> tables = new List<string> { "Users", "Lists", "Tasks" };

                    foreach (var table in tables)
                    {
                        cmd.CommandText = $"DELETE FROM {table}";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void ExportDatabase(string filePath)
        {
            string sourcePath = Path.Combine(Environment.CurrentDirectory, "Database.accdb");
            File.Copy(sourcePath, filePath, true);
        }

        public static void AddTasksList(int UserId, string ListName)
        {
            
            if (DoesListNameAlreadyExist(UserId, ListName))
            {
                MessageBox.Show("Lista o takiej nazwie już istnieje!");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "INSERT INTO Lists (ListName, UserId) VALUES (@ListName, @UserId)";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ListName", ListName);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            RetrieveUserData();
        }


        public static bool DoesTaskNameAlreadyExist(int listId, string taskName)
        {
            taskName = taskName.Trim(); // Remove leading and trailing spaces

            using (OleDbConnection conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "SELECT COUNT(*) FROM Tasks WHERE StrComp(Title, @TaskName, 0) = 0 AND ListId = @ListId";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TaskName", taskName);
                    cmd.Parameters.AddWithValue("@ListId", listId);

                    conn.Open();
                    int taskCount = (int)cmd.ExecuteScalar(); // return the count of matching tasks in the database
                    conn.Close();

                    return taskCount > 0;
                }
            }
        }

        public static bool DoesListNameAlreadyExist(int UserId, string ListName)
        {
            ListName = ListName.Trim(); // Remove leading and trailing spaces

            using (OleDbConnection conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "SELECT COUNT(*) FROM Lists WHERE StrComp(ListName, @ListName, 0) = 0 AND UserId = @UserId";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ListName", ListName);
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    conn.Open();
                    int listCount = (int)cmd.ExecuteScalar(); // return the count of matching lists in the database
                    conn.Close();

                    return listCount > 0;
                }
            }
        }

        public static void RenameTasksList(int listId, int userId, string newName)
        {
            
            newName = newName.Trim();
           
            using (OleDbConnection conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "UPDATE Lists SET ListName = @NewName WHERE ListId = @ListId AND UserId = @UserId";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NewName", newName);
                    cmd.Parameters.AddWithValue("@ListId", listId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
 
            RetrieveUserData();
        }

        public static bool CheckPassword(string userName, string password)
        {
            //Connection References
            OleDbConnection conn;
            OleDbCommand cmd;
            OleDbDataAdapter adapter;
            DataTable dt;

            userName = userName.Trim();  // Do not take into account spaces in login 

            conn = new OleDbConnection(connectionAndDataSetting);

            string query = "SELECT COUNT(*) FROM Users WHERE  StrComp(Name, @Name,0) = 0 AND  StrComp(UserPassword ,@UserPassword, 0) = 0";
            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", userName);
            cmd.Parameters.AddWithValue("@UserPassword", password);

            conn.Open();
            int userCount = (int)cmd.ExecuteScalar();
            conn.Close();

            return userCount > 0;
        }

        public static void DeleteSingleTask(int taskId)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "DELETE FROM Tasks WHERE TaskId = @TaskId";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TaskId", taskId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            RetrieveUserData();
        }

        public static void RenameSingleTask(int taskId, string newTaskName)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "UPDATE Tasks SET Title = @Title WHERE TaskId = @TaskId";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", newTaskName);
                    cmd.Parameters.AddWithValue("@TaskId", taskId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            RetrieveUserData();
        }

        public static void DeleteTasksList(int UserId, int listId)
        {
            DeleteAllTasksFromList(listId);

            using (OleDbConnection conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "DELETE FROM Lists WHERE ListId = @ListId AND UserId = @UserId";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ListId", listId);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            RetrieveUserData();
        }
        public static void DeleteAllTasksFromList(int listId)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "DELETE FROM Tasks WHERE ListId = @ListId";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ListId", listId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void RetrieveUserData()
        {
            int userId = 0;
            string userLogin = "", userPassword = "", image = "";

            OleDbConnection conn;
          
            using (conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "SELECT Id, Name, UserPassword, UserImage FROM Users WHERE Name = @Name AND UserPassword = @UserPassword";
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
                            image = (string)reader["UserImage"];
                        }
                    }
                }
            }

            List<TasksList> tasksLists = new List<TasksList>();

            using (conn = new OleDbConnection(connectionAndDataSetting))
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
                using (conn = new OleDbConnection(connectionAndDataSetting))
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
                                bool isCompleted = (bool)reader["IsCompleted"];

                                tasksLists[i].Tasks.Add(new Scripts.Tasks.Task(taskId, tasksLists[i].Id, taskName, description, priority, realizationDate, isCompleted));
                            }
                        }
                    }
                }
            }

            State.User = new User(userId, userLogin, userPassword, tasksLists, image);
        }

        public static void UpdateTask(Scripts.Tasks.Task task)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "UPDATE Tasks SET Title = @Title, Description = @Description, Priority = @Priority, RealizationDate = @RealizationDate WHERE TaskId = @TaskId";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", task.Title);
                    cmd.Parameters.AddWithValue("@Description", task.Description);
                    cmd.Parameters.AddWithValue("@Priority", task.Priority);
                    cmd.Parameters.AddWithValue("@RealizationDate", task.Date);
                    cmd.Parameters.AddWithValue("@TaskId", task.Id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<Scripts.Tasks.Task> GetTasksForUser(int userId)
        {
            // Kod do pobierania zadań użytkownika z bazy danych
            List<Scripts.Tasks.Task> tasks = new List<Scripts.Tasks.Task>();

            using (OleDbConnection conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "SELECT * FROM Tasks WHERE UserId = @UserId";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int taskId = (int)reader["TaskId"];
                            int listId = (int)reader["ListId"];
                            string title = reader["Title"].ToString();
                            string description = reader["Description"].ToString();
                            string priority = reader["Priority"].ToString();
                            DateTime realizationDate = (DateTime)reader["RealizationDate"];
                            bool isCompleted = (bool)reader["IsCompleted"];

                            tasks.Add(new Scripts.Tasks.Task(taskId, listId, title, description, priority, realizationDate, isCompleted));
                        }
                    }
                }
            }

            return tasks;
        }

        public static void UpdateTask(int taskId, string title, string description, string priority, DateTime realizationDate)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "UPDATE Tasks SET Title = @Title, Description = @Description, Priority = @Priority, RealizationDate = @RealizationDate WHERE TaskId = @TaskId";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Priority", priority);
                    cmd.Parameters.AddWithValue("@RealizationDate", realizationDate);
                    cmd.Parameters.AddWithValue("@TaskId", taskId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            RetrieveUserData();
            MessageBox.Show($"Zadanie zaktualizowane!");
        }

        public static bool TryLoginUserAutomatically(string login, string password)
        {
            using (var conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "SELECT Id, Name, UserPassword, UserImage FROM Users WHERE Name = @Name AND UserPassword = @UserPassword";
                using (var cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", login);
                    cmd.Parameters.AddWithValue("@UserPassword", password);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = reader.GetInt32(reader.GetOrdinal("Id"));
                            string userLogin = reader.GetString(reader.GetOrdinal("Name"));
                            string userPassword = reader.GetString(reader.GetOrdinal("UserPassword"));
                            string image = reader.GetString(reader.GetOrdinal("UserImage"));

                            List<TasksList> tasksLists = GetUserTasksLists(userId);

                            State.User = new User(userId, userLogin, userPassword, tasksLists, image);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static User GetUserData(string login, string password)
        {
            int userId = 0;
            string userLogin = "", userPassword = "", image = "";

            using (var conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "SELECT Id, Name, UserPassword, UserImage FROM Users WHERE Name = @Name AND UserPassword = @UserPassword";
                using (var cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", login);
                    cmd.Parameters.AddWithValue("@UserPassword", password);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = reader.GetInt32(reader.GetOrdinal("Id"));
                            userLogin = reader.GetString(reader.GetOrdinal("Name"));
                            userPassword = reader.GetString(reader.GetOrdinal("UserPassword"));
                            image = (string)reader["UserImage"];
                        }
                    }
                }
            }

            if (userId > 0)
            {
                var tasksLists = GetUserTasksLists(userId);
                return new User(userId, userLogin, userPassword, tasksLists, image);
            }

            return null;
        }

        private static List<TasksList> GetUserTasksLists(int userId)
        {
            List<TasksList> tasksLists = new List<TasksList>();

            using (var conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "SELECT * FROM Lists where UserId = @UserId";

                using (var cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tasksListName = reader["ListName"].ToString();
                            int tasksListId = (int)reader["ListId"];
                            List<Scripts.Tasks.Task> tasks = new List<Scripts.Tasks.Task>();

                            tasksLists.Add(new TasksList(userId, tasksListId, tasksListName, tasks));
                        }
                    }
                }
            }

            for (int i = 0; i < tasksLists.Count; i++)
            {
                using (var conn = new OleDbConnection(connectionAndDataSetting))
                {
                    string query = "SELECT * FROM Tasks where ListId = @ListId";

                    using (var cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ListId", tasksLists[i].Id);

                        conn.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string taskName = reader["Title"].ToString();
                                int taskId = (int)reader["TaskId"];
                                string description = reader["Description"].ToString();
                                string priority = (string)reader["Priority"];
                                DateTime realizationDate = (DateTime)reader["RealizationDate"];
                                bool isCompleted = (bool)reader["IsCompleted"];

                                tasksLists[i].Tasks.Add(new Scripts.Tasks.Task(taskId, tasksLists[i].Id, taskName, description, priority, realizationDate, isCompleted));
                            }
                        }
                    }
                }
            }

            return tasksLists;
        }

        public static DataTable GetAllRegisteredUsers()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionAndDataSetting))
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Users", conn))
                {
                    DataTable dt = new DataTable();
                    conn.Open();
                    adapter.Fill(dt);

                    return dt;
                }
            }
        }

        public static void ClearData()
        {
            //Connection References
            OleDbConnection conn;
            OleDbCommand cmd;
            OleDbDataAdapter adapter;
            DataTable dt;

            conn = new OleDbConnection(connectionAndDataSetting);

            string query = "DELETE FROM Users";
            cmd = new OleDbCommand(query, conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void ImplementUserData(int userId, string userLogin, string userPassword, string userImage)
        {
            //Connection References
            OleDbConnection conn;
            OleDbCommand cmd;
            OleDbDataAdapter adapter;
            DataTable dt;

            List<TasksList> tasksLists = new List<TasksList>();

            using (conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "SELECT * FROM Lists where UserId = @UserId";

                using (cmd = new OleDbCommand(query, conn))
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
                using (conn = new OleDbConnection(connectionAndDataSetting))
                {
                    string query = "SELECT * FROM Tasks where ListId = @ListId";

                    using (cmd = new OleDbCommand(query, conn))
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
                                bool isCompleted = (bool)reader["IsCompleted"];

                                tasksLists[i].Tasks.Add(new Scripts.Tasks.Task(taskId, tasksLists[i].Id, taskName, description, priority, realizationDate, isCompleted));
                            }
                        }
                    }
                }
            }

            State.User = new User(userId, userLogin, userPassword, tasksLists, userImage);
        }

        public static void LoginUser(string name, string password)
        {
            //Connection References
            OleDbConnection conn;
            OleDbCommand cmd;
            OleDbDataAdapter adapter;
            DataTable dt;

            int userId = 0;
            string userLogin = "", userPassword = "", image = "";

            using (conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "SELECT Id, Name, UserPassword, UserImage FROM Users WHERE Name = @Name AND UserPassword = @UserPassword";
                using (cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@UserPassword", password);

                    conn.Open();

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = reader.GetInt32(reader.GetOrdinal("Id"));
                            userLogin = reader.GetString(reader.GetOrdinal("Name"));
                            userPassword = reader.GetString(reader.GetOrdinal("UserPassword"));
                            image = (string)reader["UserImage"];
                        }
                    }
                }
            }

            ImplementUserData(userId, userLogin, userPassword, image);
        }

        public static void MarkTaskAsCompleted(int id, bool isCompleted)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "UPDATE Tasks SET IsCompleted = @IsCompleted WHERE TaskId = @TaskId";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IsCompleted", isCompleted);
                    cmd.Parameters.AddWithValue("@TaskId", id);
                    conn.Open(); // Open Connetction With DataBase
                    cmd.ExecuteNonQuery(); // Allows to Insert Changes in DataBase
                }

                RetrieveUserData();
            }

        }

        public static void SetUserImage(string fileName)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionAndDataSetting))
            {
                //string query = "UPDATE Tasks SET Title = @Title, Description = @Description, Priority = @Priority, RealizationDate = @RealizationDate WHERE TaskId = @TaskId";
                string query = "UPDATE Users SET UserImage = @UserImage WHERE Id = @Id";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserImage", fileName);
                    cmd.Parameters.AddWithValue("@Id", State.User.Id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            RetrieveUserData();
        }

        public static void RegisterUser(string name, string password)
        {
            string userId;

            //Connection References
            OleDbConnection conn;
            OleDbCommand cmd;
            OleDbDataAdapter adapter;
            DataTable dt;

            using (conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "INSERT INTO Users (Name, UserPassword, UserImage) VALUES (@Name, @UserPassword, @UserImage)";

                using (cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name); // Add Parameters name to DataBase
                    cmd.Parameters.AddWithValue("@UserPassword", password);
                    cmd.Parameters.AddWithValue("@UserImage", "c1.png");

                    conn.Open(); // Open Connetction With DataBase
                    cmd.ExecuteNonQuery(); // Allows to Insert Changes in DataBase
                    //conn.Close(); // Close Connetction With DataBase

                    cmd.CommandText = "SELECT @@IDENTITY";
                    userId = cmd.ExecuteScalar().ToString();
                }
            }

            using (conn = new OleDbConnection(connectionAndDataSetting))
            {
                string query = "INSERT INTO Lists (ListName, UserId) VALUES (@ListName, @UserId)";

                using (cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ListName", "Dzisiaj");
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            LoginUser(name, password);
        }

        
    }
}