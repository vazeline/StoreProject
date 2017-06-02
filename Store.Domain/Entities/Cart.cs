using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Store.Domain.Entities
{

    [XmlInclude(typeof(CartLine))]
    public class Cart
    {
        [XmlArray("CartLinesArray")]
        [XmlArrayItem("CartLineObject")]
        public List<CartLine> _items = new List<CartLine>();

        public void AddItem(Product product, int quantity)
        {
            lock (this)
            {
                var storedItem = _items
                .Where(prod => prod.Product.ProductID == product.ProductID)
                .FirstOrDefault();

                if (storedItem == null)
                {
                    _items.Add(new CartLine() { Product = product, Quantity = quantity });
                }
                else
                {
                    storedItem.Quantity += quantity;
                }
            }
        }

        public void RemoveItem(Product product)
        {
            lock (this)
            {
                _items.RemoveAll(cartLine => cartLine.Product.ProductID == product.ProductID);
            }
        }

        public decimal ComputeTotalValue()
        {
            lock (this)
            {
                return _items.Sum(cartLine => cartLine.Product.Price * cartLine.Quantity);    
            }
        }

        public IEnumerable<CartLine> Lines
        {
            get
            {
                return _items;
            }
        }

        public void Clear()
        {
            lock (this)
            {
                _items.Clear();    
            }
        }
    }
}
