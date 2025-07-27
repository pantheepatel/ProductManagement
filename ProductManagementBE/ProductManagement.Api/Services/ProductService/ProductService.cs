namespace ProductManagement.Api.Services.ProductService;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Product> _productRepo;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _productRepo = _unitOfWork.GetRepository<Product>();
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepo
            .Query()
            .Include(p => p.Prices)
            .ToListAsync();

        var today = DateTime.UtcNow;

        return products.Select(p =>
        {
            // Find active seasonal price
            var activePrice = p.Prices
                .FirstOrDefault(price => price.StartDate <= today && price.EndDate >= today);

            var priceToUse = activePrice?.SeasonalPrice ?? p.BasePrice;
            var currentPrice = priceToUse * (1 + p.Tax / 100);

            return new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Tax = p.Tax,
                BasePrice = p.BasePrice,
                CurrentPrice = currentPrice,
                CategoryId = p.CategoryId,
                Prices = p.Prices.Select(price => new ProductPriceUpdateDto
                {
                    Id = price.Id,
                    ProductId = price.ProductId,
                    SeasonalPrice = price.SeasonalPrice,
                    StartDate = price.StartDate,
                    EndDate = price.EndDate
                }).ToList()
            };
        });
    }


    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        //var product = await _productRepo.GetByIdAsync(id);
        var product = await _productRepo
            .Query()
            .Include(p => p.Prices)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
            return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Tax = product.Tax,
            BasePrice = product.BasePrice,
            CategoryId = product.CategoryId,
            Prices = product.Prices?.Select(price => new ProductPriceUpdateDto
            {
                Id = price.Id,
                ProductId = price.ProductId,
                SeasonalPrice = price.SeasonalPrice,
                StartDate = price.StartDate,
                EndDate = price.EndDate
            }).ToList() ?? new List<ProductPriceUpdateDto>()
        };
    }

    public async Task<int> CreateProductAsync(ProductCreateDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Tax = dto.Tax,
            BasePrice = dto.BasePrice,
            CategoryId = dto.CategoryId,
            Prices = dto.Prices?.Select(price => new ProductPrice
            {
                SeasonalPrice = price.SeasonalPrice,
                StartDate = price.StartDate,
                EndDate = price.EndDate
            }).ToList() ?? new List<ProductPrice>()
        };

        await _productRepo.AddAsync(product);
        await _unitOfWork.CompleteAsync();

        return product.Id;
    }

    public async Task<bool> UpdateProductAsync(int id, ProductDto dto)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null)
            return false;

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Tax = dto.Tax;
        product.BasePrice = dto.BasePrice;
        product.CategoryId = dto.CategoryId;

        // Update Prices if needed (basic approach: replace)
        product.Prices = dto.Prices.Select(price => new ProductPrice
        {
            Id = price.Id,
            ProductId = price.ProductId,
            SeasonalPrice = price.SeasonalPrice,
            StartDate = price.StartDate,
            EndDate = price.EndDate
        }).ToList();

        _productRepo.Update(product);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null)
            return false;

        _productRepo.Remove(product);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}
