using System.Collections.Generic;
using System.IO;
using System.Text;
using Slithin.Core.Services;

namespace Slithin.Core.Remarkable.LinesAreBeatiful
{
    public class Notebook
    {
        public Notebook(string id)
        {
            var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

            var files = Directory.GetFiles(Path.Combine(pathManager.NotebooksDir, id), "*.rm");
            PageCount = files.Length;

            Version = 3;
            ID = id;
        }

        public string ID { get; set; }
        public int PageCount { get; set; }
        public List<Page> Pages { get; set; } = new();

        public int Version { get; set; }

        public static Notebook Load(string id)
        {
            var notebook = new Notebook(id);
            var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

            for (int p = 0; p < notebook.PageCount; ++p)
            {
                var path = Path.Combine(pathManager.NotebooksDir, id, ".rm");
                var strm = File.OpenRead(path);
                var br = new BinaryReader(strm);

                // skip header
                strm.Seek(32, SeekOrigin.Begin);

                var version = br.ReadByte();

                // skip 10x space padding
                strm.Seek(10, SeekOrigin.Current);

                // layers
                Page curPage = new();
                curPage.Layers = new();

                var layerCount = br.ReadInt32();

                for (int nlay = 0; nlay < layerCount; ++nlay)
                {
                    ReadLayer(br, curPage);
                }

                br.Close();

                notebook.Pages.Add(curPage);
            }

            return notebook;
        }

        public void Save()
        {
            var pathManager = ServiceLocator.Container.Resolve<IPathManager>();
            for (var i = 0; i < Pages.Count; i++)
            {
                var p = Pages[i];

                var path = Path.Combine(pathManager.NotebooksDir, i.ToString(), ".rm");
                var strm = File.OpenWrite(path);
                var bw = new BinaryWriter(strm);

                // write header (33 bytes)
                bw.Write(Encoding.ASCII.GetBytes("reMarkable .lines file, version=3"));

                // write space padding
                bw.Write(Encoding.ASCII.GetBytes("          "));

                bw.Write(p.Layers.Count);
                foreach (var layer in p.Layers)
                {
                    bw.Write(layer.Lines.Count);

                    foreach (var line in layer.Lines)
                    {
                        bw.Write((int)line.BrushType);
                        bw.Write((int)line.Color);
                        bw.Write(line.unknown_line_attribute);
                        bw.Write(BaseSizes.GetValue(line.BrushBaseSize));

                        bw.Write(line.Points.Count);

                        foreach (var point in line.Points)
                        {
                            bw.Write(point.X);
                            bw.Write(point.Y);

                            bw.Write(point.Speed);
                            bw.Write(point.Direction);
                            bw.Write(point.Width);
                            bw.Write(point.Pressure);
                        }
                    }
                }

                bw.Close();
            }
        }

        private static void ReadLayer(BinaryReader fstream, Page curPage)
        {
            Layer curLayer = new();
            curLayer.Lines = new();

            var lineCount = fstream.ReadInt32();

            for (int nl = 0; nl < lineCount; ++nl)
            {
                ReadLine(fstream, curLayer);
            }

            curPage.Layers.Add(curLayer);
        }

        private static void ReadLine(BinaryReader fstream, Layer curLayer)
        {
            Line curLine = new();
            curLine.Points = new();

            // select 1-1: 2 (pen)
            // select 1-2: 3 (pen)
            // select 1-3: 4 (fine liner)
            // select 2-1: 7 (pencil sharp)
            // select 2-2: 1 (pencil wide)
            // select 3:   0 (brush)
            // select 4:   5 (marker/highlighter: always color 0)
            // what is/was 6? :-)
            curLine.BrushType = (Brushes)fstream.ReadInt32();

            // color (0: black, 1: grey, 2: white)
            curLine.Color = (Colors)fstream.ReadInt32();

            // unknown 4 Byte (int32_t?), always zero?
            // non-stored information about "selected" lines?
            curLine.unknown_line_attribute = fstream.ReadInt32();

            // brush base size: 1.875, 2.0, 2.125
            curLine.BrushBaseSize = BaseSizes.Parse(fstream.ReadSingle());

            var pointCount = fstream.ReadInt32();

            for (int n = 0; n < pointCount; ++n)
            {
                ReadPoint(fstream, curLine);
            }

            curLayer.Lines.Add(curLine);
        }

        private static void ReadPoint(BinaryReader fstream, Line curLine)
        {
            Point curPoint;

            curPoint.X = fstream.ReadSingle();
            curPoint.Y = fstream.ReadSingle();

            curPoint.Speed = fstream.ReadSingle();
            curPoint.Direction = fstream.ReadSingle();
            curPoint.Width = fstream.ReadSingle();

            // pressure and rotation of the pen to page normal
            // rotation: for centrially symmetric brushes as now, one attribute
            //           would be sufficient,
            //           let's add a flat nib, calligraphic pen as conversion target! :)
            // range [0.0:1.0]
            curPoint.Pressure = fstream.ReadSingle();

            curLine.Points.Add(curPoint);
        }
    }
}
