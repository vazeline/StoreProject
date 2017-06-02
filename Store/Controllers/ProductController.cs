using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Store.Domain.Abstract;
using StoreWeb.Models;

namespace StoreWeb.Controllers
{
    public class ProductController : Controller
    {
        public int pageSize = 4;
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ViewResult List(string category, int page = 1)
        {
            var totalProducts = _productRepository.Products.ToList();
            var itemsToShow = totalProducts
                .Where(prod => category == null || prod.Category.Equals(category))
                .OrderBy(p => p.ProductID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var pagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = category == null ? totalProducts.Count : totalProducts.Count(prod => prod.Category.Equals(category))
            };
            ViewBag.requestUri = StoreWeb.HtmlHelpers.Helpers.GetUri(Request);
            return View(new ProductListViewModel() { PagingInfo = pagingInfo, Products = itemsToShow, CurrentCategory = category });
        }

    }
}
