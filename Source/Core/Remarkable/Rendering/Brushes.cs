using Slithin.Core.Remarkable.LinesAreBeatiful;
namespace Slithin.Core.Remarkable.Rendering
{
    public enum Brushes
    {
        /**
         * Ballpoint pen.
         *
         * GUI: 1-1
         */
        Pen1 = 2,

        /**
         * Marker pen.
         *
         * GUI: 1-2
         */
        Pen2 = 3,

        /**
         * Fineliner pen.
         *
         * GUI: 1-3
         */
        Fineliner = 4,

        /**
         * Sharp pencil.
         *
         * GUI: 2-1
         */
        Pencilsharp = 7,

        /**
         * Tilt pencil.
         *
         * GUI: 2-2
         */
        Penciltilt = 1,

        /**
         * Paintbrush.
         *
         * GUI: 3
         */
        Brush = 0,

        /**
         * Highlighter.
         *
         * GUI: 4
         * (always color 0)
         */
        Highlighter = 5,

        /**
         * Eraser.
         *
         * GUI: 5-1
         */
        Rubber = 6,

        /**
         * not in GUI
         */
        Unknown_brush = 7,

        /**
         * Erase selection.
         *
         * GUI: 5-2
         */
        Rubberarea = 8,

        /**
         * Erase page.
         *
         * GUI: 5-3
         */
        Eraseall = 9,

        /**
         * Selection brush
         *
         * not in GUI
         */
        Selectionbrush1 = 10,

        /**
         * Selection brush
         *
         * not in GUI
         */
        Selectionbrush2 = 11,

        /**
         * Fine line
         *
         * not in GUI
         */
        Fineline1 = 12,
        Fineline2 = 13,
        Fineline3 = 14
    };
}
