namespace Todo.MVVM.Model
{
    public class TaskCategory
    {
        public int TaskId { get; set; }
        public int CategoryId { get; set; }

        public virtual Task Task { get; set; }
        public virtual Category Category { get; set; }
    }
}
