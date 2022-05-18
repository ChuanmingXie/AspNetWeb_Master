using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Shared.Entities;
using SportsStore.Shared.ViewModel;

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
        public ViewResult IndexNoLink(int page = 1)
        {
            return View(repository.Products
                .OrderBy(p => p.ProductID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                );
        }

        public ViewResult IndexNoCategory(int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                                     .OrderBy(p => p.ProductID)
                                     .Skip((page - 1) * PageSize)
                                     .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Products.Count()
                }
            };
            return View(model);
        }

        public ViewResult Index(string category, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                                    .Where(p => category == null || p.Category == category)
                                    .OrderBy(p => p.ProductID)
                                    .Skip((page - 1) * PageSize)
                                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                            repository.Products.Count() :
                            repository.Products.Where(e => e.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }
        public FileContentResult GetImage(int productId)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                return File(product.ImageData, product.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}