using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIContactBook.Migrations
{
    public partial class seedState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
             table: "States",
             columns: new[] { "StateId", "StateName", "CountryId" },
             values: new object[,]
             {
                   // States for India
                    { 1, "Gujarat", 1 },
                    { 2, "Uttar Pradesh", 1 },
                    { 3, "Karnataka", 1 },
                    { 4, "Maharastra", 1 },
                    { 5, "Rajasthan", 1 },

                    // States for Germany
                    { 6, "Berlin", 2 },
                    { 7, "North Rhine-Westphalia", 2 },
                    { 8, "Baden-Württemberg", 2 },
                    { 9, "Lower Saxony", 2 },
                    { 10, "Hesse", 2 },

                    // States for America
                    { 11, "California", 3 },
                    { 12, "Texas", 3 },
                    { 13, "Florida", 3 },
                    { 14, "New York", 3 },
                    { 15, "Illinois", 3 },

                    // States for United Kingdom
                    { 16, "England", 4 },
                    { 17, "Scotland", 4 },
                    { 18, "Wales", 4 },
                    { 19, "Northern Ireland", 4 },
                    { 20, "London", 4 },

                    // States for Australia
                    { 21, "New South Wales", 5 },
                    { 22, "Queensland", 5 },
                    { 23, "Victoria", 5 },
                    { 24, "Western Australia", 5 },
                    { 25, "South Australia", 5 },

                    // States for Canada
                    { 26, "Ontario", 6 },
                    { 27, "Quebec", 6 },
                    { 28, "British Columbia", 6 },
                    { 29, "Alberta", 6 },
                    { 30, "Manitoba", 6 }

             });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
               table: "States",
               keyColumn: "StateId",
               keyValues: new object[]
               {
                    // Delete all states
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                    21, 22, 23, 24, 25, 26, 27, 28, 29, 30
               });
        }
    }
}
