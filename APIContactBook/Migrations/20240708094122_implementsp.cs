using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIContactBook.Migrations
{
    public partial class implementsp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE GetPaginatedContacts
    @Page INT,
    @PageSize INT,
    @Letter CHAR(1) = NULL,
    @Search NVARCHAR(100) = NULL,
    @SortOrder VARCHAR(4) = 'asc'
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Skip INT = (@Page - 1) * @PageSize;

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
    FROM [ContactBook].[dbo].[Contacts] c
    INNER JOIN States s ON s.StateId = c.StateId
    INNER JOIN Countries ctr ON ctr.CountryId = c.CountryId
    WHERE (@Letter IS NULL OR c.FirstName LIKE @Letter + '%')
      AND (@Search IS NULL OR c.FirstName LIKE '%' + @Search + '%' OR c.LastName LIKE '%' + @Search + '%')
    ORDER BY
        CASE WHEN @SortOrder = 'desc' THEN c.FirstName END DESC,
        CASE WHEN @SortOrder = 'asc' THEN c.FirstName END ASC,
        CASE WHEN @SortOrder = 'desc' THEN c.LastName END DESC,
        CASE WHEN @SortOrder = 'asc' THEN c.LastName END ASC
    OFFSET @Skip ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetPaginatedContacts;");

        }
    }
}
