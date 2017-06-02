using System.Collections.Generic;
using Store.Domain.Entities;

namespace StoreWeb.Models
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }

        public string ReturnUrl { get; set; }
    }
}