using FluentMigrator;

namespace Customers.Infrastructure.Data.Migrations;

[Migration(1)]
public class CreateTableCustomer : Migration
{
    public override void Up()
    {
        Execute.Sql("""
            CREATE TABLE customers (
                id CHAR(36) PRIMARY KEY,
                name VARCHAR(100) NOT NULL,
                phone VARCHAR(20) UNIQUE NOT NULL,
                address TEXT NOT NULL,
                search_index TEXT NOT NULL,
                created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                deleted_at TIMESTAMP NULL DEFAULT NULL
            );
        """);

        Execute.Sql("""
            CREATE FULLTEXT INDEX search_index_fts ON customers (search_index);
        """);
    }

    public override void Down()
    {
        Delete.Table("customers");
    }
}
