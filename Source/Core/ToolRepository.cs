using System;
using System.Collections.Generic;
using System.Linq;

namespace Slithin.Core
{
    public class ToolRepository
    {
        public IEnumerable<ITool> FindTools()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(x => typeof(ITool).IsAssignableFrom(x) && x.IsClass);

            foreach (var type in types)
            {
                yield return (ITool)ServiceLocator.Container.Resolve(type);
            }
        }
    }
}
