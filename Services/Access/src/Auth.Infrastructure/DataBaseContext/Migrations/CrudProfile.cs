using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Access.Auth.Infrastructure.DataBaseContext.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("CustomMigration_CrudProfile")]
    public class CrudProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
				/*************************************************************************                
				 Nombre    : CrudProfile
				 Creado por   : Ronald Javier Rivera Carranza                
				 Fecha de creación : 06-Sep-2023           
				 Ej de ejecución  :                
                
				 exec CrudProfile 3, 'CB1925E6-B71B-4A0A-A13B-E80D0E1E2871', '', '', 0
                
				 Crud profile, ingreso, actualizacion y eliminar     
				  ------------------- ---------------------------------------------------------------------- ------------------    
				 FECHA MODIFICACION						MOTIVO													  USUARIO    
				 ------------------- ----------------------------------------------------------------------- ------------------    	
					 06-Sep-2023             Se crea el proceso para crud                                       Ronald Rivera
					 06-Sep-2023             Eliminar fechas                                                    Ronald Rivera
				**************************************************************************/
				CREATE PROCEDURE dbo.CrudProfile(
					@Accion int,
					@id uniqueidentifier,
					@module_id uniqueidentifier = NULL,
					@profile_name nvarchar(50) = NULL,
					@status int = NULL
				)
				AS
				BEGIN
					--insert
					IF @Accion = 1
						BEGIN
							INSERT INTO Profiles 
							(id, 
							module_id, 
							profile_name,
							status,
							created_at, 
							updated_at, 
							deleted_at)
							VALUES(@id, 
							@module_id, 
							@profile_name, 
							@status,
							getdate(), 
							null, 
							null);
							SELECT * FROM Profiles WHERE id=@id;
						END
					--update
					IF @Accion = 2
						BEGIN
							UPDATE Profiles 
							SET id=@id, 
							module_id=@module_id, 
							profile_name=@profile_name, 
							status=@status,
							updated_at=getdate()
							WHERE id=@id;
							SELECT * FROM Profiles WHERE id=@id;
						END
					--delete logic
					IF @Accion = 3
						BEGIN
							UPDATE Profiles 
							SET 
							updated_at=getdate(), 
							deleted_at=getdate()
							WHERE id=@id;
							SELECT * FROM Profiles WHERE id=@id;
						END
				END;
                ");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.CrudProfile");
        }
    }
}

