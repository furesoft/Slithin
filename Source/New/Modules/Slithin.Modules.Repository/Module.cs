﻿using AuroraModularis.Core;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Notebooks.UI.Models;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Repository;

[Priority(ModulePriority.High)]
internal class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer container)
    {
        var pathManager = container.Resolve<IPathManager>();
        pathManager.Init();

        var loginService = container.Resolve<ILoginService>();
        loginService.Init();

        return Task.CompletedTask;
    }

    public override void RegisterServices(ServiceContainer container)
    {
        container.Register<IVersionService>(new VersionServiceImpl(container)).AsSingleton();
        container.Register<IRepository>(new LocalRepository(container)).AsSingleton();
        container.Register<ILoginService>(new LoginServiceImpl(container)).AsSingleton();
        container.Register<IDatabaseService>(new DatabaseServiceImpl(container)).AsSingleton();
        container.Register<IMetadataRepository>(new MetadataRepositoryImpl()).AsSingleton();
        container.Register<ILoadingService>(new LoadingServiceImpl(container)).AsSingleton();
        container.Register<ITemplateStorage>(new TemplateStorageImpl()).AsSingleton();
        container.Register<IThumbnailLoader>(new ThumbnailLoaderImpl()).AsSingleton();
    }
}
