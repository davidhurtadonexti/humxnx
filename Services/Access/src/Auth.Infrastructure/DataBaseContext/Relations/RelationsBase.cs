using System;
using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.DataBaseContext.Relations
{
	public class RelationsBase
    {
        public void OnModelRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasOne(pr => pr.Enterprises)
                .WithMany(p => p.Users)
                .HasForeignKey(pr => pr.enterprise_id)
                .OnDelete(DeleteBehavior.NoAction); // Cambio aquí para que no se vaya en cascada la actualizacion o eliminacion

            modelBuilder.Entity<Resources>()
                .HasOne(pr => pr.Menus)
                .WithMany(p => p.Resources)
                .HasForeignKey(pr => pr.menu_id)
                .OnDelete(DeleteBehavior.NoAction); // Cambio aquí para que no se vaya en cascada la actualizacion o eliminacion

            modelBuilder.Entity<Menus>()
                .HasOne(pr => pr.Modules)
                .WithMany(p => p.Menus)
                .HasForeignKey(pr => pr.module_id)
                .OnDelete(DeleteBehavior.NoAction); // Cambio aquí para que no se vaya en cascada la actualizacion o eliminacion

            modelBuilder.Entity<Modules>()
                .HasOne(pr => pr.Enterprises)
                .WithMany(p => p.Modules)
                .HasForeignKey(pr => pr.enterprise_id)
                .OnDelete(DeleteBehavior.NoAction); // Cambio aquí para que no se vaya en cascada la actualizacion o eliminacion

            modelBuilder.Entity<Profiles>()
                .HasOne(pr => pr.Modules)
                .WithMany(p => p.Profiles)
                .HasForeignKey(pr => pr.module_id)
                .OnDelete(DeleteBehavior.NoAction); // Cambio aquí para que no se vaya en cascada la actualizacion o eliminacion

            modelBuilder.Entity<Users_Profiles>()
                .HasOne(pr => pr.Profiles)
                .WithMany(p => p.Users_Profiles)
                .HasForeignKey(pr => pr.profile_id)
                .OnDelete(DeleteBehavior.NoAction); // Cambio aquí para que no se vaya en cascada la actualizacion o eliminacion

            modelBuilder.Entity<Users_Profiles>()
                .HasOne(pr => pr.Users)
                .WithMany(r => r.Users_Profiles)
                .HasForeignKey(pr => pr.user_id)
                .OnDelete(DeleteBehavior.NoAction); // Cambio aquí para que no se vaya en cascada la actualizacion o eliminacion

            modelBuilder.Entity<Profiles_Resources>()
                .HasOne(pr => pr.Profiles)
                .WithMany(p => p.Profiles_Resources)
                .HasForeignKey(pr => pr.profile_id)
                .OnDelete(DeleteBehavior.NoAction); // Cambio aquí para que no se vaya en cascada la actualizacion o eliminacion

            modelBuilder.Entity<Profiles_Resources>()
                .HasOne(pr => pr.Resources)
                .WithMany(r => r.Profiles_Resources)
                .HasForeignKey(pr => pr.resources_id)
                .OnDelete(DeleteBehavior.NoAction); // Cambio aquí para que no se vaya en cascada la actualizacion o eliminacion

            modelBuilder.Entity<Profile_ProtectedData>()
                .HasOne(pr => pr.ProtectedData)
                .WithMany(r => r.Profile_ProtectedData)
                .HasForeignKey(pr => pr.protected_id)
                .OnDelete(DeleteBehavior.NoAction); // Cambio aquí para que no se vaya en cascada la actualizacion o eliminacion

            modelBuilder.Entity<Users>()
            .HasOne(pr => pr.Tokens)
            .WithOne(p => p.Users)
            .HasForeignKey<Users>(p => p.token_id)
            .OnDelete(DeleteBehavior.NoAction); // Cambio aquí para que no se vaya en cascada la actualizacion o eliminacion

        }
    }
}

