using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Store.Infrastructure
{
    public class CartViewComponent : ViewComponent
    {
        private readonly Cart _cart;
        public CartViewComponent(IHttpContextAccessor accessor)
        {
            _cart = CartAccessor.GetModel(accessor.HttpContext.Session);
        }
        public IViewComponentResult Invoke()
        {
            return View("Summary", _cart);
        }
    }
}
