﻿using Slithin.Modules.Menu.Models.ContextualMenu;
using Slithin.Modules.Menu.Models.ContextualMenu.ContextualElements;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Notebooks.UI.ViewModels;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Notebooks.UI;

internal class ContextualMenuProvider : IContextualMenuProvider
{
    private readonly NotebooksPageViewModel _viewModel;
    private readonly NotebooksFilter _filter;

    public ContextualMenuProvider(NotebooksPageViewModel viewModel, NotebooksFilter filter)
    {
        _viewModel = viewModel;
        _filter = filter;
    }

    public void RegisterContextualMenuElements(ContextualRegistrar registrar)
    {
        registrar.RegisterFor(UIContext.Notebook, new ContextualButton("Folder", "Ionicons.AddiOS", _viewModel.MakeFolderCommand));
        registrar.RegisterFor(UIContext.Notebook, new ContextualButton("Remove", "Cool.TrashFull", _viewModel.RemoveNotebookCommand));
        registrar.RegisterFor(UIContext.Notebook, new ContextualButton("Move", "Material.FolderMove", _viewModel.MoveCommand));
        registrar.RegisterFor(UIContext.Notebook, new ContextualButton("Rename", "BoxIcons.RegularRename", _viewModel.RenameCommand));

        registrar.RegisterFor(UIContext.Notebook, new ContextualButton("Export", "Material.ExportVariant", _viewModel.ExportCommand));

        registrar.RegisterFor(UIContext.Notebook, new ContextualButton("Pin", "Entypo+.Star", _viewModel.PinCommand));
        registrar.RegisterFor(UIContext.Notebook, new ContextualButton("Unpin", "Entypo+.StarOutline", _viewModel.UnPinCommand));
    }
}
