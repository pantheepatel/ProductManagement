namespace ProductManagement.Tests;

public static class TestDbContextFactory
{
    public static ProductDbContext CreateContext(string dbName = "TestDb")
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var context = new ProductDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;
    }
}
