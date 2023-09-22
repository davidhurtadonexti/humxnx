using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Infrastructure.DataBaseContext.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("CustomMigration_CrudMenu")]
    public class CrudMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
				/*************************************************************************                
				 Nombre    : CrudMenu    
				 Creado por   : Ronald Javier Rivera Carranza                
				 Fecha de creación : 06-Sep-2023           
				 Ej de ejecución  :                
                
				 exec CrudMenu 3, 'CB1925E6-B71B-4A0A-A13B-E80D0E1E2871', '', '', '', 0, '', 0    
                
				 Crud menu, ingreso, actualizacion y eliminar     
				  ------------------- ---------------------------------------------------------------------- ------------------    
				 FECHA MODIFICACION						MOTIVO													  USUARIO    
				 ------------------- ----------------------------------------------------------------------- ------------------    	
					 06-Sep-2023             Se crea el proceso para crud                                       Ronald Rivera
					 06-Sep-2023             Eliminar fechas                                                    Ronald Rivera
				**************************************************************************/  	
				CREATE PROCEDURE dbo.CrudMenu(
					@Accion int,
					@id uniqueidentifier,
					@parent_id uniqueidentifier = NULL,
					@module_id uniqueidentifier = NULL,
					@name nvarchar(50) = NULL,
					@order int = NULL,
					@url nvarchar(50) = NULL,
					@status int = NULL
				)
				AS
				BEGIN
					--insert
					IF @Accion = 1
						BEGIN
							INSERT INTO Menus 
							(id, 
							parent_id, 
							module_id, 
							name,
							[order],
							url,
							status,
							created_at, 
							updated_at, 
							deleted_at)
							VALUES(@id, 
							@parent_id, 
							@module_id, 
							@name,
							@order,
							@url,
							@status,
							getdate(), 
							null, 
							null);
							SELECT * FROM Menus WHERE id=@id;
						END
					--update
					IF @Accion = 2
						BEGIN
							UPDATE Menus 
							SET id=@id, 
							parent_id=@parent_id, 
							module_id=@module_id, 
							name=@name,
							[order]=@order,
							url=@url,
							status=@status,
							updated_at=getdate()
							WHERE id=@id;
							SELECT * FROM Menus WHERE id=@id;
						END
					--delete logic
					IF @Accion = 3
						BEGIN
							UPDATE Menus 
							SET 
							updated_at=getdate(), 
							deleted_at=getdate()
							WHERE id=@id;
							SELECT * FROM Menus WHERE id=@id;
						END
				END;
                ");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.CrudMenu");
        }
    }
}

