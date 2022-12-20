namespace Slithin.Modules.Backup;

public class Backup
{
    public Backup(string name, string filename)
    {
        Name = name;
        Filename = filename;
    }

    public string Filename { get; set; }
    public string Name { get; set; }
}