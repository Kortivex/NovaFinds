namespace NovaFinds.MVC.Controllers
{
    using API;
    using Microsoft.AspNetCore.Mvc;

    public class MainController(IConfiguration config) : Controller
    {
        protected ApiClient ApiClient { get; private set; } = new(config);
    }
}