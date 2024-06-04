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
        public string Name { get; }

        public TasksList(int UserId, int Id, string Name)
        {
            this.UserId = UserId;
            this.Id = Id;
            this.Name = Name;
        }

    }
}
