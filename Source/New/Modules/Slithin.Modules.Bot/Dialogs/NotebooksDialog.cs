using AuroraModularis.Core;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Repository.Models;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;

namespace Slithin.Modules.Bot.Dialogs;

public class NotebooksDialog : Dialog
{
    [Expression("How many notebooks exist?")]
    [Expression("How many notebooks do I have?")]
    public static void NotebookAmount(Context context, Result result)
    {
        var pathManager = ServiceContainer.Current.Resolve<IPathManager>();
        var metadataStorage = ServiceContainer.Current.Resolve<IMetadataRepository>();
        
        var amount = Directory.GetFiles(pathManager.NotebooksDir).Count(_ => _.EndsWith(".metadata"));
        var inTrash = metadataStorage.GetByParent("trash").Count();
        
        result.SendResponse($"There are {amount} notebooks. {inTrash} of them are in trash.");
    }

    [Expression("What is the id of @sys.text?")]
    public static void NotebookID(Context context, Result result)
    {
        var metadataStorage = ServiceContainer.Current.Resolve<IMetadataRepository>();
        var notebook = metadataStorage.GetByName(result.Entities.OfType(Sys.Text).Value);

        if (notebook)
        {
            result.SendResponse(notebook.Value.ID);
            return;
        }
        
        result.SendResponse("Cannot find a notebook '@sys.text'.");
    }
}
