using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Infrastructure.DataBaseContext.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("CustomMigration_CrudUser")]
    public class CrudUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
				/*************************************************************************                
				 Nombre    : CrudUser
				 Creado por   : Ronald Javier Rivera Carranza                
				 Fecha de creación : 06-Sep-2023           
				 Ej de ejecución  :                
                
				 exec CrudUser 3, 'CB1925E6-B71B-4A0A-A13B-E80D0E1E2871', '', '', '', '', 0
                
				 Crud user, ingreso, actualizacion y eliminar     
				  ------------------- ---------------------------------------------------------------------- ------------------    
				 FECHA MODIFICACION						MOTIVO													  USUARIO    
				 ------------------- ----------------------------------------------------------------------- ------------------    	
					 06-Sep-2023             Se crea el proceso para crud                                       Ronald Rivera
					 06-Sep-2023             Eliminar fechas                                                    Ronald Rivera
				**************************************************************************/	
				CREATE PROCEDURE dbo.CrudUser(
					@Accion int,
					@id uniqueidentifier,
					@enterprise_id uniqueidentifier = NULL,
					@username nvarchar(50) = NULL,
					@protected nvarchar(128) = NULL,
					@token_id uniqueidentifier = NULL,
					@logged bit = NULL
				)
				AS
				BEGIN
					--insert
					IF @Accion = 1
						BEGIN
							INSERT INTO Users 
							(id, 
							enterprise_id, 
							username,
							protected,
							token_id,
							logged,
							created_at, 
							updated_at, 
							deleted_at)
							VALUES(@id, 
							@enterprise_id, 
							@username,
							@protected,
							@token_id,
							@logged,
							getdate(), 
							null, 
							null);
							SELECT * FROM Users WHERE id=@id;
						END
					--update
					IF @Accion = 2
						BEGIN
							UPDATE Users 
							SET id=@id, 
							enterprise_id=@enterprise_id, 
							username=@username,
							protected=@protected,
							token_id=@token_id,
							logged=@logged,
							updated_at=getdate()
							WHERE id=@id;
							SELECT * FROM Users WHERE id=@id;
						END
					--delete logic
					IF @Accion = 3
						BEGIN
							UPDATE Users 
							SET 
							updated_at=getdate(), 
							deleted_at=getdate()
							WHERE id=@id;
							SELECT * FROM Users WHERE id=@id;
						END
				END;
                ");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.CrudUser");
        }
    }
}

