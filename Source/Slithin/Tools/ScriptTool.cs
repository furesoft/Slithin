using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Slithin.Core;
using Slithin.Core.Services;
using Slithin.Models;

namespace Slithin.Tools;

public class ScriptTool : ITool
{
    private readonly ScriptInfo _info;

    /*public ScriptTool(ScriptInfo info)
    {
        _info = info;
    }*/

    public IImage Image
    {
        get
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            Stream imageStream;

            imageStream = assets.Open(new Uri("avares://Slithin/Resources/cubes.png"));

            return new Bitmap(imageStream);
        }
    }

    public Models.ScriptInfo Info => _info;

    public bool IsConfigurable => false;

    public Control GetModal()
    {
        /*
         * return AvaloniaRuntimeXamlLoader.Parse<Control>(
            Encoding.ASCII.GetString(uiSection.Content.ToArray())
        );
        */

        return null;
    }

    public void Invoke(object data)
    {
        var _mailboxService = ServiceLocator.Container.Resolve<IMailboxService>();

        _mailboxService.PostAction(() =>
        {
        });
    }
}
