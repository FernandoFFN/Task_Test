using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Task_test.Models;

namespace Task_test.Data
{
    public class APIDbContext : DbContext
    {
        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options) { }

        public DbSet<EditorTarefas> Tarefas { get; set; }

    }
}
