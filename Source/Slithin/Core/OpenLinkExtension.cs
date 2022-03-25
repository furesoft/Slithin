using System;
using Avalonia.Markup.Xaml;

namespace Slithin.Core;

public class OpenLinkExtension : MarkupExtension
{
    public OpenLinkExtension(string link)
    {
        Link = link;
    }

    public string Link { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return new DelegateCommand(_ => Utils.OpenUrl(Link));
    }
}
