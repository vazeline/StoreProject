using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Binders;
using Store.Domain.Abstract;
using Store.Domain.Entities;
using Store.Infrastructure;
using StoreWeb.Models;

namespace StoreWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private IHttpContextAccessor _sessionAccessor;

        public CartController(IProductRepository productRepository, IHttpContextAccessor accessor)
        {
            _productRepository = productRepository;
            _sessionAccessor = accessor;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            var cartIndexViewModel = new CartIndexViewModel()
            {
                Cart = cart,
                ReturnUrl = returnUrl
            };

            return View(cartIndexViewModel);
        }

        public RedirectToActionResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            var product = _productRepository.Products
                .FirstOrDefault(prod => prod.ProductID == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            CartAccessor.SaveCartSession(cart, _sessionAccessor.HttpContext.Session);
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(Cart cart, int ProductId, string returnUrl)
        {
            var productLine = cart.Lines
                .FirstOrDefault(prod => prod.Product.ProductID == ProductId);

            if (productLine != null)
            {
                cart.RemoveItem(productLine.Product);
            }
            CartAccessor.SaveCartSession(cart, _sessionAccessor.HttpContext.Session);
            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Summary(Cart cart)
        {
            ViewBag.requestUri = StoreWeb.HtmlHelpers.Helpers.GetUri(Request);
            return View(cart);
        }
    }
}
