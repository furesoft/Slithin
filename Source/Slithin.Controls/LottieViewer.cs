using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Threading;
using CommunityToolkit.WinUI.Lottie;
using Xiejiang.SKLottie.Content;
using Xiejiang.SKLottie.Drawer;

namespace Slithin.Controls;

public class LottieViewer : Control
{
    public static readonly DirectProperty<LottieViewer, string> LottieFileProperty =
        AvaloniaProperty.RegisterDirect<LottieViewer, string>
            (
             nameof(LottieFile),
             o => o.LottieFile,
             (
                 o,
                 v
             ) => o.LottieFile = v
            );

    public static readonly DirectProperty<LottieViewer, double> MaxFrameProperty =
        AvaloniaProperty.RegisterDirect<LottieViewer, double>
            (
             nameof(MaxFrame),
             o => o.MaxFrame
            );

    public static readonly DirectProperty<LottieViewer, double> CurrentFrameProperty =
        AvaloniaProperty.RegisterDirect<LottieViewer, double>
            (
             nameof(CurrentFrame),
             o => o.CurrentFrame,
             (
                 o,
                 v
             ) => o.CurrentFrame = v
            );

    public static readonly DirectProperty<LottieViewer, float> ScaleProperty =
        AvaloniaProperty.RegisterDirect<LottieViewer, float>
            (
             nameof(Scale),
             o => o.Scale,
             (
                 o,
                 v
             ) => o.Scale = v
            );

    public static readonly DirectProperty<LottieViewer, double> TimeStretchProperty =
        AvaloniaProperty.RegisterDirect<LottieViewer, double>
            (
             nameof(TimeStretch),
             o => o.TimeStretch,
             (
                 o,
                 v
             ) => o.TimeStretch = v
            );

    private readonly Stopwatch _stopwatch = new();

    private SklLottieComposition? _sklLottieComposition;

    private string _lottieFile;

    private double _maxFrame;

    private double _currentFrame;

    private float _scale = 1f;

    private double _timeStretch = 1d;

    private bool _isPlaying;

    private bool _needRendering;

    private DrawingBuffers? _drawingBuffers;

    public LottieViewer()
    {
        ClipToBounds = true;
    }

    public SklLottieComposition? SklLottieComposition
    {
        get => _sklLottieComposition;
        private set
        {
            if (!ReferenceEquals
                    (
                     _sklLottieComposition,
                     value
                    ))
            {
                _sklLottieComposition = value;

                CurrentFrame = 0d;

                if (_sklLottieComposition is not null)
                {
                    _drawingBuffers = new DrawingBuffers
                        (
                         (int)_sklLottieComposition.Width,
                         (int)_sklLottieComposition.Height
                        );

                    MaxFrame = _sklLottieComposition.FrameCount;
                    if (IsPlaying)
                    {
                        _stopwatch.Start();
                    }
                }
                else
                {
                    MaxFrame = 0;
                    _drawingBuffers?.Dispose();
                    _drawingBuffers = null;
                }

                _needRendering = true;

                InvalidateVisual();
            }
        }
    }

    public string LottieFile
    {
        get => _lottieFile;
        set
        {
            _lottieFile = value;

            if (string.IsNullOrWhiteSpace
                    (
                     _lottieFile
                    )
               )
            {
                SklLottieComposition = null;
            }
            else if (!File.Exists
                         (
                          _lottieFile
                         ))
            {
                throw new FileNotFoundException
                    (
                     "Can not open lottie file.",
                     _lottieFile
                    );
            }

            Loader.LoadAsync
                              (
                               _lottieFile
                              )
                             .ContinueWith
                                  (
                                   t =>
                                   {
                                       if (t.IsCompletedSuccessfully)
                                       {
                                           SklLottieComposition = new SklLottieComposition
                                               (
                                                t.Result
                                               );
                                       }
                                       else
                                       {
                                           SklLottieComposition = null;
                                       }
                                   }
                                  );
        }
    }

    public double MaxFrame
    {
        get => _maxFrame;
        private set
        {
            Dispatcher.UIThread.Post
                (
                 () =>
                 {
                     SetAndRaise
                         (
                          MaxFrameProperty,
                          ref _maxFrame,
                          value
                         );
                 }
                );
        }
    }

