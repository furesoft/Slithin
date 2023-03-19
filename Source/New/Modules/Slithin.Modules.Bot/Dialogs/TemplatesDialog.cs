﻿using AuroraModularis.Core;
using Slithin.Modules.Repository.Models;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;

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
}
