﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Access.Auth.Infrastructure.DataBaseContext.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("CustomMigration_LinkProtectData")]
    public class LinkProtectData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
				/*************************************************************************                
				 Nombre    : LinkProtectData
				 Creado por   : Ronald Javier Rivera Carranza                
				 Fecha de creación : 06-Sep-2023           
				 Ej de ejecución  :                
                
				 exec LinkProtectData 3, 'CB1925E6-B71B-4A0A-A13B-E80D0E1E2871', '', ''
                
				 Crud LinkProtectData, ingreso, actualizacion y eliminar     
				  ------------------- ---------------------------------------------------------------------- ------------------    
				 FECHA MODIFICACION						MOTIVO													  USUARIO    
				 ------------------- ----------------------------------------------------------------------- ------------------    	
					 06-Sep-2023             Se crea el proceso para crud                                       Ronald Rivera
					 06-Sep-2023             Eliminar fechas                                                    Ronald Rivera
				**************************************************************************/
				CREATE PROCEDURE dbo.LinkProtectData(
					@Accion int,
					@id uniqueidentifier,
					@protected_id uniqueidentifier = NULL,
					@profile_id uniqueidentifier = NULL
				)
				AS
				BEGIN
					--insert
					IF @Accion = 1
						BEGIN
							INSERT INTO Profile_ProtectedData 
							(id, 
							protected_id, 
							profile_id,
							created_at, 
							updated_at, 
							deleted_at)
							VALUES(@id, 
							@protected_id, 
							@profile_id, 
							getdate(), 
							null, 
							null);
							SELECT * FROM Profile_ProtectedData WHERE id=@id;
						END
					--update
					IF @Accion = 2
						BEGIN
							UPDATE Profile_ProtectedData 
							SET id=@id, 
							protected_id=@protected_id, 
							profile_id=@profile_id,
							updated_at=getdate()
							WHERE id=@id;
							SELECT * FROM Profile_ProtectedData WHERE id=@id;
						END
					--delete logic
					IF @Accion = 3
						BEGIN
							UPDATE Profile_ProtectedData 
							SET  
							updated_at=getdate(), 
							deleted_at=getdate()
							WHERE id=@id;
							SELECT * FROM Profile_ProtectedData WHERE id=@id;
						END
				END;
                ");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.LinkProtectData");
        }
    }
}

