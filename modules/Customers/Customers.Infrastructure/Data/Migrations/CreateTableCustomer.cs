using FluentMigrator;

namespace Customers.Infrastructure.Data.Migrations;

[Migration(1)]
public class CreateTableCustomer : Migration
{
    public override void Up()
    {
        Create.Table("customers")
        .WithColumn("id").AsGuid().PrimaryKey()
        .WithColumn("name").AsString(100).NotNullable()
        .WithColumn("phone").AsString(20).Unique().NotNullable()
        .WithColumn("address").AsString().NotNullable()
        .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
        .WithColumn("deleted_at").AsDateTime().Nullable().WithDefaultValue(null);
    }

    public override void Down()
    {
        Delete.Table("customers");
    }
}
