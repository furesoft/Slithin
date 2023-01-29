using System.Text;
using AuroraModularis.Core;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Device.Models;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Repository.Models;

namespace Slithin.Entities.Remarkable.Rendering;

public class Notebook
{
    public Notebook(Metadata md)
    {
        var pathManager = ServiceContainer.Current.Resolve<IPathManager>();

        var path = Path.Combine(pathManager.NotebooksDir, md.ID);

        if (Directory.Exists(path))
        {
            var files = Directory.GetFiles(path, "*.rm");
            PageCount = files.Length;
        }

        Version = 5;
        ID = md.ID;
        Metadata = md;
    }

    public string ID { get; set; }
    public Metadata Metadata { get; set; }
    public int PageCount { get; set; }
    public List<Page> Pages { get; set; } = new();

    public int Version { get; set; }

    public static Notebook Load(string id)
    {
        var mdStorage = ServiceContainer.Current.Resolve<IMetadataRepository>();

        return Load(mdStorage.GetMetadata(id));
    }

    public static Notebook Load(Metadata md)
    {
        var notebook = new Notebook(md);
        var pathManager = ServiceContainer.Current.Resolve<IPathManager>();

        notebook.PageCount = md.Content.Pages.Length;

        for (var p = 0; p < notebook.PageCount; ++p)
        {
            var path = Path.Combine(pathManager.NotebooksDir, md.ID, md.Content.Pages[p] + ".rm");

            if (File.Exists(path))
            {
                var strm = File.OpenRead(path);
                var curPage = LoadPage(strm);

                notebook.Pages.Add(curPage);
            }
        }

        return notebook;
    }

    public static Page LoadPage(Stream strm)
    {
        var br = new BinaryReader(strm, Encoding.UTF8, true);

        // skip header
        strm.Seek(33, SeekOrigin.Begin);

        // skip 10x space padding
        strm.Seek(10, SeekOrigin.Current);

        // layers
        Page curPage = new();
        curPage.Layers = new List<Layer>();

        var layerCount = br.ReadInt32();

        for (var nlay = 0; nlay < layerCount; ++nlay)
        {
            ReadLayer(br, curPage);
        }

        br.Close();

        return curPage;
    }

    public static void UploadDocument(Metadata md)
    {
        var pathManager = ServiceContainer.Current.Resolve<IPathManager>();
        var device = ServiceContainer.Current.Resolve<IRemarkableDevice>();
        var pathList = ServiceContainer.Current.Resolve<PathList>();

        var notebooksDir = pathManager.NotebooksDir;

        device.Upload(new FileInfo(Path.Combine(notebooksDir, md.ID + ".metadata")),
               pathList.Documents + md.ID + ".metadata");

        device.Upload(new FileInfo(Path.Combine(notebooksDir, md.ID + "." + md.Content.FileType)),
            pathList.Documents + md.ID + "." + md.Content.FileType);
        device.Upload(new FileInfo(Path.Combine(notebooksDir, md.ID + ".content")),
            pathList.Documents + md.ID + ".content");

        device.Reload();
    }

    public static void UploadNotebook(Metadata md)
    {
        var pathManager = ServiceContainer.Current.Resolve<IPathManager>();
        var localisationService = ServiceContainer.Current.Resolve<ILocalisationService>();
        var device = ServiceContainer.Current.Resolve<IRemarkableDevice>();
        var pathList = ServiceContainer.Current.Resolve<PathList>();

        var notebooksDir = pathManager.NotebooksDir;

        device.Upload(new FileInfo(Path.Combine(notebooksDir, md.ID + ".metadata")),
               pathList.Documents + md.ID + ".metadata");

        device.RunCommand("mkdir " + pathList.Documents + md.ID);
        device.RunCommand("mkdir " + pathList.Documents + md.ID + ".thumbnails");

        /*
        device.Uploading += (s, e) =>
          {
              NotificationService.ShowProgress(
                  LocalisationService.GetStringFormat(
                      "Uploading '{0}': {1}", md.VisibleName, e.Filename)
                  , (int)e.Uploaded, (int)e.Size);
          };*/

        device.Upload(new FileInfo(Path.Combine(notebooksDir, md.ID + ".content")), pathList.Documents + md.ID + ".content");
        device.Upload(new DirectoryInfo(Path.Combine(notebooksDir, md.ID)), pathList.Documents + md.ID);
        device.Upload(new DirectoryInfo(Path.Combine(notebooksDir, md.ID)), pathList.Documents + md.ID + ".thumbnails");

        device.Reload();
    }

    public void Save()
    {
        var pathManager = ServiceContainer.Current.Resolve<IPathManager>();
        var mdStorage = ServiceContainer.Current.Resolve<IMetadataRepository>();
        var md = mdStorage.Load(ID);

        var newContent = md.Content;

        if (md.Content.Pages.Length < Pages.Count)
        {
            var difference = Pages.Count - md.Content.Pages.Length;

            var pages = new List<string>(md.Content.Pages);

            for (var i = 0; i < difference; i++)
            {
                pages.Add(Guid.NewGuid().ToString().ToLower());
            }

            newContent.Pages = pages.ToArray();
        }

        newContent.PageCount = Pages.Count;

        md.Content = newContent;

        mdStorage.SaveToDisk(md);

        for (var i = 0; i < Pages.Count; i++)
        {
            var p = Pages[i];

            var path = Path.Combine(pathManager.NotebooksDir, ID, md.Content.Pages[i] + ".rm");

            var strm = File.OpenWrite(path);
            var bw = new BinaryWriter(strm);

            // write header (33 bytes)
            bw.Write(Encoding.ASCII.GetBytes("reMarkable .lines file, version=" + (char)Version));

            // write space padding
            bw.Write(Encoding.ASCII.GetBytes("".PadRight(10, ' ')));

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

                    bw.Write(0.0f);

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
        Layer curLayer = new() { Lines = new List<Line>() };

        var lineCount = fstream.ReadInt32();

        for (var nl = 0; nl < lineCount; ++nl)
        {
            ReadLine(fstream, curLayer);
        }

        curPage.Layers.Add(curLayer);
    }

    private static void ReadLine(BinaryReader fstream, Layer curLayer)
    {
        Line curLine = new();
        curLine.Points = new List<Point>();

        // select 1-1: 2 (pen)
        // select 1-2: 3 (pen)
        // select 1-3: 4 (fine liner)
        // select 2-1: 7 (pencil sharp)
        // select 2-2: 1 (pencil wide)
        // select 3:   0 (brush)
        // select 4:   5 (marker/highlighter: always color 0)
        // what is/was 6? :-)
        curLine.BrushType = (Brushes)fstream.ReadInt32();

        curLine.Color = (Colors)fstream.ReadInt32();

        // unknown 4 Byte (int32_t?), always zero?
        // non-stored information about "selected" lines?
        curLine.unknown_line_attribute = fstream.ReadInt32();

        // brush base size: 1.875, 2.0, 2.125
        curLine.BrushBaseSize = BaseSizes.Parse(fstream.ReadSingle());

        fstream.ReadSingle();

        var pointCount = fstream.ReadInt32();

        for (var n = 0; n < pointCount; ++n)
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
