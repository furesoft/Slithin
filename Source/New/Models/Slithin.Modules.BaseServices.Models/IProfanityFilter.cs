namespace Slithin.Modules.BaseServices.Models;

public interface IProfanityFilter
{
    bool HasProfanities(string text);
}
