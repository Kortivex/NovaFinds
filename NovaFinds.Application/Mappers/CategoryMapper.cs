// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryMapper.cs" company="">
//
// </copyright>
// <summary>
//   Defines the CategoryMapper converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.Application.Mappers
{
    using CORE.Domain;
    using DTOs;

    public static class CategoryMapper
    {
        /// <summary>
        ///     Convert from Domain to DTO.
        /// </summary>
        public static CategoryDto? ToDomain(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
        
        /// <summary>
        ///     Convert from List Domain to DTO.
        /// </summary>
        public static IEnumerable<CategoryDto?> ToListDomain(IEnumerable<Category> categories)
        {
            var categoriesDto = new List<CategoryDto?>();
            foreach (var category in categories){ categoriesDto.Add(ToDomain(category)); }

            return categoriesDto;
        }
    }
}