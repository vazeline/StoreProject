using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Abstract;
using Store.Domain.Entities;

namespace Store.Infrastructure
{
    public class NavViewComponent : ViewComponent
    {
        IProductRepository  _productRepository = null;
        public NavViewComponent(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IViewComponentResult Invoke(string category = null)
        {
            ViewBag.SelectedCategory = category;

            var categories = _productRepository.Products
                .Select(prod => prod.Category)
                .Distinct()
                .OrderBy(prodCategory => prodCategory);

            return View("Menu", categories);
        }
    }
}
