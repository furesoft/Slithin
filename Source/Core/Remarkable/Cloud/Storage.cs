using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace Slithin.Core.Remarkable.Cloud
{
    public static class Storage
    {
        public static string Download(string blobUrl)
        {
            var tmp = Path.GetTempFileName();

            var wc = new WebClient();

            wc.DownloadFile(blobUrl, tmp);

            return tmp;
        }

        public static void Extract(string path, string destination)
        {
            ZipFile.ExtractToDirectory(path, destination);
        }

        public static ListItemResult[] ListItems()
        {
            var storageUri = Api.GetStorageUrl();

            var client = new RestClient(storageUri);
            var request = new RestRequest("/document-storage/json/2/docs", Method.GET, DataFormat.Json);
            request.AddQueryParameter("withBlob", "true");
            var token = Api.RefreshToken();

            request.AddHeader("Authorization", "Bearer " + token);

            var response = client.Get(request);
            var obj = JsonConvert.DeserializeObject<ListItemResult[]>(response.Content);

            return obj;
        }

        public static UploadRequestResponse RequestUpload()
        {
            var upReq = new UploadRequest();
            upReq.ID = Guid.NewGuid().ToString();
            upReq.Version = 1;
            upReq.Type = "DocumentType";

            var storageUri = Api.GetStorageUrl();

            var client = new RestClient(storageUri);
            var request = new RestRequest("document-storage/json/2/upload/request", Method.PUT, DataFormat.Json);

            var token = Api.RefreshToken();

            request.AddHeader("Authorization", "Bearer " + token);
            request.AddJsonBody(new[] { upReq });

            var response = client.Put(request);
            var obj = JsonConvert.DeserializeObject<UploadRequestResponse[]>(response.Content);

            return obj[0];
        }

        public static void UpdateMetadata(Metadata md)
        {
            var storageUri = Api.GetStorageUrl();

            var client = new RestClient(storageUri);
            var request = new RestRequest("document-storage/json/2/upload/update-status", Method.PUT, DataFormat.Json);

            var token = Api.RefreshToken();

            request.AddHeader("Authorization", "Bearer " + token);
            request.AddJsonBody(new[] { md });

            client.Put(request);
        }

        public static void Upload(UploadRequestResponse requestResponse, string filename)
        {
            var client = WebRequest.Create(requestResponse.BlobURLPut);
            client.Method = "PUT";
            var strm = client.GetRequestStream();

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var pdfFile = archive.CreateEntry(requestResponse.ID + ".pdf");

                    using var pdfStream = pdfFile.Open();
                    File.OpenRead(filename).CopyTo(pdfStream);

                    pdfStream.Close();
                    var pageDataFile = archive.CreateEntry(requestResponse.ID + ".pagedata");

                    using var pageDataStream = pageDataFile.Open();
                    var sw = new StreamWriter(pageDataStream);
                    sw.Write("");
                    sw.Close();

                    pageDataStream.Close();

                    var contentFile = archive.CreateEntry(requestResponse.ID + ".content");

                    var content = new ContentFile
                    {
                        FileType = "pdf",
                        ExtraMetadata = Array.Empty<object>(),
                        Pages = Array.Empty<string>()
                    };

                    using var contentStream = contentFile.Open();
                    var swContent = new StreamWriter(contentStream);
                    swContent.Write(JsonConvert.SerializeObject(content, Formatting.Indented));
                    swContent.Close();
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.CopyTo(strm);
            }

            var response = client.GetResponse();
        }
    }
}
