using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using SimpleInjector;
using WebAPI.Controllers;
using WebAPI.Data;
using WebAPI.Helpers;
using WebAPI.Models;
using WebAPI.RequestPipeline;

namespace WebAPI.Configuration
{
    public static class ContainerRegistration
    {
        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(IMediator).GetTypeInfo().Assembly;
            yield return typeof(Models.Requests.AlleTodos).GetTypeInfo().Assembly;
        }
        public static void UseRegistration(this IApplicationBuilder app, Container container)
        {
            // Add application presentation components:
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);
            // NOTE: Do prevent cross-wired instances as much as possible.
            // See: https://simpleinjector.org/blog/2016/07/
            // Cross-wire ASP.NET services (if any)
            container.CrossWire<IHostingEnvironment>(app);
            container.CrossWire<Microsoft.Extensions.Logging.ILoggerFactory>(app);
            container.CrossWire<Microsoft.Extensions.Logging.ILogger<AccountController>>(app);
            container.CrossWire<DataContext>(app);
            container.CrossWire<UserManager<ApplicationUser>>(app);
            container.CrossWire<SignInManager<ApplicationUser>>(app);

            // Mediatr-Setup
            var assemblies = GetAssemblies().ToArray();
            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<>), assemblies);
            container.Register(typeof(IRequestHandler<,>), assemblies);
            container.RegisterCollection(typeof(INotificationHandler<>), assemblies);

            // Pipeline - https://github.com/jbogard/MediatR/blob/master/samples/MediatR.Examples.SimpleInjector/Program.cs
            container.RegisterCollection(typeof(IPipelineBehavior<,>));
            container.RegisterCollection(typeof(IRequestPreProcessor<>));
            container.RegisterCollection(typeof(IRequestPostProcessor<,>));

            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));

            // RequestPipeline-Setup
            container.RegisterCollection(
                typeof(IMessageValidator<,>),
                new[] { typeof(IMessageValidator<,>).Assembly });

            container.Register(
                typeof(IMessageAuthorizer<>),
                new[] { typeof(IMessageAuthorizer<>).Assembly });

            // Handle the case when there is no IMessageAuthorizer
            container.ResolveUnregisteredType += (sender, e) =>
            {
                if (e.UnregisteredServiceType.IsGenericType &&
                    e.UnregisteredServiceType.GetGenericTypeDefinition() == typeof(IMessageAuthorizer<>))
                {
                    e.Register(() => null);
                }
            };

            container.RegisterDecorator(
                typeof(IRequestHandler<,>),
                typeof(Pipeline<,>));

            // App-Setup
            container.Register<IMapper>(() =>
            {
                var mapper = new Mapper();
                return mapper;
            }, Lifestyle.Singleton);

            container.Verify();
        }
    }
}
