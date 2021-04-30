using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaNetCore.Database.SqlScripts
{
    public class OnCreationSqlScripts
    {
		public static string ChangeCollation(string databaseName) => $@"
				CREATE PROCEDURE fixCollation()
				BEGIN 
					DECLARE done INT DEFAULT 0;
					DECLARE tableCommand VARCHAR(600);
					-- declare cursor
					DECLARE cur1 CURSOR FOR SELECT CONCAT(""ALTER TABLE `"", TABLE_NAME,""` CONVERT TO CHARACTER SET utf8 COLLATE utf8_unicode_ci; "")
						AS ExecuteTheString
						FROM INFORMATION_SCHEMA.TABLES
						WHERE TABLE_SCHEMA = ""{databaseName}""
						AND TABLE_TYPE = ""BASE TABLE"";
				
				   DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;

					OPEN cur1;

					REPEAT
					    FETCH cur1 INTO tableCommand;
						IF NOT done THEN					
							SET @s = tableCommand;
							PREPARE stmt FROM @s;
							EXECUTE stmt;
						END IF;
					UNTIL done END REPEAT;

					CLOSE cur1;
					END;

				CALL fixCollation();

				DROP PROCEDURE  IF EXISTS fixCollation;
			";
	}
}
