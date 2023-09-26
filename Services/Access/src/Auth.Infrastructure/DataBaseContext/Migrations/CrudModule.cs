using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Access.Auth.Infrastructure.DataBaseContext.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("CustomMigration_CrudModule")]
    public class CrudModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
				/*************************************************************************                
				 Nombre    : CrudModule
				 Creado por   : Ronald Javier Rivera Carranza                
				 Fecha de creación : 06-Sep-2023           
				 Ej de ejecución  :                
                
				 exec CrudModule 3, 'CB1925E6-B71B-4A0A-A13B-E80D0E1E2871', '', '', ''
                
				 Crud module, ingreso, actualizacion y eliminar     
				  ------------------- ---------------------------------------------------------------------- ------------------    
				 FECHA MODIFICACION						MOTIVO													  USUARIO    
				 ------------------- ----------------------------------------------------------------------- ------------------    	
					 06-Sep-2023             Se crea el proceso para crud                                       Ronald Rivera
					 06-Sep-2023             Eliminar fechas                                                    Ronald Rivera
				**************************************************************************/
				CREATE PROCEDURE dbo.CrudModule(
					@Accion int,
					@id uniqueidentifier,
					@enterprise_id uniqueidentifier = NULL,
					@name nvarchar(50) = NULL,
					@url nvarchar(50) = NULL
				)
				AS
				BEGIN
					--insert
					IF @Accion = 1
						BEGIN
							INSERT INTO Modules 
							(id, 
							enterprise_id, 
							name,
							url,
							created_at, 
							updated_at, 
							deleted_at)
							VALUES(@id, 
							@enterprise_id, 
							@name, 
							@url,
							getdate(), 
							null, 
							null);
							SELECT * FROM Modules WHERE id=@id;
						END
					--update
					IF @Accion = 2
						BEGIN
							UPDATE Modules 
							SET id=@id, 
							enterprise_id=@enterprise_id, 
							name=@name, 
							url=@url,
							updated_at=getdate()
							WHERE id=@id;
							SELECT * FROM Modules WHERE id=@id;
						END
					--delete logic
					IF @Accion = 3
						BEGIN
							UPDATE Modules 
							SET 
							updated_at=getdate(), 
							deleted_at=getdate()
							WHERE id=@id;
							SELECT * FROM Modules WHERE id=@id;
						END
				END;
                ");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.CrudModule");
        }
    }
}

