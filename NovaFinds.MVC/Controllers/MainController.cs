namespace NovaFinds.MVC.Controllers
{
    using API;
    using Microsoft.AspNetCore.Mvc;

    public class MainController(IConfiguration config) : Controller
    {
        public ApiClient ApiClient { get; private set; } = new(config);
    }
}