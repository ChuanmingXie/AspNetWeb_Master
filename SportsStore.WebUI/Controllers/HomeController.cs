﻿using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
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
        public ViewResult IndexNoPage(int page = 1)
        {
            return View(repository.Products
                .OrderBy(p => p.ProductID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                );
        }

        public ViewResult Index(int page = 1)
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
    }
}