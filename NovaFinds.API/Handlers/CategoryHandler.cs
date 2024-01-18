namespace NovaFinds.API.Handlers
{
    using Application.Services;
    using CORE.Contracts;
    using CORE.Mappers;
    using DTOs;
    using Filters;
    using IFR.Logger;
    using Microsoft.EntityFrameworkCore;

    public class CategoryHandler(IDbContext context)
    {

        private static int _size = 1000;

        /// <summary>
        /// The category service.
        /// </summary>
        private readonly CategoryService _categoryService = new(context);

        public IEnumerable<CategoryDto?> GetCategories(HttpRequest request)
        {
            Logger.Debug("List Categories Handler");
            var categories = _categoryService.GetAll().ToList();
            if (!request.Query.ContainsKey("size") || !request.Query.ContainsKey("page")) return CategoryMapper.ToListDomain(categories);

            _size = int.Parse(request.Query["size"]!);
            var page = int.Parse(request.Query["page"]!);
            categories = _categoryService.GetAll().GetPaged(page, _size).ToList();

            return CategoryMapper.ToListDomain(categories);
        }
        
        public async Task<CategoryDto?> GetCategory(HttpRequest request, string id)
        {
            Logger.Debug("Get Category Handler");
            var category = await _categoryService.GetByIdAsync(int.Parse(id));

            return CategoryMapper.ToDomain(category!);
        }
    }
}