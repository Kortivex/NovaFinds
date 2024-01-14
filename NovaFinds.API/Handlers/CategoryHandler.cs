namespace NovaFinds.API.Handlers
{
    using Application.Services;
    using CORE.Contracts;
    using CORE.Mappers;
    using DTOs;
    using IFR.Logger;

    public class CategoryHandler(IDbContext context)
    {
        /// <summary>
        /// The category service.
        /// </summary>
        private readonly CategoryService _categoryService = new(context);

        public IEnumerable<CategoryDto?> GetCategories(HttpRequest request)
        {
            Logger.Debug("List Categories Handler");
            var categories = _categoryService.GetAll().ToList();
            return CategoryMapper.ToListDomain(categories);
        }
    }
}