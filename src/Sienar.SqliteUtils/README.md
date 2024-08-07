# Sienar.SqliteUtils

This package contains a small group of SQLite utilities that I find myself reusing in projects that use SQLite as the database provider.

## Methods

### `IServiceProvider.MigrateDb<TContext>(string dbPath)`

- `string dbPath` represents the path to the SQLite database file

This extension method on `IServiceProvider` allows the developer to migrate their SQLite database at runtime. This method creates a backup of the database at `{dbPath}.backup`, fetches an instance of the database from the `IServiceProvider`, and migrates the database to the latest version. If an exception is thrown, the backup is restored before throwing a new exception with the original exception as a constructor argument. If the migration is successful, the backup is deleted.

### `DbContextOptionsBuilder.UseSqliteDb(string source)`

This extension method on `DbContextOptionsBuilder` allows the developer to call `UseSqlite()` without needing to remember the syntax for the connection string. Silly, I know, but I have the hardest time remembering.