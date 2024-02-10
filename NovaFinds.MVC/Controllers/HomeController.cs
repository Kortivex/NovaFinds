// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="">
//
// </copyright>
// <summary>
//   The home controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Controllers
{
    using API;
    using DTOs;
    using Faker;
    using Microsoft.AspNetCore.Mvc;
    using SmartBreadcrumbs.Attributes;
    using System.Globalization;

    /// <summary>
    /// The home controller.
    /// </summary>
    public class HomeController : MainController
    {
        /// <summary>
        /// The shop home sections.
        /// </summary>
        private readonly IConfiguration _shopHomeSections;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public HomeController(IConfiguration configuration) : base(configuration)
        {
            IConfiguration config = configuration.GetSection("Config").GetSection("General").GetSection("Shop");
            _shopHomeSections = config.GetSection("Pages").GetSection("Home").GetSection("Items");
        }

        /// <summary>
        /// The index of page to show.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [DefaultBreadcrumb("Home")]
        public async Task<IActionResult> Index()
        {
            string url;
            if (!HttpContext.Session.Keys.Contains("Date"))
                HttpContext.Session.SetString("Date", DateTime.Now.ToLongDateString());

            if (!HttpContext.Session.Keys.Contains("tempUser") && !User.Identity!.IsAuthenticated){
                var email = Internet.Email().Split("@")[0] + "@" + "mailinator.com";
                var user = new UserDto
                {
                    UserName = email,
                    Password = "Password01!",
                    Email = email,
                    Nif = "12345678P",
                    FirstName = Name.First(),
                    LastName = Name.Last(),
                    PhoneNumber = "918854302",
                    StreetAddress = Address.StreetAddress(),
                    City = Address.City(),
                    State = Address.UsState(),
                    Country = Address.Country(),
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = false
                };

                url = ApiEndPoints.PostUsers;
                var result = await ApiClient.Post<UserDto>(url, user);
                if (result.Errors == null || !result.Errors.Any()){ HttpContext.Session.SetString("tempUser", user.Email); }
            }

            var homeSectionsSize = int.Parse(_shopHomeSections.GetSection("Size").Value!, CultureInfo.InvariantCulture);
            url = string.Format(ApiEndPoints.GetProductsSortFilters, homeSectionsSize, "image");
            var productsSorted = await ApiClient.Get<IEnumerable<ProductDto>>(url);
            var productsSortedList = productsSorted!.ToList();

            ViewData["ProductsLatest"] = productsSortedList.OrderByDescending(product => product.Id).ToList();
            ViewData["ProductsCheapest"] = productsSortedList.OrderBy(product => product.Price).ToList();

            return View("Home");
        }

        /// <summary>
        /// The error view generator.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}