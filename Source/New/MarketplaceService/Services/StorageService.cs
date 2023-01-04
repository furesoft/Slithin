using Grpc.Core;
using MarketplaceService.Protos;
using Microsoft.AspNetCore.Authorization;

namespace MarketplaceService.Services;

public class Storage : Protos.StorageService.StorageServiceBase
{
    [Authorize]
    public override async Task<FileListResponse> GetFileList(FileListRequest request, ServerCallContext context)
    {
        var response = new FileListResponse();

        response.Filenames.Add("test.png");

        return response;
    }
}
