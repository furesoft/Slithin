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
            .Where(x => typeof(ITool).IsAssignableFrom(x) && x.IsClass)
            .Select(type => (ITool)ServiceLocator.Container.Resolve(type));

            return types;
        }
    }
}
