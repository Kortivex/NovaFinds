// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Index.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the IndexModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Admin.Pages.CRUD.Roles
{
    using API.Filters;
    using DTOs;
    using IFR.API;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using SmartBreadcrumbs.Attributes;
    using System.Globalization;

    /// <summary>
    /// The index model for role.
    /// </summary>
    [Breadcrumb("Role")]
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
        /// Gets or sets the role.
        /// </summary>
        public IList<RoleDto>? Roles { get; set; }

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

            List<RoleDto>? totalRolesDto;
            try{
                var url = string.Format(ApiEndPoints.GetRolesSortByFilters, _showPagination, "id", page);
                var rolesDto = await this.ApiClient.Get<List<RoleDto>>(url);

                url = string.Format(ApiEndPoints.GetRoles);
                totalRolesDto = await this.ApiClient.Get<List<RoleDto>>(url);

                this.Roles = rolesDto;
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            ViewData["Pagination"] = new Paginator(
                totalRolesDto!.Count,
                page,
                _showPagination,
                _maxMenuPagination);

            return Page();
        }
    }
}