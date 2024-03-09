﻿using System.Collections.Specialized;
using System.Text;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Logging;
using Avalonia.Threading;

namespace Slithin.Controls.Navigation;

/// <summary>
///     Displays <see cref="UserControl" /> instances (Pages in WinUI), supports navigation to new pages,
///     and maintains a navigation history to support forward and backward navigation.
/// </summary>
public partial class Frame : ContentControl
{
    private static readonly Dictionary<string, Frame> _frames = new();
    private readonly List<Control> _cache = new(10);

    private bool _isNavigating;

    private ContentPresenter _presenter;

    public Frame()
    {
        var back = new AvaloniaList<PageStackEntry>();
        var forw = new AvaloniaList<PageStackEntry>();

        back.CollectionChanged += OnBackStackChanged;
        forw.CollectionChanged += OnFowardwardStackChanged;

        BackStack = back;
        ForwardStack = forw;
    }

    public static Frame GetFrame(string name)
    {
        return _frames[name];
    }

    public static string GetFrameName(Frame target)
    {
        return string.Empty;
    }

    public static void SetFrameName(Frame target, string name)
    {
        _frames.TryAdd(name, target);
    }

    /// <summary>
    ///     Serializes the Frame navigation history into a string
    /// </summary>
    /// <returns></returns>
    public string GetNavigationState()
    {
        if (!IsNavigationStackEnabled)
        {
            throw new InvalidOperationException(
                "Cannot retreive navigation stack when IsNavigationStackEnabled is false");
        }

        // Format of the Navigation state string - this is not the same as WinUI
        // Full.Type.Name.Here|Serialized Parameter // First line is the current page
        // N // Number of pages in BackStack
        // -- BackStack Entries here, same format as above
        // N // Number of pages in ForwardStack
        // -- ForwardStack Entries here, same format as above

        static void AppendEntry(StringBuilder sb, PageStackEntry entry)
        {
            if (entry == null)
            {
                return;
            }

            sb.Append(entry.SourcePageType.AssemblyQualifiedName);
            sb.Append('|');
            sb.Append(entry.Parameter);

            sb.AppendLine();
        }

        var sb = new StringBuilder();

        AppendEntry(sb, CurrentEntry);

        sb.AppendLine(BackStackDepth.ToString());

        for (var i = 0; i < BackStackDepth; i++)
        {
            AppendEntry(sb, BackStack[i]);
        }

        sb.AppendLine(ForwardStack.Count.ToString());

        for (var i = 0; i < ForwardStack.Count; i++)
        {
            AppendEntry(sb, ForwardStack[i]);
        }

        return sb.ToString();
    }

    /// <summary>
    ///     Navigates to the most recent item in back navigation history, if a Frame manages its own navigation history.
    /// </summary>
    public void GoBack()
    {
        GoBack(null);
    }

    /// <summary>
    ///     Navigates to the most recent item in back navigation history, if a Frame manages its own navigation history,
    ///     and specifies the animated transition to use.
    /// </summary>
    /// <param name="infoOverride">Info about the animated transition to use.</param>
    public void GoBack(NavigationTransitionInfo infoOverride)
    {
        if (CanGoBack)
        {
            var entry = _backStack[_backStack.Count - 1];
            if (infoOverride != null)
            {
                entry.NavigationTransitionInfo = infoOverride;
            }
            else
            {
                entry.NavigationTransitionInfo = CurrentEntry?.NavigationTransitionInfo ?? null;
            }

            NavigateCore(entry, NavigationMode.Back);
        }
    }

    /// <summary>
    ///     Navigates to the most recent item in forward navigation history, if a Frame manages its own navigation history.
    /// </summary>
    public void GoForward()
    {
        if (CanGoForward)
        {
            NavigateCore(_forwardStack[_forwardStack.Count - 1], NavigationMode.Forward);
        }
    }

    /// <summary>
    ///     Causes the Frame to load content represented by the specified Page.
    /// </summary>
    /// <param name="sourcePageType">he page to navigate to, specified as a type reference to its partial class type.</param>
    /// <returns>false if a <see cref="NavigationFailed" /> event handler has set Handled to true; otherwise, true. </returns>
    public bool Navigate(Type sourcePageType)
    {
        return Navigate(sourcePageType, null, null);
    }

