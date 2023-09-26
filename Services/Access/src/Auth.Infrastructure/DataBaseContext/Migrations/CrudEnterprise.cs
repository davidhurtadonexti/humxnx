using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Access.Auth.Infrastructure.DataBaseContext.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("CustomMigration_CrudEnterprise")]
    public class CrudEnterprise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
				/*************************************************************************                
				 Nombre    : CrudEnterprise        
				 Creado por   : Ronald Javier Rivera Carranza                
				 Fecha de creación : 06-Sep-2023           
				 Ej de ejecución  :                
                
				 exec CrudEnterprise 3, 'CB1925E6-B71B-4A0A-A13B-E80D0E1E2871', '', 0, ''    
                
				 Crud enterprise, ingreso, actualizacion y eliminar     
				  ------------------- ---------------------------------------------------------------------- ------------------    
				 FECHA MODIFICACION						MOTIVO													  USUARIO    
				 ------------------- ----------------------------------------------------------------------- ------------------    	
					 06-Sep-2023             Se crea el proceso para crud                                       Ronald Rivera
					 06-Sep-2023             Eliminar fechas                                                    Ronald Rivera
				**************************************************************************/  	
				CREATE PROCEDURE dbo.CrudEnterprise(
					@Accion int,
					@id uniqueidentifier,
					@name nvarchar(50) = NULL,
					@status int = NULL,
					@identification nvarchar(13) = NULL
				)
				AS
				BEGIN
					--insert
					IF @Accion = 1
						BEGIN
							INSERT INTO Enterprises
							(id, 
							name, 
							status, 
							identification, 
							created_at, 
							updated_at, 
							deleted_at)
							VALUES(@id, 
							@name, 
							@status, 
							@identification,
							getdate(), 
							null, 
							null);
							SELECT * FROM Enterprises WHERE id=@id;
						END
					--update
					IF @Accion = 2
						BEGIN
							UPDATE Enterprises 
							SET id=@id, 
							name=@name, 
							status=@status, 
							identification=@identification,
							updated_at=getdate()
							WHERE id=@id;
							SELECT * FROM Enterprises WHERE id=@id;
						END
					--delete logic
					IF @Accion = 3
						BEGIN
							UPDATE Enterprises 
							SET
							updated_at=getdate(), 
							deleted_at=getdate()
							WHERE id=@id;
							SELECT * FROM Enterprises WHERE id=@id;
						END
				END;
                ");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.CrudEnterprise");
        }
    }
}

