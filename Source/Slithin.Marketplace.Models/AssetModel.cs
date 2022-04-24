namespace SlithinMarketplace.Models;

public class AssetModel
{
    public string CreatorID { get; set; }
    public string FileID { get; set; }
    public string ID { get; set; }
    public DateTime UploadTime { get; set; }
<<<<<<< develop

    public void InitIDs()
    {
        ID = Guid.NewGuid().ToString();
        FileID = Guid.NewGuid().ToString();
    }
=======
>>>>>>> Add Models
}
