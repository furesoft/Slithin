﻿namespace Slithin.Modules.Resources.Models;

public interface IMarketplaceAPIService
{
    string Auth(string username, string password);

    Task AddAssetAsync(AssetModel model);

    Task<string> UploadFileAsync(Stream strm);
    Task<Stream> DownloadFileAsync(string id);
}