    /// <summary>
    ///     Causes the Frame to load content represented by the specified Page, also passing a parameter to be
    ///     interpreted by the target of the navigation.
    /// </summary>
    /// <param name="sourcePageType">
    ///     The page to navigate to, specified as a type reference to its
    ///     partial class type.
    /// </param>
    /// <param name="parameter">
    ///     The navigation parameter to pass to the target page;
    ///     must have a basic type (string, char, numeric, or GUID) to support parameter serialization
    ///     using GetNavigationState.
    /// </param>
    /// <returns>
    ///     false if a <see cref="NavigationFailed" /> event handler has set Handled to true;
    ///     otherwise, true.
    /// </returns>
    public bool Navigate(Type sourcePageType, object parameter)
    {
        return Navigate(sourcePageType, parameter, null);
    }

    /// <summary>
    ///     Causes the Frame to load content represented by the specified Page -derived data type,
    ///     also passing a parameter to be interpreted by the target of the navigation, and a value
    ///     indicating the animated transition to use.
    /// </summary>
    /// <param name="sourcePageType">
    ///     The page to navigate to, specified as a type reference to
    ///     its partial class type.
    /// </param>
    /// <param name="parameter">
    ///     The navigation parameter to pass to the target page; must have a
    ///     basic type (string, char, numeric, or GUID) to support parameter serialization using
    ///     GetNavigationState.
    /// </param>
    /// <param name="infoOverride">Info about the animated transition.</param>
    /// <returns>
    ///     false if a <see cref="NavigationFailed" /> event handler has set Handled to true;
    ///     otherwise, true.
    /// </returns>
    public bool Navigate(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride)
    {
        return NavigateCore(new(sourcePageType, parameter,
            infoOverride), NavigationMode.New);
    }

    /// <summary>
    ///     Causes the Frame to load content represented by the specified Page, also passing a parameter to be
    ///     interpreted by the target of the navigation.
    /// </summary>
    /// <param name="sourcePageType">The page to navigate to, specified as a type reference to its partial class type.</param>
    /// <param name="parameter">
    ///     The navigation parameter to pass to the target page; must have a basic type
    ///     (string, char, numeric, or GUID) to support parameter serialization using GetNavigationState.
    /// </param>
    /// <param name="navOptions">
    ///     Options for the navigation, including whether it is recorded in the navigation stack
    ///     and what transition animation is used.
    /// </param>
    /// <returns>false if a <see cref="NavigationFailed" /> event handler has set Handled to true; otherwise, true.</returns>
    public bool NavigateToType(Type sourcePageType, object parameter, FrameNavigationOptions navOptions)
    {
        return NavigateCore(new(sourcePageType, parameter, navOptions?.TransitionInfoOverride),
            NavigationMode.New, navOptions);
    }

    /// <summary>
    ///     Reads and restores the navigation history of a Frame from a provided serialization string.
    /// </summary>
    /// <param name="navState">The serialization string that supplies the restore point for navigation history.</param>
    public void SetNavigationState(string navState)
    {
        SetNavigationState(navState, false);
    }

