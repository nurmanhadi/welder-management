using FluentMigrator;

namespace Customers.Infrastructure.Data.Migrations;

[Migration(2)]
public class AddColumnSearchIndexCustomer : Migration
{
    public override void Up()
    {
        Delete.Table("customers").IfExists();
        Create.Table("customers")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("phone").AsString(20).Unique().NotNullable()
            .WithColumn("address").AsString(int.MaxValue).NotNullable()
            .WithColumn("search_index").AsString(int.MaxValue).Nullable()
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("deleted_at").AsDateTime().Nullable().WithDefaultValue(null);

        Create.Index("idx_customers_ngram")
            .OnTable("customers")
            .OnColumn("search_index")
            .Ascending()
            .WithOptions().NonClustered();
    }

    public override void Down()
    {
        Delete.Table("customers");
    }
}
