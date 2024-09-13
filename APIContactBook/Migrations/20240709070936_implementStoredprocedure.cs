using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIContactBook.Migrations
{
    public partial class implementStoredprocedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Create Or ALTER PROCEDURE CountContactBasedOnCountry
	                          @CountryId INT=Null
                                AS
                                BEGIN
	                                SELECT Count(*)  AS Count

                                  FROM Contacts Where CountryId=@CountryId

                                END
                                ");
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE CountContactBasedOnGender
	                                    @Gender Char(1)=Null
                                    AS
                                    BEGIN
	                                    SELECT Count(*) AS Count
                                      FROM Contacts Where Gender=@Gender

                                    END");
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE GetDetailByBirthMonth
	                    @Month INT=NULL
                    AS
                    BEGIN
                     SELECT c.[ContactId]
                             , c.[FirstName]
                             , c.[LastName]
                             , c.[ContactNumber]
                             , c.[Image]
                             , c.[Email]
                             , c.[Gender]
                             , c.[Favourite]
                             , c.[Address]
                             , ctr.CountryName
                             , s.StateName
                             , c.[ImageByte]
                             , c.[BirthDate]
                        FROM Contacts c
                        INNER JOIN States s ON s.StateId = c.StateId
                        INNER JOIN Countries ctr ON ctr.CountryId = c.CountryId
	                    WHERE MONTH(c.BirthDate)=@Month
                    END");
            migrationBuilder.Sql(@"CREATE OR ALTER  PROCEDURE GetDetailByStateId
	                                @StateId INT=NULL
                                AS
                                BEGIN
                                 SELECT c.[ContactId]
                                         , c.[FirstName]
                                         , c.[LastName]
                                         , c.[ContactNumber]
                                         , c.[Image]
                                         , c.[Email]
                                         , c.[Gender]
                                         , c.[Favourite]
                                         , c.[Address]
                                         , ctr.CountryName
                                         , s.StateName
                                         , c.[ImageByte]
                                         , c.[BirthDate]
                                    FROM Contacts c
                                    INNER JOIN States s ON s.StateId = c.StateId
                                    INNER JOIN Countries ctr ON ctr.CountryId = c.CountryId
	                                WHERE c.StateId=@StateId
                                END");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CountContactBasedOnCountry");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CountContactBasedOnGender");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetDetailByBirthMonth"); 
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetDetailByStateId");

        }
    }
}
