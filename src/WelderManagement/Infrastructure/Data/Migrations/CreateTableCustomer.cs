using FluentMigrator;

namespace WelderManagement.Infrastructure.Data.Migrations;

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
            CREATE INDEX idx_customer_deleted_created ON customers(created_at, deleted_at);
            CREATE FULLTEXT INDEX idx_ftx ON customers (search_index);
        """);
    }

    public override void Down()
    {
        Execute.Sql("""
            DROP INDEX IF EXISTS idx_customer_deleted_created ON customers;
            DROP INDEX IF EXISTS idx_ftx ON customers;
        """);
        Delete.Table("customers");
    }
}
