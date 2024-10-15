using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Core.Api.Dependencies
{
    public class CoreApiDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<JsonApiRequestService>()
                .As<IJsonApiRequestService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<XmlApiRequestService>()
                .As<IXmlApiRequestService>()
                .InstancePerLifetimeScope();
        }


    }
}
