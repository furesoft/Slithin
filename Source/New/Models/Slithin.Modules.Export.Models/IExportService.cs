using Slithin.Entities.Remarkable;

namespace Slithin.Modules.Export.Models;

/// <summary>
/// A service to export notebooks
/// </summary>
public interface IExportService
{
    Task Export(Metadata metadata);
}
