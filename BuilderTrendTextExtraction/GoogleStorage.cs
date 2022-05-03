using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.WindowsAPICodePack.Shell;

/**
 * This class handles connecting with the Google cloud storage and performing any
 * related tasks including managing the credential verification, adding files,
 * and downloading a file.
 */
public class GoogleStorage
{
    /**
     * A relative path to the JSON file with google cloud credentials.
     */
    private String JsonPath = @"..\..\..\google-cloud-credentials.json";
    
    /**
     * The name of the bucket where files are stored in Google storage.
     */
    private String BucketName = "buildertrend-text-extraction";
    
    /**
     * Object for verifying credentials.
     */
    private GoogleCredential Credentials;
    
    /**
     * This object is the means to connect to google cloud storage, both for
     * uploading files and downloading files.
     */
    private StorageClient Client;

    /**
     * Creates a GoogleStorage object and gets ready to interact with GoogleStorage.
     * Verifies the credentials and creates the necessary StorageClient that will
     * handle the connection with Google cloud storage.
     */
    public GoogleStorage()
    {
        Credentials = GoogleCredential.FromFile(JsonPath);
        Client = StorageClient.Create(Credentials);
    }

    /**
     * Adds a single file to Google storage.
     * @param filePath A file path to a file on the user's device to be uploaded into Google storage.
     */
    public void AddFile(String filePath)
    {
        using (FileStream f = System.IO.File.OpenRead(filePath))
        {
            string objectName = Path.GetFileName(filePath);
            Client.UploadObject(BucketName, objectName, null, f);
        }
    }
    
    /**
     * Adds the files associated with the given File objects into Google cloud storage.
     * @param filePaths List of File objects to indicate which files
     *                  should be added to Google storage.
     */
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

    /**
     * Downloads a file from Google cloud storage into the user's Download directory.
     * @param objectName The name of the file to download from Google storage.
     * @return Returns the file path of the file downloaded on the user's device.
     */
    public String DownloadFile(String objectName)
    {
        String filePath = KnownFolders.Downloads.Path + "\\" + objectName;
        using var outputFile = System.IO.File.OpenWrite(filePath);
        Client.DownloadObject(BucketName, objectName, outputFile);
        return filePath;
    }
    
    
}