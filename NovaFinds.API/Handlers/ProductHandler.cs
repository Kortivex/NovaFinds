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
    using Application.Mappers;
    using Application.Services;
    using Auth;
    using CORE.Contracts;
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

        /// <summary>
        /// The product-image service.
        /// </summary>
        private readonly ProductImageService _productImagesService = new(context);

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

            if (request.Query.ContainsKey("size") && request.Query.ContainsKey("sortBy") && request.Query.ContainsKey("page")){
                _size = int.Parse(request.Query["size"]!);
                var sortBy = request.Query["sortBy"];
                var page = int.Parse(request.Query["page"]!);

                var productQuery = _productService.GetAll().GetPaged(page, _size);
                if (sortBy == "id"){ productQuery = productQuery.OrderByDescending(o => o.Id); }
                products = productQuery.ToList();
            }
            else if (request.Query.ContainsKey("size") && request.Query.ContainsKey("sortBy")){
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
            Logger.Debug("Put Product Handler");
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

        // PRODUCTS - IMAGES

        public async Task<IResult> PostProductImage(HttpRequest request, string id)
        {
            Logger.Debug("Post Product - Image Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqProductImageDto = JsonSerializer.Deserialize<ProductImageDto>(body);
            var productImageDb = ProductImageMapper.ToDb(reqProductImageDto!);
            productImageDb!.ProductId = int.Parse(id);

            await _productImagesService.CreateAsync(productImageDb);
            await _productImagesService.SaveChangesAsync();

            var productImage = _productImagesService
                .GetAll()
                .Where(productImage => productImage.ProductId == int.Parse(id))
                .Where(productImage => productImage.Image == productImageDb.Image)
                .Where(productImage => productImage.Description == productImageDb.Description)
                .OrderByDescending(productImage => productImage.Id)
                .FirstOrDefault();

            if (productImage != null){ return TypedResults.Created($"/products/{id}/images/{productImage!.Id}", ProductImageMapper.ToDomain(productImage)); }
            return TypedResults.BadRequest("product-image can not be created!");
        }

        public IEnumerable<ProductImageDto?> GetProductImages(HttpRequest request, string id)
        {
            Logger.Debug("List Product - Images Handler");
            var productImages = _productImagesService.GetAll()
                .Where(productImage => productImage.ProductId == int.Parse(id))
                .ToList();

            return ProductImageMapper.ToListDomain(productImages);
        }

        public async Task<IResult?> GetProductImage(HttpRequest request, string id, string imageId)
        {
            Logger.Debug("Get Product - Image Handler");
            var productImage = _productImagesService.GetAll()
                .FirstOrDefault(productImage => productImage.ProductId == int.Parse(id) && productImage.Id == int.Parse(imageId));

            if (productImage == null){ return TypedResults.NotFound("product-image not found"); }

            return TypedResults.Ok(ProductImageMapper.ToDomain(productImage));
        }

        public async Task<IResult> PutProductImage(HttpRequest request, string id, string imageId)
        {
            Logger.Debug("Put Product - Image Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqProductImageDto = JsonSerializer.Deserialize<ProductImageDto>(body);
            var productImageDb = ProductImageMapper.ToDb(reqProductImageDto!);
            productImageDb!.ProductId = int.Parse(id);

            var product = await _productService.GetByIdAsync(int.Parse(id));
            if (product == null){ return TypedResults.NotFound("product not found"); }

            var productImage = await _productImagesService.GetByIdAsync(int.Parse(imageId));
            if (productImage == null){ return TypedResults.NotFound("product-image not found"); }

            productImage.Id = int.Parse(imageId);
            productImage.Description = productImageDb.Description;
            productImage.Image = productImageDb.Image;
            productImage.ProductId = int.Parse(id);

            _productImagesService.Update(productImage);
            await _productImagesService.SaveChangesAsync();

            return TypedResults.Created($"/products/{id}/images/{productImage.Id}", ProductImageMapper.ToDomain(productImage));
        }

        public async Task<IResult> DeleteProductImage(HttpRequest request, string id, string imageId)
        {
            Logger.Debug("Delete Product - Image Handler");
            var productImage = _productImagesService.GetAll()
                .FirstOrDefault(productImage => productImage.ProductId == int.Parse(id) && productImage.Id == int.Parse(imageId));

            if (productImage == null){ return TypedResults.NotFound("product-image not found"); }

            await _productImagesService.DeleteByIdAsync(productImage.Id);
            await _productImagesService.SaveChangesAsync();

            return TypedResults.Empty;
        }

        // PRODUCT - IMAGES

        public async Task<IResult> PostImage(HttpRequest request)
        {
            Logger.Debug("Post Images Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqImageDto = JsonSerializer.Deserialize<ProductImageDto>(body);
            var productImageDb = ProductImageMapper.ToDb(reqImageDto!);

            await _productImagesService.CreateAsync(productImageDb!);
            await _productImagesService.SaveChangesAsync();

            var productImages = _productImagesService.GetAll()
                .Where(image => image.ProductId == productImageDb!.ProductId)
                .Where(image => image.Description == productImageDb!.Description)
                .Where(image => image.Image == productImageDb!.Image)
                .ToList();

            if (productImages.Count != 0){ return TypedResults.Created($"/products/{productImages[0].Id}", ProductImageMapper.ToDomain(productImages[0])); }
            return TypedResults.BadRequest("image can not be created!");
        }

        public IEnumerable<ProductImageDto?> GetImages(HttpRequest request)
        {
            Logger.Debug("List Images Handler");
            var productImages = _productImagesService.GetAll().ToList();

            if (request.Query.ContainsKey("size") && request.Query.ContainsKey("sortBy") && request.Query.ContainsKey("page")){
                _size = int.Parse(request.Query["size"]!);
                var sortBy = request.Query["sortBy"];
                var page = int.Parse(request.Query["page"]!);

                var productImagesQuery = _productImagesService.GetAll().GetPaged(page, _size);
                if (sortBy == "id"){ productImagesQuery = productImagesQuery.OrderByDescending(o => o.Id); }
                productImages = productImagesQuery.ToList();
            }


            return ProductImageMapper.ToListDomain(productImages);
        }

        public async Task<IResult?> GetImage(HttpRequest request, string imageId)
        {
            Logger.Debug("Get Image Handler");
            var productImage = await _productImagesService.GetByIdAsync(int.Parse(imageId));

            if (productImage == null){ return TypedResults.NotFound("product-image not found"); }

            return TypedResults.Ok(ProductImageMapper.ToDomain(productImage));
        }

        public async Task<IResult> PutImage(HttpRequest request, string imageId)
        {
            Logger.Debug("Put Image Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqProductImageDto = JsonSerializer.Deserialize<ProductImageDto>(body);
            var productImageDb = ProductImageMapper.ToDb(reqProductImageDto!);

            var productImage = _productImagesService.GetByIdAsync(int.Parse(imageId)).Result;

            if (productImage == null) return TypedResults.BadRequest("image can not be updated");

            productImage.Id = int.Parse(imageId);
            productImage.Description = productImageDb!.Description;
            productImage.Image = productImageDb.Image;
            productImage.ProductId = productImageDb.ProductId;

            _productImagesService.Update(productImage);
            await _productService.SaveChangesAsync();

            productImageDb.Id = productImage.Id;
            var productImageDto = ProductImageMapper.ToDomain(productImageDb);
            return TypedResults.Created($"/images/{imageId}", productImageDto);
        }

        public async Task<IResult> DeleteImage(HttpRequest request, string imageId)
        {
            Logger.Debug("Delete Image Handler");
            var productImage = await _productImagesService.GetByIdAsync(int.Parse(imageId));

            if (productImage == null){ return TypedResults.NotFound("product-image not found"); }

            await _productImagesService.DeleteByIdAsync(productImage.Id);
            await _productImagesService.SaveChangesAsync();

            return TypedResults.Empty;
        }
    }
}