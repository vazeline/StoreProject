using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Store.Domain.Abstract;
using Store.Domain.Entities;

namespace StoreWeb.Controllers
{
    public class EditController : Controller
    {
        private readonly IProductRepository _productRepository;

        public EditController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ViewResult Index()
        {
            return View(_productRepository.Products);
        }

        [AcceptVerbs("POST")]
        public ActionResult Index(IEnumerable<Product> prods)
        {
            string msg = "";
            foreach (var prod in prods)
            {
                if (prod.Checked)
                {
                    var product = _productRepository.Products.FirstOrDefault(p => p.ProductID == prod.ProductID);

                    if (product != null)
                    {
                        _productRepository.DeleteProduct(product);
                        msg += string.Format("{0} был удален", product.Name);
                    }
                }
            }
            TempData["message"] = msg;

            return RedirectToAction("Index");
        }
        public ViewResult Edit(int productId)
        {
            var product = _productRepository.Products
                .Where(prod => prod.ProductID == productId)
                .FirstOrDefault();

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.SaveProduct(product);
                TempData["message"] = string.Format("{0} был добавлен.", product.Name);

                return RedirectToAction("Index");
            }

            return View(product);
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }


    }
}
