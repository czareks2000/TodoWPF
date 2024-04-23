using Microsoft.EntityFrameworkCore;
using Todo.Models;
using Task = Todo.Models.Task;

namespace Todo.DB
{
    public class DataContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TaskCategory> TaskCategories { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost; Port=5432; User Id=admin; Password=secret; Database=todo");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TaskCategory>(x => x.HasKey(tc => new { tc.TaskId, tc.CategoryId }));

            builder.Entity<TaskCategory>()
                .HasOne(tc => tc.Task)
                .WithMany(t => t.Categories)
                .HasForeignKey(tc => tc.TaskId);

            builder.Entity<TaskCategory>()
                .HasOne(tc => tc.Category)
                .WithMany(c => c.Tasks)
                .HasForeignKey(tc => tc.CategoryId);
        }
    }
}
