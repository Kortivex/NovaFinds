﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Index.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the IndexModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Admin.Pages.CRUD.Product
{
    using API;
    using DTOs;
    using IFR.Logger;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using NovaFinds.API.Filters;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The index model for product.
    /// </summary>
    [Breadcrumb("Products")]
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        /// <summary>
        /// The API client.
        /// </summary>
        protected ApiClient ApiClient { get; private set; }

        /// <summary>
        /// The max menu pagination.
        /// </summary>
        private readonly int _maxMenuPagination;

        /// <summary>
        /// The shop config.
        /// </summary>
        private readonly IConfigurationSection _shopConfig;

        /// <summary>
        /// The shop crud config.
        /// </summary>
        private readonly IConfigurationSection _shopCrudConfig;

        /// <summary>
        /// The show pagination.
        /// </summary>
        private readonly int _showPagination;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexModel"/> class.
        /// </summary>
        /// <param name="config">
        /// The configuration.
        /// </param>
        public IndexModel(IConfiguration config)
        {
            this.ApiClient = new ApiClient(config);

            _shopConfig = config.GetSection("Config").GetSection("General").GetSection("Shop");
            _shopCrudConfig = _shopConfig.GetSection("Pages").GetSection("Crud");

            var pagination = _shopCrudConfig.GetSection("Pagination");
            _showPagination = int.Parse(pagination.GetSection("Show").Value!, CultureInfo.InvariantCulture);
            _maxMenuPagination = int.Parse(pagination.GetSection("Max_Menu").Value!, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        public IList<ProductDto>? Product { get; set; }

        /// <summary>
        /// The on get async.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IActionResult> OnGetAsync([FromQuery] int page = 1)
        {
            ViewData["PaginationText"] = _shopCrudConfig.GetSection("SortFilter").GetChildren()
                .ToDictionary(x => x.Key, x => x.Value);

            List<ProductDto>? totalProductsDto;
            try{
                var url = string.Format(ApiEndPoints.GetProductsSortByFilters, _showPagination, "id", page);
                var productsDto = await this.ApiClient.Get<List<ProductDto>>(url);

                url = string.Format(ApiEndPoints.GetProducts);
                totalProductsDto = await this.ApiClient.Get<List<ProductDto>>(url);

                this.Product = productsDto;
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            ViewData["Pagination"] = new Paginator(
                totalProductsDto!.Count,
                page,
                _showPagination,
                _maxMenuPagination);

            return Page();
        }
    }
}