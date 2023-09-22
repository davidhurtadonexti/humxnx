using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Infrastructure.DataBaseContext.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("CustomMigration_LoadGrantsUser")]
    public class LoadGrantsUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"
			/*************************************************************************                
			 Nombre    : LoadGrantsUser          
			 Creado por   : Ronald Javier Rivera Carranza                
			 Fecha de creación : 06-Sep-2023           
			 Ej de ejecución  :                
                
			 exec LoadGrantsUser  'tokenID', '@module_id'             
                
			 select * from Users       
			 ------------------- ---------------------------------------------------------------------- ------------------    
			 FECHA MODIFICACION						MOTIVO													  USUARIO    
			 ------------------- ---------------------------------------------------------------------- ------------------    
			     05-Sep-2023             Se crea y actualiza el SP para cargar los recursos                Ronald Rivera
                 08-Sep-2023             Se actualiza la relacion de las tablas                            Ronald Rivera
			**************************************************************************/ 

			CREATE PROCEDURE dbo.LoadGrantsUser(                
				@token_id uniqueidentifier,
				@module_id uniqueidentifier
			) 
			AS
			BEGIN
				--SET NOCOUNT ON;
				DECLARE @UserId uniqueidentifier;
				DECLARE @enterprise_id uniqueidentifier;
				SELECT @UserId = id, @enterprise_id = enterprise_id
				FROM Users
				WHERE token_id = @token_id;
		
				IF @UserId IS NOT NULL
					BEGIN
						   SELECT 
						   Menus.id AS MenuID,
						   Modules.id AS ModuleID,
						   Profiles.id AS ProfileID, 
						   Resources.id AS ResourceID, 
						   Profiles_Resources.access AS Access, 
						   'Ok' AS Message
						   FROM Users_Profiles
						   JOIN Profiles ON Profiles.id = Users_Profiles.profile_id AND Profiles.deleted_at IS NULL
						   JOIN Profiles_Resources ON Profiles_Resources.profile_id = Profiles.id AND Profiles_Resources.deleted_at IS NULL
						   JOIN Modules ON Modules.enterprise_id = @enterprise_id AND Modules.id = @module_id AND Modules.deleted_at IS NULL
						   JOIN Menus ON Menus.module_id = Modules.id AND Menus.deleted_at IS NULL
						   LEFT JOIN Profile_ProtectedData ON Profile_ProtectedData.profile_id = Profiles.id
						   LEFT JOIN ProtectedData ON ProtectedData.id = Profile_ProtectedData.protected_id
						   JOIN Resources ON Resources.id = Profiles_Resources.resources_id AND Resources.menu_id = Menus.id AND Resources.deleted_at IS NULL
						   WHERE Users_Profiles.user_id = @UserId AND Users_Profiles.deleted_at IS NULL;
					END
				ELSE
					BEGIN
	       				SELECT 
						   NULL AS MenuID,
						   NULL AS ModuleID,
						   NULL AS ProfileID, 
						   NULL AS ResourceID, 
						   NULL AS Access, 
						   'not token active' AS Message;
					END
			END;
            ");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.LoadGrantsUser");
        }
    }
}

