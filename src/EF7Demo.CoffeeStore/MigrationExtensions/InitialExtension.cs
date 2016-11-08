using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EF7Demo.CoffeeStore.MigrationExtensions
{
    public static class MigrationBuilderExtensions
    {
        public static void InitialUpExtension(this MigrationBuilder migration)
        {
            var sql = @"create procedure dbo.SearchForCoffee @searchTerm nvarchar(50) as
                        SELECT @searchTerm = RTRIM(@searchTerm) + '%';
                        SELECT * FROM Coffee
                        WHERE [Name] LIKE @searchTerm
                        ORDER BY [Name] ASC;";

            migration.Operations.Add(new SqlOperation
            {
                Sql = sql
            });
        }


        public static void InitialDownExtension(this MigrationBuilder migration)
        {
            var sql = @"drop procedure dbo.SearchForCoffee 
                        GO";

            migration.Operations.Add(new SqlOperation
            {
                Sql = sql
            });
        }
    }
}
