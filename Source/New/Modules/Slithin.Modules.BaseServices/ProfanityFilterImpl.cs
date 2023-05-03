using Slithin.Modules.BaseServices.Models;

namespace Slithin.Modules.BaseServices;

public class ProfanityFilterImpl : IProfanityFilter
{
    public bool HasProfanities(string text)
    {
        var filter = new ProfanityFilter.ProfanityFilter();

        return filter.DetectAllProfanities(text).Any();
    }
}
