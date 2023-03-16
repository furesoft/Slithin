using Avalonia;
using Avalonia.Animation;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using JetBrains.Annotations;
using Slithin.Controls.Gif.Decoding;

namespace Slithin.Controls.Gif;

internal class GifInstance : IDisposable
{
    public IterationCount IterationCount { get; set; }
    public bool AutoStart { get; private set; } = true;
    private readonly GifDecoder _gifDecoder;
    private readonly WriteableBitmap _targetBitmap;
    private TimeSpan _totalTime;
    private readonly List<TimeSpan> _frameTimes;
    private uint _iterationCount;
    private int _currentFrameIndex;
    private uint _totalFrameCount;
    private readonly List<ulong> _colorTableIdList;

    public CancellationTokenSource CurrentCts { get; }

    public GifInstance(object newValue)
    {
        CurrentCts = new CancellationTokenSource();

        var currentStream = newValue switch
        {
            Stream s => s,
            Uri u => GetStreamFromUri(u),
            string str => GetStreamFromString(str),
            _ => throw new InvalidDataException("Unsupported source object")
        };

        if (!currentStream.CanSeek)
            throw new InvalidDataException("The provided stream is not seekable.");

        if (!currentStream.CanRead)
            throw new InvalidOperationException("Can't read the stream provided.");

        if (currentStream.CanSeek)
        {
            currentStream.Seek(0, SeekOrigin.Begin);
        }

        _gifDecoder = new GifDecoder(currentStream, CurrentCts.Token);
        var pixSize = new PixelSize(_gifDecoder.Header.Dimensions.Width, _gifDecoder.Header.Dimensions.Height);

        _targetBitmap = new WriteableBitmap(pixSize, new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Opaque);
        GifPixelSize = pixSize;

        _totalTime = TimeSpan.Zero;

        _frameTimes = _gifDecoder.Frames.Select(frame =>
        {
            _totalTime = _totalTime.Add(frame.FrameDelay);
            return _totalTime;
        }).ToList();

        _gifDecoder.RenderFrame(0, _targetBitmap);

        // Save the color table cache ID's to refresh them on cache while
        // // the image is either stopped/paused.
        // _colorTableIdList = _gifDecoder.Frames
        //     .Where(p => p.IsLocalColorTableUsed)
        //     .Select(p => p.LocalColorTableCacheID)
        //     .ToList();

        // if (_gifDecoder.Header.HasGlobalColorTable)
        //     _colorTableIdList.Add(_gifDecoder.Header.GlobalColorTableCacheID);
    }

    private Stream GetStreamFromString(string str)
    {
        if (!Uri.TryCreate(str, UriKind.RelativeOrAbsolute, out var res))
        {
            throw new InvalidCastException("The string provided can't be converted to URI.");
        }

        return GetStreamFromUri(res);
    }

    private Stream GetStreamFromUri(Uri uri)
    {
        var uriString = uri.OriginalString.Trim();

        if (!uriString.StartsWith("resm") && !uriString.StartsWith("avares"))
            throw new InvalidDataException(
                "The URI provided is not currently supported.");

        var assetLocator = AvaloniaLocator.Current.GetService<IAssetLoader>();

        if (assetLocator is null)
            throw new InvalidDataException(
                "The resource URI was not found in the current assembly.");

        return assetLocator.Open(uri);
    }

    public PixelSize GifPixelSize { get; }

    public void Dispose()
    {
        CurrentCts.Cancel();
        _targetBitmap?.Dispose();
    }

    [CanBeNull]
    public WriteableBitmap ProcessFrameTime(TimeSpan stopwatchElapsed)
    {
        if (!IterationCount.IsInfinite && _iterationCount > IterationCount.Value)
        {
            return null;
        }

        if (CurrentCts.IsCancellationRequested)
        {
            return null;
        }

        var timeModulus = TimeSpan.FromTicks(stopwatchElapsed.Ticks % _totalTime.Ticks);
        var targetFrame = _frameTimes.LastOrDefault(x => x <= timeModulus);
        var currentFrame = _frameTimes.IndexOf(targetFrame);
        if (currentFrame == -1) currentFrame = 0;

        if (_currentFrameIndex != currentFrame)
        {
            // We skipped too much frames in between render updates
            // so clear the frame and redraw the previous one.
            if (currentFrame - _currentFrameIndex > 1)
            {
            }

            _currentFrameIndex = currentFrame;

            _gifDecoder.RenderFrame(_currentFrameIndex, _targetBitmap);

            _totalFrameCount++;

            if (!IterationCount.IsInfinite && _totalFrameCount % _frameTimes.Count == 0)
                _iterationCount++;
        }

        return _targetBitmap;
    }
}
