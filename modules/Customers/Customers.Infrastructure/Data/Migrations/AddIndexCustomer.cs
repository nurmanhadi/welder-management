using FluentMigrator;

namespace Customers.Infrastructure.Data.Migrations;

[Migration(2)]
public class AddIndexCustomer : Migration
{
    public override void Up()
    {
        Execute.Sql("""
            CREATE INDEX idx_customer_deleted_created ON customers(created_at, deleted_at);
        """);
    }

    public override void Down()
    {
        Execute.Sql("""
            DROP INDEX IF EXISTS idx_customer_deleted_created ON customers;
        """);
    }
}
