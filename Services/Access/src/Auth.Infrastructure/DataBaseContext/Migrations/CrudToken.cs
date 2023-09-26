using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Access.Auth.Infrastructure.DataBaseContext.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("CustomMigration_CrudToken")]
    public class CrudToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
				/*************************************************************************                
				 Nombre    : CrudToken         
				 Creado por   : Ronald Javier Rivera Carranza                
				 Fecha de creación : 06-Sep-2023           
				 Ej de ejecución  :                
                
				 exec CrudToken 3, 'CB1925E6-B71B-4A0A-A13B-E80D0E1E2871', '', '', '', '', 0    
                
				 Crud token, ingreso, actualizacion y eliminar     
				 ------------------- ---------------------------------------------------------------------- ------------------    
				 FECHA MODIFICACION						MOTIVO													  USUARIO    
				 ------------------- ---------------------------------------------------------------------- ------------------    	
					 06-Sep-2023             Se crea el proceso para crud                                      Ronald Rivera
					 06-Sep-2023             Eliminar fechas                                                   Ronald Rivera
				**************************************************************************/  

				CREATE PROCEDURE dbo.CrudToken(
					@Accion int,
					@id uniqueidentifier,
					@access_token nvarchar(600) = NULL,
					@expiration_token_time nvarchar(MAX) = NULL,
					@refresh_token nvarchar(600) = NULL,
					@expiration_refresh_token_time nvarchar(MAX) = NULL,
					@status int = NULL
				)
				AS
				BEGIN
					--insert
					IF @Accion = 1
						BEGIN
							INSERT INTO Tokens
							(id, 
							access_token, 
							expiration_token_time, 
							refresh_token, 
							expiration_refresh_token_time, 
							status, 
							created_at, 
							updated_at, 
							deleted_at)
							VALUES(@id, 
							@access_token, 
							@expiration_token_time, 
							@refresh_token, 
							@expiration_refresh_token_time, 
							@status, 
							getdate(), 
							null, 
							null);
							SELECT * FROM Tokens WHERE id=@id;
						END
					--update
					IF @Accion = 2
						BEGIN
							UPDATE Tokens 
							SET access_token=@access_token, 
							expiration_token_time=@expiration_token_time, 
							refresh_token=@refresh_token, 
							expiration_refresh_token_time=@expiration_refresh_token_time, 
							status=@status,
							updated_at=getdate()
							WHERE id=@id;
							SELECT * FROM Tokens WHERE id=@id;
						END
					--delete logic
					IF @Accion = 3
						BEGIN
							UPDATE Tokens 
							SET 
							updated_at=getdate(), 
							deleted_at=getdate()
							WHERE id=@id;
							SELECT * FROM Tokens WHERE id=@id;
						END
				END;
			");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.CrudToken");
        }
    }
}

