using FluentMigrator;

namespace ShoppingList.API.Migrations
{
    [Migration(1, description: "Initial migration.")]
    public class InitDatabaseMigration : Migration
    {
        public override void Up()
        {
            Create.Table("ShoppingItems")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("ItemName").AsString(250)
                .WithColumn("Quantity").AsFloat().Nullable()
                .WithColumn("CreationDate").AsDateTime()
                .WithColumn("CheckOffDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table("ShoppingItems");
        }
    }
}
