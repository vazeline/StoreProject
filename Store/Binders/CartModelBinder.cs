using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Store.Domain.Entities;
using Store.Infrastructure;

namespace Store.Binders
{
    public class CartModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return CheckAndReturnModel(bindingContext);
        }

        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var model = CheckAndReturnModel(bindingContext);
            return Task.FromResult(model);
        }

        private static Cart CheckAndReturnModel(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }
            var s = bindingContext.ActionContext?.HttpContext?.Session;
            var model = CartAccessor.GetModel(s);
            bindingContext.Result = ModelBindingResult.Success(model);
            return model;
        }
    }
}