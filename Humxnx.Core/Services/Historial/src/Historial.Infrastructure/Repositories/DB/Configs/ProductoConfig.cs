using Humxnx.Historial.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Humxnx.Historial.Core.Infrastructure.Repositories.DB.Configs;

internal class ProductoConfig : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        // builder.ToTable("tblProductos");
        builder.HasKey(c => c.productoId);

        builder
            .HasMany(producto => producto.VentaDetalles)
            .WithOne(detalle => detalle.Producto);
    }
}