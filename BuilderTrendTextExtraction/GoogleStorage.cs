using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.WindowsAPICodePack.Shell;

public class GoogleStorage
{

    private String JsonPath = @"..\..\..\google-cloud-credentials.json";
    private String BucketName = "buildertrend-text-extraction";
    private GoogleCredential Credentials;
    private StorageClient Client;

    public GoogleStorage()
    {
        Credentials = GoogleCredential.FromFile(JsonPath);
        Client = StorageClient.Create(Credentials);
    }

    public void AddFile(String filePath)
    {
        using (FileStream f = System.IO.File.OpenRead(filePath))
        {
            string objectName = Path.GetFileName(filePath);
            Client.UploadObject(BucketName, objectName, null, f);
        }
    }
    
    
    public void AddFiles(List<File> filePaths)
    {
        foreach (var path in filePaths)
        {
            using (FileStream f = System.IO.File.OpenRead(path.FileName))
            {
                string objectName = Path.GetFileName(path.FileName);
                Client.UploadObject(BucketName, objectName, null, f);
            }
        }
    }

    public String DownloadFile(String objectName)
    {
        String filePath = KnownFolders.Downloads.Path + "\\" + objectName;
        using var outputFile = System.IO.File.OpenWrite(filePath);
        Client.DownloadObject(BucketName, objectName, outputFile);
        return filePath;
    }
    
    
}