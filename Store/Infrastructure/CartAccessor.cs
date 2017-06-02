using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Store.Domain.Entities;

namespace Store.Infrastructure
{
    public class CartAccessor
    {
        private const string SessionKey = "Cart";
        public static Cart GetModel(ISession session)
        {
            Cart cart = null;
            if (session == null)
                return cart;

            byte[] crt;
            session.TryGetValue(SessionKey, out crt);
            System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(typeof(Cart));

            using (var stream = new MemoryStream())
            {
                if (crt == null)
                {
                    cart = new Cart();
                    xml.Serialize(stream, cart);
                    session.Set(SessionKey, stream.ToArray());
                }
                else
                {
                    stream.Write(crt, 0, crt.Length);
                    stream.Position = 0;
                    cart = (Cart)xml.Deserialize(stream);
                }
            }

            return cart;
        }

        public static void SaveCartSession(Cart cart, ISession session)
        {
            System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(typeof(Cart));
            using (var stream = new MemoryStream())
            {
                xml.Serialize(stream, cart);
                session.Set(SessionKey, stream.ToArray());
            }
        }
    }
}
