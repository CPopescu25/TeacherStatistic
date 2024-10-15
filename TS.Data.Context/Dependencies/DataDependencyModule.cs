using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Data.Context.Dependencies
{
    public class DataDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new TSClientContext())
                .As<TSClientContext>()
                .InstancePerLifetimeScope();

        }

    }
}
