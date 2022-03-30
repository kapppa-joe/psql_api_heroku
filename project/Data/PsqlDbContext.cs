using Microsoft.EntityFrameworkCore;
using project.Models;

namespace project.Data;

public class PsqlDbContext : DbContext
{
    public PsqlDbContext(DbContextOptions<PsqlDbContext> options) : base(options)
    {
        // comment out this as it would cause issue if we need to run migration in future.
        // this.Database.EnsureCreated();
    }

    public DbSet<Note> Notes => Set<Note>();
}