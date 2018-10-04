namespace clean_full.Infrastructure.Modules
{
    using Autofac;
    using System;

    public class WebApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //
            // Register all Types in clean_full.WebApi
            //

            Type startup = Type.GetType("clean_full.WebApi.Startup, clean_full.WebApi");

            builder.RegisterAssemblyTypes(startup.Assembly)
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}
