using Humxnx.Historial.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Humxnx.Historial.Core.Infrastructure.Repositories.DB.Configs;

internal class VentaConfig : IEntityTypeConfiguration<Venta>
{
    public void Configure(EntityTypeBuilder<Venta> builder)
    {
        // builder.ToTable("tblVentas");
        builder.HasKey(c => c.ventaId);

        builder
            .HasMany(venta => venta.VentaDetalles)
            .WithOne(detalle => detalle.Venta);
    }
}