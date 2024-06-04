using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoLista.Scripts.Tasks
{
    internal class Task
    {
        private Guid Id;
        private Guid ListId;
        public string Title { get; set; }
        public string Description { get; set; }
        public ETaskPriority Priority { get; set; }
        public DateTime Date {  get; set; }

        public Task(Guid ListId, string Title, string Description, ETaskPriority Priority, DateTime Date)
        {
            Id = Guid.NewGuid();
            this.ListId = ListId;
            this.Title = Title;
            this.Description = Description;
            this.Priority = Priority;
            this.Date = Date;
        }

        // title, priority (low, medium, high), description, date, etc
    }
}
