using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Infrastructure.DataBaseContext.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("CustomMigration_LinkResources")]
    public class LinkResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
				/*************************************************************************                
				 Nombre    : LinkResources
				 Creado por   : Ronald Javier Rivera Carranza                
				 Fecha de creación : 06-Sep-2023           
				 Ej de ejecución  :                
                
				 exec LinkResources 3, 'CB1925E6-B71B-4A0A-A13B-E80D0E1E2871', '', ''
                
				 Crud LinkResources, ingreso, actualizacion y eliminar     
				  ------------------- ---------------------------------------------------------------------- ------------------    
				 FECHA MODIFICACION						MOTIVO													  USUARIO    
				 ------------------- ----------------------------------------------------------------------- ------------------    	
					 06-Sep-2023             Se crea el proceso para crud                                       Ronald Rivera
					 06-Sep-2023             Eliminar fechas                                                    Ronald Rivera
				**************************************************************************/	
				CREATE PROCEDURE dbo.LinkResources(
					@Accion int,
					@id uniqueidentifier,
					@profile_id uniqueidentifier = NULL,
					@resources_id uniqueidentifier = NULL,
					@access nvarchar(5) = NULL
				)
				AS
				BEGIN
					--insert
					IF @Accion = 1
						BEGIN
							INSERT INTO Profiles_Resources 
							(id, 
							profile_id, 
							resources_id,
							access,
							created_at, 
							updated_at, 
							deleted_at)
							VALUES(@id, 
							@profile_id, 
							@resources_id, 
							@access,
							getdate(), 
							null, 
							null);
							SELECT * FROM Profiles_Resources WHERE id=@id;
						END
					--update
					IF @Accion = 2
						BEGIN
							UPDATE Profiles_Resources 
							SET id=@id, 
							profile_id=@profile_id, 
							resources_id=@resources_id,
							access=@access,
							updated_at=getdate()
							WHERE id=@id;
							SELECT * FROM Profiles_Resources WHERE id=@id;
						END
					--delete logic
					IF @Accion = 3
						BEGIN
							UPDATE Profiles_Resources 
							SET 
							updated_at=getdate(), 
							deleted_at=getdate()
							WHERE id=@id;
							SELECT * FROM Profiles_Resources WHERE id=@id;
						END
				END;
                ");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.LinkResources");
        }
    }
}

