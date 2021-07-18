using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiteDB;
using Slithin.Core.Sync;

namespace Slithin.Core
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [BsonIgnore]
        public SynchronisationService SyncService => ServiceLocator.SyncService;

        protected void SetValue<T>(ref T field, T value, [CallerMemberName] string? property = null)
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
