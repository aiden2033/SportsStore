using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }
        public IActionResult List(int productPage = 1) =>
            View(new ProductsListViewModel()
            {
                Products = repository.Products.OrderBy(p => p.ProductID)//соритурем по айди
                                          .Skip((productPage - 1) * PageSize)//берем все с такого-то айди
                                          .Take(PageSize),// берем первые (PageSize (4) ) позиции
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Products.Count()
                }
            });
    }
}
