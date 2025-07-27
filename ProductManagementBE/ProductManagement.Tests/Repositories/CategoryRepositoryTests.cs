namespace ProductManagement.Tests.Repositories;

[TestFixture]
public class CategoryRepositoryTests
{
    private ProductDbContext _context;
    private Repository<Category> _repository;

    [SetUp]
    public void Setup()
    {
        _context = TestDbContextFactory.CreateContext(Guid.NewGuid().ToString());
        _repository = new Repository<Category>(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task AddAsync_Should_Add_Category()
    {
        var category = new Category { Name = "Electronics", Description = "Devices" };

        await _repository.AddAsync(category);
        await _context.SaveChangesAsync();

        var categories = await _repository.GetAllAsync();

        Assert.That(categories.Count(), Is.EqualTo(1));
        Assert.That(categories.First().Name, Is.EqualTo("Electronics"));
    }

    [Test]
    public async Task GetAllAsync_Should_Return_All_Categories()
    {
        var cat1 = new Category { Name = "Electronics", Description = "Devices" };
        var cat2 = new Category { Name = "Books", Description = "Various books" };

        await _repository.AddAsync(cat1);
        await _repository.AddAsync(cat2);
        await _context.SaveChangesAsync();

        var categories = await _repository.GetAllAsync();

        Assert.That(categories.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetByIdAsync_Should_Return_Correct_Category()
    {
        var category = new Category { Name = "Sports", Description = "Sporting goods" };
        await _repository.AddAsync(category);
        await _context.SaveChangesAsync();

        var fetched = await _repository.GetByIdAsync(category.Id);

        Assert.That(fetched, Is.Not.Null);
        Assert.That(fetched.Name, Is.EqualTo("Sports"));
    }

    [Test]
    public async Task Update_Should_Modify_Category()
    {
        var category = new Category { Name = "OldName", Description = "OldDesc" };
        await _repository.AddAsync(category);
        await _context.SaveChangesAsync();

        category.Name = "NewName";
        _repository.Update(category);
        await _context.SaveChangesAsync();

        var updated = await _repository.GetByIdAsync(category.Id);

        Assert.That(updated.Name, Is.EqualTo("NewName"));
    }

    [Test]
    public async Task Remove_Should_Delete_Category()
    {
        var category = new Category { Name = "ToDelete", Description = "Desc" };
        await _repository.AddAsync(category);
        await _context.SaveChangesAsync();

        _repository.Remove(category);
        await _context.SaveChangesAsync();

        var categories = await _repository.GetAllAsync();
        Assert.That(categories.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task Query_Should_Filter_Categories()
    {
        var cat1 = new Category { Name = "Electronics", Description = "Devices" };
        var cat2 = new Category { Name = "Books", Description = "Various books" };

        await _repository.AddAsync(cat1);
        await _repository.AddAsync(cat2);
        await _context.SaveChangesAsync();

        var queryResult = _repository.Query().Where(c => c.Name.Contains("Book")).ToList();

        Assert.That(queryResult.Count, Is.EqualTo(1));
        Assert.That(queryResult.First().Name, Is.EqualTo("Books"));
    }
}