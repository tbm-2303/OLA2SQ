using System;

namespace OLA2SQ.Models
{
    public class ToDoTask
    {
        public long Id { get; set; }  // Unique identifier for the task
        public string? Title { get; set; }  // Title of the task
        public bool IsCompleted { get; set; }  // Status of the task
        public DateTime DueDate { get; set; }  // Deadline for the task
    }
}