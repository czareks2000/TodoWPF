namespace Todo.Models
{
    public class SubTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TaskId { get; set; }
        public TaskStatus Status { get; set; }

        public virtual Task Task { get; set; }
    }
}
