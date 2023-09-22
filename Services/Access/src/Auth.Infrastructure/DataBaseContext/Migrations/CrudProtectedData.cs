using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Infrastructure.DataBaseContext.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("CustomMigration_CrudProtectedData")]
    public class CrudProtectedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
				/*************************************************************************                
				 Nombre    : CrudProtectedData
				 Creado por   : Ronald Javier Rivera Carranza                
				 Fecha de creación : 06-Sep-2023           
				 Ej de ejecución  :                
                
				 exec CrudProtectedData 3, 'CB1925E6-B71B-4A0A-A13B-E80D0E1E2871', '', ''
                
				 Crud ProtectedData, ingreso, actualizacion y eliminar     
				  ------------------- ---------------------------------------------------------------------- ------------------    
				 FECHA MODIFICACION						MOTIVO													  USUARIO    
				 ------------------- ----------------------------------------------------------------------- ------------------    	
					 06-Sep-2023             Se crea el proceso para crud                                       Ronald Rivera
					 06-Sep-2023             Eliminar fechas                                                    Ronald Rivera
				**************************************************************************/
				CREATE PROCEDURE dbo.CrudProtectedData(
					@Accion int,
					@id uniqueidentifier,
					@tabla_name nvarchar(50) = NULL,
					@fields nvarchar(200) = NULL
				)
				AS
				BEGIN
					--insert
					IF @Accion = 1
						BEGIN
							INSERT INTO ProtectedData 
							(id, 
							tabla_name, 
							fields,
							created_at, 
							updated_at, 
							deleted_at)
							VALUES(@id, 
							@tabla_name, 
							@fields, 
							getdate(), 
							null, 
							null);
							SELECT * FROM ProtectedData WHERE id=@id;
						END
					--update
					IF @Accion = 2
						BEGIN
							UPDATE ProtectedData 
							SET id=@id, 
							tabla_name=@tabla_name, 
							fields=@fields, 
							updated_at=getdate()
							WHERE id=@id;
							SELECT * FROM ProtectedData WHERE id=@id;
						END
					--delete logic
					IF @Accion = 3
						BEGIN
							UPDATE ProtectedData 
							SET 
							updated_at=getdate(), 
							deleted_at=getdate()
							WHERE id=@id;
							SELECT * FROM ProtectedData WHERE id=@id;
						END
				END;
                ");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.CrudProtectedData");
        }
    }
}

