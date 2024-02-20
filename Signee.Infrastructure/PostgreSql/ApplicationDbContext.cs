using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Signee.Domain.Entities.Display;
using Signee.Domain.Identity;

namespace Signee.Infrastructure.PostgreSql;

public class ApplicationDbContext : IdentityUserContext<ApplicationUser>
{
    public DbSet<Display> Displays => Set<Display>();
        
    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
            
    }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure one-to-many relationship between ApplicationUser and Display
        // modelBuilder.Entity<ApplicationUser>()
        //     .HasMany(user => user.CreatedDisplays) 
        //     .WithOne(display => display.CreatedBy)
        //     .HasForeignKey(display => display.CreatedById)
        //     .IsRequired();
    }
}