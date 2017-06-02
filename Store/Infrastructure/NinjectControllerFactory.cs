using System;
using Microsoft.AspNetCore.Mvc.Controllers;
using Ninject;
using Store.Domain;
using Store.Domain.Abstract;

namespace StoreWeb.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _kernel;

        public NinjectControllerFactory()
        {
            _kernel = new StandardKernel();
            AddBindings();
        }

        
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            IController controller = null;

            if (controllerType != null)
            {
                controller = (IController)_kernel.Get(controllerType);
            }

            return controller;
        }

        private void AddBindings()
        {
            _kernel.Bind<IProductRepository>().To<EFProductRepository>();
        }
    }
}