using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Access.Auth.Infrastructure.DataBaseContext.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("CustomMigration_LinkProfile")]
    public class LinkProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
				/*************************************************************************                
				 Nombre    : LinkProfile
				 Creado por   : Ronald Javier Rivera Carranza                
				 Fecha de creación : 06-Sep-2023           
				 Ej de ejecución  :                
                
				 exec LinkProfile 3, 'CB1925E6-B71B-4A0A-A13B-E80D0E1E2871', '', ''
                
				 Crud LinkProfile, ingreso, actualizacion y eliminar     
				  ------------------- ---------------------------------------------------------------------- ------------------    
				 FECHA MODIFICACION						MOTIVO													  USUARIO    
				 ------------------- ----------------------------------------------------------------------- ------------------    	
					 06-Sep-2023             Se crea el proceso para crud                                       Ronald Rivera
					 06-Sep-2023             Eliminar fechas                                                    Ronald Rivera
				**************************************************************************/
				CREATE PROCEDURE dbo.LinkProfile(
					@Accion int,
					@id uniqueidentifier,
					@user_id uniqueidentifier = NULL,
					@profile_id uniqueidentifier = NULL
				)
				AS
				BEGIN
					--insert
					IF @Accion = 1
						BEGIN
							INSERT INTO Users_Profiles 
							(id, 
							user_id, 
							profile_id,
							created_at, 
							updated_at, 
							deleted_at)
							VALUES(@id, 
							@user_id, 
							@profile_id, 
							getdate(), 
							null, 
							null);
							SELECT * FROM Users_Profiles WHERE id=@id;
						END
					--update
					IF @Accion = 2
						BEGIN
							UPDATE Users_Profiles 
							SET id=@id, 
							user_id=@user_id, 
							profile_id=@profile_id,
							updated_at=getdate()
							WHERE id=@id;
							SELECT * FROM Users_Profiles WHERE id=@id;
						END
					--delete logic
					IF @Accion = 3
						BEGIN
							UPDATE Users_Profiles 
							SET 
							updated_at=getdate(), 
							deleted_at=getdate()
							WHERE id=@id;
							SELECT * FROM Users_Profiles WHERE id=@id;
						END
				END;
                ");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.LinkProfile");
        }
    }
}

