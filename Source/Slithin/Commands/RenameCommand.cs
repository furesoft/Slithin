using System;
using System.Windows.Input;
using Serilog;
using Slithin.Controls;
using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Remarkable.Models;

namespace Slithin.Commands;

[Context(UIContext.Notebook)]
public class RenameCommand : ICommand, IContextCommand
{
    private readonly ILocalisationService _localisationService;
    private readonly ILogger _logger;

    public RenameCommand(ILocalisationService localisationService, ILogger logger)
    {
        _localisationService = localisationService;
        _logger = logger;
    }

    public event EventHandler CanExecuteChanged;

    public object ParentViewModel { get; set; }
    public string Titel => _localisationService.GetString("Rename");

    public bool CanExecute(object parameter)
    {
        return parameter != null
               && parameter is Metadata md
               && md.VisibleName != _localisationService.GetString("Quick sheets")
               && md.VisibleName != _localisationService.GetString("Up ..")
               && md.VisibleName != _localisationService.GetString("Trash");
    }

    public bool CanHandle(object data)
    {
        return CanExecute(data);
    }

    public async void Execute(object parameter)
    {
        var name = await DialogService.ShowPrompt(_localisationService.GetString("Rename"),
                _localisationService.GetString("Name"), ((Metadata)parameter).VisibleName);

        if (!string.IsNullOrEmpty(name))
        {
            Rename((Metadata)parameter, name);
        }
    }

    public void Invoke(object data)
    {
        Execute(data);
    }

    private void Rename(Metadata md, string newName)
    {
        _logger.Information($"Renamed '{md.VisibleName}' to '{newName}'");

        md.VisibleName = newName;

        MetadataStorage.Local.Remove(md);
        MetadataStorage.Local.AddMetadata(md, out var alreadyAdded);

        if (alreadyAdded)
        {
            return;
        }

        md.Save();

        md.Upload();

        DialogService.Close();
    }
}
