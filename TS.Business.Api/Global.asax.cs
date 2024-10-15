using Autofac;
using Autofac.Integration.WebApi;
using BusinessServices.Dependencies;
using CommonCfg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TS.Business.Api.Binders;
using TS.Business.Api.Modules;
using TS.Core.Api.Dependencies;
using TS.Data.Context.Dependencies;

namespace TS.Business.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Attach simple post variable binding
            GlobalConfiguration.Configuration
                .ParameterBindingRules
                .Insert(0, PostVariableParameterBinding.HookupParameterBinding);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //EngineContext.Initialize(false);
            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;
            var builder = new ContainerBuilder();

            builder.RegisterModule(new ApiDependencyModule());
            builder.RegisterModule(new DataDependencyModule());
            builder.RegisterModule(new CoreApiDependencyModule());
            builder.RegisterModule(new BusinessDependencyModule());
            
            var container = builder.Build();
            //Set dependency resolver
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
