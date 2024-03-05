using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Signee.Domain.Entities.Display;
using Signee.Domain.Entities.Group;
using Signee.Domain.Entities.Widget;
using Signee.Domain.Identity;

namespace Signee.Infrastructure.PostgreSql;
using View = Domain.Entities.View.View;
public class ApplicationDbContext : IdentityUserContext<ApplicationUser>
{
    public DbSet<Display> Displays => Set<Display>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<View> Views => Set<View>();
    public DbSet<Widget> Widgets => Set<Widget>();
    public DbSet<WidgetSettings> WidgetSettings => Set<WidgetSettings>();
    
        
    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
            
    }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // modelBuilder.Entity<Group>()
        //     .HasMany(g => g.Displays)
        //     .WithOne(d => d.Group)
        //     .IsRequired();

        modelBuilder.Entity<Group>().Navigation(g => g.Displays).AutoInclude();
    }
}