// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductHandler.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Product Handler type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.API.Handlers
{
    using Application.Services;
    using Auth;
    using CORE.Contracts;
    using CORE.Mappers;
    using DTOs;
    using Filters;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authorization;
    using System.Text.Json;

    [Authorize(AuthenticationSchemes = ApiKeySchemeOptions.AuthenticateScheme)]
    public class ProductHandler(IDbContext context)
    {
        private static int _size = 1000;

        /// <summary>
        /// The product service.
        /// </summary>
        private readonly ProductService _productService = new(context);

        public async Task<IResult> PostProducts(HttpRequest request)
        {
            Logger.Debug("Post Products Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqProductDto = JsonSerializer.Deserialize<ProductDto>(body);
            var productDb = ProductMapper.ToDb(reqProductDto!);

            await _productService.CreateAsync(productDb!);
            await _productService.SaveChangesAsync();

            var products = _productService.GetAll()
                .Where(product => product.Name == productDb!.Name)
                .Where(product => product.Description == productDb!.Description)
                .Where(product => product.Brand == productDb!.Brand)
                .Where(product => product.CategoryId == productDb!.CategoryId)
                .ToList();

            if (products.Count != 0){ return TypedResults.Created($"/products/{products[0].Id}", ProductMapper.ToDomain(products[0])); }
            return TypedResults.BadRequest("product can not be created!");
        }

        public IEnumerable<ProductDto?> GetProducts(HttpRequest request)
        {
            Logger.Debug("List Products Handler");
            var products = _productService.GetAll().ToList();

            if (request.Query.ContainsKey("size") && request.Query.ContainsKey("sortBy")){
                if (request.Query.ContainsKey("size")){
                    _size = int.Parse(request.Query["size"]!);
                    products = _productService.GetWithCategoryImageSize(_size).ToList();
                }

                if (!request.Query.ContainsKey("sortBy")) return ProductMapper.ToListDomain(products);
                var sortBy = request.Query["sortBy"];
                if (sortBy == "image"){ _productService.SortByImageOrder(products); }
            }
            else if (request.Query.ContainsKey("name")){
                var name = request.Query["name"];
                var size = request.Query["size"];
                products = _productService.FindByNameSize(name!, int.Parse(size!)).ToList();
            }
            else if (request.Query.ContainsKey("category")){
                var category = int.Parse(request.Query["category"]!);
                if (request.Query.ContainsKey("size") && request.Query.ContainsKey("page")){
                    _size = int.Parse(request.Query["size"]!);
                    var page = int.Parse(request.Query["page"]!);
                    products = _productService.GetByCategoryIdWithImages(category).GetPaged(page, _size).ToList();
                }
                else{ products = _productService.GetByCategoryIdWithImages(category).ToList(); }
            }

            return ProductMapper.ToListDomain(products);
        }

        public Task<ProductDto?> GetProduct(HttpRequest request, string id)
        {
            Logger.Debug("Get Product Handler");
            var product = _productService.GetByIdWithImage(int.Parse(id)).FirstOrDefault();
            if (product == null){ product = _productService.GetAll().FirstOrDefault(p => p.Id == int.Parse(id)); }
            return Task.FromResult(ProductMapper.ToDomain(product!));
        }

        public async Task<IResult> PutProduct(HttpRequest request, string id)
        {
            Logger.Debug("Put Role Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqProductDto = JsonSerializer.Deserialize<ProductDto>(body);
            var productDb = ProductMapper.ToDb(reqProductDto!);

            var product = _productService.GetByIdAsync(int.Parse(id)).Result;

            if (product == null) return TypedResults.BadRequest("product can not be updated");

            product.Name = productDb!.Name;
            product.Description = productDb.Description;
            product.Brand = productDb.Brand;
            product.Price = productDb.Price;
            product.Stock = productDb.Stock;
            product.CategoryId = productDb.CategoryId;

            _productService.Update(product);
            await _productService.SaveChangesAsync();

            productDb.Id = product.Id;
            var productDto = ProductMapper.ToDomain(productDb);
            return TypedResults.Created($"/products/{id}", productDto);
        }

        public async Task<IResult> DeleteProduct(HttpRequest request, string id)
        {
            Logger.Debug("Delete Product Handler");
            var products = _productService.GetAll()
                .Where(product => product.Id == int.Parse(id)).ToList();

            if (products.Count == 0){ return TypedResults.NotFound(); }

            var product = products[0];

            await _productService.DeleteByIdAsync(product.Id);
            await _productService.SaveChangesAsync();

            return TypedResults.Empty;
        }
    }
}