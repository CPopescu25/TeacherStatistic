using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using TS.Business.Api.Attributes;
using TS.Business.Api.Controllers;
using Module = Autofac.Module;

namespace TS.Business.Api.Modules
{
    public class ApiDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // Get our HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            //Register the API controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();
        }

    }
}