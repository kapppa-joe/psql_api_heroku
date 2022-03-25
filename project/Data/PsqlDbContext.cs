using Microsoft.EntityFrameworkCore;
using project.Models;

namespace project.Data;

public class PsqlDbContext : DbContext
{
    public PsqlDbContext(DbContextOptions<PsqlDbContext> options) : base(options)
    {
        this.Database.EnsureCreated();
    }

    public DbSet<Note> Notes => Set<Note>();
}