    public double CurrentFrame
    {
        get => _currentFrame;
        set
        {
            if (SklLottieComposition is not null)
            {
                SklLottieComposition.CurrentFrame = value;
                _needRendering = true;
            }

            Dispatcher.UIThread.Post
                (
                 () =>
                 {
                     SetAndRaise
                         (
                          CurrentFrameProperty,
                          ref _currentFrame,
                          value
                         );
                 }
                );
        }
    }

    public float Scale
    {
        get => _scale;
        set
        {
            if (SklLottieComposition is not null)
            {
                SklLottieComposition.CurrentFrame = value;
                _needRendering = true;
            }

            Dispatcher.UIThread.Post
                (
                 () =>
                 {
                     SetAndRaise
                         (
                          ScaleProperty,
                          ref _scale,
                          value
                         );
                 }
                );
        }
    }

    public double TimeStretch
    {
        get => _timeStretch;
        set
        {
            _needRendering = true;
            SetAndRaise
                (
                 TimeStretchProperty,
                 ref _timeStretch,
                 value
                );
        }
    }

    public bool IsPlaying
    {
        get => _isPlaying;
        set
        {
            _isPlaying = value;

            if (_isPlaying)
            {
                _stopwatch.Start();
            }
            else
            {
                _stopwatch.Stop();
            }
        }
    }

    public override void Render
    (
        DrawingContext context
    )
    {
        if (SklLottieComposition is null || _drawingBuffers is null)
        {
            return;
        }

        //按照真实时间
        if (_stopwatch.IsRunning)
        {
            if (_stopwatch.Elapsed * TimeStretch >= TimeSpan.FromSeconds
                    (
                     SklLottieComposition.FrameCount / SklLottieComposition.FramesPerSecond
                    ))
            {
                _stopwatch.Restart();
            }

            SklLottieComposition.Time = _stopwatch.Elapsed * TimeStretch;
            CurrentFrame = SklLottieComposition.CurrentFrame;
        }

        if (_needRendering)
        {
            _needRendering = false;
        }

        var noSkia = new FormattedText();
        noSkia.Text = "Current rendering API is not Skia";
        noSkia.FontSize = 12;

        context.Custom
            (
             new LottieDrawOperation
                 (
                  new Rect
                      (
                       0,
                       0,
                       Bounds.Width,
                       Bounds.Height
                      ),
                  noSkia,
                  _drawingBuffers,
                  SklLottieComposition,
                  Scale
                 )
            );

        Dispatcher.UIThread.InvokeAsync
            (
             InvalidateVisual,
             DispatcherPriority.Background
            );
    }
}

public class LottieDrawOperation : ICustomDrawOperation
{
    private static Stopwatch St = Stopwatch.StartNew();
    private readonly FormattedText _noSkia;

    private readonly float _scale;
    private DrawingBuffers _drawingBuffers;
    private SklLottieComposition _sklLottieComposition;

    public LottieDrawOperation
    (
        Rect bounds,
        FormattedText noSkia,
        DrawingBuffers drawingBuffers,
        SklLottieComposition sklLottieComposition,
        float scale
    )
    {
        _noSkia = noSkia;
        Bounds = bounds;
        _drawingBuffers = drawingBuffers;
        _sklLottieComposition = sklLottieComposition;
        _scale = scale;
    }

    public Rect Bounds { get; }

    public void Dispose()
    {
        // No-op
    }

    public bool HitTest
    (
        Point p
    ) => false;

    public bool Equals
    (
        ICustomDrawOperation other
    ) =>
        false;

    public void Render
    (
        IDrawingContextImpl context
    )
    {
        var canvas = (context as ISkiaDrawingContextImpl)?.SkCanvas;
        if (canvas == null)
        {
            using (var drawingContext = new DrawingContext
                       (
                        context,
                        false
                       ))
            {
                drawingContext.DrawText
                    (
                     new ImmutableSolidColorBrush
                         (
                          Colors.AliceBlue
                         ),
                     new Point(),
                     _noSkia
                    );
            }
        }
        else
        {
            canvas.Save();

            try
            {
                LottieDrawer.DrawSklLottieComposition
                    (
                     canvas,
                     _sklLottieComposition,
                     _drawingBuffers,
                     _scale,
                     false
                    );
            }
            catch (Exception e)
            {
                Console.WriteLine
                    (
                     e
                    );
            }

            canvas.Restore();
        }
    }
}
