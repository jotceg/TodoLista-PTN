using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoLista.Scripts.Tasks
{
    public class Task
    {
        private int Id;
        private int ListId;
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public DateTime Date {  get; set; }

        public Task(int Id, int ListId, string Title, string Description, string Priority, DateTime Date)
        {
            this.Id = Id;
            this.ListId = ListId;
            this.Title = Title;
            this.Description = Description;
            this.Priority = Priority;
            this.Date = Date;
        }

        // title, priority (low, medium, high), description, date, etc
    }
}
