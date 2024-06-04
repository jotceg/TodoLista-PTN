using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoLista.Scripts.Tasks;

namespace TodoLista.Scripts.LoginScripts
{
    public class User
    {
        public int Id { get; }
        public string Login { get; }
        public string Password { get; }
        public List<TasksList> TasksLists { get; }

        public User(int Id, string Login, string Password, List<TasksList> TasksLists)
        {
            this.Id = Id;
            this.Login = Login;
            this.Password = Password;
            this.TasksLists = TasksLists;
        }
    }
}
