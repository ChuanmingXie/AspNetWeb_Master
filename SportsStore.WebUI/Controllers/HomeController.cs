using System.Web.Mvc;
using SportsStore.Domain.Abstract;

namespace SportsStore.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository repository;
        public HomeController(IProductRepository repository)
        {
            this.repository = repository;
        }

        // GET: Home
        public ActionResult Index()
        {
            return View(repository.Products);
        }
    }
}