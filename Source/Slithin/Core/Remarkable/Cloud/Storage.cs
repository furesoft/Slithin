using System;
using System.IO;
using System.Net;
using Ionic.Zip;
using Newtonsoft.Json;
using RestSharp;

namespace Slithin.Core.Remarkable.Cloud;

public class Storage
{
    private readonly Api _api;
    private readonly string _storageUri;
    private readonly string _token;

    public Storage(Api api)
    {
        _api = api;
        _storageUri = _api.GetStorageUrl();
        _token = _api.RefreshToken();
    }

    public void Delete(Metadata md)
    {
        var client = new RestClient(_storageUri);
        var request = new RestRequest("document-storage/json/2/upload/update-status", Method.PUT, DataFormat.Json);

        request.AddHeader("Authorization", "Bearer " + _token);
        request.AddJsonBody(new[] {md});

        client.Put(request);
    }

    public string Download(string blobUrl)
    {
        var tmp = Path.GetTempFileName();

        var wc = new WebClient();

        wc.DownloadFile(blobUrl, tmp);

        wc.Dispose();

        return tmp;
    }

    public void Extract(string path, string destination)
    {
        ZipFile.Read(path).ExtractAll(destination);
    }

    public ListItemResult[] ListItems()
    {
        var client = new RestClient(_storageUri);
        var request = new RestRequest("/document-storage/json/2/docs", Method.GET, DataFormat.Json);
        request.AddQueryParameter("withBlob", "true");

        request.AddHeader("Authorization", "Bearer " + _token);

        var response = client.Get(request);
        var obj = JsonConvert.DeserializeObject<ListItemResult[]>(response.Content);

        return obj;
    }

    public UploadRequestResponse RequestUpload()
    {
        var upReq = new UploadRequest {ID = Guid.NewGuid().ToString().ToLower(), Version = 1, Type = "DocumentType"};

        var client = new RestClient(_storageUri);
        var request = new RestRequest("document-storage/json/2/upload/request", Method.PUT, DataFormat.Json);

        request.AddHeader("Authorization", "Bearer " + _token);
        request.AddJsonBody(new[] {upReq});

        var response = client.Put(request);
        var obj = JsonConvert.DeserializeObject<UploadRequestResponse[]>(response.Content);

        return obj[0];
    }

    public void UpdateMetadata(CloudMetadata md)
    {
        var client = new RestClient(_storageUri);
        var request = new RestRequest("/document-storage/json/2/upload/update-status", Method.PUT, DataFormat.Json);

        request.AddHeader("Authorization", "Bearer " + _token);
        request.AddJsonBody(new[] {md});

        client.Put(request);
    }

    public void Upload(UploadRequestResponse requestResponse, string filename)
    {
        var client = WebRequest.Create(requestResponse.BlobURLPut);
        client.Method = "PUT";
        var strm = client.GetRequestStream();

        using (var memoryStream = new MemoryStream())
        {
            using (var zip = new ZipFile())
            {
                zip.AddEntry(requestResponse.ID + ".pdf", File.OpenRead(filename));
                zip.AddEntry(requestResponse.ID + ".pagedata", "");

                var content = new ContentFile
                {
                    FileType = "pdf", ExtraMetadata = Array.Empty<object>(), Pages = Array.Empty<string>()
                };

                zip.AddEntry(requestResponse.ID + ".content",
                    JsonConvert.SerializeObject(content, Formatting.Indented));

                zip.Save(memoryStream);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            memoryStream.CopyTo(strm);
        }

        client.GetResponse();
    }
}
