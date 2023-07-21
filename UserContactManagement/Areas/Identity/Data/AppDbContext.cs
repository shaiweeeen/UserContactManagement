using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using UserContactManagement.Areas.Identity.Data;
using UserContactManagement.Models;

namespace UserContactManagement.Areas.Identity.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<Contact> Contacts { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<User>(entity =>
        {
            entity.Property(x => x.FirstName).HasMaxLength(100);
            entity.Property(x => x.LastName).HasMaxLength(100);
        });

        builder.Entity<Contact>()
           .HasOne(c => c.User)                      // Navigation property to User entity
           .WithMany(u => u.Contacts)                // Navigation property to Contacts collection in User entity
           .HasForeignKey(c => c.UserId);            // Foreign key property in Contact entity
    }

   

}


