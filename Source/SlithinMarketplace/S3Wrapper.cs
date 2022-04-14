using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;

namespace SlithinMarketplace;

public class S3Wrapper
{
    private AmazonS3Client client;

    public S3Wrapper(AmazonS3Client client)
    {
        this.client = client;
    }

    public FileInfo GetObject(string bucket, string key, string output)
    {
        var name = Path.GetFileName(key);
        var outputPath = Path.Join(output, name);
        Console.WriteLine("Downloading: " + key + " to " + outputPath);

        try
        {
            GetObjectRequest request = new()
            {
                BucketName = bucket,
                Key = key
            };

            var task = client.GetObjectAsync(request);
            task.Wait();
            var response = task.Result;
            var responseStream = response.ResponseStream;
            var fileStream = File.OpenWrite(outputPath);
            responseStream.CopyTo(fileStream);
            fileStream.Close();
        }
        catch (AmazonS3Exception e)
        {
            // If the bucket or the object do not exist
            Console.WriteLine($"Error: '{e.Message}'");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unhandled Error: {e.Message}");
        }

        return new FileInfo(outputPath);
    }

    public T GetObject<T>(string bucket, string key)
    {
        var strm = GetObjectStream(bucket, key);
        var json = new StreamReader(strm).ReadToEnd();

        return JsonConvert.DeserializeObject<T>(json);
    }

    public Stream GetObjectStream(string bucket, string key)
    {
        var strm = new MemoryStream();

        try
        {
            GetObjectRequest request = new()
            {
                BucketName = bucket,
                Key = key
            };

            var task = client.GetObjectAsync(request);
            task.Wait();
            var response = task.Result;
            var responseStream = response.ResponseStream;

            responseStream.CopyTo(strm);

            strm.Seek(0, SeekOrigin.Begin);
            strm.Close();
        }
        catch (AmazonS3Exception e)
        {
            // If the bucket or the object do not exist
            Console.WriteLine($"Error: '{e.Message}'");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unhandled Error: {e.Message}");
        }

        return strm;
    }

    public List<S3Object> ListObjects(string bucketName)
    {
        Console.WriteLine("Listing files from Bucket: " + bucketName);
        List<S3Object> result = new();
        try
        {
            ListObjectsV2Request request = new()
            {
                BucketName = bucketName,
                MaxKeys = 5
            };

            var response = new ListObjectsV2Response();
            do
            {
                var task = client.ListObjectsV2Async(request);
                task.Wait();
                response = task.Result;
                result.AddRange(response.S3Objects);

                // If the response is truncated, set the request ContinuationToken
                // from the NextContinuationToken property of the response.
                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine($"Error:'{e.Message}'");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unhandled Error: {e.Message}");
        }

        return result;
    }

    public void UploadObject(string bucket, string id, object obj)
    {
        static Stream Serialize(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var jsonRaw = Encoding.ASCII.GetBytes(json);

            return new MemoryStream(jsonRaw);
        }

        UploadObjectFromStream(bucket, id, Serialize(obj));
    }

    public void UploadObjectFromFile(string bucketName, string objectName, string filePath)
    {
        Console.WriteLine("Uploading: " + filePath + " to " + bucketName + " >> " + objectName);
        try
        {
            PutObjectRequest putRequest = new()
            {
                BucketName = bucketName,
                Key = objectName,
                InputStream = File.OpenRead(filePath)
            };

            var task = this.client.PutObjectAsync(putRequest);
            task.Wait();
            var response = task.Result;
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unhandled Error: {e.Message}");
        }
    }

    public void UploadObjectFromStream(string bucketName, string objectName, Stream strm)
    {
        try
        {
            PutObjectRequest putRequest = new()
            {
                BucketName = bucketName,
                Key = objectName,
                InputStream = strm
            };

            var task = this.client.PutObjectAsync(putRequest);
            task.Wait();
            var response = task.Result;
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unhandled Error: {e.Message}");
        }
    }
}
