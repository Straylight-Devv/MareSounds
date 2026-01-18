using Autofac;
using Autofac.Features.ResolveAnything;
using MareSounds.Core.Interfaces.Repos;
using MareSounds.Core.Interfaces.Services;
using MareSounds.Core.Models;
using MareSounds.Infrastructure.Repos;
using MareSounds.Infrastructure.Services;
using MareSounds.UI.Models;
using MareSounds.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;

namespace MareSounds
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string appdataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\MareSounds";

            if (!Directory.Exists(appdataFolder))
            {
                Directory.CreateDirectory(appdataFolder);
            }

            SqliteService.InitDatabase();

            ContainerBuilder builder = new();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            builder.RegisterType<AddMareViewModel>();
            builder.RegisterType<MareDBRepo>().As<IDBRepo<Mare>>().SingleInstance();
            builder.RegisterType<VoiceClipDBRepo>().As<IDBRepo<VoiceClip>>().SingleInstance();
            builder.RegisterType<MareService>().As<IDBService<Mare>>().SingleInstance();
            builder.RegisterType<VoiceClipService>().As<IDBService<VoiceClip>>().SingleInstance();
            builder.RegisterType<SessionContext>().As<SessionContext>().SingleInstance();

            ServiceCollection services = new();
            services.AddDbContext<DBContext>();
            services.AddSingleton<MareService>();
            services.AddSingleton<VoiceClipService>();
            services.AddSingleton<MareDBRepo>();
            services.AddSingleton<VoiceClipDBRepo>();
            services.AddSingleton<SessionContext>();

            IContainer container = builder.Build();
            DISource.Resolver = container.Resolve;
        }

    }

}
