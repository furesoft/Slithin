using System.Collections.Generic;
using Avalonia.Controls;

namespace Slithin.Core.ItemContext;

public interface IContextProvider
{
    object ParentViewModel { get; set; }

    bool CanHandle(object obj);

    ICollection<MenuItem> GetMenu(object obj);
}