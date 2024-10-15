using Autofac;
using DependencyInjectionCrossLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Resources.Services;

namespace BusinessServices.Dependencies
{
    public class BusinessDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var typeFinder = new BinAppTypeFinder();
            var assemblies = typeFinder.GetAssemblies().ToArray();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(IBusinessService)
                .IsAssignableFrom(t))
                .InstancePerRequest()
                .AsImplementedInterfaces();

            builder.RegisterType<ResourcesLoader>()
                     .As<IResourcesLoader>()
                     .AsImplementedInterfaces();

        }

    }
}