    /// <summary>
    ///     Reads and restores the navigation history of a Frame from a provided serialization string,
    ///     and optionally supresses navigation to the last page type
    /// </summary>
    /// <param name="navState">The serialization string that supplies the restore point for navigation history.</param>
    /// <param name="suppressNavigate">
    ///     true to restore navigation history without navigating to the current page; otherwise,
    ///     false.
    /// </param>
    /// <remarks>
    ///     Calling SetNavigationState with suppressNavigate set to true, OnNavigatedTo is not called and the current page is
    ///     placed into
    ///     the BackStack
    /// </remarks>
    public void SetNavigationState(string navState, bool suppressNavigate)
    {
        if (!IsNavigationStackEnabled)
        {
            throw new InvalidOperationException("Cannot set navigation stack when IsNavigationStackEnabled is false");
        }

        BackStack.Clear();
        ForwardStack.Clear();
        CurrentEntry = null;
        Content = null;
        _cache.Clear();

        // Format of the Navigation state string - this is not the same as WinUI
        // Full.Type.Name.Here|Serialized Parameter // First line is the current page
        // N // Number of pages in BackStack
        // -- BackStack Entries here, same format as above
        // N // Number of pages in ForwardStack
        // -- ForwardStack Entries here, same format as above

        using (var reader = new StringReader(navState))
        {
            var firstLine = reader.ReadLine(); // Current Page

            var addCurrentEntryToBackStack = false;
            // Current page was null when saved, don't restore null
            // This is the only place we're allowed to have null - since a call to
            // Navigate(null) will fail to navigate & nothing is added to the stack
            if (firstLine[0] != '|')
            {
                var indexOfSep = firstLine.IndexOf('|');
                var pageType = Type.GetType(firstLine.Substring(0, indexOfSep));
                var param = firstLine.Substring(indexOfSep + 1);
                CurrentEntry = new(pageType, param, null);

                if (!suppressNavigate)
                {
                    var page = CreatePageAndCacheIfNecessary(pageType);
                    CurrentEntry.Instance = page;

                    SetContentAndAnimate(CurrentEntry);
                    // We only raise the NavigatedEvent
                    page.RaiseEvent(
                        new NavigationEventArgs(page, NavigationMode.New, null, param, pageType)
                        {
                            RoutedEvent = NavigatedToEvent
                        });
                }
                else
                {
                    addCurrentEntryToBackStack = true;
                }
            }

            var numBackLine = int.Parse(reader.ReadLine());

            for (var i = 0; i < numBackLine; i++)
            {
                var line = reader.ReadLine();
                var indexOfSep = line.IndexOf('|');
                var pageType = Type.GetType(line.Substring(0, indexOfSep));

                if (pageType == null)
                {
                    // Don't fail if we get an invalid page, log & continue
                    Logger.TryGet(LogEventLevel.Error, "Frame")?
                        .Log("Frame",
                            $"Attempting to parse the type '{line.Substring(0, indexOfSep)}' failed. Page was skipped");

                    continue;
                }

                var param = line.Substring(indexOfSep + 1);

                var entry = new PageStackEntry(pageType, param, null);
                BackStack.Add(entry);
            }

            if (addCurrentEntryToBackStack)
            {
                BackStack.Add(CurrentEntry);
                CurrentEntry = null;
            }

            var numForwardLine = int.Parse(reader.ReadLine());

            for (var i = 0; i < numForwardLine; i++)
            {
                var line = reader.ReadLine();
                var indexOfSep = line.IndexOf('|');
                var pageType = Type.GetType(line.Substring(0, indexOfSep));
                var param = line.Substring(indexOfSep + 1);

                if (pageType == null)
                {
                    // Don't fail if we get an invalid page, log & continue
                    Logger.TryGet(LogEventLevel.Error, "Frame")?
                        .Log("Frame",
                            $"Attempting to parse the type '{line.Substring(0, indexOfSep)}' failed. Page was skipped");

                    continue;
                }

                var entry = new PageStackEntry(pageType, param, null);
                ForwardStack.Add(entry);
            }
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _presenter = e.NameScope.Find<ContentPresenter>("ContentPresenter");
    }


    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == ContentProperty)
        {
            if (change.NewValue == null)
            {
                CurrentEntry = null;
            }
        }
        else if (change.Property == SourcePageTypeProperty)
        {
            if (!_isNavigating)
            {
                if (change.NewValue is null)
                {
                    throw new InvalidOperationException("SourcePageType cannot be null. Use Content instead.");
                }

                Navigate((Type)change.NewValue);
            }
        }
    }

    private Control CheckCacheAndGetPage(Type srcPageType)
    {
        if (CacheSize == 0)
        {
            return null;
        }

        for (var i = _cache.Count - 1; i >= 0; i--)
        {
            if (_cache[i].GetType() == srcPageType)
            {
                var item = _cache[i];
                _cache.RemoveAt(i);
                return item;
            }
        }

        return null;
    }

    private Control CreatePageAndCacheIfNecessary(Type srcPageType)
    {
        if (CacheSize == 0)
        {
            return Activator.CreateInstance(srcPageType) as Control;
        }

        for (var i = 0; i < _cache.Count; i++)
        {
            if (_cache[i].GetType() == srcPageType)
            {
                throw new($"An object of type {srcPageType} has already been added to the Navigation Stack");
            }
        }

        var newPage = Activator.CreateInstance(srcPageType) as Control;
        _cache.Add(newPage);
        if (_cache.Count > CacheSize)
        {
            _cache.RemoveAt(0);
        }

        return newPage;
    }

    private bool NavigateCore(PageStackEntry entry, NavigationMode mode, FrameNavigationOptions options = null)
    {
        try
        {
            _isNavigating = true;

            var ea = new NavigatingCancelEventArgs(mode,
                entry.NavigationTransitionInfo,
                entry.Parameter,
                entry.SourcePageType);

            Navigating?.Invoke(this, ea);

            if (ea.Cancel)
            {
                OnNavigationStopped(entry, mode);
                return false;
            }

            // Tell the current page we want to navigate away from it
            if (CurrentEntry?.Instance is Control oldPage)
            {
                ea.RoutedEvent = NavigatingFromEvent;
                oldPage.RaiseEvent(ea);

                if (ea.Cancel)
                {
                    OnNavigationStopped(entry, mode);
                    return false;
                }
            }

            //Navigate to new page
            var prevEntry = CurrentEntry;

            if (mode == NavigationMode.New)
            {
                //Check if we already have an instance of the page in the cache
                entry.Instance = CheckCacheAndGetPage(entry.SourcePageType);
            }

            if (entry.Instance == null)
            {
                var page = CreatePageAndCacheIfNecessary(entry.SourcePageType);
                if (page == null)
                {
                    throw new ArgumentException($"The type {entry.SourcePageType} is not a valid page type.");
                }

                entry.Instance = page;
            }

            var oldEntry = CurrentEntry;
            CurrentEntry = entry;

            var navEA = new NavigationEventArgs(
                CurrentEntry.Instance,
                mode, entry.NavigationTransitionInfo,
                entry.Parameter,
                entry.SourcePageType);

            // Old page is now unloaded, raise OnNavigatedFrom
            if (oldEntry != null)
            {
                navEA.RoutedEvent = NavigatedFromEvent;
                oldEntry.Instance.RaiseEvent(navEA);
            }

            SetContentAndAnimate(entry);

            var addToNavStack = options?.IsNavigationStackEnabled ?? IsNavigationStackEnabled;

            if (addToNavStack)
            {
                switch (mode)
                {
                    case NavigationMode.New:
                        ForwardStack.Clear();
                        if (prevEntry != null)
                        {
                            if (BackStack.Count == CacheSize)
                            {
                                if (BackStack.Count > 0)
                                {
                                    BackStack.RemoveAt(0);
                                }
                            }

                            BackStack.Add(prevEntry);
                        }

                        break;

                    case NavigationMode.Back:
                        ForwardStack.Add(prevEntry);
                        BackStack.Remove(CurrentEntry);
                        break;

                    case NavigationMode.Forward:
                        BackStack.Add(prevEntry);
                        ForwardStack.Remove(CurrentEntry);
                        break;

                    case NavigationMode.Refresh:
                        break;
                }
            }

            SourcePageType = entry.SourcePageType;
            //CurrentSourcePageType = entry.SourcePageType;

            Navigated?.Invoke(this, navEA);

            // New Page is loaded, let's tell the page
            if (entry.Instance is Control newPage)
            {
                navEA.RoutedEvent = NavigatedToEvent;
                newPage.RaiseEvent(navEA);
            }

            //Need to find compatible method for this
            //VisualTreeHelper.CloseAllPopups();

            return true;
        }
        catch (Exception ex)
        {
            NavigationFailed?.Invoke(this, new(ex, entry.SourcePageType));

            //I don't really want to throw an exception and break things. Just return false
            return false;
        }
        finally
        {
            _isNavigating = false;
        }
    }

    private void OnBackStackChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        var oldCount = _backStack.Count - (e.NewItems?.Count ?? 0) + (e.OldItems?.Count ?? 0);

        var oldBack = oldCount > 0;
        var newBack = _backStack.Count > 0;
    /*    RaisePropertyChanged(new AvaloniaPropertyChangedEventArgs<bool>(this,
            CanGoBackProperty, oldBack, newBack, BindingPriority.LocalValue));
        RaisePropertyChanged(new AvaloniaPropertyChangedEventArgs<int>(this,
            BackStackDepthProperty, oldCount, _backStack.Count, BindingPriority.LocalValue));*/
    }

    private void OnFowardwardStackChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        var oldCount = _forwardStack.Count - (e.NewItems?.Count ?? 0) + (e.OldItems?.Count ?? 0);

        var oldForward = oldCount > 0;
        var newForward = _forwardStack.Count > 0;
        /*RaisePropertyChanged(new AvaloniaPropertyChangedEventArgs<bool>(this,
            CanGoForwardProperty, oldForward, newForward, BindingPriority.LocalValue));*/
    }

    private void OnNavigationStopped(PageStackEntry entry, NavigationMode mode)
    {
        NavigationStopped?.Invoke(this, new(entry.Instance,
            mode, entry.NavigationTransitionInfo, entry.Parameter, entry.SourcePageType));
    }

    private void SetContentAndAnimate(PageStackEntry entry)
    {
        if (entry == null)
        {
            return;
        }

        Content = entry.Instance;

        if (_presenter != null)
        {
            //Default to entrance transition
            entry.NavigationTransitionInfo = entry.NavigationTransitionInfo ?? new EntranceNavigationTransitionInfo();
            _presenter.Opacity = 0;
            // Very busy pages will delay loading b/c layout & render has to occur first
            // Posting this helps a little bit, but not much
            // Not really sure how to get the transition to occur while the page is loading
            // so speed is comparable to WinUI...this may be an Avalonia limitation???
            Dispatcher.UIThread.Post(() =>
            {
                entry.NavigationTransitionInfo.RunAnimation(_presenter);
            }, DispatcherPriority.Loaded);
        }
    }
}
