using Humxnx.Historial.Core.Domain.Entities;
using Humxnx.Historial.Core.Infrastructure.Repositories.DB.Configs;
using Microsoft.EntityFrameworkCore;

namespace Humxnx.Historial.Core.Infrastructure.Repositories.DB.Contexts;

public class VentaContexto : DbContext
{
    public DbSet<Producto> Productos { get; set; }

    public DbSet<Venta> Ventas { get; set; }

    public DbSet<VentaDetalle> VentaDetalles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // options.UseSqlServer("Server=tcp:serverHistorial.database.windows.net,1433;Initial Catalog=app-venta;Persist Security Info=False;User ID=adminHistorial;Password=Channel321*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ProductoConfig());
        builder.ApplyConfiguration(new VentaConfig());
        builder.ApplyConfiguration(new VentaDetalleConfig());
    }
}