namespace Slithin.Modules.Device;

public class CustomIniSerializer
{
    public static void WriteFile(string filename, IniParser.Model.IniData data)
    {
        var fileStream = File.OpenWrite(filename);
        var writer = new StreamWriter(fileStream);

        foreach (var section in data.Sections)
        {
            writer.WriteLine($"[{section.SectionName}]");

            foreach (var key in section.Keys)
            {
                writer.WriteLine($"{key.KeyName}={key.Value}");
            }

            writer.WriteLine();
        }

        writer.Close();
        fileStream.Close();
    }
}
