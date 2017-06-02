using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Store.Domain.Abstract;

namespace StoreWeb.Controllers
{

    public class NavController : Controller
    {
        private readonly IProductRepository _productRepository;

        public NavController(IProductRepository productRepository)
        {
            _productRepository = productRepository;                
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            var categories = _productRepository.Products
                .Select(prod => prod.Category)
                .Distinct()
                .OrderBy(prodCategory => prodCategory);

            return PartialView(categories);
        }

    }
}
