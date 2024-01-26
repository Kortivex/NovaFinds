﻿namespace NovaFinds.API.Handlers
{
    using Application.Services;
    using Auth;
    using CORE.Contracts;
    using CORE.Mappers;
    using DTOs;
    using Filters;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authorization;

    [Authorize(AuthenticationSchemes = ApiKeySchemeOptions.AuthenticateScheme)]
    public class ProductHandler(IDbContext context)
    {
        private static int _size = 1000;

        /// <summary>
        /// The product service.
        /// </summary>
        private readonly ProductService _productService = new(context);

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
            return Task.FromResult(ProductMapper.ToDomain(product!));
        }
    }
}