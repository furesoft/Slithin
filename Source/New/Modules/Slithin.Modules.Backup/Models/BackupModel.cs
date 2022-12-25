namespace Slithin.Modules.Backup.Models;

internal class BackupModel
{
    public BackupModel(string name, string filename)
    {
        Name = name;
        Filename = filename;
    }

    public string Filename { get; set; }
    public string Name { get; set; }
}
