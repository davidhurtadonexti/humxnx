using System.ComponentModel.DataAnnotations.Schema;
using Access.Auth.Domain.Entities;
using Access.Auth.Infrastructure.DataBaseContext.Extensions;
using Access.Auth.Infrastructure.DataBaseContext.Relations;
using Microsoft.EntityFrameworkCore;

namespace Access.Auth.Infrastructure.DataBaseContext;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
    : base(options)
    {
    }
    public virtual DbSet<Enterprises> Enterprises { get; set; }
    public virtual DbSet<Menus> Menus { get; set; }
    public virtual DbSet<Modules> Modules { get; set; }
    public virtual DbSet<Profiles> Profiles { get; set; }
    public virtual DbSet<ProtectedData> ProtectedData { get; set; }
    public virtual DbSet<Resources> Resources { get; set; }
    public virtual DbSet<Users> Users { get; set; }
    public virtual DbSet<Profile_ProtectedData> Profile_ProtectedData { get; set; }
    public virtual DbSet<Users_Profiles> Users_Profiles { get; set; }
    public virtual DbSet<Profiles_Resources> Profiles_Resources { get; set; }
    public virtual DbSet<Tokens> Tokens { get; set; }
    [NotMapped]
    public virtual DbSet<LoginSP> LoginSP { get; set; }
    [NotMapped]
    public virtual DbSet<ProtectedDataSP> ProtectedDataSP { get; set; }

    public void EnsureMissingTablesCreated()
    {
        this.EnsureCreatingMissingTables();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Omitir la entidad LoginSP en el modelo de datos
        //modelBuilder.Ignore<LoginSP>();
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LoginSP>().HasNoKey(); // Define that LoginSP is a keyless entity
        // Llamada al método de instancia desde la clase base
        var relationsBase = new RelationsBase();
        relationsBase.OnModelRelations(modelBuilder);
        //OnModelCreatingPartial(modelBuilder);
    }
}