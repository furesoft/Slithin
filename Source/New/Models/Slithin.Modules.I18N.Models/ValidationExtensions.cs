using AuroraModularis;

namespace Slithin.Modules.I18N.Models;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, IList<TElement>> WithLocalisedMessage<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, string key)
    {
        var localisatinService = Container.Current.Resolve<ILocalisationService>();
        
        return ruleBuilder.WithMessage(localisatinService.GetString(key));
    }
}
