namespace Todo.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TaskCategory> Tasks { get; set; }
    }
}
