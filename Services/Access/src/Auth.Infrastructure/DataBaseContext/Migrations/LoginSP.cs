using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Access.Auth.Infrastructure.DataBaseContext.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("CustomMigration_LoginSP")]
    public class LoginSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
				/*************************************************************************                
				 Nombre    : LoginSP               
				 Creado por   : Ronald Javier Rivera Carranza                
				 Fecha de creación : 25-Agos-2023           
				 Ej de ejecución  :                
                
				 exec LoginSP 'JhonDoe', 'MyPasswordHasedHere', 'MyIDEnterprise', '@module_id'        
                
				 select * from Users       
				  ------------------- ---------------------------------------------------------------------- ------------------    
				 FECHA MODIFICACION						MOTIVO													  USUARIO    
				 ------------------- ----------------------------------------------------------------------- ------------------    
				28-Agos-2023			Se cargan todos los datos a donde estan vinculados el user             Ronald Rivera 	
				31-Agos-2023			Cambio de joins                                                        Ronald Rivera 	
				05-Sep-2023             Cambio de resultado del select                                         Ronald Rivera
				08-Sep-2023             Actualizacion de tablas                                                Ronald Rivera
				12-Sep-2023             Actualizacion de query y se agrego campos                              Ronald Rivera
				**************************************************************************/   
				CREATE PROCEDURE dbo.LoginSP
				(                
				 @user varchar(50),                
				 @pass  varchar(128),
				 @enterprise_id uniqueidentifier,
				 @module_id uniqueidentifier
				) 
				AS
				BEGIN
					--SET NOCOUNT ON;
					DECLARE @UserId uniqueidentifier;
					DECLARE @guidValidMenu uniqueidentifier = '00000000-0000-0000-0000-000000000000';
					SELECT @UserId = id
					FROM Users
					WHERE username = @user AND enterprise_id = @enterprise_id AND protected = @pass;
					--- WHERE Username = @user AND Password = HASHBYTES('SHA2_256', @Password);
		
					IF @UserId IS NOT NULL
						BEGIN
							IF NOT EXISTS (SELECT * FROM Users_Profiles WHERE user_id = @UserId)
								BEGIN
									SELECT 
									   NULL AS MenuID,
									   NULL AS ParentID,
									   NULL AS [Order],
									   NULL AS ModuleID,
									   NULL AS ProfileID, 
									   NULL AS ResourceID, 
									   NULL AS Access,
									   'Usuario no autorizado' AS Message;
								END
		
							IF NOT EXISTS (SELECT * FROM Modules WHERE id = @module_id)
								BEGIN
									SELECT 
									   NULL AS MenuID,
									   NULL AS ParentID,
									   NULL AS [Order],
									   NULL AS ModuleID,
									   NULL AS ProfileID, 
									   NULL AS ResourceID, 
									   NULL AS Access, 
									   'Usuario no autorizado' AS Message;
								END
							IF NOT EXISTS (SELECT * FROM Users_Profiles JOIN Profiles ON Profiles.id = Users_Profiles.profile_id AND Profiles.deleted_at IS NULL JOIN Profiles_Resources ON Profiles_Resources.profile_id = Profiles.id AND Profiles_Resources.deleted_at IS NULL JOIN Modules ON Modules.enterprise_id = @enterprise_id AND Modules.deleted_at IS NULL JOIN Menus ON Menus.module_id = Modules.id AND Menus.deleted_at IS NULL LEFT JOIN Profile_ProtectedData ON Profile_ProtectedData.profile_id = Profiles.id LEFT JOIN ProtectedData ON ProtectedData.id = Profile_ProtectedData.protected_id JOIN Resources ON Resources.id = Profiles_Resources.resources_id AND Resources.menu_id = Menus.id AND Resources.deleted_at IS NULL WHERE Users_Profiles.user_id = @UserId AND Users_Profiles.deleted_at IS NULL)
							BEGIN
		    					SELECT 'Usuario no autorizado' AS Message, NULL AS UserID, NULL AS MenuID, NULL AS ModuleID, NULL AS ProfileID, NULL AS ProtectedDataID, NULL AS ResourceID, NULL AS Access;
							END
							ELSE
							BEGIN
							   SELECT 
							   CASE 
							        WHEN Menus.parent_id = @guidValidMenu THEN Menus.id
							        ELSE Menus.parent_id
							   END AS MenuID,
							   CASE 
							        WHEN Menus.parent_id != @guidValidMenu THEN Menus.id
							        ELSE NULL
							   END AS ParentID,
							   Menus.[order] AS [Order],
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
						END
					ELSE
						BEGIN
	       					SELECT 
							   NULL AS MenuID,
							   NULL AS ParentID,
							   NULL AS [Order],
							   NULL AS ModuleID, 
							   NULL AS ProfileID, 
							   NULL AS ResourceID, 
							   NULL AS Access, 
							   'Error usuario o contraseña' AS Message;
						END
				END;
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.LoginSP");
        }
    }
}

