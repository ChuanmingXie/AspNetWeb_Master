using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;

namespace SportsStore.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository repository;
        public int PageSize = 4;
        public HomeController(IProductRepository repository)
        {
            this.repository = repository;
        }

        // GET: Home
        public ViewResult Index(int page = 1)
        {
            return View(repository.Products
                .OrderBy(p=>p.ProductID)
                .Skip((page-1)*PageSize)
                .Take(PageSize)
                );
        }
    }
}