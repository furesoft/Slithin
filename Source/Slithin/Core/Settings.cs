using LiteDB;

namespace Slithin.Core
{
    public class Settings
    {
        public ObjectId _id { get; set; }

        public bool AutomaticTemplateRecovery { get; set; }
        public bool HasFirstGalleryShown { get; set; }
    }
}
