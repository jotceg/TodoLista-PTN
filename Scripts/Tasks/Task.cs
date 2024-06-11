using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoLista.Scripts.Tasks
{
    public class Task
    {
        public int Id { get; } 

        private int ListId { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public DateTime Date {  get; set; }
        public bool isCompleted { get; set; }

        public int RealizationHour
        {
            get => Date.Hour;
            set => Date = new DateTime(Date.Year, Date.Month, Date.Day, value, Date.Minute, 0);
        }

        public int RealizationMinute
        {
            get => Date.Minute;
            set => Date = new DateTime(Date.Year, Date.Month, Date.Day, Date.Hour, value, 0);
        }

        public Task(int Id, int ListId, string Title, string Description, string Priority, DateTime Date, bool isCompleted)
        {
            this.Id = Id;
            this.ListId = ListId;
            this.Title = Title;
            this.Description = Description;
            this.Priority = Priority;
            this.Date = Date;
            this.isCompleted = isCompleted;
        }
    }
}
