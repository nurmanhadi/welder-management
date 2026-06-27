using FluentMigrator;

namespace Projects.Infrastructure.Data.Migrations;

[Migration(202606262155)]
public class CreateTableProject : Migration
{
    public override void Up()
    {
        Execute.Sql("""
            CREATE TABLE projects(
                id CHAR(36) PRIMARY KEY,
                pid VARCHAR(20) UNIQUE NOT NULL,
                customer_id CHAR(36) NOT NULL,
                title VARCHAR(20) NOT NULL,
                description TEXT NOT NULL,
                cost DECIMAL(10,2) NOT NULL,
                status ENUM('Draft', 'Survey', 'Negotiation', 'Approved', 'InProgres', 'Finished', 'Cencelled') NOT NULL,
                start_date DATE NOT NULL,
                end_date DATE NOT NULL,
                search_index TEXT NOT NULL,
                created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                deleted_at TIMESTAMP NULL DEFAULT NULL
            );
            """);
        Execute.Sql("CREATE INDEX idx_customers_customerid_status_created_deleted ON projects(customer_id, status, created_at, deleted_at);");
        Execute.Sql("CREATE FULLTEXT INDEX idx_projects_fts ON projects(search_index);");
    }
    public override void Down()
    {
        Execute.Sql("DROP INDEX IF EXISTS idx_customers_customerid_status_created_deleted ON projects;");
        Execute.Sql("DROP INDEX IF EXISTS idx_projects_fts ON projects;");
        Delete.Table("projects");
    }
}
