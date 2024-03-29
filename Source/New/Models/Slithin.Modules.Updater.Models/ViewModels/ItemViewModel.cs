﻿using NuGet.Versioning;
using Slithin.Core.MVVM;

namespace Slithin.Modules.Updater.Models.ViewModels;

public class ItemViewModel : BaseViewModel
{
    private bool _isDone;

    public string Name { get; set; }
    public NuGetVersion Version { get; set; }

    public bool IsDone
    {
        get => _isDone;
        set => SetValue(ref _isDone, value);
    }
}
