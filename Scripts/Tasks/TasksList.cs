using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoLista.Scripts.Tasks
{
    public class TasksList
    {
        public int UserId { get; }
        public int Id { get; }
        public string Name { get; set; }

        public List<Task> Tasks { get; private set; }

        public TasksList(int UserId, int Id, string Name, List<Task> Tasks)
        {
            this.UserId = UserId;
            this.Id = Id;
            this.Name = Name;
            this.Tasks = Tasks;
        }

    }
}
