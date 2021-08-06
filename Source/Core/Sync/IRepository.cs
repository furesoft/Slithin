using System;
using Slithin.Core.Remarkable;

namespace Slithin.Core.Sync
{
    public interface IRepository
    {
        void Add(Template template);

        Template[] GetTemplates();

        Version GetVersion();

        void Remove(Template template);
    }
}
