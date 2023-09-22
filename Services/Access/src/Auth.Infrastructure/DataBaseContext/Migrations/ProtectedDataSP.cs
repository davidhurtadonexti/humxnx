using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Infrastructure.DataBaseContext.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("CustomMigration_ProtectedDataSP")]
    public class ProtectedDataSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"

			/*************************************************************************                
			 Nombre    : ProtectedDataSP
			 Creado por   : Ronald Javier Rivera Carranza                
			 Fecha de creación : 13-Sep-2023           
			 Ej de ejecución  :                
                
			 exec ProtectedDataSP  'tokenID'           
                
			 select * from Users       
			 ------------------- ---------------------------------------------------------------------- ------------------    
			 FECHA MODIFICACION						MOTIVO													  USUARIO    
			 ------------------- ---------------------------------------------------------------------- ------------------    
			     13-Sep-2023             Se crea y actualiza el SP para cargar la data protegida           Ronald Rivera
			**************************************************************************/ 

			CREATE PROCEDURE dbo.ProtectedDataSP(                
				@token_id uniqueidentifier
			) 
			AS
			BEGIN
				--SET NOCOUNT ON;
				DECLARE @UserId uniqueidentifier;
				SELECT @UserId = id
				FROM Users
				WHERE token_id = @token_id;
		
				IF @UserId IS NOT NULL
					BEGIN
						   SELECT 
							   Users_Profiles.profile_id AS ProfileID, 
							   tabla_name AS TableName,
							   fields AS Campos,
						   'Ok' AS Message
						   FROM Users_Profiles
						   JOIN Profile_ProtectedData ON Profile_ProtectedData.profile_id = Users_Profiles.profile_id AND Profile_ProtectedData.deleted_at IS NULL
						   JOIN ProtectedData ON ProtectedData.id = Profile_ProtectedData.protected_id AND ProtectedData.deleted_at IS NULL
						   WHERE Users_Profiles.user_id = @UserId AND Users_Profiles.deleted_at IS NULL;
					END
				ELSE
					BEGIN
	       				SELECT 
						   NULL AS ProfileID,
						   NULL AS TableName,
						   NULL AS Campos,  
						   'not token active' AS Message;
					END
			END;
            ");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.ProtectedDataSP");
        }
    }
}

