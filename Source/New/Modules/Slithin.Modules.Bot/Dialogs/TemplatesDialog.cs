using AuroraModularis.Core;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using DotNext;
using Slithin.Modules.Notebooks.UI.Models;
using Slithin.Modules.Repository.Models;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using Syn.Bot.Oscova.Messages;
using Result = Syn.Bot.Oscova.Result;

namespace Slithin.Modules.Bot.Dialogs;

public class TemplatesDialog : Dialog
{
    [Expression("How many templates exist?")]
    [Expression("How many templates do I have?")]
    public static void TemplatesAmount(Context context, Result result)
    {
        var templateStorage = ServiceContainer.Current.Resolve<ITemplateStorage>();
        
        var amount = templateStorage.Templates.Length;
        
        result.SendResponse($"You have {amount} templates.");
    }

    [Expression("how many templates are @orientation?")]
    public static void TemplateOrientation(Context context, Result result)
    {
        var templateStorage = ServiceContainer.Current.Resolve<ITemplateStorage>();

        var selectedOrientation = result.Entities.OfType("orientation").Value;
        var isLandscapeSelected = selectedOrientation == "landscape";

        var templatesAmount = templateStorage.Templates.Count(_ => _.Landscape == isLandscapeSelected);
        
        result.SendResponse($"You have {templatesAmount} templates that are $orientation.");
    }

    [Expression("Show me the @asset @sys.text")]
    public static void ShowTemplate(Context context, Result result)
    {
        var asset = result.Entities.OfType("asset").Value;
        var name = result.Entities.OfType(Sys.Text).Value;

        Result<IImage> bitmap = null;

        if (asset == "template")
        {
            bitmap = GetTemplateImage(name);
        }
        else if (asset == "notebook")
        {
            bitmap = GetNotebookThumbnail(name);
        }

        if (!bitmap.IsSuccessful)
        {
            result.SendResponse(bitmap.Error.Message);
            return;
        }

        var response = new Response();
        response.Messages.Add(new BitmapMessage(bitmap.Value));
        
        result.SendResponse(response);
    }

    private static Result<IImage> GetNotebookThumbnail(string name)
    {
        var thumbnailLoader = ServiceContainer.Current.Resolve<IThumbnailLoader>();
        var metadataStorage = ServiceContainer.Current.Resolve<IMetadataRepository>();

        var metadata = metadataStorage.GetByName(name);

        if (metadata)
        {
            return new(thumbnailLoader.LoadImage(new FileModel(metadata.Value.VisibleName, metadata,
                metadata.Value.IsPinned)));
        }

        return new(new Exception("Notebook not found"));
    }

    private static Result<IImage> GetTemplateImage(string name)
    {
        var templateStorage = ServiceContainer.Current.Resolve<ITemplateStorage>();

        var template = templateStorage.Templates.FirstOrDefault(_ => _.Name == name);

        if (templateStorage is null)
        {
            return new(new Exception("Template not found"));
        }
        
        return new(template.Image);
    }
}
