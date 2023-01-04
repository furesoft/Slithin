using Slithin.Entities.Remarkable;

namespace Slithin.Modules.Export.Models;

public interface IExportService
{
    Task Export(Metadata metadata);
}
